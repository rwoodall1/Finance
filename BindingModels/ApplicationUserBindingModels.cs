using System.ComponentModel.DataAnnotations;

using System;

namespace ApiBindingModels
{

    public class AdminUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
        public string Schcode { get; set; }
    }



    public class UserProfile
    {
        public string EmailAddress{get;set;}
        public string UserName { get; set; }
        public string FirstName{get;set;}
        public string LastName{get;set;} 
        public string ShipFirstName{get;set;} 
        public string ShipLastName{get;set;} 
        public string ShipStreet{get;set;}
        public string ShipCity { get; set; }
        public string ShipState{get;set;} 
        public string ShipZipCode{get;set;} 
        public string BillFirstName{get;set;} 
        public string BillLastName{get;set;} 
        public string BillStreet{get;set;} 
        public string BillCity { get; set; }
        public string BillState{get;set;} 
        public string BillZipCode{get;set;} 
        public string Password{get;set;} 
        public string Id{get;set;} 
    }
    public class UserOfList
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime SignUpDate { get; set; }
        public string Schcode { get; set; }
        public string Id { get; set; }
    }
    public class ReportUserOfList
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShipFirstName { get; set; }
        public string ShipLastName { get; set; }
        public string ShipStreet { get; set; }
        public string ShipCity { get; set; }
        public string ShipState { get; set; }
        public string ShipZipCode { get; set; }
        public string BillFirstName { get; set; }
        public string BillLastName { get; set; }
        public string BillStreet { get; set; }
        public string BillCity { get; set; }
        public string BillState { get; set; }
        public string BillZipCode { get; set; }
        public DateTime SignUpDate { get; set; }
        public string Schcode { get; set; }
        public string Id { get; set; }
    }
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(LoggedInUser user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            Token = token;
        }
    }
    public class LoggedInUser
    {
        //public LoggedInUser()
        //{
        //    UserName = EmailAddress;
        //}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public int Rank { get; set; }
        public string Schcode { get; set; }
        public string SchoolName { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
    }
    public class ChildInfo
    {
       
        public string FullName { get; set; }
       
        public string Teacher { get; set; }
      
        public string Class { get; set; }
  
    }
    public class OrderRetLoginUser: RetLoginUser
    {
        public int Invno { get; set; }
    }
    public class RetLoginUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
      
        public RetLoginUserRole Role{get;set;}
       
        public bool IsAdmin { get; set;}
    
        public string Id { get; set; }
    }
    public class RetLoginUserRole { 
        public string Name { get; set; }
        public int Rank { get; set; }
    }
    public class UserCheck
    {
        public string RoleId { get; set; }
        public string EmailAddress { get; set; }
        public string Schcode { get; set; }
        public string UserId { get; set; }
        public string NameTitle { get; set; }
    }
    public class UserBindingModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "First Name Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "School Code Required")]
        public string Schcode { get; set; }
        [Required(ErrorMessage = "Login Value Required")]
        public string EmailAddress { get; set; }
        public string ClientID { get; set; }
        public bool IsActive { get; set; }
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

    }
    public class ForgotPasswordModel
    {
        public string EmailAddress { get; set; }
        public string Schcode { get; set; }
    }
}

    
