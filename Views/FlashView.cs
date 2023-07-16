using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tz.Controllers;
using Tz.Models;
using Tz.Utils;
using Tz.Utils.Interfaces;

namespace Tz.Views
{
    public partial class FlashView : Form, IFlashView
    {
        FlashController fct = new FlashController();
        Flash flash = new Flash();
        public FlashView()
        {
            InitializeComponent();

            listView1.Columns.Add("№", 24);
            listView1.Columns.Add("Название компании", 150);
            listView1.Columns.Add("Дата эксплуатации", 150);
            listView1.Columns.Add("Серийный номер", 150);
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.View = View.Details;

            RefreshGrid();
        }
        public string SerialNumber
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
                flash.SerialNumber = value;
            }
        }
        public string NameCompany
        {
            get
            {
                return textBox2.Text;
            }
            
            set
            {
                textBox2.Text = value;
                flash.NameCompany = value;
            }
        }
        public string DateCreate
        {
            get
            {
                return dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            set
            {
                dateTimePicker1.Value = Convert.ToDateTime(value);
                flash.DateCreate = value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            flash = new Flash(SerialNumber, NameCompany, DateCreate);
            AddFlashToDb(flash);
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            List<List<string>> lst = new List<List<string>>();
            foreach (var l in fct.GetFromFlashDb)
            {
                var _lst = new List<string>();
                foreach (var _l in l)
                {
                    _lst.AddRange(new[] { _l.NameCompany, _l.DateCreate, _l.SerialNumber });
                }
                lst.Add(_lst);
            }

            FormUtils.ClearAllSubitem(listView1);
            FormUtils.WriteToDataGrid(listView1, lst);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!FormUtils.EmptyRedWhite(this)) return;
            fct.DeleteFromDb(flash);
            RefreshGrid();
        }

        public void AddFlashToDb(Flash dp)
        {
            fct.AddToDb(dp);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            var s = ((ListView)sender);
            var hitTest = ((ListView)sender).HitTest(e.Location);
            var itemIndex = hitTest.Item.Index;
            NameCompany = listView1.Items[itemIndex].SubItems[1].Text;
            SerialNumber = listView1.Items[itemIndex].SubItems[3].Text;
            DateCreate = listView1.Items[itemIndex].SubItems[2].Text;
        }

        private void FlashView_Resize(object sender, EventArgs e)
        {

        }
    }
}
