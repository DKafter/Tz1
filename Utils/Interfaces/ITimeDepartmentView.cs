using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tz.Models;

namespace Tz.Utils.Interfaces
{
    public interface ITimeDepartmentView
    {
        void RefreshGrid();
        string ID_Department { get; set; }
        string ID_Flash { get; }
        string SerialFlash { get; set; }
        bool IsReservation { get; set; }
        string DateNow { get; set; }
        string DateEnd { get; }
    }
}
