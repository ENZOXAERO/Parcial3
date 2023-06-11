using System;
using System.Net;
using System.Text;
using System.Data;
using wsIncidents.Core;
using wsIncidents.Models;
using System.Collections;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace wsIncidents.Repositories {

    public class authRepository {

        private Hashtable parameters;

        private readonly IConfiguration config;

        public authRepository(IConfiguration configuration) {
            config = configuration;
        }

        /// <summary>
        /// Genera el token, para acceder a los metodos de los controladores
        /// </summary>
        /// <param name="model">Usuario y Pass, para iniciar session</param>
        /// <returns>Token</returns>
        public async Task<tokenResponseModel> login(authModel model) {

            return await Task.Run(() => {

                if(model == null) {
                    return new tokenResponseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "Username or password are invalid"
                    };
                }

                parameters = new Hashtable() {
                    { "@user", model.user },
                    { "@password", Helpers.hash.encrypt(model.password) },
                };

                DataTable user = new DataTable();

                using(connection con = new connection()) {
                    user = con.get("login",parameters);
                }

                if(user.Rows.Count <= 0) {
                    return new tokenResponseModel() {
                        status = HttpStatusCode.BadRequest,
                        message = "Username or password are invalid"
                    };
                }

                var jwt = config.GetSection("JwtConfig").Get<jwtConfigModel>();

                int codusr = Convert.ToInt32(user.Rows[0]["usr_code"].ToString());

                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("code", codusr.ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                var singIn = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                DateTime _exprire = DateTime.UtcNow.AddMinutes(30);

                var token = new JwtSecurityToken(
                    issuer: jwt.issuer,
                    audience: jwt.audience,
                    claims: claims,
                    expires: _exprire,
                    signingCredentials: singIn
                );

                return new tokenResponseModel() {
                    status = HttpStatusCode.OK,
                    message = "success",
                    result = new JwtSecurityTokenHandler().WriteToken(token),
                    expire = _exprire,
                    user = new userModel() { 
                        code = codusr,
                        userName = user.Rows[0]["usr_name"].ToString(),
                        codrol = Convert.ToInt32(user.Rows[0]["usr_codrol"].ToString())
                    }
                };
            });

        }
    }
}
