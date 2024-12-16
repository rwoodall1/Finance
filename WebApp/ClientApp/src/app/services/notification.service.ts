//#region Imports
import { Injectable } from '@angular/core';
import { ApiProcessingError } from '../bindingmodels/coreBindingModel';
import { ToastrService, ActiveToast } from 'ngx-toastr';


import { Observable } from 'rxjs';

//#endregion

@Injectable()
export class NotificationService {

  constructor(public toastrService: ToastrService) {

  }

  //#region Toastr
  displaySuccess(successMessage: string) {
    let id = this.toastrService.success(successMessage, "Success", { timeOut: 5000 }).toastId;
    setTimeout(() => { this.toastrService.remove(id); }, 5000);
  }

  displayError(errorMessage: string) {
    let id = this.toastrService.error(errorMessage, "Error").toastId;
    // setTimeout(() => { this.toastrService.remove(id); }, 60000);
  }

  displayErrors(errors: ApiProcessingError[]) {
    for (let error of errors) {
      let id = this.toastrService.error(error.errorMessage, "Error").toastId;
      // setTimeout(() => { this.toastrService.remove(id); }, 60000);
    }
  }

  displayWarning(warningMessage: string) {
    let id = this.toastrService.warning(warningMessage, "Warning").toastId;
    setTimeout(() => { this.toastrService.remove(id); }, 60000);
  }

  displayInfo(infoMessage: string) {
    let id = this.toastrService.info(infoMessage, "Info").toastId;
    setTimeout(() => { this.toastrService.remove(id); }, 10000);
  }

  display(message: string) {
    let id = this.toastrService.show(message).toastId;
    setTimeout(() => { this.toastrService.remove(id); }, 10000);
  }

  clear() {
    this.toastrService.clear();
  }
  //#endregion

}
