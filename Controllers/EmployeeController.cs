using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tz.Models;
using Tz.Utils;

namespace Tz.Controllers
{
    public class EmployeeController
    {
        private List<List<Employee>> _EmployeeDb;
        private ArrayList _IdDepartments;
        public EmployeeController() 
        {
            _IdDepartments = new ArrayList();
            _EmployeeDb = new List<List<Employee>>();
        }

        public List<List<Employee>> GetFromEmployeeDb
        {
            get
            {
                _EmployeeDb = new List<List<Employee>>();
                String ID_Department = null;
                String surname = null;
                String name = null;
                String patronymic = null;
                byte is_name_black_list = 0;

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT id_department, surname, name, patronymic, ISNAMEBLACKLIST FROM employee;";
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
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("ISNAMEBLACKLIST")))
                            {
                                is_name_black_list = cmdDb.GetByte("ISNAMEBLACKLIST");
                            }

                            var lst = new List<Employee>
                            {
                                new Employee(name, surname, patronymic, ID_Department, is_name_black_list)
                            };
                            _EmployeeDb.Add(lst);
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _EmployeeDb;
            }

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
        public void AddToDb(Employee emp)
        {
            string command = "INSERT INTO employee(id_department, name, patronymic, surname, ISNAMEBLACKLIST) " +
                $"VALUES (@id_department, @name, @patronymic, @surname, @ISNAMEBLACKLIST);";
            var isHasID = _EmployeeDb.Any(t => t.Contains(emp));
            if (!isHasID)
            {
                DBUtils.ExecuteToDb(command,
                    new string[]
                    {
                    "id_department",
                    "name",
                    "patronymic",
                    "surname",
                    "ISNAMEBLACKLIST"
                    },
                    new string[]
                    {
                    emp.ID_Department,
                    emp.FirstName,
                    emp.Patronymic,
                    emp.Surname,
                    emp.Is_Name_Black_List.ToString()
                    });
            }
        }

        public void DeleteFromDb(Employee emp)
        {
            string command = $"DELETE FROM employee WHERE surname = '{emp.Surname}' AND name = '{emp.FirstName}' AND patronymic = '{emp.Patronymic}';";
            DBUtils.ExecuteToDb(command);
            _EmployeeDb.RemoveAll(t => t.Any(a => a.Surname.Equals(emp.Surname) && a.FirstName.Equals(emp.FirstName) && a.Patronymic.Equals(emp.Patronymic)));
        }

        public void ChangeToDb(Employee emp, string field, string message)
        {
            string command = $"UPDATE employee SET {field} = '{message}' WHERE surname = '{emp.Surname}' AND  name = '{emp.FirstName}' AND patronymic = '{emp.Patronymic}';";
            DBUtils.ExecuteToDb(command);
        }
    }
}
