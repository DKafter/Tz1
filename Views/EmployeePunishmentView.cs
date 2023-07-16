using System.Windows.Forms;
using Tz.Controllers;
using Tz.Utils;
using Tz.Utils.Interfaces;

namespace Tz.Views
{
    public partial class EmployeePunishmentView : Form, IPunishmentView
    {
        PunishmentController punishment = new PunishmentController();
        public EmployeePunishmentView()
        {
            InitializeComponent();
            listView1.Columns.Add("№", 150);
            listView1.Columns.Add("Фамилия", 150);
            listView1.Columns.Add("Имя", 150);
            listView1.Columns.Add("Отчество", 150);
            listView1.Columns.Add("Телефон", 150);
            listView1.Columns.Add("Серийный номер флешки", 150); 
            listView1.FullRowSelect = true;


            RefreshGrid();
        }

        public void RefreshGrid()
        {
            FormUtils.ClearAllSubitem(listView1);
            FormUtils.WriteToDataGrid(listView1, punishment.GetFromEmployeePunishment);
        }

        private void EmployeePunishmentView_Resize(object sender, System.EventArgs e)
        {

        }
    }
}
