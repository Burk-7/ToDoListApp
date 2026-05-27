using System;
using System.Drawing;
using System.Windows.Forms;

namespace ToDoListApp
{
    // Yeni gorev eklemek veya mevcut gorevi duzenlemek icin 
    public class TaskForm : Form
    {
        public string TaskName { get; private set; }
        public DateTime DueDate { get; private set; }
        public Priority TaskPriority { get; private set; }

        private TextBox txtName;
        private DateTimePicker dtpDate;
        private ComboBox cmbPriority;
        private Button btnSave;
        private Button btnCancel;

        // Yeni gorev modu
        public TaskForm()
        {
            this.TaskName = "";
            BuildUI("Add New Task", DateTime.Now.AddDays(1), Priority.Medium);
        }

        // Duzenleme modu
        public TaskForm(TaskItem task)
        {
            this.TaskName = "";
            BuildUI("Edit Task", task.DueDate, task.Priority);
            txtName.Text = task.Name;
        }

        private void BuildUI(string title, DateTime defaultDate, Priority defaultPriority)
        {
            this.Text = title;
            this.Size = new Size(400, 260);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblName = new Label();
            lblName.Text = "Task Name:";
            lblName.Location = new Point(20, 20);
            lblName.AutoSize = true;

            txtName = new TextBox();
            txtName.Location = new Point(20, 40);
            txtName.Width = 340;

            Label lblDate = new Label();
            lblDate.Text = "Due Date:";
            lblDate.Location = new Point(20, 75);
            lblDate.AutoSize = true;

            dtpDate = new DateTimePicker();
            dtpDate.Location = new Point(20, 95);
            dtpDate.Width = 340;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd.MM.yyyy   HH:mm";
            dtpDate.Value = defaultDate;

            Label lblPriority = new Label();
            lblPriority.Text = "Priority:";
            lblPriority.Location = new Point(20, 130);
            lblPriority.AutoSize = true;

            cmbPriority = new ComboBox();
            cmbPriority.Location = new Point(20, 150);
            cmbPriority.Width = 340;
            cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPriority.Items.Add("Low");
            cmbPriority.Items.Add("Medium");
            cmbPriority.Items.Add("High");
            cmbPriority.SelectedIndex = (int)defaultPriority - 1;

            btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.Location = new Point(200, 185);
            btnSave.Size = new Size(75, 28);
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(285, 185);
            btnCancel.Size = new Size(75, 28);
            btnCancel.DialogResult = DialogResult.Cancel;

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpDate);
            this.Controls.Add(lblPriority);
            this.Controls.Add(cmbPriority);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) == true)
            {
                MessageBox.Show("Task name cannot be empty.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            this.TaskName = txtName.Text.Trim();
            this.DueDate = dtpDate.Value;
            this.TaskPriority = (Priority)(cmbPriority.SelectedIndex + 1);
            this.DialogResult = DialogResult.OK;
        }
    }
}
