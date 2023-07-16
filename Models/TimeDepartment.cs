using System;

namespace Tz.Models
{
    public class TimeDepartment
    {
        private string _ID_Department;
        public string ID_Department
        {
            get { return _ID_Department; }
            set
            {
                _ID_Department = value.Trim();
            }
        }

        private string _ID_Flash;
        public string ID_Flash
        {
            get { return _ID_Flash; }
            set
            {
                _ID_Flash = value.Trim();
            }
        }

        private string _SerialFlash;
        public string SerialFlash
        {
            get { return _SerialFlash; }
            set
            {
                _SerialFlash = value.Trim();
            }
        }

        private string _DateNow;
        public string DateNow
        {
            get { return _DateNow; }
            set
            {
                _DateNow = value;
            }
        }

        private string _DateEnd;
        public string DateEnd
        {
            get { return _DateEnd; }
            set
            {
                _DateEnd = value;
            }
        }

        private bool _IsReservation;
        public bool IsReservation
        {
            get { return _IsReservation; }
            set
            {
                _IsReservation = value;
            }
        }

        public TimeDepartment(string id_department, string id_flash, string dateNow, string dateEnd, string serialFlash, bool isReservation = false)
        {
            ID_Department = id_department;
            ID_Flash = id_flash;
            DateNow = dateNow;
            DateEnd = dateEnd;
            SerialFlash = serialFlash;
            IsReservation = isReservation;
        }

        public TimeDepartment() { }
    }
}