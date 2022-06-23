using System.Net;
using System.Net.Mail;

namespace AulpagCollecte.Services
{
    public class Envoi
    {
        
        public static void SendMail()
        {
            MailMessage Message = new MailMessage();
            Message.Subject = "Planning horaires Paris-Granville";
            Message.Body = "Dernière mise à jour";
            Message.From = new MailAddress("philippe.andanson@gmail.com") ;
            Message.To.Add(new MailAddress("philippe.andanson@gmail.com"));
          //  Message.To.Add(new MailAddress("philippe.contis@gmail.com"));
         //   Message.To.Add(new MailAddress("pierre.forme@free.fr "));
            Message.Attachments.Add(new Attachment(@"D:\BddWindev\Edition planning\Horaires Sncf Paris-Granville.pdf"));
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.googlemail.com";
           
            client.Port = 587;
         // client.UseDefaultCredentials = false;
            client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("philippe.andanson@gmail.com","%Contis40$");
        //client.Send(Message);
        }
    }
}

