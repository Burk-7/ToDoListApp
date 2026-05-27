using System;
using System.Globalization;

namespace ToDoListApp
{

    public class TaskItem : ITask
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }


        public TaskItem(int id, string name, DateTime dueDate, Priority priority)
        {
            this.Id = id;
            this.Name = name;
            this.DueDate = dueDate;
            this.Priority = priority;
            this.IsCompleted = false;
        }

        public TaskItem()
        {
            this.Name = "";
        }

        public void MarkAsCompleted()
        {
            this.IsCompleted = true;
        }

        public string GetInfo()
        {
            string status;
            if (this.IsCompleted == true)
            {
                status = "[X]";
            }
            else
            {
                status = "[ ]";
            }

            string info = status + " #" + this.Id + " | " + this.Name
                + " | Date: " + this.DueDate.ToString("dd.MM.yyyy HH:mm")
                + " | Priority: " + this.Priority.ToString();
            return info;
        }

        public override string ToString()
        {
            return GetInfo();
        }


        public string ToFileString()
        {
            string dateStr = this.DueDate.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            string completedStr;
            if (this.IsCompleted == true)
            {
                completedStr = "true";
            }
            else
            {
                completedStr = "false";
            }

            return this.Id + ";" + this.Name + ";" + dateStr + ";" + this.Priority.ToString() + ";" + completedStr;
        }

        public static TaskItem FromFileString(string line)
        {
            if (string.IsNullOrEmpty(line) == true)
            {
                return null;
            }

            string[] parts = line.Split(';');
            if (parts.Length < 5)
            {
                return null;
            }

            try
            {
                int id = int.Parse(parts[0]);
                string name = parts[1];
                DateTime date = DateTime.ParseExact(parts[2], "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                Priority pr = (Priority)Enum.Parse(typeof(Priority), parts[3]);
                bool completed = bool.Parse(parts[4]);

                TaskItem item = new TaskItem(id, name, date, pr);
                item.IsCompleted = completed;
                return item;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
