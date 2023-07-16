using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tz.Models
{
    public class TimeEmployee: TimeDepartment
    {
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! FirstName must be less than 51 characters!");
                _FirstName = value;
            }
        }

        private string _Patronymic;
        public string Patronymic
        {
            get { return _Patronymic; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! FirstName must be less than 51 characters!");
                _Patronymic = value;
            }
        }

        private string _ID_Employee;
        public string ID_Employee
        {
            get { return _ID_Employee; }
            set
            {
                _ID_Employee = value;
            }
        }

        private string _Surname;
        public string Surname
        {
            get { return _Surname; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! LastName must be less than 51 characters!");
                _Surname = value;
            }
        }

        public TimeEmployee(string name, string surname, string patronymic, 
            string dateNow, string dateEnd, string serialFlash, bool isReservation = false)
        {
            FirstName = name;
            Surname = surname;
            Patronymic = patronymic;
            DateNow = dateNow;
            DateEnd = dateEnd;
            SerialFlash = serialFlash;
            IsReservation = isReservation;
        }

        public TimeEmployee() { }
    }
}
