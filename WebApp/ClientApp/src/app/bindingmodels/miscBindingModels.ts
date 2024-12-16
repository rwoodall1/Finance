/*import { TransferListItem } from "worker_threads";*/

export class EmailModel {
  constructor(_ToAddress:string,_Subject:string,_Body:string,_Attachement:OutlookAttachement) {
    this.toAddress = _ToAddress;
    this.subject = _Subject;
    this.body = _Body;
    this.attachement = _Attachement;
  }
    public toAddress:string  
    public subject:string 
    public body:string
    public attachement: OutlookAttachement
  }

  export class OutlookAttachement {
    public  path:string 
    public  name:string
  }

export class EnvironmentInfo {
  public environment: string;
  public isDeveloperMachine: boolean;
}
export class State {
  public name: string;
  public abrev: string;

}
export class FunctionBalances {
  constructor() {
    this.depositesCleared = 0;
    this.depositesAmt = 0;
    this.checksCleared = 0;
    this.checksClearedAmt = 0;
    this.clearedBalance = 0;
    this.difference = 0;

  }
  public depositesCleared: number;
  public depositesAmt: number;
  public checksCleared: number;
  public checksClearedAmt: number;
  public clearedBalance: number
  public difference: number;
}
