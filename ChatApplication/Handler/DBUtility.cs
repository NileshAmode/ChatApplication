using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Reflection;

namespace ChatApplication.Handler
{
    public class DBUtility
    {
        #region Member variables
        static string databaseOwner = "dbo";   // overwrite in web.config
        #endregion

        #region Connection

        //public static void InsertProc(string strProcName)
        //{
        //    SqlCommand command = new SqlCommand();
        //    command.Parameters.Add("@ProcName", SqlDbType.VarChar, 8000).Value = strProcName;
        //    ExecuteSP("InsertProcInfo", command);
        //}
        public static SqlConnection getConnection()
        {


            SqlConnection conn;
            try
            {

                string constr = GetConnectionString();
                conn = new SqlConnection(constr);

            }
            catch
            {
                throw new Exception("SQL Connection String is invalid.");
            }
            return conn;
        }



        public static string GetConnectionString()
        {
            string connectionString = SettingsConfigHelper.settingvalue();
            return connectionString.Trim();
        }
        //public static void InsertPageList(string strProcName, string FullPath, string UserId, string IpAddress)
        //{
        //    SqlCommand command = new SqlCommand();
        //    command.Parameters.Add("@ProcName", SqlDbType.VarChar, 8000).Value = strProcName;
        //    command.Parameters.Add("@FullPath", SqlDbType.VarChar, 8000).Value = FullPath;
        //    command.Parameters.Add("@UserId", SqlDbType.VarChar, 8000).Value = UserId;
        //    command.Parameters.Add("@IpAddress", SqlDbType.VarChar, 8000).Value = IpAddress;

        //    ExecuteSP("InsertPageList", command);
        //}


        #endregion

        #region Common Proceudures
        public static DataTable ConvertListToDatatable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static ArrayList GetArrayList(string procedureName, SqlCommand command, string ColumnName)
        {
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;

            ArrayList Arr = new ArrayList();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Arr.Add(Convert.ToInt32(dr[ColumnName].ToString()));
                }

                //InsertProc(procedureName);
                dr.Close();
                connection.Close();

                return Arr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();

                command.Dispose();
                connection.Dispose();
            }

        }

        public static ArrayList GetArrayListStringValues(string procedureName, SqlCommand command, string ColumnName)
        {
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;

            ArrayList Arr = new ArrayList();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Arr.Add(dr[ColumnName].ToString());
                }
                //InsertProc(procedureName);

                dr.Close();
                connection.Close();

                return Arr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();

                command.Dispose();
                connection.Dispose();
            }

        }


        //Execute Stored Procedure And Returns Dataview as a resultset
        public static DataTable GetDataTable(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable table = null;


            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = command;
                da.Fill(ds);


                //InsertProc(procedureName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            {
                table = ds.Tables[0];
            }
            return table;
        }

        //Execute Stored Procedure And Returns Dataview as a resultset
        public static DataTable GetDataTable(string procedureName, SqlCommand command, string tableName)
        {
            SqlConnection connection = getConnection();

            SqlDataAdapter da = new SqlDataAdapter();

            DataTable dt = new DataTable();


            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;

                da.SelectCommand = command;


                da.Fill(dt);
                dt.TableName = tableName;

                //InsertProc(procedureName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return dt;
        }




        public static DataTable GetDataTable(string sSQl)
        {
            SqlConnection connection = getConnection();
            DataTable datatable;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sSQl, connection);
                datatable = new DataTable();
                da.Fill(datatable);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            return datatable;

        }


        public static DataTable GetDataTable(string sqlQuery, string TableName)
        {
            SqlConnection connection = getConnection();
            DataTable datatable;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
                datatable = new DataTable();
                da.Fill(datatable);
                datatable.TableName = TableName.Trim();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            return datatable;

        }

        public static void Insert_Invalid_Login(string LoginId, string DesignationId, string PageName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@LoginId", SqlDbType.VarChar, 100).Value = CommonUtility.GetString(LoginId);
            cmd.Parameters.Add("@DesignationId", SqlDbType.VarChar, 100).Value = CommonUtility.GetString(DesignationId);
            cmd.Parameters.Add("@PageName", SqlDbType.VarChar, 100).Value = CommonUtility.GetString(PageName);

            int i = DBUtility.ExecuteSP("Insert_Invalid_Logins", cmd);

        }

        public static int ExecuteSP(string procedureName, SqlCommand command)
        {
            int i = 0;
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();

                //if (procedureName != "InsertProcInfo")
                //{
                //    InsertProc(procedureName);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }

        public static int ExecuteSQL(string strQuery, SqlCommand command)
        {
            int i = 0;
            SqlConnection connection = getConnection();

            try
            {

                command.CommandText = strQuery;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.Text;

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }

        public static int ExecuteSQL(string strQuery)
        {
            SqlCommand command = new SqlCommand();
            int i = 0;
            SqlConnection connection = getConnection();

            try
            {

                command.CommandText = strQuery;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.Text;

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }



        public static int GetSPCount(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();


                return Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }




        public static string ExecuteScalarString(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                return Convert.ToString(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }



        public static Int32 ExecuteScalar(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            Int32 intReturnValue = 0;

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                intReturnValue = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }

            return intReturnValue;
        }



        public static string ExecuteScalarString(string sql)
        {
            SqlConnection connection = getConnection();
            SqlCommand command = new SqlCommand();
            string retValue = "";

            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;


                connection.Open();
                retValue = Convert.ToString(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return retValue;
        }


        public static Int32 ExecuteScalar(string sql)
        {
            SqlConnection connection = getConnection();
            SqlCommand command = new SqlCommand();
            Int32 retValue = 0;

            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;


                connection.Open();
                retValue = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return retValue;
        }



        public static string GetSPOutPut(string ProcedureName, SqlCommand command, int OutputParameterIndex)
        {
            SqlDataReader dr = null;
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                //InsertProc(ProcedureName);

                dr.Close();
                connection.Close();
                return command.Parameters[OutputParameterIndex].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                if (!dr.IsClosed) dr.Close();
                dr.Dispose();

                command.Dispose();
                connection.Dispose();
            }
        }






        public static Hashtable GetSPOutPut(string ProcedureName, SqlCommand command, params string[] OutputParameter)
        {
            SqlDataReader dr = null;
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                //InsertProc(ProcedureName);


                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                dr.Close();
                connection.Close();


                //create hash table to return all output parameters
                Hashtable htable = new Hashtable();
                string parameterValue = string.Empty;

                foreach (string optPara in OutputParameter)
                {
                    parameterValue = command.Parameters[optPara].Value.ToString();
                    htable.Add(optPara, parameterValue);
                }

                return htable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                if (!dr.IsClosed) dr.Close();
                dr.Dispose();

                command.Dispose();
                connection.Dispose();
            }
        }



        public static DataView GetDataView(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                command.CommandTimeout = 2000;

                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = command;
                da.Fill(ds);

                //InsertProc(procedureName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return ds.Tables[0].DefaultView;
        }

        public static DataView GetDataView(string sqlQuery)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            DataView dv = new DataView();

            try
            {
                da = new SqlDataAdapter(sqlQuery, connection);
                da.Fill(ds);
                dv.Table = ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return dv;
        }
        //public static void InsertErrorInfo(Exception Ex, string strPageName)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    int i;

        //    string ErrorMessage = Ex.Message;
        //    string StackTrace = Ex.StackTrace.ToString();

        //    cmd.Parameters.Add("@PageName", SqlDbType.VarChar, 2000).Value = CommonUtility.GetString(strPageName);
        //    cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 8000).Value = CommonUtility.GetString(ErrorMessage);
        //    cmd.Parameters.Add("@StackTrace", SqlDbType.VarChar, 8000).Value = CommonUtility.GetString(StackTrace);

        //    i = DBUtility.ExecuteSP("InsertErrorInfo", cmd);

        //}


        public static DataSet GetDataSet(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                da.SelectCommand = command;
                da.Fill(ds);

                //InsertProc(procedureName);
            }
            catch (Exception ex)
            {
               // InsertErrorInfo(ex, procedureName);
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return ds;
        }

        public static DataSet GetDataSet(string sqlQuery)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                da = new SqlDataAdapter(sqlQuery, connection);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return ds;
        }


        public static SqlDataAdapter GetDataAdapter(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = command;

                //InsertProc(procedureName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return da;
        }

        public static SqlDataReader GetDataReader(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;
            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                //InsertProc(procedureName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //command.Connection.Close();
                //command.Connection.Dispose();
                //command.Dispose();
                //if (connection.State == ConnectionState.Open) connection.Close();
            }

            return dr;
        }

        #endregion
    }
}
