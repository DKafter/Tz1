using System;

namespace Tz.Models
{
    public class Flash
    {
        private string _SerialNumber;
        public string SerialNumber
        {
            get { return _SerialNumber; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! FirstName must be less than 51 characters!");
                else
                    _SerialNumber = value;
            }
        }

        private string _NameCompany;
        public string NameCompany
        {
            get { return _NameCompany; }
            set
            {
                if (value.Length > 50)
                    Console.WriteLine("Error! FirstName must be less than 51 characters!");
                else
                    _NameCompany = value;
            }
        }

        private string _DateCreate;
        public string DateCreate
        {
            get { return _DateCreate; }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    _DateCreate = value;
                }
            }
        }

        public Flash( 
            string serialNumber, 
            string nameCompany,
            string dateCreate)
        {
            SerialNumber = serialNumber;
            NameCompany = nameCompany;
            DateCreate = dateCreate;
        }

        public Flash() { }
    }
}
