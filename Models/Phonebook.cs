using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tz.Models
{
    public class Phonebook
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

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! LastName must be less than 51 characters!");
                _Phone = value;
            }
        }

        private string _ID_Employee;
        public string ID_Employee
        {
            get { return _ID_Employee; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! LastName must be less than 51 characters!");
                _ID_Employee = value;
            }
        }

        public Phonebook(string idEmployee, string surname, string firstname, string patronymic, string phone)
        {
            ID_Employee = idEmployee;
            Surname = surname;
            FirstName = firstname;
            Patronymic = patronymic;
            Phone = phone;
        }

        public Phonebook() { }
    }
}
