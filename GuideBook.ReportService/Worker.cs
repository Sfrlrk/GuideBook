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

            const string queueName = "Report";
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            try
            {
                channel.QueueDeclare(queueName, false, false, false, null);
            }
            catch { }

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, false, consumer);
            consumer.Received += async (sender, e) =>
            {
                var rawData = e.Body.ToArray();
                var data = Encoding.UTF8.GetString(rawData);
                var reportVm = JsonConvert.DeserializeObject<ReportViewModel>(data);

                await CreateReport(reportVm);

                try
                {
                    channel.BasicAck(e.DeliveryTag, false);
                }
                catch { }
            };

            _logger.LogInformation($"Worker Worked at: {DateTimeOffset.Now}");

            await Task.Delay(5000, stoppingToken);
        }
    }

    async Task CreateReport(ReportViewModel reportVm)
    {
        var reportData = await GetReportData(reportVm.Location);

        var filePath = ExcelOperations.CreateExcel(reportData);

        ChangeType(reportVm.Id, filePath);

        try
        {
            SenderService.SendMail(filePath, reportVm.EmailAddress);
        }
        catch { }
    }

    async Task<ExcelReportViewModel> GetReportData(string location)
    {
        var result = await GetAsync<ServiceResult<ExcelReportViewModel>>("http://localhost:5048/api", $"Contact/GetReportByLocation/{location}");
        return result.Data;
    }
    async void ChangeType(Guid Id, string filePath)
    {
        await GetAsync<ServiceResult<bool>>("http://localhost:22634/api", $"/Report/ChangeType/{Id}/{filePath}");
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
