using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tz.Models;
using Tz.Utils;

namespace Tz.Controllers
{
    public class TimeEmployeeController
    {
        private List<List<TimeEmployee>> _TimeEmployeeDb;
        private ArrayList _IdDepartments;
        private ArrayList _SerialFlash;
        private ArrayList _Names;
        private string _selectedIndexIDDepartment = string.Empty;
        private string _selectedSerialFlash = string.Empty;
        private string _selectedIndexIDEmployee = string.Empty;
        public TimeEmployeeController()
        {
            _IdDepartments = new ArrayList();
            _SerialFlash = new ArrayList();
            _TimeEmployeeDb = new List<List<TimeEmployee>>();
            _Names = new ArrayList();
        }

        public List<List<TimeEmployee>> GetFromTimeDepartmentDb
        {
            get
            {
                _TimeEmployeeDb = new List<List<TimeEmployee>>();
                String ID_Department = null;
                String ID_Flash = null;
                String Date_Now = null;
                String Date_End = null;
                String SerialNumber = null;
                String Name = null;
                String Patronymic = null;
                String Surname = null;

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT t.id_department, id_flash, serial_number, date_now, date_end, e.surname, e.name, e.patronymic FROM takedateemployees as t INNER JOIN employee e ON e.id_department = t.id_department AND e.id_employee = t.id_employee;";
                    var cmd = new MySqlCommand(command, dbConnection);
                    cmd.CommandType = CommandType.Text;
                    dbConnection.Open();
                    using (var cmdDb = cmd.ExecuteReader())
                    {
                        while (cmdDb.Read())
                        {
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_department")))
                            {
                                ID_Department = cmdDb.GetInt32("id_department").ToString();
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("date_now")))
                            {
                                Date_Now = cmdDb.GetDateTime("date_now").ToString();
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("date_end")))
                            {
                                Date_End = cmdDb.GetDateTime("date_end").ToString();
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_flash")))
                            {
                                ID_Flash = cmdDb.GetString("id_flash");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("serial_number")))
                            {
                                SerialNumber = cmdDb.GetString("serial_number");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("name")))
                            {
                                Name = cmdDb.GetString("name");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("patronymic")))
                            {
                                Patronymic = cmdDb.GetString("patronymic");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("surname")))
                            {
                                Surname = cmdDb.GetString("surname");
                            }
                            var lst = new List<TimeEmployee>
                                        {
                                            new TimeEmployee(
                                            Name,
                                            Surname,
                                            Patronymic,
                                            Date_Now,
                                            Date_End,
                                            SerialNumber)
                                        };
                             _TimeEmployeeDb.Add(lst);

                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _TimeEmployeeDb;
            }
        }

        public string GetIdFromSerial(string serial)
        {
            string idFlash = null;
            string command = $"SELECT id_flash FROM TakeDateDepartments WHERE serial_number = '{serial}';";
            using (var dbConnection = DBUtils.GetDBConnection())
            {
                var cmd = new MySqlCommand(command, dbConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = dbConnection;
                dbConnection.Open();
                using (var cmdDb = cmd.ExecuteReader())
                {
                    while (cmdDb.Read())
                    {
                        if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_flash")))
                        {
                            idFlash = cmdDb.GetString("id_flash");
                        }
                    }
                    cmdDb.Close();
                }
                dbConnection.Close();
            }
            return idFlash;
        }

        public IList GetIdDepartmentFromDb
        {
            get
            {
                string ID_Department = null;
                string command = "SELECT id_department FROM TakeDateDepartments";
                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    var cmd = new MySqlCommand(command, dbConnection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = dbConnection;
                    dbConnection.Open();
                    using (var cmdDb = cmd.ExecuteReader())
                    {
                        while (cmdDb.Read())
                        {
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_department")))
                            {
                                ID_Department = cmdDb.GetInt32("id_department").ToString();
                            }

                            var lst = new List<String> { ID_Department };
                            _IdDepartments.AddRange(lst);
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _IdDepartments;
            }
        }

        public bool GetIsReservationFromSerial(string serial)
        {
            bool isReservation = true;
            if (string.IsNullOrEmpty(serial)) return isReservation;
            string command = $"SELECT is_reservation FROM TakeDateDepartments WHERE serial_number = {serial}";
            using (var dbConnection = DBUtils.GetDBConnection())
            {
                var cmd = new MySqlCommand(command, dbConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = dbConnection;
                dbConnection.Open();
                using (var cmdDb = cmd.ExecuteReader())
                {
                    while (cmdDb.Read())
                    {
                        if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("is_reservation")))
                        {
                            isReservation = cmdDb.GetBoolean("is_reservation");
                        }
                    }
                    cmdDb.Close();
                }
                dbConnection.Close();
            }
            return isReservation;
        }

        public IList GetSerialFlashDb(TimeEmployee tmp)
        {
            _SerialFlash = new ArrayList();
            String SerialFlash = null;
            Boolean IsReservation = true;

            string command = $"SELECT serial_number, e.id_employee, is_reservation, surname, name, patronymic, t.id_department FROM TakeDateDepartments as t INNER JOIN employee e ON e.name = '{tmp.FirstName}' AND e.surname = '{tmp.Surname}' AND e.patronymic = '{tmp.Patronymic}' AND e.id_department = t.id_department WHERE is_reservation != 1;";
            using (var dbConnection = DBUtils.GetDBConnection())
            {
                var cmd = new MySqlCommand(command, dbConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = dbConnection;
                dbConnection.Open();
                using (var cmdDb = cmd.ExecuteReader())
                {
                    while (cmdDb.Read())
                    {
                        if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("is_reservation")))
                        {
                            IsReservation = cmdDb.GetBoolean("is_reservation");
                        }
                        if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("serial_number")) && !IsReservation)
                        {
                            _selectedSerialFlash = SerialFlash = cmdDb.GetString("serial_number");
                        }
                        if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_department")) && !IsReservation)
                        {
                            _selectedIndexIDDepartment = cmdDb.GetInt32("id_department").ToString();
                        }
                        if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_employee")) && !IsReservation)
                        {
                            _selectedIndexIDEmployee = cmdDb.GetString("id_employee");
                        }

                        var lst = new List<String> { SerialFlash };
                        _SerialFlash.AddRange(lst);

                    }
                    cmdDb.Close();
                }
                dbConnection.Close();
            }
            return _SerialFlash;
        }

        public IList GetNamesFromDb
        {
            get
            {
                String Name = null;
                String Surname = null;
                String Patronymic = null;
                _Names = new ArrayList();
                string command = "SELECT DISTINCT e.surname, e.name, e.patronymic FROM Employee e INNER JOIN takedatedepartments t ON t.id_department = e.id_department;";
                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    var cmd = new MySqlCommand(command, dbConnection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = dbConnection;
                    dbConnection.Open();
                    using (var cmdDb = cmd.ExecuteReader())
                    {
                        while (cmdDb.Read())
                        {
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("name")))
                            {
                                Name = cmdDb.GetString("name");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("surname")))
                            {
                                Surname = cmdDb.GetString("surname");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("patronymic")))
                            {
                                Patronymic = cmdDb.GetString("patronymic");
                            }
                            StringBuilder fullName = new StringBuilder();
                            fullName.Append($"{Surname} {Name} {Patronymic}");
                            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Surname) && !string.IsNullOrEmpty(Patronymic))
                            {
                                var lst = new List<String> { fullName.ToString() };
                                _Names.AddRange(lst);
                            }
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _Names;
            }
        }

        public void AddToDb(TimeEmployee tmp)
        {
            string command = "INSERT INTO takedateemployees(id_department, id_employee, id_flash, serial_number, name, surname, patronymic, date_now, date_end) " +
                $"VALUES (@id_department, @id_employee, @id_flash, @serial_number, @name, @surname, @patronymic, @date_now, @date_end);";
            tmp.ID_Department = _selectedIndexIDDepartment;
            tmp.ID_Employee = _selectedIndexIDEmployee;
            var isHasID = _TimeEmployeeDb.Any(t => t.Contains(tmp));
            if (!isHasID)
            {
                DBUtils.ExecuteToDb(command,
                    new string[]
                    {
                    "id_department",
                    "id_employee",
                    "id_flash",
                    "serial_number",
                    "name",
                    "surname",
                    "patronymic",
                    "date_now",
                    "date_end"
                    },
                    new string[]
                    {
                    tmp.ID_Department,
                    tmp.ID_Employee,
                    tmp.ID_Flash,
                    tmp.SerialFlash,
                    tmp.FirstName,
                    tmp.Surname,
                    tmp.Patronymic,
                    tmp.DateNow,
                    tmp.DateEnd
                    });
            }
        }

        public void UpdateReservationFromSerialID(TimeEmployee tmp)
        {
            string command = $"UPDATE takedatedepartments SET is_reservation = '{Convert.ToByte(tmp.IsReservation)}' WHERE serial_number = '{tmp.SerialFlash}' AND  id_flash = '{tmp.ID_Flash}'";
            DBUtils.ExecuteToDb(command);
        }

        public void DeleteFromDb(TimeEmployee tmp)
        {
            string command = $"DELETE FROM takedateemployees WHERE " +
                $"id_department = '{_selectedIndexIDDepartment}' AND id_flash = '{tmp.ID_Flash}' " +
                $"AND surname = '{tmp.Surname}' AND name = '{tmp.FirstName}' AND patronymic = '{tmp.Patronymic}';";
            DBUtils.ExecuteToDb(command);
            UpdateReservationFromSerialID(tmp);
        }
    }
}
