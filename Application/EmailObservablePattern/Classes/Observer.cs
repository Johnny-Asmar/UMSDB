using Domain.Models;
using PCP.Application.EmailObservablePattern.Interfaces;

namespace PCP.Application.EmailObservablePattern.Classes;

public class Observer : User, IObserver
{
    public void Update() {  
        //Observer can update his system accordingly  
        Console.WriteLine("Hello, a new article has been published by the author.");  
    } 
}