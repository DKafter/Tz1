using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tz.Models;
using Tz.Utils;

namespace Tz.Controllers
{
    public class FlashController
    {
        private List<List<Flash>> _FlashDb;
        public FlashController()
        {
            _FlashDb = new List<List<Flash>>();
        }
        public List<List<Flash>> GetFromFlashDb
        {
            get
            {
                _FlashDb = new List<List<Flash>>();
                List<List<Flash>> list = new List<List<Flash>>();

                String Name_Company = "Не заполнено";
                String Date_Create = "Не заполнено";
                String Serial_Number = "Не заполнено";
                String ID_Flash = "Не заполнено";

                using (var dbConnection = DBUtils.GetDBConnection())
                {
                    string command = "SELECT id_flash, name_company, date_create, serial_number FROM Flash;";
                    var cmd = new MySqlCommand(command, dbConnection);
                    cmd.CommandType = CommandType.Text;
                    dbConnection.Open();
                    using (var cmdDb = cmd.ExecuteReader())
                    {
                        while (cmdDb.Read())
                        {
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("ID_Flash")))
                            {
                                ID_Flash = cmdDb.GetString("ID_Flash");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("Name_Company")))
                            {
                                Name_Company = cmdDb.GetString("Name_Company");
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("Date_Create")))
                            {
                                Date_Create = cmdDb.GetDateTime("Date_Create").ToString();
                            }
                            if (!cmdDb.IsDBNull(cmdDb.GetOrdinal("Serial_Number")))
                            {
                                Serial_Number = cmdDb.GetString("Serial_Number");
                            }
                            var lst = new List<Flash>()
                            {
                                new Flash(Serial_Number, Name_Company, Date_Create)
                            };
                            _FlashDb.Add(lst);
                        }
                        cmdDb.Close();
                    }

                    dbConnection.Close();
                }
                return _FlashDb;
            }
        }

        public void AddToDb(Flash flash)
        {
            string command = "INSERT INTO flash(name_company, date_create, serial_number) VALUES " +
                $"('{flash.NameCompany}', '{flash.DateCreate}', '{flash.SerialNumber}');";
            var isHasID = _FlashDb.Any(t => t.Contains(flash));
            if (!isHasID)
            {
                DBUtils.ExecuteToDb(command);
            }
        }

        public void DeleteFromDb(Flash flash)
        {
            string command = $"DELETE FROM Flash WHERE serial_number = '{flash.SerialNumber}';";
            DBUtils.ExecuteToDb(command);
            RemoveIDItemFromList(flash);
        }

        private void RemoveIDItemFromList(Flash flash)
        {
            int itemRemoveItem = 0;
            for (int i = 0; i < _FlashDb.Count; ++i)
            {
                var item = _FlashDb[i];
                if (item.IndexOf(flash) != -1)
                {
                    itemRemoveItem = i;
                    break;
                }
            }
            _FlashDb.RemoveAt(itemRemoveItem);
        }

    }
}
