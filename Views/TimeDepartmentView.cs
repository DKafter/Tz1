using Google.Protobuf.WellKnownTypes;
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
    public partial class TimeDepartmentView : Form, ITimeDepartmentView
    {
        TimeDepartmentController tcnt = new TimeDepartmentController();
        TimeDepartment timeDepartment = new TimeDepartment();
        private bool _isReservation = false;
        public string ID_Department
        {
            get
            {
                return comboBox1.Text;
            }

            set
            {
                comboBox1.Text = value;
                timeDepartment.ID_Department = value;
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
                timeDepartment.SerialFlash = value;
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
                timeDepartment.DateNow = value;
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
                timeDepartment.DateNow = value;
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

        public TimeDepartmentView()
        {
            InitializeComponent();

            listView1.Columns.Add("№", 24);
            listView1.Columns.Add("Номер подразделения", 150);
            listView1.Columns.Add("Серийный номер флешки", 150);
            listView1.Columns.Add("Дата начала использования флешки", 150);
            listView1.Columns.Add("Дата окончания использования флешки", 150);
            listView1.GridLines = true;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;

            AddIdToBox(this);
            RefreshGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            IsReservation = !_isReservation;
            timeDepartment = new TimeDepartment
            {
                ID_Department = ID_Department,
                ID_Flash = ID_Flash,
                SerialFlash = SerialFlash,
                DateNow = DateNow,
                DateEnd = DateEnd,
                IsReservation = IsReservation
            };
            AddIdToBox(this);
            tcnt.AddToDb(timeDepartment);
            tcnt.UpdateReservationFromSerialID(timeDepartment);;
            RefreshGrid();
        }

        public void AddIdToBox(Control lst)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            if (!FormUtils.WriteToDataGrid(comboBox1, tcnt.GetIdDepartmentFromDb) ||
                !FormUtils.WriteToDataGrid(comboBox2, tcnt.GetSerialFlashDb))
            {
                if (string.IsNullOrEmpty(ID_Department.Trim()) || string.IsNullOrEmpty(SerialFlash.Trim()))
                {
                    foreach (Control c in FormUtils.GetAll(lst))
                    {
                        if (c.GetType() != typeof(ListView) || c.Name != "button3")
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
            RefreshGrid();
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
                        _l.ID_Department,
                        _l.SerialFlash,
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
            if (!FormUtils.EmptyRedWhite(this)) return;
            timeDepartment = new TimeDepartment
            {
                ID_Department = ID_Department,
                ID_Flash = ID_Flash,
                SerialFlash = SerialFlash,
                DateNow = DateNow,
                DateEnd = DateEnd,
                IsReservation = IsReservation
            };
            AddIdToBox(this);
            tcnt.DeleteFromDb(timeDepartment);
            RefreshGrid();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            var s = ((ListView)sender);
            var hitTest = ((ListView)sender).HitTest(e.Location);
            var itemIndex = hitTest.Item.Index;
            ID_Department = listView1.Items[itemIndex].SubItems[1].Text;
            SerialFlash = listView1.Items[itemIndex].SubItems[2].Text;
            DateNow = listView1.Items[itemIndex].SubItems[3].Text;
            DateEnd = listView1.Items[itemIndex].SubItems[4].Text;
        }

        private void TimeDepartmentView_Resize(object sender, EventArgs e)
        {

        }
    }
}
