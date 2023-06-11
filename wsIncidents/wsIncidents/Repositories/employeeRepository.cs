using System;
using System.Net;
using wsIncidents.Core;
using System.Collections;
using wsIncidents.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace wsIncidents.Repositories {

    public class employeeRepository {


        private Hashtable parameters;

        /// <summary>
        /// Create new employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> create(employeeModel model) {

            return await Task.Run(() => {

                if(model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@name", model.name },
                    { "@lastName", model.lastName },
                    { "@mail", model.email },
                    { "@phone", model.phone },
                    { "@codusr", model.codusr },
                };

                bool result = false;

                using(connection con = new connection()) {
                    result = con.cud("man_ins_emp_employee",parameters);
                }

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to save the data"
                    };
                }

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been saved successfully",
                    code = 1
                };
            });

        }

        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> update(employeeModel model) {

            return await Task.Run(() => {

                if(model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@codemp", model.code },
                    { "@name", model.name },
                    { "@lastName", model.lastName },
                    { "@mail", model.email },
                    { "@phone", model.phone },
                    { "@state", model.state },
                };

                bool result = false;

                using(connection con = new connection()) {
                    result = con.cud("man_upd_emp_employee",parameters);
                }

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to update the data"
                    };
                }

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been updated successfully",
                    code = 1
                };
            });

        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="model">Employee code</param>
        /// <returns></returns>
        public async Task<responseModel> delete(employeeModel model) {

            return await Task.Run(() => {

                if(model?.code == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "Insert a code"
                    };
                }

                parameters = new Hashtable() {
                    { "@codemp", model.code }
                };

                bool result = false;

                using(connection con = new connection()) {
                    result = con.cud("man_del_emp_employee",parameters);
                }

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to delete the data"
                    };
                }

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been deleted successfully",
                    code = 1
                };
            });

        }

        /// <summary>
        /// Get employee data by code
        /// </summary>
        /// <param name="code">Employee code</param>
        /// <returns></returns>
        public async Task<responseModel> get(int? code) {

            return await Task.Run(() => {

                if(code == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "Insert a code"
                    };
                }

                parameters = new Hashtable() {
                    { "@codemp", code }
                };

                using(connection con = new connection()) {
                    var result = con.get("man_sel_emp_employee",parameters);

                    if(result.Rows.Count <= 0) return new responseModel() {
                        status = HttpStatusCode.NotFound,
                        message = "No data found"
                    }; 

                    return new responseModel() {
                        status = HttpStatusCode.OK,
                        message = "Employee data",
                        code = (int)code,
                        reponse = new employeeModel() {
                            code = Convert.ToInt32(result.Rows[0]["emp_code"].ToString()),
                            name = result.Rows[0]["emp_name"].ToString(),
                            lastName = result.Rows[0]["emp_lastName"].ToString(),
                            email = result.Rows[0]["emp_email"].ToString(),
                            phone = result.Rows[0]["emp_phone"].ToString(),
                            codusr = Convert.ToInt32(result.Rows[0]["emp_codusr"].ToString())
                        }
                    };

                }
            });

        }

        /// <summary>
        /// Get employee data list
        /// </summary>
        /// <param name="search">search</param>
        /// <returns></returns>
        public async Task<responseModel> getAll(string search) {

            return await Task.Run(() => {

                parameters = new Hashtable() {
                    { "@search", search ?? string.Empty }
                };

                using(connection con = new connection()) {
                    var result = con.get("man_sel_emp_employees",parameters);

                    if(result.Rows.Count <= 0) return new responseModel() {
                        status = HttpStatusCode.NotFound,
                        message = "No data found"
                    };

                    var list = new List<employeeModel>();

                    for(int x = 0; x < result.Rows.Count; x++) {
                        list.Add(new employeeModel() {
                            code = Convert.ToInt32(result.Rows[x]["emp_code"].ToString()),
                            name = result.Rows[x]["emp_name"].ToString(),
                            lastName = result.Rows[x]["emp_lastName"].ToString(),
                            email = result.Rows[x]["emp_email"].ToString(),
                            phone = result.Rows[x]["emp_phone"].ToString(),
                            codusr = Convert.ToInt32(result.Rows[x]["emp_codusr"].ToString())
                        });
                    }

                    return new responseModel() {
                        status = HttpStatusCode.OK,
                        message = "List employees",
                        code = 0,
                        reponse = list
                    };

                }
            });

        }
    }
}
