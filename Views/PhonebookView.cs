using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tz.Controllers;
using Tz.Models;
using Tz.Utils;
using Tz.Utils.Interfaces;

namespace Tz.Views
{
    public partial class PhonebookView : Form, IPhonebookView
    {
        public PhonebookView()
        {
            InitializeComponent();

            listView1.Columns.Add("№", 24);
            listView1.Columns.Add("Фамилия", 150);
            listView1.Columns.Add("Имя", 150);
            listView1.Columns.Add("Отчество", 150);
            listView1.Columns.Add("Телефон", 150);
            listView1.GridLines = true;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;

            FormUtils.WriteToDataGrid(comboBox1, pnct.GetNames);
            RefreshGrid();
        }

        PhonebookController pnct = new PhonebookController();
        Phonebook phonebook = new Phonebook();

        public List<string> Names
        {
            get
            {
                return comboBox1.Text.Split().ToList();
            }
        }
        public string Surname
        {
            get
            {
                return Names[0];
            }

            set
            {
                Names[0] = value;
                phonebook.Surname = value;
            }
        }
        public string FirstName
        {
            get
            {
                return Names[1];
            }

            set
            {
                Names[1] = value;
                phonebook.FirstName = value;
            }
        }

        public string Patronymic
        {
            get
            {
                return Names[2];
            }

            set
            {
                Names[2] = value;
                phonebook.Patronymic = value;
            }
        }

        public string Phone
        {
            get
            {
                return textBox4.Text;
            }

            set
            {
                textBox4.Text = value;
                phonebook.Phone = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            phonebook = new Phonebook
            {
                Surname = Surname,
                FirstName = FirstName,
                Patronymic = Patronymic,
                Phone = Phone
            };

            phonebook.ID_Employee = pnct.GetIdFromNames(phonebook);

            pnct.AddToDb(phonebook);
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            List<List<string>> lst = new List<List<string>>();
            foreach (var l in pnct.GetFromPhonebookDb)
            {
                var _lst = new List<string>();
                foreach (var _l in l)
                {
                    _lst.AddRange(new[] { _l.Surname, _l.FirstName, _l.Patronymic, _l.Phone });
                }
                lst.Add(_lst);
            }

            FormUtils.ClearAllSubitem(listView1);
            FormUtils.WriteToDataGrid(listView1, lst);
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            var s = ((ListView)sender);
            var hitTest = ((ListView)sender).HitTest(e.Location);
            var itemIndex = hitTest.Item.Index;
            Surname = listView1.Items[itemIndex].SubItems[1].Text;
            FirstName = listView1.Items[itemIndex].SubItems[2].Text;
            Patronymic = listView1.Items[itemIndex].SubItems[3].Text;
            Phone = listView1.Items[itemIndex].SubItems[4].Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pnct.DeleteFromDb(phonebook);
            RefreshGrid();
        }

        private void Form_Resize(object sender, EventArgs e)
        {

        }
    }
}
