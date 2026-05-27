using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ToDoListApp
{
    public class MainForm : Form
    {
        // Singleton orneklere referans
        private TaskManager manager = TaskManager.GetInstance();
        private Logger logger = Logger.GetInstance();

        private Label lblTitle;
        private Label lblSort;
        private ComboBox cmbSort;
        private DataGridView dgvTasks;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnComplete;
        private Label lblStatus;

        public MainForm()
        {
            BuildUI();
            RefreshTaskList();
            logger.Log("Main window opened.");
        }

        private void BuildUI()
        {
            this.Text = "To-Do List Application";
            this.Size = new Size(820, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += MainForm_Closing;

            lblTitle = new Label();
            lblTitle.Text = "TO-DO LIST";
            lblTitle.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;

            lblSort = new Label();
            lblSort.Text = "Sort by:";
            lblSort.Location = new Point(480, 22);
            lblSort.AutoSize = true;

            cmbSort = new ComboBox();
            cmbSort.Location = new Point(540, 18);
            cmbSort.Width = 240;
            cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSort.Items.Add("Default");
            cmbSort.Items.Add("By Date (Delegate)");
            cmbSort.Items.Add("By Priority (Delegate)");
            cmbSort.Items.Add("Only Overdue (LINQ)");
            cmbSort.SelectedIndex = 0;
            cmbSort.SelectedIndexChanged += CmbSort_Changed;

            BuildGrid();

            btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(20, 410);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(130, 410);
            btnEdit.Size = new Size(100, 30);
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(240, 410);
            btnDelete.Size = new Size(100, 30);
            btnDelete.Click += BtnDelete_Click;

            btnComplete = new Button();
            btnComplete.Text = "Complete / Undo";
            btnComplete.Location = new Point(350, 410);
            btnComplete.Size = new Size(140, 30);
            btnComplete.Click += BtnComplete_Click;

            lblStatus = new Label();
            lblStatus.Location = new Point(20, 450);
            lblStatus.Size = new Size(760, 20);
            lblStatus.Text = "";

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSort);
            this.Controls.Add(cmbSort);
            this.Controls.Add(dgvTasks);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnComplete);
            this.Controls.Add(lblStatus);
        }

        private void BuildGrid()
        {
            dgvTasks = new DataGridView();
            dgvTasks.Location = new Point(20, 55);
            dgvTasks.Size = new Size(760, 340);
            dgvTasks.ReadOnly = true;
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.AllowUserToResizeRows = false;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.MultiSelect = false;
            dgvTasks.RowHeadersVisible = false;
            dgvTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvTasks.CellDoubleClick += DgvTasks_DoubleClick;

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "colId";
            colId.HeaderText = "#";
            colId.Width = 40;

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.Name = "colName";
            colName.HeaderText = "Task Name";
            colName.Width = 360;
            // Task Name sutunu kalan bos alani doldurur (sagdaki bosluk kapanir)
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewTextBoxColumn colDate = new DataGridViewTextBoxColumn();
            colDate.Name = "colDate";
            colDate.HeaderText = "Due Date";
            colDate.Width = 140;

            DataGridViewTextBoxColumn colPriority = new DataGridViewTextBoxColumn();
            colPriority.Name = "colPriority";
            colPriority.HeaderText = "Priority";
            colPriority.Width = 90;

            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "colStatus";
            colStatus.HeaderText = "Status";
            colStatus.Width = 110;

            dgvTasks.Columns.Add(colId);
            dgvTasks.Columns.Add(colName);
            dgvTasks.Columns.Add(colDate);
            dgvTasks.Columns.Add(colPriority);
            dgvTasks.Columns.Add(colStatus);
        }

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            logger.Log("Application closed.");
        }

        private void CmbSort_Changed(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        private void DgvTasks_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                OpenEditForm();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            OpenAddForm();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            OpenEditForm();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
            CompleteSelected();
        }

        // Tabloyu siralama secimine gore yeniler.
        // Her gorev DECORATOR ile sarilarak metni olusturulur:
        //   - Tarihi gecmis ise OverdueTaskDecorator -> "[OVERDUE] ..."
        //   - Yuksek oncelikli + tamamlanmamis ise UrgentTaskDecorator -> "*** URGENT *** ..."
        // Tamamlanmis gorev satirlarinin uzeri cizgi ile gosterilir (Strikeout font).
        public void RefreshTaskList()
        {
            dgvTasks.Rows.Clear();

            List<TaskItem> tasksToShow = GetDisplayTasks();

            for (int i = 0; i < tasksToShow.Count; i++)
            {
                TaskItem item = tasksToShow[i];

                // Decorator zinciri - hocanin istedigi pattern burada calisiyor
                ITask shown = item;
                if (item.IsCompleted == false && item.DueDate < DateTime.Now)
                {
                    shown = new OverdueTaskDecorator(shown);
                }
                if (item.Priority == Priority.High && item.IsCompleted == false)
                {
                    shown = new UrgentTaskDecorator(shown);
                }

                string statusText;
                if (item.IsCompleted == true)
                {
                    statusText = "Completed";
                }
                else if (item.DueDate < DateTime.Now)
                {
                    statusText = "Overdue";
                }
                else
                {
                    statusText = "Pending";
                }

                string nameToShow = item.Name;
                if (shown is UrgentTaskDecorator)
                {
                    nameToShow = "*** URGENT *** " + item.Name;
                }
                else if (shown is OverdueTaskDecorator)
                {
                    nameToShow = "[OVERDUE] " + item.Name;
                }

                int rowIndex = dgvTasks.Rows.Add(
                    item.Id,
                    nameToShow,
                    item.DueDate.ToString("dd.MM.yyyy HH:mm"),
                    item.Priority.ToString(),
                    statusText
                );

                // Tamamlanmis gorevlerin uzerine cizgi (Strikeout)
                if (item.IsCompleted == true)
                {
                    DataGridViewRow row = dgvTasks.Rows[rowIndex];
                    row.DefaultCellStyle.Font = new Font(dgvTasks.Font, FontStyle.Strikeout);
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                }
            }

            UpdateStatusBar();
        }

        // Siralama secimine gore listeyi dondurur (Delegate + LINQ)
        private List<TaskItem> GetDisplayTasks()
        {
            List<TaskItem> all = manager.GetAllTasks();
            int secim = cmbSort.SelectedIndex;

            if (secim == 1)
            {
                // Delegate: tarihe gore sirala
                return manager.SortByDate(all);
            }
            else if (secim == 2)
            {
                // Delegate: oncelige gore sirala
                return manager.SortByPriority(all);
            }
            else if (secim == 3)
            {
                // LINQ: sadece tarihi gecmis gorevler
                return manager.GetOverdueTasks();
            }
            else
            {
                return all;
            }
        }

        private void UpdateStatusBar()
        {
            List<TaskItem> all = manager.GetAllTasks();
            int total = all.Count;

            int done = 0;
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].IsCompleted == true)
                {
                    done = done + 1;
                }
            }

            int overdue = manager.GetOverdueTasks().Count;
            int pending = total - done;

            lblStatus.Text = "Total: " + total + "   Pending: " + pending
                + "   Completed: " + done + "   Overdue: " + overdue;
        }

        // Secili gorevin Id'sini dondurur (-1 ise secim yok)
        private int GetSelectedId()
        {
            if (dgvTasks.CurrentRow == null)
            {
                return -1;
            }

            object val = dgvTasks.CurrentRow.Cells["colId"].Value;
            if (val == null)
            {
                return -1;
            }
            return (int)val;
        }

        private void OpenAddForm()
        {
            TaskForm form = new TaskForm();
            DialogResult result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                manager.AddTask(form.TaskName, form.DueDate, form.TaskPriority);
                RefreshTaskList();
            }
            form.Dispose();
        }

        private void OpenEditForm()
        {
            int id = GetSelectedId();
            if (id < 0)
            {
                MessageBox.Show("Please select a task.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TaskItem task = null;
            List<TaskItem> all = manager.GetAllTasks();
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].Id == id)
                {
                    task = all[i];
                    break;
                }
            }

            if (task == null)
            {
                return;
            }

            TaskForm form = new TaskForm(task);
            DialogResult result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                manager.EditTask(id, form.TaskName, form.DueDate, form.TaskPriority);
                RefreshTaskList();
            }
            form.Dispose();
        }

        private void DeleteSelected()
        {
            int id = GetSelectedId();
            if (id < 0)
            {
                MessageBox.Show("Please select a task.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Delete this task?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                manager.DeleteTask(id);
                RefreshTaskList();
            }
        }

        private void CompleteSelected()
        {
            int id = GetSelectedId();
            if (id < 0)
            {
                MessageBox.Show("Please select a task.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            manager.ToggleComplete(id);
            RefreshTaskList();
        }
    }
}
