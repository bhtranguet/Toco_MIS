using CoreMVC.Models.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreMVC.Models.DL
{
    public class DLBase<T> : IDisposable
    {
        MySqlConnection conn;
        string connectionString = "server=127.0.0.1;uid=bhtrang;pwd=13111997;database=mis";
        public string TableName
        {
            get
            {
                return Mapping.TableName[typeof(T).Name];
            }
        }

        public DLBase()
        {
            Connection();
        }

        /// <summary>
        /// Giải phóng tài nguyên
        /// </summary>
        public void Dispose()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Mở kết nối với DB
        /// </summary>
        public void Connection()
        {
            if (conn == null)
            {
                conn = new MySqlConnection(connectionString);
            }
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Query data từ DB
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> Query(string query, Dictionary<string, object> param = null)
        {
            MySqlCommand command = CreateCommand(query, CommandType.Text);
            if (param != null) AddParams(command, param);
            MySqlDataReader dataReader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(dataReader);
            return ConvertDataTableToList(table);
        }

        /// <summary>
        /// Hàm thực hiện câu lệnh
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <returns>Số rows ảnh hưởng</returns>
        public int Execute(string query, Dictionary<string, object> param = null)
        {
            MySqlCommand command = CreateCommand(query, CommandType.Text);
            if (param != null) AddParams(command, param);

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Covert Datatable tới List
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<T> ConvertDataTableToList(DataTable table)
        {
            List<T> entities = new List<T>();
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<DataColumn> columns = table.Columns.Cast<DataColumn>().ToList();
            foreach (DataRow row in table.Rows)
            {
                T entity = Activator.CreateInstance<T>();
                foreach (var property in properties)
                {
                    if (columns.Any(column => column.ColumnName.ToLower() == property.Name.ToLower()))
                    {
                        if (row[property.Name] != DBNull.Value)
                        {
                            property.SetValue(entity, row[property.Name]);
                        }
                        else
                        {
                            property.SetValue(entity, null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }

        /// <summary>
        /// Thêm mới đối tượng
        /// </summary>
        /// <param name="entity"></param>
        public bool Insert(T entity)
        {
            // Danh sách param của câu insert
            var insertParam = GetParams(entity);

            // Danh sách các cột khớp giữa DB và Entity
            var columns = insertParam.Keys.ToList();

            // Lấy câu insert
            string insertQuery = BuildInsertQuery(columns, TableName);

            // Thực hiện insert
            return Execute(insertQuery, insertParam) > 0;


        }

        /// <summary>
        /// Lấy ra danh sách tham số gồm tên cột và giá trị
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetParams(T entity)
        {
            List<string> columns = GetColumns(TableName);

            List<PropertyInfo> properties = entity.GetType().GetProperties().ToList();

            // Danh sách tham số
            Dictionary<string, object> param = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                // Lấy các trường khớp giữa DB và Entity
                if (columns.Any(column => column.ToLower() == property.Name.ToLower()))
                {
                    param.Add(property.Name.ToLower(), property.GetValue(entity));
                }
            }
            return param;
        }

        /// <summary>
        /// Build câu Insert
        /// </summary>
        /// <param name="columnNames">Danh sách các cột</param>
        /// <param name="tableName">Tên bảng</param>
        /// <returns></returns>
        public string BuildInsertQuery(List<string> columnNames, string tableName)
        {
            string columns = string.Join(",", columnNames);
            string values = string.Join(",", columnNames.Select(columnName => $"@{columnName}"));
            return $"INSERT INTO {tableName}({columns}) VAlUES ({values});";
        }

        /// <summary>
        /// Lấy về danh sách các cột của một table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<string> GetColumns(string tableName)
        {
            List<string> columns = new List<string>();
            using (MySqlCommand command = conn.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = $"select COLUMN_NAME from information_schema.columns where TABLE_NAME='{tableName}'";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    columns.Add(reader.GetString(0));
                }
                
            }

            return columns;
        }

        /// <summary>
        /// Tạo command
        /// </summary>
        /// <param name="cmdText">Command Text</param>
        /// <param name="cmdType">Command Type</param>
        /// <returns></returns>
        public MySqlCommand CreateCommand(string cmdText, CommandType cmdType)
        {
            MySqlCommand command = conn.CreateCommand();
            command.CommandType = cmdType;
            command.CommandText = cmdText;
            return command;
        }

        /// <summary>
        /// Thêm param cho command
        /// </summary>
        /// <param name="command"></param>
        public void AddParams(MySqlCommand command, Dictionary<string, object> param)
        {
            foreach (var item in param)
            {
                string paramName = $"@{item.Key}";
                object value = item.Value;
                command.Parameters.AddWithValue(paramName, value);
            }
        }

        /// <summary>
        /// Lấy ra tất cả bản ghi
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            string query = $"SELECT * FROM {TableName}";
            return Query(query);
        }

        /// <summary>
        /// Lấy chi tiết bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetByID(int id)
        {
            string query = $"SELECT * FROM {TableName} WHERE ID = {id}";
            return Query(query).FirstOrDefault();
        }

        public string BuildUpdateQuery(List<string> columnNames, string tableName, int id)
        {
            string setString = string.Join(",", columnNames.Select(c => $"{c}=@{c}"));
            return $"UPDATE {tableName} SET {setString} WHERE ID = {id};";
        }

        /// <summary>
        /// Cập nhật một enity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(int id, T entity)
        {
            // Danh sách param của câu insert
            var updateParam = GetParams(entity);

            // Danh sách các cột khớp giữa DB và Entity
            var columns = updateParam.Keys.ToList();

            // Lấy câu query update
            string updateQuery = BuildUpdateQuery(columns, TableName, id);

            // Thực hiện insert
            return Execute(updateQuery, updateParam) > 0;
        }

        /// <summary>
        /// Xóa một entity
        /// </summary>
        /// <returns>Số bản ghi xóa</returns>
        public int Delete(int id)
        {
            string query = $"DELETE FROM {TableName} WHERE id = @id;";
            return Execute(query, new Dictionary<string, object> { { "id", id} });
        }
    }
}
