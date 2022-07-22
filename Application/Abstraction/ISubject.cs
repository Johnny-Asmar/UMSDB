using Domain.Models;
using PCP.Application.EmailObservablePattern.Classes;

namespace PCP.Application.EmailObservablePattern.Interfaces;

public interface ISubject {  
    void registerObserver(User observer);  
    void unregisterObserver(User observer);
    public void notifyObservers(string bodyMessage);
    public void SendEmailIfSubscribed(User user, string bodyMessage);
    public void RefreshObservers();
}  