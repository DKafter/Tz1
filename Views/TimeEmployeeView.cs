using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tz.Controllers;
using Tz.Models;
using Tz.Utils;
using Tz.Utils.Interfaces;

namespace Tz.Views
{
    public partial class TimeEmployeeView : Form, ITimeEmployeeView
    {
        TimeEmployeeController tcnt = new TimeEmployeeController();
        TimeEmployee timeEmployee = new TimeEmployee();
        private bool _isReservation = false;
        private string _deleteName = null;
        private string _deleteSurname = null;
        private string _deletePatronymic = null;
        public TimeEmployeeView()
        {
            InitializeComponent();

            listView1.Columns.Add("№", 24);
            listView1.Columns.Add("Серийный номер флешки", 150);
            listView1.Columns.Add("Фамилия", 150);
            listView1.Columns.Add("Имя", 150);
            listView1.Columns.Add("Отчество", 150);
            listView1.Columns.Add("Дата начала использования флешки", 150);
            listView1.Columns.Add("Дата окончания использования флешки", 150);
            listView1.GridLines = true;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;

            FormUtils.WriteToDataGrid(comboBox3, tcnt.GetNamesFromDb);
            RefreshGrid();
        }
        public string ID_Department
        {
            get
            {
                return "0";
            }

            set
            {
                timeEmployee.ID_Department = value;
            }
        }

        public List<string> Names
        {
            get
            {
                if (!string.IsNullOrEmpty(comboBox3.Text))
                {
                    return comboBox3.Text.Split().ToList();
                }
                return new List<string>() { };
            }

            set
            {
                comboBox3.Text = string.Join(" ", value);
            }
        }

        public string SerialFlash
        {
            get
            {
                return comboBox2.Text;
            }

            set
            {
                comboBox2.Text = value;
                timeEmployee.SerialFlash = value;
            }
        }

        public string DateNow
        {
            get
            {
                return dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            set
            {
                dateTimePicker1.Value = Convert.ToDateTime(value);
                timeEmployee.DateNow = value;
            }
        }

        public string DateEnd
        {
            get
            {
                return dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            set
            {
                dateTimePicker2.Value = Convert.ToDateTime(value);
                timeEmployee.DateNow = value;
            }
        }

        public bool IsReservation
        {
            get
            {
                return _isReservation;
            }

            set
            {
                _isReservation = value;
            }
        }

        public string ID_Flash
        {
            get
            {
                return tcnt.GetIdFromSerial(SerialFlash);
            }
        }
        public string Name
        {
            get
            {
                if (Names.Count > 0)
                {
                    return Names[1];
                }
                return null;
            }

            set
            {
                if (Names.Count > 0)
                {
                    Names[1] = value;
                }
            }
        }
        public string Surname 
        { 
            get
            {
                if (Names.Count > 0)
                {
                    return Names[0];
                }
                return null;
            }

            set
            {
                if (Names.Count > 0)
                {
                    Names[0] = value;
                }
            }
        }
        public string Patronymic 
        { 
            get
            {
                if (Names.Count > 0)
                {
                    return Names[2];
                }
                return null;
            }

            set
            {
                if (Names.Count > 0)
                {
                    Names[0] = value;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            IsReservation = true;
            timeEmployee = new TimeEmployee
            {
                ID_Department = ID_Department,
                ID_Flash = ID_Flash,
                SerialFlash = SerialFlash,
                FirstName = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                DateNow = DateNow,
                DateEnd = DateEnd,
                IsReservation = IsReservation
            };
            tcnt.AddToDb(timeEmployee);
            tcnt.UpdateReservationFromSerialID(timeEmployee);
            RefreshGrid();
            comboBox3_TextChanged(sender, e);
        }

        public void AddIdToBox(Control lst)
        {
        }

        public void RefreshGrid()
        {
            List<List<string>> lst = new List<List<string>>();
            foreach (var l in tcnt.GetFromTimeDepartmentDb)
            {
                var _lst = new List<string>();
                foreach (var _l in l)
                {
                    _lst.AddRange(new[]
                    {
                        _l.SerialFlash,
                        _l.Surname,
                        _l.FirstName,
                        _l.Patronymic,
                        _l.DateNow,
                        _l.DateEnd
                    });
                }
                lst.Add(_lst);
            }
            FormUtils.ClearAllSubitem(listView1);
            FormUtils.WriteToDataGrid(listView1, lst);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IsReservation = false;
            timeEmployee = new TimeEmployee
            {
                FirstName = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                SerialFlash = SerialFlash,
                IsReservation = IsReservation
            };
            tcnt.DeleteFromDb(timeEmployee);
            tcnt.UpdateReservationFromSerialID(timeEmployee);
            RefreshGrid();
            comboBox3_TextChanged(sender, e);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            var s = ((ListView)sender);
            var hitTest = ((ListView)sender).HitTest(e.Location);
            var itemIndex = hitTest.Item.Index;
            SerialFlash = listView1.Items[itemIndex].SubItems[1].Text;
            Surname = listView1.Items[itemIndex].SubItems[2].Text;
            Name = listView1.Items[itemIndex].SubItems[3].Text;
            Patronymic = listView1.Items[itemIndex].SubItems[4].Text;
            _deleteSurname = listView1.Items[itemIndex].SubItems[2].Text;
            _deleteName = listView1.Items[itemIndex].SubItems[3].Text;
            _deletePatronymic = listView1.Items[itemIndex].SubItems[4].Text;
            DateNow = listView1.Items[itemIndex].SubItems[5].Text;
            DateEnd = listView1.Items[itemIndex].SubItems[6].Text;
            var fullName = new List<string>
            {
                Surname,
                Name,
                Patronymic
            };
            Names = fullName;
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            IsReservation = !_isReservation;
            timeEmployee = new TimeEmployee
            {
                FirstName = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                SerialFlash = SerialFlash,
                IsReservation = IsReservation
            };
            comboBox2.Items.Clear();
            FormUtils.WriteToDataGrid(comboBox2, tcnt.GetSerialFlashDb(timeEmployee));
        }

        private void TimeEmployeeView_Resize(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            timeEmployee = new TimeEmployee
            {
                FirstName = _deleteName,
                Surname = _deleteSurname,
                Patronymic = _deletePatronymic,
                SerialFlash = SerialFlash,
                IsReservation = IsReservation
            };
            timeEmployee.ID_Flash = tcnt.GetIdFromSerial(SerialFlash);
            timeEmployee.IsReservation = false;
            tcnt.DeleteFromDb(timeEmployee);
            comboBox3_TextChanged(sender, e);
            RefreshGrid();
        }
    }
}
