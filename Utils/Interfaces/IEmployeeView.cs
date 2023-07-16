using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tz.Controllers;
using Tz.Models;

namespace Tz.Utils.Interfaces
{
    public interface IEmployeeView
    {
        void RefreshGrid();
        string FirstName { get; set; }
        string Surname { get; set; }
        string Patronymic { get; set; }
        string ID_Department { get; set; }
        byte Is_Black_List { get; }
    }
}
