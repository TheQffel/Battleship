using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Battleship
{
    static class Database
    {
        private static MySqlConnection Connection = new();
        private static object MySqlLock = new();
        
        public static void Connect(string Host, string User, string Pass, string Db)
        {
            lock(MySqlLock)
            {
                try
                {
                    string ConnectionString = "Server=" + Host + ";" + "UserId=" + User + ";" + "Password=" + Pass + ";" + "Database=" + Db + ";" + "SslMode=None";
                    Connection = new(ConnectionString);
                    Connection.Open();
                    
                    Console.Log(Console.LogType.Ok, "Successfully connected to database!");
                }
                catch (Exception Ex)
                {
                    Console.Log(Console.LogType.Error, Ex.ToString());
                }
            }
        }
        
        public static void Disconnect()
        {
            lock(MySqlLock)
            {
                try
                {
                    Connection.Close();
                    Connection.Dispose();
                }
                catch (Exception Ex)
                {
                    Console.Log(Console.LogType.Error, Ex.ToString());
                }
            }
        }
        
        public static void SpecialCommand(string Command)
        {
            lock(MySqlLock)
            {
                try
                {
                    MySqlCommand Cmd = Connection.CreateCommand();
                    Cmd.CommandText = Command;
                    Cmd.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    Console.Log(Console.LogType.Error, Ex.ToString());
                }
            }
        }
        
        public static string[][] Get(string Table, string Column = "*", string When = "", string Order = "", string Group = "", byte Limit = 0)
        {
            DataTable Result = new();
            
            lock(MySqlLock)
            {
                try
                {
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT " + Column + " FROM " + Table;
                    if (When != "")
                    {
                        Command.CommandText += " WHERE " + When;
                    }
                    if (Order != "")
                    {
                        Command.CommandText += " ORDER BY " + Order;
                    }
                    if (Group != "")
                    {
                        Command.CommandText += " GROUP BY " + Group;
                    }
                    if (Limit > 0)
                    {
                        Command.CommandText += " LIMIT " + Limit;
                    }
                    Result.Load(Command.ExecuteReader());
                }
                catch (Exception Ex)
                {
                    Console.Log(Console.LogType.Error, Ex.ToString());
                }
            }
            if (Result.Rows.Count > 0 && Result.Columns.Count > 0)
            {
                object?[][] Objects = Result.AsEnumerable().Select(x => x.ItemArray).ToArray();
                string[][] Results = new string[Objects.Length][];
                for (int i = 0; i < Objects.Length; i++)
                {
                    Results[i] = new string[Objects[i].Length];
                    for (int j = 0; j < Objects[i].Length; j++)
                    {
                        Results[i][j] = Objects[i][j]?.ToString() ?? " ";
                    }
                }
                return Results;
            }
            return new[] { new []{ "" } };
        }

        public static int Set(string Table, string ColumnValues, string When)
        {
            int Result = -1;
            lock(MySqlLock)
            {
                try
                {
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "UPDATE " + Table + " SET " + ColumnValues + " WHERE " + When;
                    Result = Command.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    Console.Log(Console.LogType.Error, Ex.ToString());
                }
            }
            return Result;
        }

        public static int Add(string Table, string Columns, string Values)
        {
            int Result = -1;
            lock(MySqlLock)
            {
                try
                {
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "INSERT INTO " + Table + " (" + Columns + ") VALUES (" + Values + ")";
                    Result = Command.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    Console.Log(Console.LogType.Error, Ex.ToString());
                }
            }
            return Result;
        }

        public static int Del(string Table, string Where)
        {
            int Result = -1;
            lock(MySqlLock)
            {
                try
                {
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "DELETE FROM " + Table + " WHERE " + Where;
                    Result = Command.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    Console.Log(Console.LogType.Error, Ex.ToString());
                }
            }
            return Result;
        }
    }
}