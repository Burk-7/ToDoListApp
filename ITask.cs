using System;

namespace ToDoListApp
{
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
