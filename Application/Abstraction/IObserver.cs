namespace PCP.Application.EmailObservablePattern.Interfaces;

public interface IObserver {  
    public void Update(string ObserverEmail, string bodyMessage);  
}  