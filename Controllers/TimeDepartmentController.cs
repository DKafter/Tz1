using Microsoft.SqlServer.Server;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Tz.Models;
using Tz.Utils;

namespace Tz.Controllers
{
    public class TimeDepartmentController
    {
        private List<List<TimeDepartment>> _TimeDepartmentDb;
        private ArrayList _IdDepartments;
        private ArrayList _SerialFlash;
        public TimeDepartmentController()
        {
            _IdDepartments = new ArrayList();
            _SerialFlash = new ArrayList();
            _TimeDepartmentDb = new List<List<TimeDepartment>>();
        }

        public List<List<TimeDepartment>> GetFromTimeDepartmentDb
        {
            get
            {
                _TimeDepartmentDb = new List<List<TimeDepartment>>();
                String ID_Department = null;
                String ID_Flash = null;
                String Date_Now = null;
                String Date_End = null;
                String SerialNumber = null;

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT id_department, id_flash, serial_number, date_now, date_end FROM takedatedepartments;";
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
                            var lst = new List<TimeDepartment>
                                        {
                                            new TimeDepartment(
                                            ID_Department,
                                            ID_Flash,
                                            Date_Now,
                                            Date_End,
                                            SerialNumber)
                                        };
                             _TimeDepartmentDb.Add(lst);
                            
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _TimeDepartmentDb;
            }
        }

        public string GetIdFromSerial(string serial)
        {
            string idFlash = null;
            string command = $"SELECT id_flash FROM flash WHERE serial_number = '{serial}';";
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
                _IdDepartments = new ArrayList();
                string ID_Department = null;
                string command = "SELECT id_department FROM Department";
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
            string command = $"SELECT is_reservation FROM Flash WHERE serial_number = {serial}";
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

        public void UpdateReservationFromSerialID(TimeDepartment tmp)
        {
            string command = $"UPDATE flash SET is_reservation = '{Convert.ToByte(tmp.IsReservation)}' WHERE serial_number = '{tmp.SerialFlash}' AND  id_flash = '{tmp.ID_Flash}'";
            DBUtils.ExecuteToDb(command);
        }

        public IList GetSerialFlashDb
        {
            get
            {
                _SerialFlash = new ArrayList();
                string serialFlash = null;
                bool isReservation = true;
                string command = "SELECT serial_number, is_reservation FROM Flash WHERE is_reservation != 1;";
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
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("serial_number")) && !isReservation)
                            {
                                serialFlash = cmdDb.GetString("serial_number");
                            }

                            var isHas = _SerialFlash.Contains(serialFlash);
                            if (!isHas)
                            {
                                var lst = new List<String> { serialFlash };
                                _SerialFlash.AddRange(lst);
                            }

                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _SerialFlash;
            }
        }

        public void AddToDb(TimeDepartment tmp)
        {
                string command = "INSERT INTO takedatedepartments(id_flash, id_department, serial_number, date_now, date_end) " +
                    $"VALUES (@id_flash, @id_department, @serial_number, @date_now, @date_end);";
                var isHasID = _TimeDepartmentDb.Any(t => t.Contains(tmp));
                if (!isHasID)
                {
                    DBUtils.ExecuteToDb(command,
                        new string[]
                        {
                    "id_flash",
                    "id_department",
                    "serial_number",
                    "date_now",
                    "date_end",
                        },
                        new string[]
                        {
                    tmp.ID_Flash,
                    tmp.ID_Department,
                    tmp.SerialFlash,
                    tmp.DateNow,
                    tmp.DateEnd
                        });
                
            }
        }

        public void DeleteFromDb(TimeDepartment tmp)
        {
            string command = $"DELETE FROM takedatedepartments WHERE serial_number = '{tmp.SerialFlash}' AND id_flash = '{tmp.ID_Flash}';";
            DBUtils.ExecuteToDb(command);
            UpdateReservationFromSerialID(tmp);
        }
    }
}
