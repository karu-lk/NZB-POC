using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

namespace Donor
{
    public class AppointmentDatabase
    {
        static object locker = new object();
        public SqliteConnection connection;
        public string path;

        public AppointmentDatabase(string dbPath)
        {
            var output = "";
            path = dbPath;
            bool exists = File.Exists(dbPath);

            if (!exists)
            {
                connection = new SqliteConnection("Data Source=" + dbPath);

                connection.Open();
                var commands = new[] {
                    "CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, Name NTEXT, Notes NTEXT, Done INTEGER, AppDate NTEXT);"
                };
                foreach (var command in commands)
                {
                    using (var c = connection.CreateCommand())
                    {
                        c.CommandText = command;
                        var i = c.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // db is already exists, do nothing. TODO: add config parameter
            }
            Console.WriteLine(output);
        }

        Appointment FromReader(SqliteDataReader r)
        {
            var t = new Appointment();
            t.ID = Convert.ToInt32(r["_id"]);
            t.Name = r["Name"].ToString();
            t.Notes = r["Notes"].ToString();
            t.Done = Convert.ToInt32(r["Done"]) == 1 ? true : false;
            t.AppDate = r["AppDate"].ToString();
            return t;
        }

        public IEnumerable<Appointment> GetItems()
        {
            var tl = new List<Appointment>();

            lock (locker)
            {
                connection = new SqliteConnection("Data Source=" + path);
                connection.Open();
                using (var contents = connection.CreateCommand())
                {
                    contents.CommandText = "SELECT [_id], [Name], [Notes], [Done], [AppDate] from [Items]";
                    var r = contents.ExecuteReader();
                    while (r.Read())
                    {
                        tl.Add(FromReader(r));
                    }
                }
                connection.Close();
            }
            return tl;
        }

        public Appointment GetItem(int id)
        {
            var t = new Appointment();
            lock (locker)
            {
                connection = new SqliteConnection("Data Source=" + path);
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT [_id], [Name], [Notes], [Done], [AppDate] from [Items] WHERE [_id] = ?";
                    command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = id });
                    var r = command.ExecuteReader();
                    while (r.Read())
                    {
                        t = FromReader(r);
                        break;
                    }
                }
                connection.Close();
            }
            return t;
        }

        public int SaveItem(Appointment item)
        {
            int r;
            lock (locker)
            {
                if (item.ID != 0)
                {
                    connection = new SqliteConnection("Data Source=" + path);
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE [Items] SET [Name] = ?, [Notes] = ?, [Done] = ?, [AppDate] = ? WHERE [_id] = ?;";
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Name });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Notes });
                        command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = item.Done });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.AppDate });
                        command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = item.ID });
                        r = command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return r;
                }
                else
                {
                    connection = new SqliteConnection("Data Source=" + path);
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done], [AppDate]) VALUES (? ,?, ?, ?)";
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Name });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Notes });
                        command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = item.Done });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.AppDate });
                        r = command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return r;
                }

            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                int r;
                connection = new SqliteConnection("Data Source=" + path);
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [Items] WHERE [_id] = ?;";
                    command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = id });
                    r = command.ExecuteNonQuery();
                }
                connection.Close();
                return r;
            }
        }
    }
}