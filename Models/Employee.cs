using System;

namespace Tz.Models
{
    public class Employee
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

        private string _ID_Department;
        public string ID_Department
        {
            get { return _ID_Department; }
            set 
            {
                _ID_Department = value.Trim();
            }
        }

        private byte _Is_Name_Black_List;

        public byte Is_Name_Black_List
        {
            get { return _Is_Name_Black_List; }
            set
            {
                if(value == 1)
                {
                    _Is_Name_Black_List = 1;
                    return;
                }

                _Is_Name_Black_List = 0;
            }
        }

        public Employee(
            string firstname, 
            string lastname,
            string patronymic,
            string department, 
            byte is_black_list = 0
            )
        {
            FirstName = firstname;
            Surname = lastname;
            Patronymic = patronymic;
            ID_Department = department;
            Is_Name_Black_List = is_black_list;
        }

        public Employee() { }
    }
}
