using System.Net.Mail;
using System.Text;
using CommonFunctions.Interfaces;

namespace CommonFunctions;

public class SendEmail : ISendEmail
{

    public void sendEmailToStudent(string MailTo, string bodyMessage)
    {
        string to = MailTo; //To address    
        string from = "johnny.asmar123@gmail.com"; //From address    
        MailMessage message = new MailMessage(from, to);  
  
                    
        string mailbody = bodyMessage ;  
        message.Subject = "Enrollnment";  
        message.Body = mailbody;  
        message.BodyEncoding = Encoding.UTF8;  
        message.IsBodyHtml = true;  
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
        System.Net.NetworkCredential basicCredential1 = new  
            System.Net.NetworkCredential("johnny.asmar123@gmail.com", "shhueyjpnpxerkag");  
        client.EnableSsl = true;  
        client.UseDefaultCredentials = false;  
        client.Credentials = basicCredential1;  
        try   
        {  
            client.Send(message);  
        }   
  
        catch (Exception ex)   
        {  
            throw ex;  
        }
    }
    
    
    
}