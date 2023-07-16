using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tz.Models;
using Tz.Utils;
using Tz.Utils.Interfaces;
using Tz.Views;

namespace Tz.Controllers
{
    public class PhonebookController
    {
        private List<List<Phonebook>> _PhonebookDb;
        private ArrayList _Names;
        public PhonebookController()
        {
            _PhonebookDb = new List<List<Phonebook>>();
            _Names = new ArrayList();
        }

        public List<List<Phonebook>> GetFromPhonebookDb
        {
            get
            {
                _PhonebookDb = new List<List<Phonebook>>();
                String surname = null;
                String name = null;
                String patronymic = null;
                String phone = null;
                String id_employee = null;

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT p.id_employee, p.surname, p.name, p.patronymic, p.phone FROM phonebook p INNER JOIN employee e ON e.id_employee = p.id_employee;";
                    var cmd = new MySqlCommand(command, dbConnection);
                    cmd.CommandType = CommandType.Text;
                    dbConnection.Open();
                    using (var cmdDb = cmd.ExecuteReader())
                    {
                        while (cmdDb.Read())
                        {
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("surname")))
                            {
                                surname = cmdDb.GetString("surname");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("name")))
                            {
                                name = cmdDb.GetString("name");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("patronymic")))
                            {
                                patronymic = cmdDb.GetString("patronymic");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("phone")))
                            {
                                phone = cmdDb.GetString("phone");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_employee")))
                            {
                                id_employee = cmdDb.GetString("id_employee").ToString();
                            }

                            Phonebook pn = new Phonebook(id_employee, surname, name, patronymic, phone);
                            var isHas = _PhonebookDb.Any(t => t.Contains(pn));
                            if (!isHas)
                            {
                                var lst = new List<Phonebook>
                            {
                                pn
                            };
                                _PhonebookDb.Add(lst);
                            }
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _PhonebookDb;
            }
        }

        public IList GetNames
        {
            get
            {
                String surname = null;
                String name = null;
                String patronymic = null;

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT surname, name, patronymic FROM employee;";
                    var cmd = new MySqlCommand(command, dbConnection);
                    cmd.CommandType = CommandType.Text;
                    dbConnection.Open();
                    using (var cmdDb = cmd.ExecuteReader())
                    {
                        while (cmdDb.Read())
                        {
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("surname")))
                            {
                                surname = cmdDb.GetString("surname");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("name")))
                            {
                                name = cmdDb.GetString("name");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("patronymic")))
                            {
                                patronymic = cmdDb.GetString("patronymic");
                            }

                            if (!_Names.Contains(surname) && !_Names.Contains(name) && !_Names.Contains(patronymic))
                            {
                                _Names.Add($"{surname} {name} {patronymic}");
                            }
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _Names;
            }
        }

        public string GetIdFromNames(Phonebook pb)
        {
            string idEmployee = null;
            string command = $"SELECT id_employee FROM employee WHERE surname = '{pb.Surname}' AND name = '{pb.FirstName}' AND patronymic = '{pb.Patronymic}';";
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
                        if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("id_employee")))
                        {
                            idEmployee = cmdDb.GetString("id_employee");
                        }
                    }
                    cmdDb.Close();
                }
                dbConnection.Close();
            }
            return idEmployee;
        }

        public void AddToDb(Phonebook pb)
        {
            string command = "INSERT INTO phonebook(id_employee, surname, name, patronymic, phone) " +
            $"VALUES('{pb.ID_Employee}', '{pb.Surname}', '{pb.FirstName}', '{pb.Patronymic}', '{pb.Phone}');";
            var isHasID = _PhonebookDb.Any(t => t.Contains(pb));
            if (!isHasID)
            {
                DBUtils.ExecuteToDb(command);
            }
        }

        public void DeleteFromDb(Phonebook pb)
        {
            string command = $"DELETE FROM phonebook WHERE phone = {pb.Phone}";
            DBUtils.ExecuteToDb(command);
            RemoveIDItemFromList(pb);
        }
        public void ChangeToDb(Phonebook dmp, string field, string message)
        {
            string command = $"UPDATE phonebook SET {field} = '{message}' WHERE phone = {dmp.Phone};";
            DBUtils.ExecuteToDb(command);
        }

        private void RemoveIDItemFromList(Phonebook dmp)
        {
            int itemRemoveItem = 0;
            for (int i = 0; i < _PhonebookDb.Count; ++i)
            {
                var item = _PhonebookDb[i];
                if (item.IndexOf(dmp) != -1)
                {
                    itemRemoveItem = i;
                    break;
                }
            }
            _PhonebookDb.RemoveAt(itemRemoveItem);
        }
    }
}
