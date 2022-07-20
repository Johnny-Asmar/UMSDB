namespace CommonFunctions.Interfaces;

public interface ISendEmail
{
    public void sendEmailToStudent(string MailTo, string bodyMessage);
}