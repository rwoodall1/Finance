
export class ReconciledData {
  public statementDate: Date;
  public seviceCharge: number;
  public serviceChargeDate: Date;
  public previousStatementDate: Date;
  public serviceChargeAccount: number;
  public interestDate: Date;
  public interestCharge: number;
  public intererestAccount: number;
  public beginningBalance:number
  public endingBalance: number;
  public reconciledBalance: number;
  public accountId:number
  public reconciled: boolean;
  public id: number;

}
export class TransActionCrudModel {
 public accountName: string;
  public transActionId: number;
  public payeeId: number;
  public accountId: number;
  public debit: number;
  public credit: number;
  public childAccountId: number;
  public transType: string;
  public refNumber: string;
  public memo: string;
  public reconciled: boolean;
  public reconciledId: number;
  public clr: boolean;
  public transDate: Date;
  public balance: number;

}
export class addModidfyData {
  constructor(data,type) {
    this.data = data;
    this.transType = type;
  }
  public data: TransActionCrudModel;
  public transType: string;

}
export class Balances {
  public reconciledBalance: number;
  public endingBalance: number;
  public statementDate: Date;
  public previousStatementDate: Date;
  public reconciled: boolean;
  public accountId: number;
  public id: number;

}
