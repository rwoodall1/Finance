using Core;

using SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Utilities;
using System.Data.SqlClient;
using System.Configuration;

namespace Services {
    //public class ApplicationRoleDataService {
    //    public ApiProcessingResult<List<ApplicationRole>> GetAllAsync() {
    //        var result = new ApiProcessingResult<List<ApplicationRole>>();
    //        try {
    //            var sqlClient = new SQLCustomClient();
    //            sqlClient.ConnectionString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    //            sqlClient.CommandText("SELECT * FROM AspNetRoles");
    //            var roleResult = sqlClient.SelectMany<ApplicationRole>();
    //            if (roleResult.Data != null) {
    //                result.Data = (List<ApplicationRole>)roleResult.Data;
    //            }

    //        } catch (Exception ex) {
    //            result.IsError = true;
    //            result.Errors.Add(new ApiProcessingError(ex.InnerException.Message, ex.InnerException.Message, "SQLERR"));
    //        }
    //        return result;
    //    }

    //    public ApiProcessingResult<ApplicationRole> GetApplicationRoleByRank(int rank) {
    //        var result = new ApiProcessingResult<ApplicationRole>();

    //        try {
    //            var sqlClient = new SQLCustomClient();
    //            sqlClient.ConnectionString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    //            sqlClient.AddParameter("@Rank", rank);
    //            sqlClient.CommandText("SELECT * FROM AspNetUserRoles WHERE Rank=@Rank");
    //            var roleResult = sqlClient.Select<ApplicationRole>();
    //            if (roleResult.Data != null) {
    //                result.Data = (ApplicationRole)roleResult.Data;
    //            }
    //        } catch (Exception ex) {
    //            result.IsError = true;
    //            result.Errors.Add(new ApiProcessingError(ex.InnerException.Message, ex.InnerException.Message, "SQLERR"));
    //        }
    //        return result;
    //    }
    //}
}
