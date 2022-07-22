using CommonFunctions.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using PCP.Application.EmailObservablePattern.Interfaces;

namespace PCP.Application.EmailObservablePattern.Classes;

public class Observer : User, IObserver
{
    private ISendEmail _sendEmail;

    public Observer(ISendEmail sendEmail)
    {
        _sendEmail = sendEmail;
        
    }

    public void Update(string ObserverEmail, string bodyMessage) {  
        //Observer can update his system accordingly  
        _sendEmail.sendEmailToStudent(ObserverEmail, bodyMessage);
    }


}