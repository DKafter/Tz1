using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tz.Controllers;
using Tz.Models;
using Tz.Utils;
using Tz.Utils.Interfaces;

namespace Tz.Views
{
    public partial class DepartmentView : Form, IDepartmentView
    {
        DepartmentController dp = new DepartmentController();
        Department department = new Department();
        public string ID
        {
            get
            {
                return textBox3.Text;
            }

            set
            {
                textBox3.Text = value;
                department.ID = value;
            }
        }

        public string NameDepartment
        {
            get
            {
                return textBox2.Text;
            }

            set
            {
                textBox2.Text = value;
                department.NameDepartment = value;
            }
        }

        public string ShortNameDepartment
        {
            get
            {
                return textBox1.Text;
            }

            set
            {
                textBox1.Text = value;
                department.ShortNameDepartment = value;
            }
        }

        public DepartmentView()
        {
            InitializeComponent();
            listView1.Columns.Add("№", 24);
            listView1.Columns.Add("ID_Подразделения", 150);
            listView1.Columns.Add("Краткое наименование", 150);
            listView1.Columns.Add("Полное наименование", 150);
            listView1.FullRowSelect = true;
            Console.WriteLine($"size {listView1.Width - Width}");
            RefreshGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            department = new Department
            {
                NameDepartment = NameDepartment,
                ShortNameDepartment = ShortNameDepartment,
                ID = ID
            };
            AddDepartmentToDb(department);
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            List<List<string>> lst = new List<List<string>>();
            foreach(var l in dp.GetFromDepartmentDb)
            {
                var _lst = new List<string>();
                foreach(var _l in l)
                {
                    _lst.AddRange(new[] { _l.ID, _l.ShortNameDepartment, _l.NameDepartment });
                }
                lst.Add(_lst);
            }

            FormUtils.ClearAllSubitem(listView1);
            FormUtils.WriteToDataGrid(listView1, lst);
        }

        public void AddDepartmentToDb(Department dp)
        {
            this.dp.AddToDb(dp);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            dp.DeleteFromDb(department);
            RefreshGrid();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            var s = ((ListView)sender);
            var hitTest = ((ListView)sender).HitTest(e.Location);
            var itemIndex = hitTest.Item.Index;
            ShortNameDepartment = listView1.Items[itemIndex].SubItems[2].Text;
            NameDepartment = listView1.Items[itemIndex].SubItems[3].Text;
            ID = listView1.Items[itemIndex].SubItems[1].Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            dp.ChangeToDb(department, "name_department", ShortNameDepartment);
            dp.ChangeToDb(department, "full_name_department", NameDepartment);
            RefreshGrid();
        }

        private void DepartmentView_Resize(object sender, EventArgs e)
        {

        }
    }
}
