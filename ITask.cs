using System;

namespace ToDoListApp
{
    // Interface: tum gorev turleri bu arayuzu uygular 
    public interface ITask
    {
        int Id { get; }
        string Name { get; }
        DateTime DueDate { get; }
        Priority Priority { get; }
        bool IsCompleted { get; }

        string GetInfo();
    }
}
