using System.Globalization;

namespace Core
{
    public static class ErrorValues
    {
        public static readonly string ContactSupportUserHelp = "An error occurred, please try again later. If the problem persists, contact support";

        public static readonly string UserDoesNotExistInUserHelp = "This user does not belong to you.";
        public static readonly string UserUpdateFailedUserMessage = "Failed to edit user.";
        public static readonly string UserUpdateValidationFailedUserMessage = "User update validation failed.";
        public static readonly string UserDeleteFailedUserMessage = "Failed to delete user.";
        public static readonly string UserCreationFailedUserMessage = "Failed to create user.";
        public static readonly string UserCreationValidationFailedUserMessage = "User create validation failed.";
        public static readonly string UserNameAlreadyExistsUserHelp = "User failed validation. Username, Email or External User ID already exist in the database.";

        public static readonly string UserCrudPermissionsUserHelp = "You do not have a high enough role to perform this action on this user.  If you think this is a mistake, please contact an Administrator";
        public static readonly string CreateUserNotInUserHelp = "You are attempting to create a user in a different .";
        public static readonly string UserMustBeAssignedToTeamUserHelp = "This user must be assigned to a Manager. Please select a Manager and try again.";
        public static readonly string InvalidRoleForTeamAssignmentUserHelp = "A user with this role cannot be assigned to a Manager.";
        public static readonly string ManagerDoesntOwnTeamUserHelp = "You do not manage this Manager and you can only assign users to Managers that you manage.";
        public static readonly string CannotAssignRoleToUserUserHelp = "You do not have permission to assign this role to a user.";

        public static readonly string UserDeleteFailedDueToManagerAssignmentUserHelp = "User cannot be deleted because they are assigned as a Manager.";
        public static readonly string AdminsCannotBeDeletedUserHelp = "Unable to delete user because Administrators cannot be deleted.";

        public static readonly string GroupCreateFailedUserMessage = "Failed to create group.";
        public static readonly string GroupUpdateFailedUserMessage = "Failed to update group.";
        public static readonly string GroupGetFailedUserMessage = "Failed to get group.";
        public static readonly string GroupDeleteFailedUserMessage = "Failed to delete Manager.";
        public static readonly string GroupDeleteValidationFailedUserMessage = "Group deletion validation failed.";
        public static readonly string CannotDeleteGroupWithChildGroupsOrTeamsUserHelp = "A group that has group or Manager children cannot be deleted. If you think this is a mistake, please contact support.";
        public static readonly string CannotDeleteGroupWithManagersAssignedUserHelp = "A group that has managers assigned to it cannot be deleted. If you think this is a mistake, please contact support.";

        public static readonly string TeamCreateFailedUserMessage = "Failed to create Manager.";
        public static readonly string TeamCreateValidationFailedUserMessage = "Manager add validation failed.";
        public static readonly string TeamUpdateFailedUserMessage = "Failed to update Manager.";
        public static readonly string TeamUpdateValidationFailedUserMessage = "Manager update validation failed.";
        public static readonly string TeamDeleteFailedUserMessage = "Failed to delete Manager.";
        public static readonly string TeamDeleteValidationFailedUserMessage = "Manager deletion validation failed.";
        public static readonly string TeamAssignmentInvalidUserMessage = "User cannot be assigned to this Manager.";
        public static readonly string TeamCrudPermissionsUserHelp = "You do not have a high enough role to perform this action on a Manager.  If you think this is a mistake, please contact an Administrator";
        public static readonly string ManageTeamUserPermissionUserHelp = "You do not have permission to manage Managers in this group. If you think this is a mistake, please contact an Administrator.";
        public static readonly string ModifyTeamGroupNotUserHelp = "The group you attempted to add the Manager to does not exist within your .  If you think this is a mistake, please contact support";
        public static readonly string DeleteTeamNotUserHelp = "You are attempting to delete a Manager that does not belong to your . If you think this is a mistake, please contact support";

        public static readonly string InvalidStateUserHelp = "Please provide a valid 2 letter state code.";
        public static readonly string TeamCommissionFailedUserMessage = "Failed to create commissions for this order";
        public static readonly string TeamCommissionFailedUserHelp = "A problem occurred saving commissions for this order. An error log has been created.";

        public static readonly ApiProcessingError GENERIC_FATAL_BACKEND_ERROR = new ApiProcessingError(ContactSupportUserHelp, ContactSupportUserHelp, "ERRFBE");
        public static readonly ApiProcessingError USER_CRUD_PERMISSIONS_ERROR = new ApiProcessingError("You cannot perform this action on this user.", UserCrudPermissionsUserHelp, "");
        public static readonly ApiProcessingError CREATE_USER_ROLE_ASSIGNMENT_ERROR = new ApiProcessingError(UserCreationFailedUserMessage, CannotAssignRoleToUserUserHelp, "");
        public static readonly ApiProcessingError CREATE_USER_WITH_EXISTING_USERNAME_ERROR = new ApiProcessingError(UserCreationFailedUserMessage, UserNameAlreadyExistsUserHelp, "");
        public static readonly ApiProcessingError CREATE_USER_NOT_IN__ERROR = new ApiProcessingError(UserCreationFailedUserMessage, CreateUserNotInUserHelp, "");
        public static readonly ApiProcessingError USER_CREATION_FAILED_ERROR = new ApiProcessingError(UserCreationFailedUserMessage, ContactSupportUserHelp, "");

        public static readonly ApiProcessingError GET_USER_WITHOUT_ID_ERROR = new ApiProcessingError("Cannot get user without an Id.", "Please provide a user id and try again", "");
        public static readonly ApiProcessingError GENERIC_COULD_NOT_FIND_USER_ERROR = new ApiProcessingError("Failed to find user.", "Could not find this user.  If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError GENERIC_COULD_NOT_FIND_USERS_ERROR = new ApiProcessingError("Failed to find any users.", "Could not find any users. If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError GENERIC_COULD_NOT_FIND_MANAGERS_ERROR = new ApiProcessingError("Failed to find any managers.", "Could not find and managers. If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError USER_UPDATE_PERMISSIONS_ERROR = new ApiProcessingError(UserUpdateFailedUserMessage, "You do not have permission to edit this user.  If you think this is a mistake, please contact an Administrator.", "");
        public static readonly ApiProcessingError UPDATE_USER_INVALID_USER_ROLE_ID_PROVIDED_ERROR = new ApiProcessingError(UserUpdateFailedUserMessage, "Could not update user because the RoleId provided was invalid.", "");
        public static readonly ApiProcessingError GENERIC_UPDATE_USER_FAILED_ERROR = new ApiProcessingError(UserUpdateFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError COULD_NOT_FIND_USER_TO_UPDATE_ERROR = new ApiProcessingError(UserUpdateFailedUserMessage, UserDoesNotExistInUserHelp, "");
        public static readonly ApiProcessingError COULD_NOT_FIND_USER_TO_BEGIN_ORDER_ERROR = new ApiProcessingError("Order cannot be started.", "Failed to find User to begin order with. If the problem persists, please contact support.", "");
        public static readonly ApiProcessingError UPDATE_USER_WITH_INVALID_ROW_VERSION_ERROR = new ApiProcessingError(UserUpdateFailedUserMessage, "This user was edited by someone else while you were editing them.  Please refresh the page and make the desired changes.", "");
        public static readonly ApiProcessingError CANNOT_DELETE_USER_ASSIGNED_AS_MANAGER = new ApiProcessingError("User cannot be deleted.", UserDeleteFailedDueToManagerAssignmentUserHelp, "");
        public static readonly ApiProcessingError INVALID_ROLE_ASSIGNMENT_ERROR = new ApiProcessingError("Error creating/editing user because Role was invalid.", "An error occurred because you attempted to add an invalid role to the user. If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError GET_LOGGED_IN_USER_INFO_ERROR = new ApiProcessingError("An error occurred while getting logged in user's info.", "An error occurred while retrieving your info. If the problem persists, please contact support.", "");
        public static readonly ApiProcessingError GENERIC_VALIDATE_USER_PERMISSIONS_ERROR = new ApiProcessingError("An error occurred while validating user permissions.", "An error occurred while trying to ensure you had permission to perform this action. If the problem persists, please contact support.", "");

        public static readonly ApiProcessingError GENERIC_DELETE_USER_FAILED_ERROR = new ApiProcessingError(UserDeleteFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError COULD_NOT_FIND_USER_TO_DELETE_ERROR = new ApiProcessingError(UserDeleteFailedUserMessage, UserDoesNotExistInUserHelp, "");
        public static readonly ApiProcessingError USER_DELETE_PERMISSIONS_ERROR = new ApiProcessingError(UserDeleteFailedUserMessage, "You do not have permission to delete this user. If you think this is a mistake, please contact an Administrator", "");
        public static readonly ApiProcessingError USER_ROLE_UPDATE_FAILED_ERROR = new ApiProcessingError("Failed to update user's role.", ContactSupportUserHelp, "");
        public static readonly ApiProcessingError CANNOT_RESET_PASSWORD_FOR_USER_ROLE_ERROR = new ApiProcessingError("Could not reset password because you don't have the proper permissions.", "Your request could not be processed because you cannot perform this operation on a user with this role.", "");
        public static readonly ApiProcessingError STORAGE_CREDENTIAL_ERROR = new ApiProcessingError("Failed to retrieve storage credentials. Could not create document.", "Failed to retrieve storage credentials.", "");

        public static readonly ApiProcessingError GENERIC_GROUP_PERMISSIONS_ERROR = new ApiProcessingError("Failed to complete operation for group.", "You do not have permission to complete this operation. If you think this is a mistake, please contact an Administrator.", "");
        public static readonly ApiProcessingError GENERIC_ADD_GROUP_ERROR = new ApiProcessingError(GroupCreateFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError ADD_GROUP_PARENT_GROUP_NOT_IN__ERROR = new ApiProcessingError(GroupCreateFailedUserMessage, "You are attempting to assign this group to a parent group that does not exist within your . If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError GENERIC_GET_GROUP_WITH_MANAGER_IN_TREE_ERROR = new ApiProcessingError("Failed to get group with manager in tree.", ContactSupportUserHelp, "");
        public static readonly ApiProcessingError GENERIC_GET_GROUP_IN__ERROR = new ApiProcessingError(GroupGetFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError GENERIC_GET_ALL_GROUPS_IN__ERROR = new ApiProcessingError("Failed to find any groups in this .", "Could not find any groups in this . If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError GET_GROUP_WITHOUT_ID_ERROR = new ApiProcessingError(GroupGetFailedUserMessage, "Failed to get group because no Id was provided. Please try again.", "");
        public static readonly ApiProcessingError GENERIC_UPDATE_GROUP_ERROR = new ApiProcessingError(GroupUpdateFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError UPDATE_GROUP_NOT_IN__ERROR = new ApiProcessingError(GroupUpdateFailedUserMessage, "You are attempting to edit a group that does not belong to your .  If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError UPDATE_GROUP_PARENT_GROUP_NOT_IN__ERROR = new ApiProcessingError(GroupUpdateFailedUserMessage, "You are attempting to assign this group to a parent group that does not exist within your . If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError GENERIC_DELETE_GROUP_ERROR = new ApiProcessingError(GroupDeleteFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError DELETE_GROUP_NOT_IN__ERROR = new ApiProcessingError(GroupDeleteFailedUserMessage, "You are attempting to delete a group that does not belong to your .  If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError DELETE_GROUP_WITHOUT_ID_ERROR = new ApiProcessingError(GroupDeleteFailedUserMessage, "Failed to delete group because no Id was provided. Please try again.", "");
        public static readonly ApiProcessingError COULD_NOT_FIND_GROUP_IN__ERROR = new ApiProcessingError("Failed to find group in .", "Could not find this group in this . If you think this is a mistake, please contact support.", "");

        public static readonly ApiProcessingError TEAM_CRUD_PERMISSIONS_ERROR = new ApiProcessingError("You cannot perform this action on a Manager.", TeamCrudPermissionsUserHelp, "");
        public static readonly ApiProcessingError CREATE_TEAM_INVALID_STATE_ERROR = new ApiProcessingError(TeamCreateFailedUserMessage, InvalidStateUserHelp, "");
        public static readonly ApiProcessingError CREATE_TEAM_MANAGER_PERMISSION_ERROR = new ApiProcessingError(TeamCreateFailedUserMessage, ManageTeamUserPermissionUserHelp, "");
        public static readonly ApiProcessingError CREATE_TEAM_GROUP_NOT_IN__ERROR = new ApiProcessingError(TeamCreateFailedUserMessage, ModifyTeamGroupNotUserHelp, "");
        public static readonly ApiProcessingError GENERIC_ADD_TEAM_ERROR = new ApiProcessingError(TeamCreateFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError GENERIC_GET_TEAM_IN__ERROR = new ApiProcessingError("Failed to get Manager in .", ContactSupportUserHelp, "");
        public static readonly ApiProcessingError GET_TEAM_WITHOUT_ID_ERROR = new ApiProcessingError(GroupGetFailedUserMessage, "Failed to get group because no Id was provided. Please try again.", "");
        public static readonly ApiProcessingError COULD_NOT_FIND_TEAM_IN__ERROR = new ApiProcessingError("Failed to find Manager in .", "Could not find this Manager in this . If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError COULD_NOT_FIND_TEAMS_IN__ERROR = new ApiProcessingError("Failed to find any Managers in .", "Could not find any Managers in this . If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError UPDATE_TEAM_INVALID_STATE_ERROR = new ApiProcessingError(TeamUpdateFailedUserMessage, InvalidStateUserHelp, "");
        public static readonly ApiProcessingError GENERIC_UPDATE_TEAM_ERROR = new ApiProcessingError(TeamUpdateFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError GENERIC_UPDATE_TEAM_COMMISSIONS_ERROR = new ApiProcessingError("Failed to update Manager commissions.", ContactSupportUserHelp, "");
        public static readonly ApiProcessingError UPDATE_TEAM_NOT_IN__ERROR = new ApiProcessingError(TeamUpdateFailedUserMessage, "You are attempting to edit a Manager that does not belong to your .  If you think this is a mistake, please contact support.", "");
        public static readonly ApiProcessingError UPDATE_TEAM_GROUP_NOT_IN_ = new ApiProcessingError(TeamUpdateFailedUserMessage, "You are attempting to assign this Manager to a group that does not exist within your . If you think this is a mistake, please contact support", "");
        public static readonly ApiProcessingError GENERIC_DELETE_TEAM_ERROR = new ApiProcessingError(TeamDeleteFailedUserMessage, ContactSupportUserHelp, "");
        public static readonly ApiProcessingError DELETE_TEAM_NOT_IN__ERROR = new ApiProcessingError(TeamDeleteFailedUserMessage, DeleteTeamNotUserHelp, "");
        public static readonly ApiProcessingError DELETE_TEAM_WITHOUT_ID_ERROR = new ApiProcessingError(TeamDeleteFailedUserMessage, "Please provide the Id of the Manager you would like to delete.", "");
        public static readonly ApiProcessingError MANAGER_DOESNT_OWN_TEAM_ERROR = new ApiProcessingError(TeamAssignmentInvalidUserMessage, ManagerDoesntOwnTeamUserHelp, "");
        public static readonly ApiProcessingError INVALID_ROLE_FOR_TEAM_ASSIGNMENT_ERROR = new ApiProcessingError(TeamAssignmentInvalidUserMessage, InvalidRoleForTeamAssignmentUserHelp, "");
        public static readonly ApiProcessingError TEAM_ASSIGNMENT_VALIDATION_FAILED_ERROR = new ApiProcessingError("Failed to validate Manager assignment.", ContactSupportUserHelp, "");
        public static readonly ApiProcessingError TEAM_COMMISSION_CREATION_ERROR = new ApiProcessingError(TeamCommissionFailedUserMessage, TeamCommissionFailedUserHelp, "");

        public static readonly ApiProcessingError GENERIC_SEARCH_WITHOUT_CRITERIA_ERROR = new ApiProcessingError("Failed to search.", "Search could not be completed because no criteria was provided.", "");
        public static readonly ApiProcessingError GENERIC_SEARCH_ERROR = new ApiProcessingError("An error occurred while completing search.", ContactSupportUserHelp, "");

        //systems
        public static readonly ApiProcessingError CANNOT_GET_SYTEMS = new ApiProcessingError("Problem retrieving systems status, please contact support", "Problem retrieving systems status, please contact support", "");
        public static readonly ApiProcessingError GENERIC_UPDATE_SystemStatus_FAILED_ERROR = new ApiProcessingError("Failed to edit system status.", "Failed to edit system status.", "");
        public static readonly ApiProcessingError COULD_NOT_FIND_SYSTEM_TO_UPDATE_ERROR = new ApiProcessingError("Could not find user to update.", "Could not find user to update.", "");
        public static readonly ApiProcessingError COULD_DELETE_SYSTEMSTATUS_ERROR = new ApiProcessingError("Could not remove system status.", "Could not remove system status.", "");
        public static readonly ApiProcessingError GENERIC_SYSTEM_CREATE_ERROR = new ApiProcessingError("A new system status failed to be created.", "A new system status failed to be created.", "");
        public static readonly ApiProcessingError GENERIC_DELETE_SYSTEM_ERROR = new ApiProcessingError("Failed to delete system.", "Failed to delete system.", "");
        public static readonly ApiProcessingError COULD_NOT_FIND_CompaySystem_TO_UPDATE_ERROR = new ApiProcessingError("Could not find  system to update.", "Could not find  system to update.", "");
        public static readonly ApiProcessingError GENERIC_UPDATE__FAILED_ERROR = new ApiProcessingError("Failed to edit .", "Failed to edit .", "");
        public static readonly ApiProcessingError GENERIC__CREATE_ERROR = new ApiProcessingError("A new  failed to be created.", "A new  failed to be created.", "");
        public static readonly ApiProcessingError GENERIC_DELETE__ERROR = new ApiProcessingError("Failed to remove .", "Failed to remove .", "");
        public static readonly ApiProcessingError DELETE__WITHOUT_ID_ERROR = new ApiProcessingError("Please provide the Id of the  you would like to delete.", "Please provide the Id of the  you would like to delete.", "");
        public static readonly ApiProcessingError UPDATE__PARENT_SYSTEMSTATUS_NOT_IN__ERROR = new ApiProcessingError("Failed to update  system status.", "Failed to update  system status.", "");
        public static readonly ApiProcessingError Commission_Exceeds_Limit = new ApiProcessingError("Commission exceeds limits.", "Commission exceeds limits.", "");
        public static readonly ApiProcessingError PERMISSION_ERROR = new ApiProcessingError("You do not have the permission to execute this function.", "You do not have the permissions to execute this function.", "");

    }
}
