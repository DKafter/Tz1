using System;

namespace Tz.Models
{
    public class Department
    {
        private string _NameDepartment = string.Empty;

        public string NameDepartment
        {
            get
            {
                return _NameDepartment;
            }
            set
            {
                if (value.Length > 50) Console.WriteLine("ERROR");
                else _NameDepartment = value;
            }
        }
        private string _ShortNameDepartment = string.Empty;
        public string ShortNameDepartment
        {
            get { return _ShortNameDepartment; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! FirstName must be less than 51 characters!");
                else
                    _ShortNameDepartment = value;
            }
        }

        private string _ID = string.Empty;
        public string ID
        {
            get { return _ID; }
            set
            {
                if (value.Length > 9)
                    Console.WriteLine("Error! ID must be less than 10 characters!");
                else
                    _ID = value;
            }
        }

        public Department(string nameDepartment, string shortNameDepartment, string id)
        {
            NameDepartment = nameDepartment;
            ShortNameDepartment = shortNameDepartment;
            ID = id;
        }

        public Department() { }
    }
}
