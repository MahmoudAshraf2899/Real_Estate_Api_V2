using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Commands.Visitors;
using System.Net;
using System.Net.Mail;

namespace RealEstateApi.Handler.Visitors
{
    public class SendEmailToVisitorsHandler : IRequestHandler<SendMailToVisitorsCommand, List<VisitorsEmailsDto>>
    {
        private readonly IVisitorRepository _visitorRepository;

        public SendEmailToVisitorsHandler(IVisitorRepository visitorRepository)
        {
           _visitorRepository = visitorRepository;
        }

        public async Task<List<VisitorsEmailsDto>> Handle(SendMailToVisitorsCommand request, CancellationToken cancellationToken)
        {
            var visitorsList = await _visitorRepository.getAllActiveVisitorsEmails();
            foreach (var item in visitorsList)
            {
                try
                {
                    using (var client = new SmtpClient())
                    {
                        var message = new MailMessage();
                        message.To.Add(new MailAddress(item.email));
                        message.From = new MailAddress("mahmodashrf79@gmail.com", "Real Estate");//Move To AppSettings

                        message.Subject = request.subject;
                        message.Body = request.body;

                        client.Host = "smtp.gmail.com";
                        client.Port = 587;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("mahmodashrf79@gmail.com", "vohoaanrrbqkoaww");//Move To AppSettings
                        client.EnableSsl = true;
                        client.Send(message);
                    }
                }

                catch (Exception ex)
                {

                    throw;
                }
            }
            return visitorsList;
        }
    }
}
