using System;

namespace ToDoListApp
{

    public abstract class TaskDecorator : ITask
    {

        protected ITask task;

        public TaskDecorator(ITask task)
        {
            this.task = task;
        }

        public int Id
        {
            get { return task.Id; }
        }

        public string Name
        {
            get { return task.Name; }
        }

        public DateTime DueDate
        {
            get { return task.DueDate; }
        }

        public Priority Priority
        {
            get { return task.Priority; }
        }

        public bool IsCompleted
        {
            get { return task.IsCompleted; }
        }

 
        public virtual string GetInfo()
        {
            return task.GetInfo();
        }

        public override string ToString()
        {
            return GetInfo();
        }
    }

    public class UrgentTaskDecorator : TaskDecorator
    {
        public UrgentTaskDecorator(ITask task) : base(task)
        {
        }

        public override string GetInfo()
        {
            return "*** URGENT *** " + base.GetInfo();
        }
    }

    public class OverdueTaskDecorator : TaskDecorator
    {
        public OverdueTaskDecorator(ITask task) : base(task)
        {
        }

        public override string GetInfo()
        {
            if (task.IsCompleted == false && task.DueDate < DateTime.Now)
            {
                return "[OVERDUE] " + base.GetInfo();
            }
            else
            {
                return base.GetInfo();
            }
        }
    }
}
