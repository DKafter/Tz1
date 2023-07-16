using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tz.Utils;

namespace Tz.Controllers
{
    public class PunishmentController
    {
        private List<List<string>> _employeePunishments;
        public PunishmentController() {
        }
        public List<List<string>> GetFromEmployeePunishment
        {
            get
            {
                _employeePunishments = new List<List<string>>();
                String surname = null;
                String name = null;
                String patronymic = null;
                String Phone = null;       
                String Serial_Number = null;

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT DISTINCT te.name, te.surname, te.patronymic, te.serial_number, z.phone FROM takedateemployees te INNER JOIN takedatedepartments td ON te.id_department = td.id_department AND(td.date_end - te.date_now) < 0 AND(td.date_end - te.date_end) < 0 INNER JOIN Phonebook z ON z.id_employee = te.id_employee;";
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
                                Phone = cmdDb.GetString("phone");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("serial_number")))
                            {
                                Serial_Number = cmdDb.GetString("serial_number").ToString();
                            }

                            var _emp = new List<String> {surname, name, patronymic, Phone,  Serial_Number };
                            _employeePunishments.Add(_emp);
                        }
                        cmdDb.Close();
                    }
                    dbConnection.Close();
                }
                return _employeePunishments;
            }

        }
    }
}
