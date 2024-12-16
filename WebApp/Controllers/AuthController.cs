using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApp.Models;
using SqlClient;
using ApiBindingModels;
using System.Threading.Tasks;
using Core;
using NLog;
using System.Text.Json;
using Utilities;
using Services;
namespace WebApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
       public AuthController()
        {
            log = LogManager.GetCurrentClassLogger();
        }
        protected Logger log { get; set; }

        [HttpPost, Route("login")]
        public async Task<ActionResult> Login([FromBody] User user)
        {
          
          //email address in this case is username
            var processingResult = new ApiProcessingResult<RetLoginUser>();
           // try { log.Error("test authcontroller line35"); }catch(Exception ex) { };
           
            if (user == null)
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Invalid client request", "Invalid client request", ""));
               return Ok(new { ApiProcessingResult = processingResult });
            }
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Validation failure:" + messages, "Validation failure:" + messages, ""));
                log.Error("Validation failure:" + messages);
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var sqlClient = new SQLCustomClient();
           
                sqlClient.CommandText(@"Select FirstName,LastName,EmailAddress,UserName,RoleId,U.Id From ApplicationUsers U
                 Where U.UserName=@UserName AND Password=@Password");

      


            sqlClient.AddParameter("@UserName", user.UserName);
            sqlClient.AddParameter("@Password", user.Password);
            sqlClient.AddParameter("@EncryptionKey", ApplicationConfig.SqlPassPhrase);

            var result = sqlClient.Select<RetLoginUser>();
            if (result.IsError)
            {
                log.Error("SqlFailure:" + result.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve user", "Failed to retrieve user", ""));
                return Ok(new { ApiProcessingResult = processingResult });
               
            }
            if (result.Data == null)
            {
                var jsonStr = JsonSerializer.Serialize(user);
              processingResult.IsError = true;  
                processingResult.Errors.Add(new ApiProcessingError("Your account was not found with the credentials you provided.", "Your account was not found with the credentials you provided.", ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var data =(RetLoginUser) result.Data;
          
            string _roleId = data.RoleId;
            sqlClient.ClearParameters();
            sqlClient.CommandText("Select Name,Rank From ApplicationRoles Where Id=@Id ");
            sqlClient.AddParameter("@Id", _roleId);
            var roleResult=sqlClient.Select<RetLoginUserRole>();
            if (roleResult.IsError)
            {
                log.Error("SqlRole Failure:" + roleResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;    
                processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve role", "Failed to retrieve role", ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }
           
            if (result.Data!=null && roleResult.Data!=null)
            {
                var _role =(RetLoginUserRole) roleResult.Data;
                data.Role = _role;
                string _isAdmin = _role.Rank <= 1?"True" : "False";
                string _isAdviser = _role.Rank == 2 ? "True" : "False";
                data.IsAdmin = _role.Name.ToUpper() == "SA"|| _role.Name.ToUpper()=="ADMINISTRATOR";
              
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationConfig.APISecretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                try
                {
                    var tokeOptions = new JwtSecurityToken(
                        issuer: ApplicationConfig.TokenDomain,
                        audience: ApplicationConfig.TokenDomain,
                         claims: new List<Claim>(new List<Claim> {
                             new Claim("IsAdmin",_isAdmin,ClaimValueTypes.Boolean),
                             
                            new Claim("UserName",data.UserName),
                            new Claim("FirstName",data.FirstName),
                            new Claim("LastName",data.LastName),
                            new Claim("EmailAddress",data.EmailAddress),
                            new Claim("Role",data.Role.Name),
                            new Claim("Rank",data.Role.Rank.ToString()),
                              new Claim("Id",data.Id),
                                    }),
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    data.Token = tokenString;
                    processingResult.Data = data;
                }
                catch (Exception ex)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to generate token", ex.Message, ""));    
                 return Ok(new { ApiProcessingResult = processingResult });
                }
                return Ok(new { ApiProcessingResult = processingResult });
            }
            else
            {
                return Unauthorized();
            }
           
        }
        [HttpPost, Route("register")]
        public async Task<ActionResult> Register(UserBindingModel dataModel)
        {

            var processingResult = new ApiProcessingResult<RetLoginUser>();
           
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Validation failure:" + messages, "Validation failure:" + messages, ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }
           
            if (string.IsNullOrEmpty(dataModel.RoleId))
            {
                dataModel.RoleId = "3DC0F802-C147-4EB0-9B59-967EA912E8BB";//always a parent here
            }


            string userID = "";
            if (string.IsNullOrEmpty(dataModel.Schcode))
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Missing school code", "Missing school code", ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }
            //check that user/ with schcode  exist
            var checkSqlClient = new SQLCustomClient();
            checkSqlClient.CommandText(@"
               Select AU.Id AS UserId,AU.EmailAddress,OP.Schcode,OP.NameTitle From ApplicationUsers AU 
            Left Join ApplicationUser_Schcode US ON AU.Id=US.UserId
            Left Join OpyProducts OP  On US.Schcode=OP.Schcode
            WHERE AU.UserName=@UserName
            ");

            checkSqlClient.AddParameter("@UserName", dataModel.EmailAddress);
            var checkResult = checkSqlClient.SelectMany<UserCheck>();
            if (checkResult.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to check for duplicate user", checkResult.Errors[0].DeveloperMessage, ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }
            bool InsertAll = true;
           
            if (checkResult.Data != null)//User exists
            {
                var vData = (List<UserCheck>)checkResult.Data;
                var rec = vData.Find(x => x.Schcode == dataModel.Schcode.Trim());
                if (rec != null)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("The email address is already associate with school " + rec.NameTitle.Trim(), "The email address is already associate with school " + rec.NameTitle.Trim(), ""));
                    return Ok(new { ApiProcessingResult = processingResult });
                }
                userID = vData[0].UserId;//doesn't matter if there are two recs they both have same ID
           
                InsertAll = false;
            }


            //get school name/title-------------------------
            string _schname = "";
            var custSqlClient = new SQLCustomClient();
         
            custSqlClient.CommandText(@"Select NameTitle From OpyProducts
                        WHERE Schcode = @Schcode");
            custSqlClient.AddParameter("@Schcode",dataModel.Schcode);
            var schNameResult = custSqlClient.SelectSingleColumn();
            if (schNameResult.IsError)
            {
                log.Error(schNameResult.Errors[0].DeveloperMessage);
            }
            _schname = schNameResult.Data;
            if (_schname=="")
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("The school code,"+ dataModel.Schcode + " entered was not found.", "The school code," + dataModel.Schcode + " entered was not found.", ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }
          

            var UserSqlClient = new SQLCustomClient();
            UserSqlClient.IdentityColumn("Id");


            if (InsertAll)
            {
                UserSqlClient.CommandText(@"
                INSERT INTO ApplicationUsers (FirstName,LastName,EmailAddress,UserName,Password,RoleId) VALUES(@FirstName,@LastName
                        ,@EmailAddress,@UserName,EncryptByPassPhrase(@EncryptionKey,Cast(@Password as varchar(100))),@RoleId);
            ");
                UserSqlClient.AddParameter("@FirstName", dataModel.FirstName);
                UserSqlClient.AddParameter("@LastName", dataModel.LastName);
                UserSqlClient.AddParameter("@emailAddress", dataModel.EmailAddress);
                UserSqlClient.AddParameter("@Password", dataModel.Password);
                UserSqlClient.AddParameter("@UserName",dataModel.EmailAddress);//for parents always email address
                UserSqlClient.AddParameter("@RoleId", dataModel.RoleId);//parent
                UserSqlClient.AddParameter("@EncryptionKey", ApplicationConfig.SqlPassPhrase);

                var userResult = UserSqlClient.Insert();
                if (userResult.IsError)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Error inserting user", userResult.Errors[0].DeveloperMessage, ""));
                    return Ok(new { ApiProcessingResult = processingResult });
                }

                UserSqlClient.ClearParameters();
                UserSqlClient.CommandText(@"
                INSERT INTO ApplicationRoles_User (UserId,RoleId) VALUES(@UserId,@RoleId)");

                userID = userResult.Data;

                UserSqlClient.AddParameter("@RoleId", dataModel.RoleId);//parent
                UserSqlClient.AddParameter("@UserId", userID);
                var roleResult = UserSqlClient.Insert();
                if (roleResult.IsError)
                {
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to insert role.", "Failed to insert role.", ""));
                    return Ok(new { ApiProcessingResult = processingResult });
                }
            }

            UserSqlClient.ClearParameters();
            UserSqlClient.CommandText(@"
                INSERT INTO [dbo].[ApplicationUser_Schcode] ([Schcode],[UserId]) VALUES (@Schcode,@UserId)
            ");

            UserSqlClient.AddParameter("@Schcode", dataModel.Schcode);//parent
            UserSqlClient.AddParameter("@UserId", userID);
            var schoolResult = UserSqlClient.Insert();
            if (schoolResult.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Failed to insert associated schcode.", "Failed to insert associated schcode.", ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }
            var data = new RetLoginUser()
            {
                FirstName = dataModel.FirstName,
                LastName = dataModel.LastName,
                Id=userID,
                EmailAddress = dataModel.EmailAddress,
           
                Role=new RetLoginUserRole() { Name="Parent",Rank=5},
                IsAdmin=false,
           

            };
         
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationConfig.APISecretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                try
                {
                    var tokeOptions = new JwtSecurityToken(
                        issuer: ApplicationConfig.TokenDomain,
                        audience: ApplicationConfig.TokenDomain,
                         claims: new List<Claim>(new List<Claim> {
                           new Claim("IsAdmin","false",ClaimValueTypes.Boolean),
                             new Claim("IsAdviser","false",ClaimValueTypes.Boolean),
                            new Claim("UserName",dataModel.EmailAddress),
                            new Claim("FirstName",dataModel.FirstName),
                            new Claim("LastName",dataModel.LastName),
                            new Claim("EmailAddress",dataModel.EmailAddress),
                            new Claim("Schcode",dataModel.Schcode==null?"":dataModel.Schcode),
                            new Claim("SchoolName",_schname),
                            new Claim("Role","Parent"),
                            new Claim("Rank","5"),
                              new Claim("Id",userID),
                                    }),
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    data.Token = tokenString;
                    processingResult.Data = data;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok(new { ApiProcessingResult = processingResult });
            }
        [HttpPost, Route("forgotPassword")]
        public async Task<ActionResult> forgotPassword(ForgotPasswordModel dataModel)
        {

            var processingResult = new ApiProcessingResult<bool>();
            if (dataModel == null)
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError(" Data was not received, please try again.", "Data was not received, please try again.", ""));
                return Ok(new { ApiProcessingResult = processingResult });
            }

            var emailResult =await new ApplicationUserDataService().ForgotPassword(dataModel);
            if (emailResult.IsError)
            {
                processingResult.IsError = true;
                processingResult.Errors = emailResult.Errors;
                
            }
           
            return Ok(new { ApiProcessingResult = processingResult });
        }

    }
    }
