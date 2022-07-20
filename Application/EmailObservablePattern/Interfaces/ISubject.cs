using Domain.Models;
using PCP.Application.EmailObservablePattern.Classes;

namespace PCP.Application.EmailObservablePattern.Interfaces;

public interface ISubject {  
    void registerObserver(Observer observer);  
    void unregisterObserver(Observer observer);  
    void notifyObservers();  
}  