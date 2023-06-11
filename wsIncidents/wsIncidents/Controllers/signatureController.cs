using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using wsIncidents.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using wsIncidents.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Mail;
using wsIncidents.Helpers;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace wsIncidents.Controllers {

    [ApiController]
    [ApiVersion("1.0")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/documentsDTE")]
    public class signatureController : ControllerBase {

        private readonly signatureRepository repository = new signatureRepository();

        /// <summary>
        /// Sign the document, and send it to the MH
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST v1.0/documentsDTE/send
        ///     {
        ///         "code": 1478129,
        ///         "prefix": "01",
        ///         "type": "A",
        ///         "transmissionType": 1,
        ///         "origin": "T",
        ///         "codusr": 1
        ///     }
        /// </remarks>
        /// <param name="model">Json Structure</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("send"), Authorize]
        [SwaggerOperation(Tags = new[] { "Send DTE" })]
        [
            SwaggerResponse(200,"Model document if all it's ok",typeof(requestDocumentModel)),
            SwaggerResponse(400,"If not sign or connection is lost, or something is wrong")
        ]
        public async Task<ActionResult<responseModel>> send(requestDocumentModel model) {
            return await repository.send(model);
        }


        //[HttpGet("get")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<OkObjectResult> GetAsync() {



        //    receptionRequestModel model = new receptionRequestModel() {
        //        environment = "00",
        //        shippingCode = "1",
        //        version = 1,
        //        type = "01",
        //        sign = "eyJhbGciOiJSUzUxMiJ9.ewogICJpZGVudGlmaWNhY2lvbiIgOiB7CiAgICAidmVyc2lvbiIgOiAxLAogICAgImFtYmllbnRlIiA6ICIwMCIsCiAgICAidGlwb0R0ZSIgOiAiMDEiLAogICAgIm51bWVyb0NvbnRyb2wiIDogIkRURS0wMS0wMDAwMDAwMi0wMDAwMDAwMDAwMDAwMDIiLAogICAgImNvZGlnb0dlbmVyYWNpb24iIDogIjNGOTM1NzYwLTQzM0EtNEUwNC1BOEYwLTg4QTlEMDVBMTVBQyIsCiAgICAidGlwb01vZGVsbyIgOiAxLAogICAgInRpcG9PcGVyYWNpb24iIDogMSwKICAgICJ0aXBvQ29udGluZ2VuY2lhIiA6IG51bGwsCiAgICAibW90aXZvQ29udGluIiA6IG51bGwsCiAgICAiZmVjRW1pIiA6ICIyMDIzLTA1LTAzIiwKICAgICJob3JFbWkiIDogIjExOjI5OjIwIiwKICAgICJ0aXBvTW9uZWRhIiA6ICJVU0QiCiAgfSwKICAiZG9jdW1lbnRvUmVsYWNpb25hZG8iIDogbnVsbCwKICAiZW1pc29yIiA6IHsKICAgICJuaXQiIDogIjEyMTczMTEwODEwMDIyIiwKICAgICJucmMiIDogIjEwMjg3MzEiLAogICAgIm5vbWJyZSIgOiAiVU5JVkVSU0lEQUQgREUgT1JJRU5URSIsCiAgICAiY29kQWN0aXZpZGFkIiA6ICI4NTMwMSIsCiAgICAiZGVzY0FjdGl2aWRhZCIgOiAiRURVQ0FDSU9OIiwKICAgICJub21icmVDb21lcmNpYWwiIDogIlVOSVZFUlNJREFEIERFIE9SSUVOVEUiLAogICAgInRpcG9Fc3RhYmxlY2ltaWVudG8iIDogIjAyIiwKICAgICJkaXJlY2Npb24iIDogewogICAgICAiZGVwYXJ0YW1lbnRvIiA6ICIxMiIsCiAgICAgICJtdW5pY2lwaW8iIDogIjE3IiwKICAgICAgImNvbXBsZW1lbnRvIiA6ICI0dGEgQ2FsbGUgUG9uaWVudGUgTm8uIDcwNSwgU2FuIE1pZ3VlbCwgRWwgU2FsdmFkb3IsIENlbnRybyBBbcOpcmljYSIKICAgIH0sCiAgICAidGVsZWZvbm8iIDogIjI2NjgzNzAwIiwKICAgICJjb3JyZW8iIDogImxnb256YWxlekB1bml2by5lZHUuc3YiLAogICAgImNvZEVzdGFibGVNSCIgOiBudWxsLAogICAgImNvZEVzdGFibGUiIDogbnVsbCwKICAgICJjb2RQdW50b1ZlbnRhTUgiIDogbnVsbCwKICAgICJjb2RQdW50b1ZlbnRhIiA6IG51bGwKICB9LAogICJyZWNlcHRvciIgOiB7CiAgICAidGlwb0RvY3VtZW50byIgOiAiMTMiLAogICAgIm51bURvY3VtZW50byIgOiAiMDU5MjUxNTQtMyIsCiAgICAibnJjIiA6IG51bGwsCiAgICAibm9tYnJlIiA6ICJST1NBIE1BUklCRUwgQVJHVUVUQSBBUkdVRVRBIiwKICAgICJjb2RBY3RpdmlkYWQiIDogbnVsbCwKICAgICJkZXNjQWN0aXZpZGFkIiA6IG51bGwsCiAgICAiZGlyZWNjaW9uIiA6IHsKICAgICAgImRlcGFydGFtZW50byIgOiAiMTIiLAogICAgICAibXVuaWNpcGlvIiA6ICIxNyIsCiAgICAgICJjb21wbGVtZW50byIgOiAiTEFTIENBU0lUQVMsIENBTlTDk04gQ0VSUk8gQk9OSVRPLCBTQU4gTUlHVUVMIgogICAgfSwKICAgICJ0ZWxlZm9ubyIgOiAiNzk0NTYxNzgiLAogICAgImNvcnJlbyIgOiAiamF5YWxhQHVuaXZvLmVkdS5zdiIKICB9LAogICJvdHJvc0RvY3VtZW50b3MiIDogbnVsbCwKICAidmVudGFUZXJjZXJvIiA6IG51bGwsCiAgImN1ZXJwb0RvY3VtZW50byIgOiBbIHsKICAgICJudW1JdGVtIiA6IDEsCiAgICAidGlwb0l0ZW0iIDogMSwKICAgICJjb2RpZ28iIDogIjIxNTA2OTMiLAogICAgIm51bWVyb0RvY3VtZW50byIgOiAiMjE1MDY5MyIsCiAgICAiY29kVHJpYnV0byIgOiBudWxsLAogICAgImRlc2NyaXBjaW9uIiA6ICJDdW90YSAjIDIiLAogICAgImNhbnRpZGFkIiA6IDEsCiAgICAidW5pTWVkaWRhIiA6IDEsCiAgICAicHJlY2lvVW5pIiA6IDc1LjAsCiAgICAibW9udG9EZXNjdSIgOiAwLjAsCiAgICAidmVudGFOb1N1aiIgOiAwLjAsCiAgICAidmVudGFFeGVudGEiIDogMC4wLAogICAgInZlbnRhR3JhdmFkYSIgOiAwLjAsCiAgICAidHJpYnV0b3MiIDogbnVsbCwKICAgICJwc3YiIDogNzUuMCwKICAgICJub0dyYXZhZG8iIDogMC4wLAogICAgIml2YUl0ZW0iIDogMC4wCiAgfSwgewogICAgIm51bUl0ZW0iIDogMiwKICAgICJ0aXBvSXRlbSIgOiAxLAogICAgImNvZGlnbyIgOiAiMjE1MDY5NCIsCiAgICAibnVtZXJvRG9jdW1lbnRvIiA6ICIyMTUwNjk0IiwKICAgICJjb2RUcmlidXRvIiA6IG51bGwsCiAgICAiZGVzY3JpcGNpb24iIDogIkRlcmVjaG9zIGRlIExhYm9yYXRvcmlvIGRlIEluZ2xlcywgY3VvdGEgbWVuc3VhbCAoMiBtYXRlcmlhcykiLAogICAgImNhbnRpZGFkIiA6IDEsCiAgICAidW5pTWVkaWRhIiA6IDEsCiAgICAicHJlY2lvVW5pIiA6IDI1LjAsCiAgICAibW9udG9EZXNjdSIgOiAwLjAsCiAgICAidmVudGFOb1N1aiIgOiAwLjAsCiAgICAidmVudGFFeGVudGEiIDogMC4wLAogICAgInZlbnRhR3JhdmFkYSIgOiAwLjAsCiAgICAidHJpYnV0b3MiIDogbnVsbCwKICAgICJwc3YiIDogMjUuMCwKICAgICJub0dyYXZhZG8iIDogMC4wLAogICAgIml2YUl0ZW0iIDogMC4wCiAgfSwgewogICAgIm51bUl0ZW0iIDogMywKICAgICJ0aXBvSXRlbSIgOiAxLAogICAgImNvZGlnbyIgOiAiMjE1MDY5NSIsCiAgICAibnVtZXJvRG9jdW1lbnRvIiA6ICIyMTUwNjk1IiwKICAgICJjb2RUcmlidXRvIiA6IG51bGwsCiAgICAiZGVzY3JpcGNpb24iIDogIlJlY2FyZ28gcG9yIE1vcmEiLAogICAgImNhbnRpZGFkIiA6IDEsCiAgICAidW5pTWVkaWRhIiA6IDEsCiAgICAicHJlY2lvVW5pIiA6IDUuMDUsCiAgICAibW9udG9EZXNjdSIgOiAwLjAsCiAgICAidmVudGFOb1N1aiIgOiAwLjAsCiAgICAidmVudGFFeGVudGEiIDogMC4wLAogICAgInZlbnRhR3JhdmFkYSIgOiAwLjAsCiAgICAidHJpYnV0b3MiIDogbnVsbCwKICAgICJwc3YiIDogNS4wNSwKICAgICJub0dyYXZhZG8iIDogMC4wLAogICAgIml2YUl0ZW0iIDogMC42NgogIH0gXSwKICAicmVzdW1lbiIgOiB7CiAgICAidG90YWxOb1N1aiIgOiAwLjAsCiAgICAidG90YWxFeGVudGEiIDogMC4wLAogICAgInRvdGFsR3JhdmFkYSIgOiAwLjAsCiAgICAic3ViVG90YWxWZW50YXMiIDogMTA1LjA1LAogICAgImRlc2N1Tm9TdWoiIDogMC4wLAogICAgImRlc2N1RXhlbnRhIiA6IDEwMC4wLAogICAgImRlc2N1R3JhdmFkYSIgOiAwLjAsCiAgICAicG9yY2VudGFqZURlc2N1ZW50byIgOiAwLjAsCiAgICAidG90YWxEZXNjdSIgOiAwLjAsCiAgICAidHJpYnV0b3MiIDogbnVsbCwKICAgICJzdWJUb3RhbCIgOiAxMDUuMDUsCiAgICAiaXZhUmV0ZTEiIDogMC4wLAogICAgInJldGVSZW50YSIgOiAwLjAsCiAgICAibW9udG9Ub3RhbE9wZXJhY2lvbiIgOiAxMDUuNzEsCiAgICAidG90YWxOb0dyYXZhZG8iIDogMC4wLAogICAgInRvdGFsUGFnYXIiIDogMTA1LjcxLAogICAgInRvdGFsTGV0cmFzIiA6ICJDSUVOVE8gQ0lOQ08gNzEvMTAwIETDk0xBUkVTIiwKICAgICJ0b3RhbEl2YSIgOiAwLjY2LAogICAgInNhbGRvRmF2b3IiIDogMC4wLAogICAgImNvbmRpY2lvbk9wZXJhY2lvbiIgOiAxLAogICAgInBhZ29zIiA6IG51bGwsCiAgICAibnVtUGFnb0VsZWN0cm9uaWNvIiA6IG51bGwKICB9LAogICJleHRlbnNpb24iIDogewogICAgIm5vbWJFbnRyZWdhIiA6ICJHUklTRUxEQSBHVUFEQUxVUEUgSEVSTkFOREVaIFZFTlRVUkEiLAogICAgImRvY3VFbnRyZWdhIiA6ICIwNDY4ODY1ODMiLAogICAgIm5vbWJSZWNpYmUiIDogbnVsbCwKICAgICJkb2N1UmVjaWJlIiA6IG51bGwsCiAgICAib2JzZXJ2YWNpb25lcyIgOiBudWxsLAogICAgInBsYWNhVmVoaWN1bG8iIDogbnVsbAogIH0sCiAgImFwZW5kaWNlIiA6IG51bGwKfQ.JV0rw6vpfSyZst6FNl1MTU1wTxYVH6Cqa2FkoXOghluC6Y5hsmOgJH4SIqzZC6-cezhMBtBYx4cKYtGS53gvk1n4TWy1hi63tZ64xx-0-isai9E2nAaMSxDiP6Bb8S9U-gKW6pPs-1273LSh3d_naYZcfqZ5MglVVIeQfm1JY-fuvlTPaUZDpp_SYXvPF55Pfxl8Z3NXB-X-T_Ln0lnZvbFf-qEZ-OebdvoslZK7SxVEgqpOefDpxMzhqYSScYf7DaksTn3fPeVMtsevM4luMBafbT9RLv6J0xZQsJZZycFg4n9pVKG5NVHvIuHc6e5MWNaat8NgUL6mwhLhItyFBQ",
        //        generationCode = "544926D9-0C9A-4BA9-82B6-BC795B32F78F"
        //    };

        //    string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxMjE3MzExMDgxMDAyMiIsImF1dGhvcml0aWVzIjpbIlVTRVIiLCJVU0VSX0FQSSIsIlVzdWFyaW8iXSwiaWF0IjoxNjgzMTIzMTMyLCJleHAiOjE2ODMyMDk1MzJ9.MPHjsTLHNzDNdeegQ7iKqMc69S47cPtmfZYtkU8Q2rYk6wVPWNkMGgyyNWYUVw4YSKivIgjz-MvVydX7onwPKA";



        //    using(HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) }) {

        //        string _json = JsonConvert.SerializeObject(model);

        //        var content = new StringContent(_json,Encoding.UTF8,"application/json");

        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        //        httpClient.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)");
        //        httpClient.DefaultRequestHeaders.Add("x-content-type-options","nosniff");
        //        httpClient.DefaultRequestHeaders.Add("x-xss-protection","1; mode=block");
        //        httpClient.DefaultRequestHeaders.Add("cache-control","no-cache, no-store, max-age=0, must-revalidate");
        //        httpClient.DefaultRequestHeaders.Add("pragma","no-cache");
        //        httpClient.DefaultRequestHeaders.Add("x-frame-options","DENY");
        //        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var send = await httpClient.PostAsync("https://apitest.dtes.mh.gob.sv/fesv/recepciondte",content);
        //        var result = send.Content.ReadAsStringAsync().Result;
        //        try {
        //            receptionResponseModel signatureResponse = JsonConvert.DeserializeObject<receptionResponseModel>(result);
        //        } catch(Exception ex) {
        //            string a = ex.Message;
        //            throw;
        //        }


        //        return Ok(result);
        //        //return result;
        //    }















        //    //HttpClient httpClient = new HttpClient() {
        //    //    Timeout = TimeSpan.FromSeconds(40),
        //    //};

        //    //httpClient.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)");
        //    //httpClient.DefaultRequestHeaders.Add("x-content-type-options","nosniff");
        //    //httpClient.DefaultRequestHeaders.Add("x-xss-protection","1; mode=block");
        //    //httpClient.DefaultRequestHeaders.Add("cache-control","no-cache, no-store, max-age=0, must-revalidate");
        //    //httpClient.DefaultRequestHeaders.Add("pragma","no-cache");
        //    //httpClient.DefaultRequestHeaders.Add("x-frame-options","DENY");


        //    //var credentials = new List<KeyValuePair<string,string>>();
        //    //credentials.Add(new KeyValuePair<string,string>("user","12173110810022"));
        //    //credentials.Add(new KeyValuePair<string,string>("pwd","7%9dOs20^0Bj"));
        //    //var response = await httpClient.PostAsync("https://apitest.dtes.mh.gob.sv/seguridad/auth",new FormUrlEncodedContent(credentials));

        //    //var result = response.Content.ReadAsStringAsync().Result;
        //    //var sqw = (JObject)JsonConvert.DeserializeObject(result);

        //    //string a = sqw["status"].ToString();
        //    //string token = sqw["body"]["token"].ToString().Replace("Bearer ", "");




        //    ////string postData = string.Format("user=12173110810022&pwd=7%9dOs20^0Bj");

        //    ////string createRequest = string.Format("https://apitest.dtes.mh.gob.sv/seguridad/auth");

        //}




        //[HttpGet("test")]
        //public async Task<IActionResult> test() {

        //    var sss = Helpers.globals.path;



        //    await Helpers.email.send("jayala@univo.edu.sv", "2023165263754D3A4EA0966DAB46255F0C97YXL7");

        //    //helpers.deleteFiles("2023165263754D3A4EA0966DAB46255F0C97YXL7");


        //    return Ok("200");
        //}

        //[HttpGet("s")]
        //public IActionResult fi() {



        //    repository.jsonForClient(null, "");

        //    return Ok("200");
        //}
    }
}
