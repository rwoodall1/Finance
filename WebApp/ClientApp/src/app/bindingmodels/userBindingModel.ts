export class ParentInformation {
  public firstName: string;
  public lastName: string;
  public emailAddress: string;
}
export class AdminUser {
  public firstName:string
  public lastName: string
  public emailAddress: string
  public userName:string
  public password: string
  public roleId:string
  public role: LoggedInRole

  public id:string


}
export class LogInData {
 
  public password: string;
  public userName: string;
}
export class ForgotPasswordData {
  public emailaddress: string;
 
}
export class LoggedInUser {
  public firstName: string;
  public lastName: string;
  public emailAddress: string;
  public token: string;

  public role: LoggedInRole;
  public roleId: string;
  
  public userName: string;
  public isAdmin: boolean;

  public id: string;
}
export class LoggedInRole {
  public name: string;
  public rank: number;
}
export class NewUserModel {
  public firstName: string;
  public lastName: string ;
  public emailAddress: string;
  public passWord: string;
  public confirmPassWord: string ;

   public dateCreated: Date;
  public dateModified: Date;
  public isActive: boolean;
  public isDeleted: boolean;

  
}
export class UserOfList {
    public emailAddress: string
    public firstName: string 
    public lastName: string 
    public signUpDate: Date
    public schcode: string 
    public id: string
}
export class UserProfile {
  public emailAddress: string;
  public userName: string;
  public firstName:string;
  public lastName: string;
 
  public password: string;

  public id: string;
}
export class ReportCustomers {
  public emailAddress: string;
  public firstName: string;
  public lastName: string;
  public shipFirstName: string;
  public shipLastName: string;
  public shipStreet: string;
  public shipCity: string;
  public shipState: string;
  public shipZipCode: string;
  public billFirstName: string;
  public billLastName: string;
  public billStreet: string;
  public billCity: string;
  public billState: string;
  public billZipCode: string;
  public password: string;
  public schcode: string;
  public signUpDate: Date;
  public id: string;

}

  




