using PCP.Application.EmailObservablePattern.Interfaces;

namespace PCP.Application.EmailObservablePattern.Classes;

public class Subject: ISubject {  
    private List < Observer > Observers = new List < Observer > ();  
    private int articlesCount = 1;  
    
    public void registerObserver(Observer observer) {  
        Observers.Add(observer);  
    }  
    public void unregisterObserver(Observer observer) {  
        Observers.Remove(observer);  
    }  
    public void notifyObservers() {  
        foreach(var observer in Observers) {  
            observer.Update();  
        }  
    }  
} 