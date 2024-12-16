using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
//using LinqKit;
using Core;

using System.IdentityModel.Tokens.Jwt;
using System.Data.SqlClient;
using SqlClient;
using ApiBindingModels;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Utilities;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Services
{
    public class ApplicationUserDataService : BaseDataService
    {
        public ApplicationUserDataService(LoggedInUser _loggedInUser) : base(_loggedInUser)
        {

        }
        public ApplicationUserDataService() 
        {

        }
        string _serviceName = "ApplicationUserDataService";

        public async Task<ApiProcessingResult> ForgotPassword(ForgotPasswordModel dataModel)
        {
            var processingResult = new ApiProcessingResult();
            string cmd = "";
            var sqlClient = new SQLCustomClient();
            if (dataModel.Schcode.ToUpper()=="ADMIN")
            {
                cmd =@"Select convert(varchar(100),DecryptByPassPhrase(@EncryptionKey,Password))As Password From ApplicationUsers AU
                                                Where AU.EmailAddress=@EmailAddress and AU.RoleId=@RoleId";
                
                sqlClient.AddParameter("@RoleId", "7d0c1aa1-44a5-48c3-8690-e4c1724f8db4");//admin
                sqlClient.AddParameter("@EmailAddress",dataModel.EmailAddress);
            }
            else
            {
                cmd = @"Select convert(varchar(100),DecryptByPassPhrase(@EncryptionKey,Password))As Password From ApplicationUsers AU
                                                Inner Join  ApplicationUser_Schcode AUS On Au.Id=AUS.UserId
                                                Where AU.EmailAddress=@EmailAddress and AUS.Schcode=@Schcode";
                sqlClient.AddParameter("@EmailAddress", dataModel.EmailAddress);
                sqlClient.AddParameter("@Schcode",dataModel.Schcode);
            }
            sqlClient.CommandText(cmd);
            sqlClient.AddParameter("@EncryptionKey", ApplicationConfig.SqlPassPhrase);
         
           
            var passworResult = sqlClient.SelectSingleColumn();
            if (passworResult.IsError)
            {
               // log.Error("Failed to retieve check results of Forgot Password:" + passworResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("There was and error checking for you email address.", "There was and error checking for you email address.",""));
                return processingResult;
            }
            if (string.IsNullOrEmpty(passworResult.Data))
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("There was no account  found with the submitted email address and school code.", "There was no account found with the submitted email address and school code.", ""));
                return processingResult;

            }
                if (!string.IsNullOrEmpty(passworResult.Data))
            {
                string subject = "Your Password";
                string toAddress =dataModel.EmailAddress;
                string body = "This is your password for your Memorybook account. </br> User name:<b>"+dataModel.EmailAddress + "</b></br>School Code:<b> " + dataModel.Schcode + " </b></br>Password: <b> " + passworResult.Data+" </b>";


                var emailResult = new EmailHelper().SendEmail(subject,toAddress,body,null);
                if (emailResult.IsError)
                {
                   // log.Error("Failed to send password email:" + emailResult.Errors[0].DeveloperMessage);
                    processingResult.IsError = true;
                    processingResult.Errors.Add(new ApiProcessingError("Failed to send password email, please check that email is a valid email address.", "Failed to send password email, please check that email is a valid email address..", ""));
                    return processingResult;
                }
            }

            return processingResult;
        }


        //public async Task<ApiProcessingResult> AddAdminUser(AdminUser user)
        //{
        //    var processingResult = new ApiProcessingResult();
        //    var sqlClient = new SQLCustomClient();
           
        //    //check if user email exist--Check if OpyProduct exists
        //    string cmdText1 = @"Select EmailAddress From ApplicationUsers WHERE EmailAddress=@EmailAddress AND RoleId=@RoleId";
        //    sqlClient.AddParameter("@EmailAddress", user.UserName);
        //    sqlClient.AddParameter("@RoleId", user.RoleId);
        //    var userCheckResult= sqlClient.SelectSingleColumn();
        //    if (userCheckResult.IsError)
        //    {
        //        log.Error("Failed to Check for duplicate user:" + userCheckResult.Errors[0].DeveloperMessage);
        //        processingResult.IsError = true;
        //        processingResult.Errors = userCheckResult.Errors;
        //        return processingResult;
        //    }
        //    if (!string.IsNullOrEmpty(userCheckResult.Data)){
               
        //        processingResult.IsError = true;
        //        processingResult.Errors.Add(new ApiProcessingError("User already exists", "User already exists",""));
        //        return processingResult;
        //    }

        //    //------------------------------------------------------------------------------------------------------------

        //    var UserSqlClient = new SQLCustomClient();
        //    UserSqlClient.IdentityColumn("Id");


        
        //        UserSqlClient.CommandText(@"
        //        INSERT INTO ApplicationUsers (FirstName,LastName,EmailAddress,UserName,Password,RoleId) VALUES(@FirstName,@LastName
        //                ,@EmailAddress,@UserName,EncryptByPassPhrase(@EncryptionKey,Cast(@Password as varchar(100))),@RoleId);
        //    ");
        //        UserSqlClient.AddParameter("@FirstName", user.FirstName);
        //        UserSqlClient.AddParameter("@LastName", user.LastName);
        //        UserSqlClient.AddParameter("@emailAddress", user.UserName);
        //        UserSqlClient.AddParameter("@UserName", user.UserName);
        //        UserSqlClient.AddParameter("@Password", user.Password);
        //        UserSqlClient.AddParameter("@RoleId", user.RoleId);
        //        UserSqlClient.AddParameter("@EncryptionKey", ApplicationConfig.SqlPassPhrase);

        //        var userResult = UserSqlClient.Insert();
        //        if (userResult.IsError)
        //        {
        //            processingResult.IsError = true;
        //            processingResult.Errors.Add(new ApiProcessingError("Error inserting admin user", userResult.Errors[0].DeveloperMessage, ""));
        //            return processingResult ;
        //        }

        //        UserSqlClient.ClearParameters();
        //        UserSqlClient.CommandText(@"
        //        INSERT INTO ApplicationRoles_User (UserId,RoleId) VALUES(@UserId,@RoleId)");

        //       string userID = userResult.Data;

        //        UserSqlClient.AddParameter("@RoleId", user.RoleId);
        //        UserSqlClient.AddParameter("@UserId", userID);
        //        var roleResult = UserSqlClient.Insert();
        //        if (roleResult.IsError)
        //        {
        //          log.Error("Failed to insert admin role:" + roleResult.Errors[0].DeveloperMessage);
        //            processingResult.IsError = true;
        //            processingResult.Errors.Add(new ApiProcessingError("Failed to insert role.", "Failed to insert role.", ""));
        //            return processingResult;
        //        }

        //    if (!string.IsNullOrEmpty(user.Schcode)) {
        //        UserSqlClient.ClearParameters();
        //        UserSqlClient.CommandText(@"
        //        INSERT INTO [dbo].[ApplicationUser_Schcode] ([Schcode],[UserId]) VALUES (@Schcode,@UserId)
        //    ");

        //        UserSqlClient.AddParameter("@Schcode", user.Schcode);//parent
        //        UserSqlClient.AddParameter("@UserId", userID);
        //        var schoolResult = UserSqlClient.Insert();
        //        if (schoolResult.IsError)
        //        {
        //            log.Error("Failed to insert associated schcode " + schoolResult.Errors[0].DeveloperMessage);
        //            processingResult.IsError = true;
        //            processingResult.Errors.Add(new ApiProcessingError("Failed to insert associated schcode.", "Failed to insert associated schcode.", ""));
        //            return processingResult ;
        //        }
        //    }
        //    EventLogger.AddEvent(new EventModel(this.LoggedInUser.UserName, _serviceName, "AddAdminUser", JsonSerializer.Serialize(user),user.Schcode, ""));
        //    return processingResult;


        //}
        //public async Task<ApiProcessingResult> UpdateAdminUser(AdminUser user)
        //{
        //    var processingResult = new ApiProcessingResult();
        //    var sqlClient = new SQLCustomClient();
        //    string cmdText = @"Update ApplicationUsers Set FirstName=@FirstName,LastName=@LastName,EmailAddress=@EmailAddress,UserName=@UserName,Password=EncryptByPassPhrase(@EncryptionKey,Cast(@Password as varchar(100)))
        //                        ,RoleId=@RoleId Where Id=@Id";
        //    sqlClient.CommandText(cmdText);
        //    sqlClient.AddParameter("@Id",user.Id);
        //    sqlClient.AddParameter("@FirstName", user.FirstName);
        //    sqlClient.AddParameter("@LastName", user.LastName);
        //    sqlClient.AddParameter("@EmailAddress", user.EmailAddress);
        //    sqlClient.AddParameter("@UserName", user.UserName);
        //    sqlClient.AddParameter("@Password", user.Password);
        //    sqlClient.AddParameter("@RoleId", user.RoleId);         
        //    sqlClient.AddParameter("@EncryptionKey", ApplicationConfig.SqlPassPhrase);


        //    var result = sqlClient.Update();
        //    if (result.IsError)
        //    {
        //        log.Error("Failed to update administrators:" + result.Errors[0].DeveloperMessage);
        //        processingResult.IsError = true;
        //        processingResult.Errors = result.Errors;
        //        return processingResult;
        //    }
        //    sqlClient.ClearParameters();
        //    sqlClient.CommandText(@"Update ApplicationRoles_User Set RoleId=@RoleId WHERE UserId=@UserId");
        //    sqlClient.AddParameter("@RoleId", user.RoleId);
        //    sqlClient.AddParameter("@UserId", user.Id);
        //    sqlClient.Update();
        //    //if it errors not stopping any thing.
        //    EventLogger.AddEvent(new EventModel(this.LoggedInUser.UserName, _serviceName, "UpdateAdminUser", JsonSerializer.Serialize(user), user.Schcode, ""));

        //    return processingResult;
        //}
        //public async Task<ApiProcessingResult<List<AdminUser>>> GetAdminUsers()
        //{
        //    var processingResult = new ApiProcessingResult<List<AdminUser>>();
        //    var sqlClient = new SQLCustomClient();
        //    string cmdText = @"Select AU.Id,AUS.Schcode,AU.EmailAddress,AU.UserName,AU.FirstName,AU.LastName,AU.RoleId,AR.Name AS Role
        //                    ,convert(varchar(100),DecryptByPassPhrase(@EncryptionKey,Password))As Password From ApplicationUsers AU
        //                        Left Join ApplicationUser_Schcode AUS ON AU.Id=AUS.UserId
        //                        Left Join ApplicationRoles AR ON AU.RoleId=AR.Id  WHERE AR.Name=@Administrator OR AR.Name=@Adviser Order By Role,LastName";
        //    sqlClient.CommandText(cmdText);
        //    sqlClient.AddParameter("@Administrator","Administrator");
        //    sqlClient.AddParameter("@Adviser", "Adviser");
        //    sqlClient.AddParameter("@EncryptionKey", ApplicationConfig.SqlPassPhrase);

        //    var result = sqlClient.SelectMany<AdminUser>();
        //    if (result.IsError)
        //    {
        //        log.Error("Failed to retriever administrators:" + result.Errors[0].DeveloperMessage);
        //        processingResult.IsError = true;
        //        processingResult.Errors = result.Errors;
        //        return processingResult;
        //    }
        //    processingResult.Data = (List<AdminUser>)result.Data;

        //    return processingResult;
        //}
 
  
        public async Task<ApiProcessingResult<UserProfile>> GetUserProfile(string userId)
        {
            var processingResult = new ApiProcessingResult<UserProfile>();
            //var sqlClient = new SQLCustomClient();
            //sqlClient.CommandText(@"
            //  Select UserName,EmailAddress,Id,FirstName,LastName,ShipFirstName,ShipLastName,ShipStreet,ShipCity,ShipZipCode,ShipState,BillFirstName,BillLastName,
            //   BillStreet,BillCity,BillZipCode,BillState From ApplicationUsers Where Id=@Id
            //");
            //sqlClient.AddParameter("@Id",userId);
            
            //var selectResult = sqlClient.Select<UserProfile>();
            //if (selectResult.IsError)
            //{
            //    processingResult.IsError = true;
            //    processingResult.Errors.Add(new ApiProcessingError("Failed to retrieve user profile", selectResult.Errors[0].DeveloperMessage, ""));
            //    return processingResult;
            //}
            
            //    processingResult.Data = (UserProfile)selectResult.Data;
                return processingResult;
            
        }
        public async Task<ApiProcessingResult> UpdateUserProfile(UserProfile model)
        {
            var processingResult = new ApiProcessingResult();
            //var sqlClient = new SQLCustomClient();

            //string fields = "";
            //if (string.IsNullOrEmpty(model.Password))
            //{
            //    fields = @"Update ApplicationUsers Set FirstName=@FirstName
            //,LastName=@LastName,EmailAddress=@EmailAddress,ShipFirstName=@ShipFirstName,UserName=@UserName
            //,ShipLastName=@ShipLastName,ShipStreet=@ShipStreet,ShipCity=@ShipCity,ShipState=@ShipState,ShipZipCode=@ShipZipCode
            //,BillFirstName=@BillFirstName,BillLastName=@BillLastName,BillStreet=@BillStreet,BillCity=@BillCity,BillState=@BillState,BillZipCode=@BillZipCode";
            //}
            //else
            //{
            //    fields = @"Update ApplicationUsers Set FirstName=@FirstName
            //    ,LastName=@LastName,EmailAddress=@EmailAddress,ShipFirstName=@ShipFirstName,UserName=@UserName
            //    ,ShipLastName=@ShipLastName,ShipStreet=@ShipStreet,ShipCity=@ShipCity,ShipState=@ShipState,ShipZipCode=@ShipZipCode
            //    ,BillFirstName=@BillFirstName,BillLastName=@BillLastName,BillStreet=@BillStreet,BillCity=@BillCity,BillState=@BillState,BillZipCode=@BillZipCode,
            //    Password = EncryptByPassPhrase(@EncryptionKey, Cast(@Password as varchar(100)))";
            //    sqlClient.AddParameter("Password", model.Password);
            //    sqlClient.AddParameter("@EncryptionKey", ApplicationConfig.SqlPassPhrase);

            //}

            //sqlClient.CommandText(fields + " Where Id=@Id");

            //sqlClient.AddParameter("@FirstName", model.FirstName);
            //sqlClient.AddParameter("@LastName", model.LastName);
            //sqlClient.AddParameter("@EmailAddress", model.EmailAddress);
            //sqlClient.AddParameter("@UserName", model.UserName);
            //sqlClient.AddParameter("@ShipFirstName", model.ShipFirstName);
            //sqlClient.AddParameter("@ShipLastName", model.ShipLastName);
            //sqlClient.AddParameter("@ShipStreet", model.ShipStreet);
            //sqlClient.AddParameter("@ShipCity", model.ShipCity);
            //sqlClient.AddParameter("@ShipState", model.ShipState);
            //sqlClient.AddParameter("@ShipZipCode", model.ShipZipCode);
            //sqlClient.AddParameter("@BillStreet", model.BillStreet);
            //sqlClient.AddParameter("@BillCity", model.BillCity);
            //sqlClient.AddParameter("@BillState", model.BillState);
            //sqlClient.AddParameter("@BillZipCode", model.BillZipCode);
            //sqlClient.AddParameter("@BillFirstName", model.BillFirstName);
            //sqlClient.AddParameter("@BillLastName", model.BillLastName);
            //sqlClient.AddParameter("@Id", model.Id);
            //var updateResult = sqlClient.Update();
            //if (updateResult.IsError)
            //{
            //    processingResult.IsError = true;
            //    processingResult.Errors.Add(new ApiProcessingError(updateResult.Errors[0].DeveloperMessage, "Failed to update profile", ""));
            //}
            //EventLogger.AddEvent(new EventModel(this.LoggedInUser.UserName, _serviceName, "UpdateUserProfile", JsonSerializer.Serialize(model), "", ""));

            return processingResult;


        }



        public async Task<ApiProcessingResult> DeleteUser(string userId)
        {

            var processingResult = new ApiProcessingResult();
            //check that user/schcode  exist
            var sqlClient = new SQLCustomClient();
            sqlClient.CommandText(@"Delete From ApplicationUsers Where Id=@Id");
            sqlClient.AddParameter("@Id", userId);
            var sqlResult = sqlClient.Delete();
            if (sqlResult.IsError)
            {
                //log.Error("Failed to remove user with id:"+userId+": "+sqlResult.Errors[0].DeveloperMessage);
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError(sqlResult.Errors[0].DeveloperMessage, sqlResult.Errors[0].DeveloperMessage, ""));
                return processingResult;
            }
            EventLogger.AddEvent(new EventModel(this.LoggedInUser.UserName, _serviceName, "DeleteUser",userId, "", ""));
            return processingResult;
        }

        //public async Task<ApiProcessingResult> SaveChild(ChildInfo model)
        //{

        //    var processingResult = new ApiProcessingResult();
        //    var sqlClient = new SQLCustomClient();
        //    if (string.IsNullOrEmpty(model.Id))
        //    {

        //        sqlClient.CommandText(@"
        //            Insert Into ClassChildren (FirstName,LastName,Teacher,Class,ParentId) Values(@FirstName,@LastName,@Teacher,@Class,@ParentId)
        //        ");
        //        sqlClient.AddParameter("@FirstName",model.FirstName);
        //        sqlClient.AddParameter("@LastName",model.LastName);
        //        sqlClient.AddParameter("@Teacher",model.Teacher);
        //        sqlClient.AddParameter("@Class",model.Class);
        //        sqlClient.AddParameter("@ParentId",model.ParentId);
        //        var sqlResult = sqlClient.Insert();
        //        if (sqlResult.IsError)
        //        {
        //            processingResult.IsError = true;
        //            processingResult.Errors.Add(new ApiProcessingError(sqlResult.Errors[0].DeveloperMessage, sqlResult.Errors[0].DeveloperMessage, ""));
        //            return processingResult;
        //        }
        //    }
        //    else
        //    {
        //        sqlClient.CommandText(@"
        //            Update ClassChildren Set FirstName=@FirstName,LastName=@LastName,Teacher=@Teacher,Class=@Class,ParentId=@ParentId WHERE Id=@Id
        //        ");
        //        sqlClient.AddParameter("@FirstName", model.FirstName);
        //        sqlClient.AddParameter("@LastName", model.LastName);
        //        sqlClient.AddParameter("@Teacher", model.Teacher);
        //        sqlClient.AddParameter("@Class", model.Class);
        //        sqlClient.AddParameter("@ParentId", model.ParentId);
        //        sqlClient.AddParameter("@Id", model.Id);
        //        var sqlResult = sqlClient.Update();
        //        if (sqlResult.IsError)
        //        {
        //            processingResult.IsError = true;
        //            processingResult.Errors.Add(new ApiProcessingError(sqlResult.Errors[0].DeveloperMessage, sqlResult.Errors[0].DeveloperMessage, ""));
        //            return processingResult;
        //        }
        //    }

        //    return processingResult;
        //}
        //public async Task<ApiProcessingResult> RemoveChild(string Id)
        //{

        //    var processingResult = new ApiProcessingResult();
        //    var sqlClient = new SQLCustomClient();


        //        sqlClient.CommandText(@"
        //            Delete From ClassChildren Where Id=@Id
        //        ");

        //        sqlClient.AddParameter("@Id",Id);
        //        var sqlResult = sqlClient.Delete();
        //        if (sqlResult.IsError)
        //        {
        //            processingResult.IsError = true;
        //            processingResult.Errors.Add(new ApiProcessingError(sqlResult.Errors[0].DeveloperMessage, sqlResult.Errors[0].DeveloperMessage, ""));
        //            return processingResult;
        //        }



        //    return processingResult;
        //}

      

    }
}
 