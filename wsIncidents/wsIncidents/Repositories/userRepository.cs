using System;
using System.Net;
using wsIncidents.Core;
using System.Collections;
using wsIncidents.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace wsIncidents.Repositories {

    public class userRepository
    {

        private Hashtable parameters;

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> create(userModel model) {

            return await Task.Run(async () => {

                if(model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@name", model.userName },
                    { "@password", Helpers.hash.encrypt(model.password) },
                    { "@codrol", model.codrol},
                    { "@codemp", model.codemp },
                };

                bool result = false;

                var res = new DataTable();
                using(connection con = new connection()) {
                    res = con.get("man_ins_usr_user", parameters);
                }
                result = Convert.ToBoolean(res.Rows.Count);

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to save the data"
                    };
                }

                int code = Convert.ToInt32(res.Rows[0]["codusr"].ToString());

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been saved successfully",
                    code = code
                };
            });

        }

        /// <summary>
        /// Update an incident
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> update(incidentModel model) {

            return await Task.Run(() => {

                if(model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@codinc", model.code },
                    { "@description", model.description },
                    { "@codtyp", model.type },
                    { "@state", model.state }
                };

                bool result = false;

                using(connection con = new connection()) {
                    result = con.cud("man_upd_inc_incident",parameters);
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

    }
}
