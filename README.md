# ToDoListApp

A Windows Forms to-do list application written in C#.
Collegium Da Vinci — Object-Oriented Programming final project.

## What it does
- Add, edit, delete tasks with due date and priority (Low / Medium / High)
- Mark tasks as complete / reopen them
- View overdue tasks
- Sort tasks by date or priority
- Auto-saves to file, logs all actions

## OOP Patterns Used
- Singleton (TaskManager, Logger)
- Decorator (UrgentTaskDecorator, OverdueTaskDecorator)
- Interface (ITask)
- Delegate & Lambda (SortStrategy)
- LINQ (filtering, sorting)

> Windows only (.NET, WinForms)
