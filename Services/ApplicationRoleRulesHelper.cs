using Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace Services {
    //public static class ApplicationRoleRulesHelper {

    //    private static readonly string CannotAssignRoleToUserUserHelp = "You do not have permission to assign this role to a user.";

    //    public static bool IsSuperAdmin(this ApplicationRole role) {
    //        return role.Rank == ValidApplicationRoles.SuperAdmin.Rank;
    //    }

    //    public static bool IsAdmin(this ApplicationRole role) {
    //        return role.Rank == ValidApplicationRoles.Admin.Rank;
    //    }

    //    public static bool CanAssign(this ApplicationRole creatorRole, ApplicationRole roleBeingAssigned) {
    //        if (roleBeingAssigned == null || (creatorRole.Rank >= roleBeingAssigned.Rank)) {
    //            return false;
    //        }
    //        return creatorRole.Rank < roleBeingAssigned.Rank;
    //    }


    //    public static ApiProcessingResult<ValidationResult> ValidateRoleAssignmentFor(ApplicationRole roleBeingAssigned) {
    //        var processingResult = new ApiProcessingResult<ValidationResult> { IsError = false };
    //        var validationResult = new ValidationResult {
    //            IsValid = true
    //        };

    //        if (roleBeingAssigned == null) {
    //            validationResult.IsValid = false;
    //            validationResult.Errors.Add("Invalid role. Please select a valid role and try again.");
    //        } else {
    //            validationResult.IsValid = false;
    //            validationResult.Errors.Add(CannotAssignRoleToUserUserHelp);
    //        }
    //        processingResult.Data = validationResult;

    //        return processingResult;
    //    }
    //}
}
