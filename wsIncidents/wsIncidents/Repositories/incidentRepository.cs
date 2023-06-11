using System;
using System.Net;
using wsIncidents.Core;
using System.Collections;
using wsIncidents.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace wsIncidents.Repositories {

    public class incidentRepository {

        private Hashtable parameters;

        /// <summary>
        /// Create new incident
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> create(incidentModel model) {

            return await Task.Run(async () => {
                model.details[0].codusr = (int)model.codusr;
                if (model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@description", model.description },
                    { "@codtyp", model.type },
                    { "@image", model.details[0].image },
                    { "@codusr", model.codusr },
                };

                bool result = false;

                var res = new DataTable();
                using(connection con = new connection()) {
                    res = con.get("man_ins_inc_incident",parameters);
                }
                result = Convert.ToBoolean(res.Rows.Count);

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to save the data"
                    };
                }

                int code = Convert.ToInt32(res.Rows[0]["codinc"].ToString());
                await Helpers.email.send(code);

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been saved successfully",
                    code = code
                };
            });

        }

        /// <summary>
        /// Create new incident detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> createDetail(incidentDetailsModel model) {

            return await Task.Run(async () => {

                if(model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@codinc", model.codinc },
                    { "@description", model.description },
                    { "@codest", model.codsta },
                    { "@image", model.image },
                    { "@codusr", model.codusr },
                };

                bool result = false;

                var res = new DataTable();
                using(connection con = new connection()) {
                    res = con.get("man_ins_det_inc_incidents",parameters);
                }
                result = Convert.ToBoolean(res.Rows.Count);

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to save the data"
                    };
                }

                int code = Convert.ToInt32(model.codinc);
                await Helpers.email.send(code);

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been saved successfully",
                    code = code
                };
            });

        }

        /// <summary>
        /// Create new incident detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> updateDetail(incidentDetailsModel model) {

            return await Task.Run(() => {

                if(model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@coddet", model.code },
                    { "@description", model.description },
                    { "@codsta", model.codsta },
                    { "@image", model.image },
                };

                bool result = false;

                var res = new DataTable();
                using(connection con = new connection()) {
                    res = con.get("man_upd_det_inc_incidents",parameters);
                }
                result = Convert.ToBoolean(res.Rows.Count);

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to save the data"
                    };
                }

                int code = Convert.ToInt32(model.codinc);
                //await Helpers.email.send(code);

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been saved successfully",
                    code = model.codsta
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

        /// <summary>
        /// Delete an incident
        /// </summary>
        /// <param name="model">Incident </param>
        /// <returns></returns>
        public async Task<responseModel> delete(incidentModel model) {

            return await Task.Run(() => {

                if(model?.code == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "Insert a code"
                    };
                }

                parameters = new Hashtable() {
                    { "@codinc", model.code }
                };

                bool result = false;

                using(connection con = new connection()) {
                    result = con.cud("man_del_inc_incident",parameters);
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
                    { "@codinc", code }
                };

                using(connection con = new connection()) {
                    var result = con.get("man_sel_inc_incident",parameters);

                    if(result.Rows.Count <= 0) return new responseModel() {
                        status = HttpStatusCode.NotFound,
                        message = "No data found"
                    };

                    int codinc = Convert.ToInt32(result.Rows[0]["inc_code"].ToString());

                    var detailsModel = new List<incidentDetailsModel>();

                    using(connection _con = new connection()) {
                        var details = _con.get("man_sel_det_inc_incidents",new Hashtable() { { "@codinc",codinc } });

                        for(int k = 0; k < details.Rows.Count; k++) {
                            detailsModel.Add(new incidentDetailsModel() {
                                code = Convert.ToInt32(details.Rows[k]["det_code"].ToString()),
                                codinc = codinc,
                                codsta = Convert.ToInt32(details.Rows[k]["det_codsta"].ToString()),
                                description = details.Rows[k]["det_description"].ToString(),
                                image = details.Rows[k]["det_image"].ToString(),
                                stateDescription = details.Rows[k]["sta_description"].ToString(),
                            });
                        }
                    }

                    var incident = new incidentModel() {
                        code = codinc,
                        description = result.Rows[0]["inc_description"].ToString(),
                        type = Convert.ToInt32(result.Rows[0]["typ_code"]),
                        state = Convert.ToByte(result.Rows[0]["inc_state"]),
                        typeDescription = result.Rows[0]["typ_description"].ToString(),
                        details = detailsModel
                    };

                    return new responseModel() {
                        status = HttpStatusCode.OK,
                        message = "List incidents",
                        code = 0,
                        reponse = incident
                    };

                }
            });

        }

        /// <summary>
        /// Get employee data by code
        /// </summary>
        /// <param name="code">Employee code</param>
        /// <returns></returns>
        public async Task<responseModel> getDetail(int? code) {

            return await Task.Run(() => {

                if(code == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "Insert a code"
                    };
                }

                parameters = new Hashtable() {
                    { "@coddet", code }
                };

                using(connection con = new connection()) {
                    var result = con.get("man_sel_det_inc_incidents_detail",parameters);

                    if(result.Rows.Count <= 0) return new responseModel() {
                        status = HttpStatusCode.NotFound,
                        message = "No data found"
                    };

                    var detail = new incidentDetailsModel() {
                        code = Convert.ToInt32(result.Rows[0]["det_code"].ToString()),
                        codinc = Convert.ToInt32(result.Rows[0]["det_codinc"].ToString()),
                        codsta = Convert.ToInt32(result.Rows[0]["det_codsta"].ToString()),
                        description = result.Rows[0]["det_description"].ToString(),
                        stateDescription = result.Rows[0]["sta_description"].ToString(),
                        image = result.Rows[0]["det_image"].ToString(),
                        codusr = Convert.ToInt32(result.Rows[0]["det_codusr"].ToString())
                    };

                    return new responseModel() {
                        status = HttpStatusCode.OK,
                        message = "Incident detail",
                        code = 0,
                        reponse = detail
                    };

                }
            });

        }

        /// <summary>
        /// Get incident data list
        /// </summary>
        /// <param name="state">state</param>
        /// <param name="search">search</param>
        /// <returns></returns>
        public async Task<responseModel> getAll(byte state,string search, int codusr) {

            return await Task.Run(() => {

                parameters = new Hashtable() {
                    { "@search", search ?? string.Empty },
                    { "@state", state },
                    { "@codusr", codusr }
                };

                using(connection con = new connection()) {
                    var result = con.get("man_sel_inc_incidents",parameters);

                    if(result.Rows.Count <= 0) return new responseModel() {
                        status = HttpStatusCode.NotFound,
                        message = "No data found"
                    };

                    var list = new List<incidentModel>();

                    int codinc = 0;
                    
                    for(int x = 0; x < result.Rows.Count; x++) {

                        codinc = Convert.ToInt32(result.Rows[x]["inc_code"].ToString());
                        
                        var detailsModel = new List<incidentDetailsModel>();

                        using(connection _con = new connection()) {
                            var details = _con.get("man_sel_det_inc_incidents",new Hashtable() { { "@codinc",codinc } });

                            for(int k = 0; k < details.Rows.Count; k++) {
                                detailsModel.Add(new incidentDetailsModel() {
                                    code = Convert.ToInt32(details.Rows[k]["det_code"].ToString()),
                                    codinc = codinc,
                                    description = details.Rows[k]["det_description"].ToString(),
                                    image = details.Rows[k]["det_image"].ToString(),
                                    stateDescription = details.Rows[k]["sta_description"].ToString(),
                                });
                            }
                        }

                        list.Add(new incidentModel() {
                            code = codinc,
                            description = result.Rows[x]["inc_description"].ToString(),
                            state = Convert.ToByte(result.Rows[x]["inc_state"]),
                            typeDescription = result.Rows[x]["typ_description"].ToString(),
                            details = detailsModel
                        });
                    }

                    return new responseModel() {
                        status = HttpStatusCode.OK,
                        message = "List incidents",
                        code = 0,
                        reponse = list
                    };

                }
            });

        }

        /// <summary>
        /// Create new incident Group
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<responseModel> createGroup(groupIncidentModel model) {

            return await Task.Run(() => {

                if(model == null) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "All field requiered"
                    };
                }

                parameters = new Hashtable() {
                    { "@codemp", model.codemp },
                    { "@codtyp", model.codtyp },
                    { "@codusr", model.codusr },
                };

                bool result = false;

                var res = new DataTable();
                using(connection con = new connection()) {
                    res = con.get("man_ins_gru_inc_groups",parameters);
                }
                result = Convert.ToBoolean(res.Rows.Count);

                if(!result) {
                    return new responseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "An error occurred while trying to save the data"
                    };
                }

                int code = Convert.ToInt32(res.Rows[0]["codgru"].ToString());

                return new responseModel() {
                    status = HttpStatusCode.OK,
                    message = "Data has been saved successfully",
                    code = code
                };
            });

        }

        /// <summary>
        /// Get incident data list
        /// </summary>
        /// <returns></returns>
        public async Task<responseModel> getTypes() {

            return await Task.Run(() => {

                using(connection con = new connection()) {
                    var result = con.get("man_sel_typ_incident_types",null);

                    if(result.Rows.Count <= 0) return new responseModel() {
                        status = HttpStatusCode.NotFound,
                        message = "No data found"
                    };

                    var list = new List<typeModel>();

                    for(int x = 0; x < result.Rows.Count; x++) {
                       
                        list.Add(new typeModel() {
                            code = Convert.ToInt32(result.Rows[x]["typ_code"].ToString()),
                            description = result.Rows[x]["typ_description"].ToString()
                        });
                    }

                    return new responseModel() {
                        status = HttpStatusCode.OK,
                        message = "List Types",
                        code = 0,
                        reponse = list
                    };

                }
            });

        }

        /// <summary>
        /// Get incident data list
        /// </summary>
        /// <returns></returns>
        public async Task<responseModel> getStates() {

            return await Task.Run(() => {

                using(connection con = new connection()) {
                    var result = con.get("man_sel_status_incidents",null);

                    if(result.Rows.Count <= 0) return new responseModel() {
                        status = HttpStatusCode.NotFound,
                        message = "No data found"
                    };

                    var list = new List<statusModel>();

                    for(int x = 0; x < result.Rows.Count; x++) {

                        list.Add(new statusModel() {
                            code = Convert.ToInt32(result.Rows[x]["sta_code"].ToString()),
                            description = result.Rows[x]["sta_description"].ToString()
                        });
                    }

                    return new responseModel() {
                        status = HttpStatusCode.OK,
                        message = "List Types",
                        code = 0,
                        reponse = list
                    };

                }
            });

        }

    }
}
