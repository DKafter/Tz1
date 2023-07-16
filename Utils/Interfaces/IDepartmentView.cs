using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tz.Models;

namespace Tz.Utils.Interfaces
{
    public interface IDepartmentView
    {
        void RefreshGrid();
        void AddDepartmentToDb(Department dp);
        string ID { get; set; }
        string NameDepartment { get; set; }
        string ShortNameDepartment { get; set; }
    }
}
