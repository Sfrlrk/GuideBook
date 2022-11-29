using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestSharp;
using GuideBook.Helper;
using GuideBook.Dto;
using GuideBook.Dto.ServiceResults;
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
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using var channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume("MckReport", false, consumer);
                consumer.Received += (sender, e) =>
                {
                    var rawData = e.Body.ToArray();
                    var data = Encoding.UTF8.GetString(rawData);

                    var res = JsonConvert.DeserializeObject<ReportViewModel>(data);

                    var filePath = ExcelOperations.CreateExcel(res.Location);
                    using (var client = new RestClient("https://localhost:44302/api"))
                    {
                        var request = new RestRequest($"/Report/ChangeReportStatus/{res.Id}");

                        var response = client.GetAsync(request).Result;
                        var changeStatusData = new ServiceResult<bool>();
                        if (!string.IsNullOrEmpty(response.Content))
                        {
                            changeStatusData = JsonConvert.DeserializeObject<ServiceResult<bool>>(response.Content);
                        }
                    }

                    SenderService.SendMail(filePath, res.EmailAddress);
                    channel.BasicAck(e.DeliveryTag, false);
                };
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
