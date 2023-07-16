using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tz.Models;
using Tz.Utils;

namespace Tz.Controllers
{
    public class DepartmentController
    {
        private List<List<Department>> _DepartmentDb;
        public DepartmentController() 
        {
            _DepartmentDb = new List<List<Department>>();
        }
        public List<List<Department>> GetFromDepartmentDb
        {
            get
            {
                _DepartmentDb = new List<List<Department>>();
                String ID_Department = string.Empty;
                String Short_Name_Department = string.Empty;
                String Full_Name_Departe = string.Empty;

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT id_department, name_department, full_name_department FROM Department;";
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
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("name_department")))
                            {
                                Short_Name_Department = cmdDb.GetString("name_department");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("full_name_department")))
                            {
                                Full_Name_Departe = cmdDb.GetString("full_name_department");
                            }

                            var lst = new List<Department> { new Department(Full_Name_Departe, Short_Name_Department, ID_Department) };
                            _DepartmentDb.Add(lst);
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _DepartmentDb;
            }
        }

        public void AddToDb(Department dp)
        {
            string command = "INSERT INTO department(id_department, name_department, full_name_department) " +
                $"VALUES('{dp.ID}', '{dp.ShortNameDepartment}', '{dp.NameDepartment}');";
            var isHasID = _DepartmentDb.Any(t => t.Contains(dp));
            if (!isHasID)
            {
                DBUtils.ExecuteToDb(command);
            }
        }

        public void ChangeToDb(Department dmp, string field, string message)
        {
            string command = $"UPDATE Department SET {field} = '{message}' WHERE id_department = {dmp.ID};";
            DBUtils.ExecuteToDb(command);
        }

        public void DeleteFromDb(Department dmp) 
        {
            string command = $"DELETE FROM Department WHERE id_department = {dmp.ID}";
            DBUtils.ExecuteToDb(command);
            RemoveIDItemFromList(dmp);
        }

        private void RemoveIDItemFromList(Department dmp)
        {
            int itemRemoveItem = 0;
            for(int i = 0; i < _DepartmentDb.Count; ++i)
            {
                var item = _DepartmentDb[i];
                if(item.IndexOf(dmp) != -1)
                {
                    itemRemoveItem = i;
                    break;
                }
            }
            _DepartmentDb.RemoveAt(itemRemoveItem);
        }
    }
}
