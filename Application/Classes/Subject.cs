using CommonFunctions.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using PCP.Application.EmailObservablePattern.Interfaces;
using Persistence;

namespace PCP.Application.EmailObservablePattern.Classes;

public class Subject: ISubject {  
    private List < User > Observers = new List < User > ();  
    private UmsContext _umsContext;
    private ISendEmail _sendEmail;
    


    public Subject(UmsContext umsContext, ISendEmail sendEmail)
    {
        _umsContext = umsContext;
        _sendEmail = sendEmail;
        
    }

    public void registerObserver(User observer) {  
        Observers.Add(observer);  
    }  
    public void unregisterObserver(User observer) {  
        Observers.Remove(observer);  
    }

  

    public void notifyObservers(string bodyMessage) {  
        foreach(var observer in Observers)
        {
            IObserver observerUser = new Observer(_sendEmail);
            observerUser.Update(observer.Email, bodyMessage);
        }  
    }

    public void RefreshObservers()
    {
        List<Domain.Models.User> users = _umsContext.Users.Select(x => x).ToList();
        foreach (var user in users)
        {
            if (user.RoleId == 3 && user.SubsribeToEmail == 1)
            {
                registerObserver(user);
            }
        }
    }

    public void SendEmailIfSubscribed(User user, string bodyMessage)
    {
        if (user.RoleId == 3 && user.SubsribeToEmail == 1)
        {
            IObserver observerUser = new Observer(_sendEmail);
            observerUser.Update(user.Email, bodyMessage);
        }
    }
} 