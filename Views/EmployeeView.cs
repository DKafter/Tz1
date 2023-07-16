using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tz.Controllers;
using Tz.Models;
using Tz.Utils;
using Tz.Utils.Interfaces;

namespace Tz.Views
{
    public partial class EmployeeView : Form, IEmployeeView
    {
        EmployeeController empct = new EmployeeController();
        Employee employee = new Employee();

        string _oldIDDepartment = null;
        string _oldSurname = null;
        string _oldName = null;
        string _oldPatronymic = null;
        public EmployeeView()
        {
            InitializeComponent();

            listView1.Columns.Add("№", 24);
            listView1.Columns.Add("Номер подразделения", 150);
            listView1.Columns.Add("Фамилия", 150);
            listView1.Columns.Add("Имя", 150);
            listView1.Columns.Add("Отчество", 150);
            listView1.GridLines = true;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;

            AddIdToBox(this);
            RefreshGrid();
        }

        public string FirstName
        {
            get 
            {
                return textBox2.Text;
            }
            set
            {
                textBox2.Text = value;
                employee.FirstName = value;
            }
        }
        public string Surname
        {
            get 
            { 
                return textBox1.Text; 
            }
            set
            {
                textBox1.Text = value;
                employee.Surname = value;
            }
        }
        public string Patronymic
        {
            get
            {
                return textBox3.Text;
            }
            set
            {
                textBox3.Text = value;
                employee.Patronymic = value;
            }
        }
        public string ID_Department
        {
            get
            {
                return comboBox1.Text;
            }
            set
            {
                comboBox1.Text = value;
                employee.ID_Department = value;
            }
        }

        public byte Is_Black_List
        {
            get
            {
                return 0;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            employee = new Employee(
                    FirstName,
                    Surname,
                    Patronymic,
                    ID_Department,
                    Is_Black_List
                );
            empct.AddToDb(employee);
            RefreshGrid();
        }
        public void RefreshGrid()
        {
            List<List<string>> lst = new List<List<string>>();
            foreach (var l in empct.GetFromEmployeeDb)
            {
                var _lst = new List<string>();
                foreach (var _l in l)
                {
                    _lst.AddRange(new[]
                    {
                        _l.ID_Department,
                        _l.Surname,
                        _l.FirstName,
                        _l.Patronymic
                    });
                }
                lst.Add(_lst);
            }
            FormUtils.ClearAllSubitem(listView1);
            FormUtils.WriteToDataGrid(listView1, lst);
        }

        public void AddIdToBox(Control lst)
        {
            if (!FormUtils.WriteToDataGrid(comboBox1, empct.GetIdDepartmentFromDb))
            {
                if (string.IsNullOrEmpty(ID_Department.Trim()))
                {
                    foreach (Control c in FormUtils.GetAll(lst))
                    {
                        if (c.GetType() != typeof(ListView))
                        {
                            c.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (Control c in FormUtils.GetAll(lst))
                {
                    c.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            empct.DeleteFromDb(employee);
            RefreshGrid();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            var s = ((ListView)sender);
            var hitTest = ((ListView)sender).HitTest(e.Location);
            var itemIndex = hitTest.Item.Index;
            Surname = _oldSurname = listView1.Items[itemIndex].SubItems[2].Text;
            FirstName = _oldName = listView1.Items[itemIndex].SubItems[3].Text;
            Patronymic = _oldPatronymic = listView1.Items[itemIndex].SubItems[4].Text;
            ID_Department = _oldIDDepartment = listView1.Items[itemIndex].SubItems[1].Text;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            employee = new Employee(_oldName, _oldSurname, _oldPatronymic, _oldIDDepartment, Is_Black_List);

            if (!FormUtils.EmptyRedWhite(this)) return;
            empct.ChangeToDb(employee, "id_department", ID_Department);
            empct.ChangeToDb(employee, "name", FirstName);
            empct.ChangeToDb(employee, "surname", Surname);
            empct.ChangeToDb(employee, "patronymic", Patronymic);
            empct.ChangeToDb(employee, "ISNAMEBLACKLIST", Is_Black_List.ToString());
            RefreshGrid();
        }

        private void EmployeeView_Resize(object sender, EventArgs e)
        {

        }
    }
}
