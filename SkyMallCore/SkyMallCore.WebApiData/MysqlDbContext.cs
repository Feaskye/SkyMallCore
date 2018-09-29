using MySql.Data.MySqlClient;
using SkyMallCore.WebApiData.Models;
using System;
using System.Collections.Generic;

namespace SkyMallCore.WebApiData
{
    public class MysqlDbContext
    {


        public string ConnectionString;

        public MysqlDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public IEnumerable<Member> GetAll()
        {
            List<Member> list = new List<Member>();
            //连接数据库
            using (MySqlConnection msconnection = GetConnection())
            {
                msconnection.Open();
                //查找数据库里面的表
                MySqlCommand mscommand = new MySqlCommand("SELECT Id,UserName,Password FROM `member`", msconnection);
                using (MySqlDataReader reader = mscommand.ExecuteReader())
                {
                    //读取数据
                    while (reader.Read())
                    {
                        list.Add(new Member()
                        {
                            Id = reader.IsDBNull(0)?"":reader.GetString("Id"),
                            UserName = reader.IsDBNull(1) ? "" : reader.GetString("UserName"),
                            Password = reader.IsDBNull(2) ? "" : reader.GetString("Password")
                        });
                    }
                }
            }
            return list;
        }

    }
}
