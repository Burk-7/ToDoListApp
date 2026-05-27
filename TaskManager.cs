using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToDoListApp
{

    public delegate List<TaskItem> SortStrategy(List<TaskItem> tasks);

    public class TaskManager
    {
        private static TaskManager instance = null;

        public static TaskManager GetInstance()
        {
            if (instance == null)
            {
                instance = new TaskManager();
            }
            return instance;
        }

        private List<TaskItem> tasks;
        private int nextId;
        private string filePath = "tasks.txt";

        private Logger logger = Logger.GetInstance();

        public SortStrategy SortByDate;
        public SortStrategy SortByPriority;

      
        private TaskManager()
        {
            tasks = new List<TaskItem>();
            nextId = 1;

            SortByDate = (list) =>
            {
                return list.OrderBy(t => t.DueDate).ToList();
            };

            SortByPriority = (list) =>
            {
                return list.OrderByDescending(t => t.Priority).ToList();
            };

            LoadFromFile();
        }

        public void AddTask(string name, DateTime dueDate, Priority priority)
        {
            TaskItem task = new TaskItem(nextId, name, dueDate, priority);
            nextId = nextId + 1;
            tasks.Add(task);
            SaveToFile();
            logger.Log("Task added: " + task.Name);
        }

        public List<TaskItem> GetAllTasks()
        {
            return tasks;
        }

        public bool EditTask(int id, string newName, DateTime newDueDate, Priority newPriority)
        {
            TaskItem task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return false;
            }

            task.Name = newName;
            task.DueDate = newDueDate;
            task.Priority = newPriority;
            SaveToFile();
            logger.Log("Task updated: #" + id + " -> " + newName);
            return true;
        }

        public bool DeleteTask(int id)
        {
            TaskItem task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return false;
            }

            tasks.Remove(task);
            SaveToFile();
            logger.Log("Task deleted: #" + id + " - " + task.Name);
            return true;
        }

        public bool ToggleComplete(int id)
        {
            TaskItem task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return false;
            }

            if (task.IsCompleted == true)
            {
                task.IsCompleted = false;
            }
            else
            {
                task.IsCompleted = true;
            }

            SaveToFile();

            string state;
            if (task.IsCompleted == true)
            {
                state = "completed";
            }
            else
            {
                state = "reopened";
            }
            logger.Log("Task " + state + ": #" + id + " - " + task.Name);
            return true;
        }


        public List<TaskItem> GetTasksByPriority(Priority priority)
        {
            return tasks.Where(t => t.Priority == priority).ToList();
        }

        public List<TaskItem> GetOverdueTasks()
        {
            return tasks.Where(t => t.IsCompleted == false && t.DueDate < DateTime.Now).ToList();
        }

        private void SaveToFile()
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath, false);
                for (int i = 0; i < tasks.Count; i++)
                {
                    writer.WriteLine(tasks[i].ToFileString());
                }
                writer.Close();
            }
            catch (Exception ex)
            {
                logger.Log("Dosyaya kayit hatasi: " + ex.Message);
            }
        }

        private void LoadFromFile()
        {
            if (File.Exists(filePath) == false)
            {
                return;
            }

            try
            {
                StreamReader reader = new StreamReader(filePath);
                string line = reader.ReadLine();
                while (line != null)
                {
                    TaskItem item = TaskItem.FromFileString(line);
                    if (item != null)
                    {
                        tasks.Add(item);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();

                if (tasks.Count > 0)
                {
                    nextId = tasks.Max(t => t.Id) + 1;
                }
            }
            catch (Exception ex)
            {
                logger.Log("Dosya okuma hatasi: " + ex.Message);
            }
        }
    }
}
