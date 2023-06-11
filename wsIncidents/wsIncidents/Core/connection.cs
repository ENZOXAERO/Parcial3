using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace wsIncidents.Core {

    public class connection : IDisposable {

        private string ConnectionString { get => ConfigurationManager.ConnectionStrings["connection"].ConnectionString; }

        public connection() {
        }

        internal DataTable get(string query,Hashtable Parameters) {

            var result = new DataTable();

            using(var con = new SqlConnection(ConnectionString)) {

                try {

                    var task = Task.Factory.StartNew(() => {
                        if(con.State != ConnectionState.Open)
                            con.Open();
                        return con.State == ConnectionState.Open;
                    });

                    if(!(task.Wait(15000) && task.Result))
                        return result;

                    using(var command = new SqlCommand(query,con) { CommandType = CommandType.StoredProcedure }) {

                        if(Parameters != null) command.Parameters.AddRange(GetParameters(Parameters));

                        new SqlDataAdapter(command).Fill(result);
                    }
                    return result;

                } catch(Exception ex) {

                    string a = ex.Message;
                }
                con.Close();
            }
            return result;
        }

        internal bool cud(string query,Hashtable parameters) {
            var validate = false;

            using(var con = new SqlConnection(ConnectionString)) {
                try {
                    var task = Task.Factory.StartNew(() => {
                        if(con.State != ConnectionState.Open)
                            con.Open();
                        return con.State == ConnectionState.Open;
                    });

                    if(!(task.Wait(15000) && task.Result))
                        validate = false;

                    using(var command = new SqlCommand(query,con) { CommandType = CommandType.StoredProcedure }) {

                        if(parameters != null) command.Parameters.AddRange(GetParameters(parameters));

                        validate = Convert.ToBoolean(command.ExecuteNonQuery());
                    }

                } catch(Exception ex) {
                    string ss = ex.Message;
                    return false;
                }
                con.Close();
            }
            return validate;
        }

        private static SqlParameter[] GetParameters(Hashtable Values) {
            List<SqlParameter> parameter = new List<SqlParameter>();
            foreach(var item in Values.Keys) {
                parameter.Add(new SqlParameter() {
                    ParameterName = item.ToString(),
                    Value = Values[item],
                });
            }
            return parameter.ToArray();
        }

        public void Dispose() {
        }
    }
}
