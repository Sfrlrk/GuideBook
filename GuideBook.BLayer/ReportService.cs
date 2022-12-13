using EnumHelper;
using GuideBook.BLayer.Interfaces;
using GuideBook.Dal.Interfaces;
using GuideBook.Dto;
using GuideBook.Dto.InfoMessages;
using GuideBook.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace GuideBook.BLayer
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository reportRepository;
        public ReportService(IReportRepository _reportRepository) => reportRepository = _reportRepository;

        public async Task<ServiceResult<Report>> Create(Report report)
        {
            try
            {
                if (report == null)
                {
                    return new ServiceResult<Report>(nameof(Messages.ReportWasNotFound), Messages.ReportWasNotFound);
                }
                var res = await reportRepository.AddAsync(report);

                return new ServiceResult<Report>(nameof(Messages.Success), Messages.Success, res);
            }
            catch
            {
                return new ServiceResult<Report>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }
        public async Task<ServiceResult<bool>> ChangeType(Guid id, string filePath, EReportType reportType = EReportType.Completed)
        {
            try
            {
                var res = await reportRepository.GetAsync(x => x.Id == id);
                if (res == null)
                {
                    return new ServiceResult<bool>(nameof(Messages.ReportWasNotFound), Messages.ReportWasNotFound);
                }

                res.ReportType = reportType;
                res.FilePath = filePath;

                await reportRepository.UpdateAsync(id, res);

                return new ServiceResult<bool>(nameof(Messages.Success), Messages.Success, true);
            }
            catch
            {
                return new ServiceResult<bool>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }

        public async Task<ServiceResult<List<Report>>> ToList()
        {
            try
            {
                var data = await reportRepository.GetAll();
                return new ServiceResult<List<Report>>(nameof(Messages.Success), Messages.Success, data);
            }
            catch
            {
                return new ServiceResult<List<Report>>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }

        public async Task<ServiceResult<Report>> GetReport(Guid id)
        {
            try
            {
                var data = await reportRepository.GetAsync(x => x.Id == id);
                return new ServiceResult<Report>(nameof(Messages.Success), Messages.Success, data);
            }
            catch
            {
                return new ServiceResult<Report>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }

        public async Task<ServiceResult<ReportViewModel>> CreateReportByLocation(string location, string email)
        {
            try
            {
                var report = await Create(new Report()
                {
                    ReportType = EReportType.Preparing,
                    Location = location,
                    RequestDate = DateTime.Now
                });

                var newData = new ReportViewModel()
                {
                    EmailAddress = email,
                    Id = report.Data.Id,
                    ReportType = report.Data.ReportType,
                    Location = report.Data.Location,
                    RequestDate = report.Data.RequestDate
                };

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Report", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    string message = JsonConvert.SerializeObject(newData);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "Report", basicProperties: null, body: body);
                }

                return new ServiceResult<ReportViewModel>(nameof(Messages.Success), Messages.Success, newData);
            }
            catch
            {
                return new ServiceResult<ReportViewModel>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }
    }
}
