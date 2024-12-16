import { TreeNode } from 'primeng/api';
export class AccountDropDown {
  public accountType: string;
  public accounts: Array<AccountNameList>
}
export class AccountNameList {
  public name: string;
  public id: number;

}
export class AccountModel {
  public id: number;
  public accountName: string;
  public parentAccountId: number;
  public name: string;
  public description: string;
  public accountNo: string;
  public accountType: string;
  public accountTypedId: number;
  public regColor: number;
  public accountBalance: number;
  public isActive: boolean;
  public isSubAccount: boolean;
  public subAccounts: Array<AccountModel>;
}
export class AccountType {
  public name: string;
  public id: number;
  public groupId: number;
  public abbrev: string;

}
export class SubAccount {
  public name: string;
  public id: number;
  public accountType: string;
}
export class ChartOfAccounts implements TreeNode {
  constructor() {

  }
}
export class NameLkpModel {
  public FullName: string;
  public NameType: string;
  public Id:number;


}





