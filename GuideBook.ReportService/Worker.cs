using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using GuideBook.Dto;
using GuideBook.Helper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestSharp;
using System.Text;

namespace GuideBook.ReportService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using var channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume("MckReport", false, consumer);
                consumer.Received += async (sender, e) =>
                {
                    var rawData = e.Body.ToArray();
                    var data = Encoding.UTF8.GetString(rawData);
                    var reportVm = JsonConvert.DeserializeObject<ReportViewModel>(data);

                    CreateReport(reportVm);

                    channel.BasicAck(e.DeliveryTag, false);
                };
            }
            _logger.LogInformation($"Worker Worked at: {DateTimeOffset.Now}");

            await Task.Delay(5000, stoppingToken);
        }
    }

    async void CreateReport(ReportViewModel reportVm)
    {
        var reportData = await GetReportData(reportVm.Location);

        var filePath = ExcelOperations.CreateExcel(reportData);

        ChangeType(reportVm.Id, filePath);

        SenderService.SendMail(filePath, reportVm.EmailAddress);
    }

    async Task<ExcelReportViewModel> GetReportData(string location)
    {
        var result = await GetAsync<ServiceResult<ExcelReportViewModel>>("https://localhost:5048/api", $"Contact/GetReportByLocation/{location}");
        return result.Data;
    }
    async void ChangeType(Guid Id, string filePath)
    {
        await GetAsync<ServiceResult<bool>>("https://localhost:5100/api", $"/Report/ChangeType/{Id}/{filePath}");
    }
    async Task<T> GetAsync<T>(string baseUrl, string resource)
    {
        using var client = new RestClient(baseUrl);
        var request = new RestRequest(resource);

        var response = await client.GetAsync(request);

        if (!string.IsNullOrEmpty(response.Content))
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
        return default;
    }
}
