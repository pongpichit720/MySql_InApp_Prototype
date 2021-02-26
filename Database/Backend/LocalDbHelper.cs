using Database.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Backend
{
    public class LocalDbHelper
    {
        private static string ConnectionString { get; } = "server=localhost;port=3306;database=localdb;uid=poonpiri;password=Login1234$"; // Local_MySQL
        //private static string ConnectionString { get; } = "server=localhost;port=53689;database=localdb;uid=azure;password=6#vWHD_$"; // Azure_MySQL-In-APP

        private static MySqlConnection Connection;

        private static LocalDbContext contextDB;
        private static LocalDbContext ContextDB
        {
            get
            {
                if (contextDB == null)
                {
                    using (Connection = new MySqlConnection(ConnectionString))
                    {
                        contextDB = new LocalDbContext(Connection, false);
                    }
                }

                return contextDB;
            }

            set { }
        }

        public static List<T1> ReadT1(FilterModel filter = null)
        {
            var result = new List<T1>();

            try
            {
                result = ContextDB.T1_Table.AsQueryable().ToList();

                if (filter != null)
                {
                    if (filter.ID != int.MaxValue)
                    {
                        result = result.Where(each => each.ID == filter.ID).ToList();
                    }

                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        result = result.Where(each => each.Name.ToUpper().Contains(filter.Name.ToUpper())).ToList();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }

        public static List<T2> ReadT2(FilterModel filter = null)
        {
            var result = new List<T2>();

            try
            {
                result = ContextDB.T2_Table.AsQueryable().ToList();

                if (filter != null)
                {
                    if (filter.ID != int.MaxValue)
                    {
                        result = result.Where(each => each.ID == filter.ID).ToList();
                    }

                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        result = result.Where(each => each.Name.ToUpper().Contains(filter.Name.ToUpper())).ToList();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }

        /*public List<T> Read<T>()
        {

            var myReturnList = new List<T>();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using (LocalDbContext contextDB = new LocalDbContext(connection, false))
                {
                    Type type = typeof(T);

                    try
                    {
                        // read data from table T1
                        if (type == typeof(T1))
                        {
                            foreach (var item in contextDB.T1_Table)
                            {
                                myReturnList.Add((T)Convert.ChangeType(item, typeof(T)));
                            }
                        }

                        // read data from table T2
                        if (type == typeof(T2))
                        {
                            foreach (var item in contextDB.T2_Table)
                            {
                                myReturnList.Add((T)Convert.ChangeType(item, typeof(T)));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return myReturnList;
        }*/

        // insert data into table T1
        public static bool Insert(T1 entity)
        {
            try
            {
                // reset context
                contextDB = null;

                ContextDB.T1_Table.Add(entity);
                ContextDB.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        // insert data into table T2
        public static bool Insert(T2 entity)
        {
            try
            {
                ContextDB.T2_Table.Add(entity);
                ContextDB.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        // delete data in table T1
        public static bool Delete(T1 entity)
        {
            try
            {
                // reset context
                contextDB = null;

                ContextDB.T1_Table.Attach(entity);
                ContextDB.T1_Table.Remove(entity);
                ContextDB.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        // update data in table T1
        public static bool Update(T1 entity)
        {
            try
            {
                var result = ContextDB.T1_Table.SingleOrDefault(each => each.ID == entity.ID);
                if (result == null)
                {
                    return false;
                }

                result.Name = entity.Name;
                ContextDB.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static bool Backup(string path = "wwwroot/data/localdb.txt")
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using MySqlCommand command = new MySqlCommand();
                using MySqlBackup backup = new MySqlBackup(command);
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    backup.ExportToFile(path);
                    connection.Close();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Restore(string path)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using MySqlCommand command = new MySqlCommand();
                using MySqlBackup backup = new MySqlBackup(command);
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    backup.ImportFromFile(path);
                    connection.Close();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        public static MemoryStream Download()
        {
            MemoryStream stream = new MemoryStream();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using MySqlCommand command = new MySqlCommand();
                using MySqlBackup backup = new MySqlBackup(command);
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    backup.ExportToMemoryStream(stream, true);
                    connection.Close();
                    stream.Position = 0;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return stream;
        }
    }
}
