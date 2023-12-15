using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Task03
{
    public class SQLiteDataManager
    {
        public void SaveToDatabase<T>(string databasePath, List<T> data)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"CREATE TABLE IF NOT EXISTS {typeof(T).Name} ({GetColumnsCreationString<T>()})";
                    command.ExecuteNonQuery();

                    command.CommandText = $"DELETE FROM {typeof(T).Name}";
                    command.ExecuteNonQuery();

                    foreach (var item in data)
                    {
                        command.CommandText = $"INSERT INTO {typeof(T).Name} VALUES ({GetValuesString(item)})";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<T> LoadFromDatabase<T>(string databasePath)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"SELECT * FROM {typeof(T).Name}";

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        List<T> loadedData = new List<T>();

                        while (reader.Read())
                        {
                            T newItem = Activator.CreateInstance<T>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                var prop = typeof(T).GetProperty(columnName);
                                if (prop != null)
                                {
                                    object value = Convert.ChangeType(reader.GetValue(i), prop.PropertyType);
                                    prop.SetValue(newItem, value, null);
                                }
                            }
                            loadedData.Add(newItem);
                        }

                        return loadedData;
                    }
                }
            }
        }

        private string GetColumnsCreationString<T>()
        {
            return string.Join(", ", typeof(T).GetProperties().Select(prop => $"{prop.Name} {GetSQLiteType(prop.PropertyType)}"));
        }

        private string GetValuesString<T>(T item)
        {
            return string.Join(", ", typeof(T).GetProperties().Select(prop => $"'{prop.GetValue(item)}'"));
        }

        private string GetSQLiteType(Type type)
        {
            if (type == typeof(int) || type == typeof(long))
                return "INTEGER";
            else if (type == typeof(float) || type == typeof(double))
                return "REAL";
            else
                return "TEXT";
        }
    }
}