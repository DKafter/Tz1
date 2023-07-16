﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tz.Utils.Interfaces
{
    public interface ITimeEmployeeView: ITimeDepartmentView
    { 
        string Name { get; set; }
        string Surname { get; set; }
        string Patronymic { get; set; }
    }
}
