using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Tz.Views;

namespace Tz
{
    public partial class Form1 : Form
    {
        static object _object = new object();
        private int iFormX, iFormY, iMouseX, iMouseY;
        private bool isDragging = false;
        private Point oldPos;
        private void MyForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.isDragging = true;
            this.oldPos = new Point();
            this.oldPos.X = e.X;
            this.oldPos.Y = e.Y;
        }

        private void MyForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging)
            {
                Point tmp = new Point(this.Location.X, this.Location.Y);
                tmp.X += e.X - this.oldPos.X;
                tmp.Y += e.Y - this.oldPos.Y;
                this.Location = tmp;
            }
        }

        private void MyForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.isDragging = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            iFormX = this.Location.X;
            iFormY = this.Location.Y;
            iMouseX = MousePosition.X;
            iMouseY = MousePosition.Y;

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int iMouseX2 = MousePosition.X;
            int iMouseY2 = MousePosition.Y;
            if (e.Button == MouseButtons.Left)
                this.Location = new Point(iFormX + (iMouseX2 - iMouseX), iFormY + (iMouseY2 - iMouseY));

        }
        public Form1()
        {
            lock (_object)
            {
                InitializeComponent();
                this.MouseDown += new MouseEventHandler(MyForm_MouseDown);
                this.MouseMove += new MouseEventHandler(MyForm_MouseMove);
                this.MouseUp += new MouseEventHandler(MyForm_MouseUp);
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
                panel2.AutoScroll = true;
                panel2.AutoSize = true;
                panel2.Size = new Size(Width, Height);
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void shrink_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lock (_object)
            {
                panel2.Controls.Clear();
                DepartmentView objForm = new DepartmentView();
                objForm.TopLevel = false;
                panel2.Controls.Add(objForm);
                objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                objForm.Dock = DockStyle.Fill;
                objForm.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lock (_object)
            {
                panel2.Controls.Clear();
                TimeDepartmentView objForm = new TimeDepartmentView();
                objForm.TopLevel = false;
                panel2.Controls.Add(objForm);
                objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                objForm.Dock = DockStyle.Fill;
                objForm.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lock (_object)
            {
                panel2.Controls.Clear();
                TimeEmployeeView objForm = new TimeEmployeeView();
                objForm.TopLevel = false;
                panel2.Controls.Add(objForm);
                objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                objForm.Dock = DockStyle.Fill;
                objForm.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lock (_object)
            {
                panel2.Controls.Clear();
                PhonebookView objForm = new PhonebookView();
                objForm.TopLevel = false;
                panel2.Controls.Add(objForm);
                objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                objForm.Dock = DockStyle.Fill;
                objForm.Show();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Height = Height;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Size = new Size(this.PointToClient(MousePosition).X, this.PointToClient(MousePosition).Y);
                panel2.Size = new Size(this.PointToClient(MousePosition).X, this.PointToClient(MousePosition).Y);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lock (_object)
            {
                panel2.Controls.Clear();
                FlashView objForm = new FlashView();
                objForm.TopLevel = false;
                panel2.Controls.Add(objForm);
                objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                objForm.Dock = DockStyle.Fill;
                objForm.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lock (_object)
            {
                panel2.Controls.Clear();
                EmployeeView objForm = new EmployeeView();
                objForm.TopLevel = false;
                panel2.Controls.Add(objForm);
                objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                objForm.Dock = DockStyle.Fill;
                objForm.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lock (_object)
            {
                panel2.Controls.Clear();
                EmployeePunishmentView objForm = new EmployeePunishmentView();
                objForm.TopLevel = false;
                panel2.Controls.Add(objForm);
                objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                objForm.Dock = DockStyle.Fill;
                objForm.Show();
            }
        }
    }
}
