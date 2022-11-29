using EnumHelper;
using GuideBook.BLayer.Interfaces;
using GuideBook.Dal.Interfaces;
using GuideBook.Dto;
using GuideBook.Dto.ErrorMessages;
using GuideBook.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace GuideBook.BLayer
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportDal;
        public ReportService(IReportRepository reportDal) => _reportDal = reportDal;

        public async Task<ServiceResult<bool>> ChangeReportStatus(Guid id)
        {
            try
            {
                var res = await _reportDal.GetAsync(x => x.Id == id);
                if (res == null)
                    return new ServiceResult<bool>(nameof(Messages.ReportWasNotFound), Messages.ReportWasNotFound);
                else
                {
                    res.ReportType = EReportType.Completed;
                    await _reportDal.UpdateAsync(id, res);
                    return new ServiceResult<bool>(nameof(Messages.Success), Messages.Success, true);
                }
            }
            catch
            {
                return new ServiceResult<bool>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }

        public async Task<ServiceResult<List<Report>>> GetAllReports()
        {
            try
            {
                var data = await _reportDal.GetAll();
                return new ServiceResult<List<Report>>(nameof(Messages.Success), Messages.Success, data);
            }
            catch
            {
                return new ServiceResult<List<Report>>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }

        public async Task<ServiceResult<Report>> GetReportDetails(Guid id)
        {
            try
            {
                var data = await _reportDal.GetAsync(x => x.Id == id);
                return new ServiceResult<Report>(nameof(Messages.Success), Messages.Success, data);
            }
            catch
            {
                return new ServiceResult<Report>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
            }
        }

        public async Task<ServiceResult<ReportViewModel>> ReceiveReportByLocation(string location, string email)
        {
            try
            {
                var data = await _reportDal.AddAsync(new Report()
                {
                    ReportType = EReportType.Preparing,
                    Location = location,
                    RequestDate = DateTime.Now
                });

                var newData = new ReportViewModel()
                {
                    EmailAddress = email,
                    Id = data.Id,
                    ReportType = data.ReportType,
                    Location = data.Location,
                    RequestDate = data.RequestDate
                };

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "MckReport", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    string message = JsonConvert.SerializeObject(newData);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "MckReport", basicProperties: null, body: body);
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
