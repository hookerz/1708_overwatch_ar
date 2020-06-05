using LitJson;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hook.HXF
{
    public class DatabaseManager
    {
        #region Constants
        
        private const string kTag = "[Hook-Unity-DB]";
        
        #endregion
        
        #region Properties

        private string _dbPath;
        private IDbConnection _dbConnection;
        
        #endregion
        
        #region Constructor

        public DatabaseManager(string databaseName)
        {
            _dbPath = "URI=file:" + Application.streamingAssetsPath + "/" + databaseName;
            _dbConnection = new SqliteConnection(_dbPath);
            _dbConnection.Open();
            Debug.LogFormat("{0} [ Connection status: {1}, Database: {2} ]", kTag, _dbConnection.State, _dbConnection.Database);
        }

        ~DatabaseManager()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
            }
        }
        
        #endregion
        
        #region Class Methods

        public T RunQuery<T>(string query)
        {
            // executing query
            IDbCommand command = _dbConnection.CreateCommand();
            IDataReader reader;
            command.CommandText = query;
            reader = command.ExecuteReader();

            // creating json writer
            var sb = new StringBuilder();
            var writer = new JsonWriter(sb);
            
            // parsing response into JsonWriter
            writer.WriteArrayStart();
            while (reader.Read())
            {
                writer.WriteObjectStart();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var column = reader.GetName(i);
                    var content = reader[i];
                    
                    writer.WritePropertyName(column);
                    if (content is Int16)
                    {
                        writer.Write((Int16)content);
                    }
                    else if (content is Int32)
                    {
                        writer.Write((Int32)content);
                    }
                    else if (content is Int64)
                    {
                        writer.Write((Int64)content);
                    }
                    else if (content is Single)
                    {
                        writer.Write(double.Parse(content.ToString()));
                        //writer.Write((double)content);
                    }
                    else if (content is string)
                    {
                        writer.Write(content.ToString());
                    }
                    else
                    {
                        Debug.LogErrorFormat("Error! No found type for {0} ({1})", content.ToString(), content.GetType().ToString());
                    }
                }
                writer.WriteObjectEnd();
            }
            writer.WriteArrayEnd();
            
            // creating response object
            var json = sb.ToString();
            var data = JsonMapper.ToObject<T>(json);
            
            return data;
        }
        
        #endregion
    }
}