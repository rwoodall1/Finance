import { Injectable } from '@angular/core';
import { State } from './../bindingmodels/miscBindingModels';


@Injectable()
export class Global {
    getParameterByName(name: any) {
        let url = window.location.href;
        name = name.replace(/[[]]/g, "\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace("/+/g", " "));
    }

  searchList(nameKey: any, myArray: any, type: any) {
    for (var i = 0; i < myArray.length; i++) {
      if (myArray[i][type] === nameKey) {
        return i;
      } else {
        return null;
      }
   
    }
    return null;
  }
}

@Injectable()
export class Constants {
    get create() {
        return 0
    }
    get modify() {
        return 1
    }
    get superadministratorRoleName() {
        return "Super Administrator"
    }
    get superadministratorRoleRank() {
        return 0
    }
    get administratorRoleName() {
        return "Administrator"
    }
    get administratorRoleRank() {
        return 1
    }
    get states() {
      let states: State[] = [
        {
          "name": "Alabama",
          "abrev": "AL"
        },
        {
          "name": "Alaska",
          "abrev": "AK"
        },
        {
          "name": "Arizona ",
          "abrev": "AZ"
        },
        {
          "name": "Arkansas",
          "abrev": "AR"
        },
        {
          "name": "California ",
          "abrev": "CA"
        },
        {
          "name": "Colorado ",
          "abrev": "CO"
        },
        {
          "name": "Connecticut",
          "abrev": "CT"
        },
        {
          "name": "Delaware",
          "abrev": "DE"
        },
        {
          "name": "Florida",
          "abrev": "FL"
        },
        {
          "name": "Georgia",
          "abrev": "GA"
        },
        {
          "name": "Hawaii",
          "abrev": "HI"
        },
        {
          "name": "Idaho",
          "abrev": "ID"
        },
        {
          "name": "Illinois",
          "abrev": "IL"
        },
        {
          "name": "Indiana",
          "abrev": "IN"
        },
        {
          "name": "Iowa",
          "abrev": "IA"
        },
        {
          "name": "Kansas",
          "abrev": "KS"
        },
        {
          "name": "Kentucky",
          "abrev": "KY"
        },
        {
          "name": "Louisiana",
          "abrev": "LA"
        },
        {
          "name": "Maine",
          "abrev": "ME"
        },
        {
          "name": "Maryland",
          "abrev": "MD"
        },
        {
          "name": "Massachusetts",
          "abrev": "MA"
        },
        {
          "name": "Michigan",
          "abrev": "MI"
        },
        {
          "name": "Minnesota",
          "abrev": "MN"
        },
        {
          "name": "Mississippi",
          "abrev": "MS"
        },
        {
          "name": "Missouri",
          "abrev": "MO"
        },
        {
          "name": "Montana",
          "abrev": "MT"
        },
        {
          "name": "Nebraska",
          "abrev": "NE"
        },
        {
          "name": "Nevada",
          "abrev": "NV"
        },
        {
          "name": "New Hampshire",
          "abrev": "NH"
        },
        {
          "name": "New Jersey",
          "abrev": "NJ"
        },
        {
          "name": "New Mexico",
          "abrev": "NM"
        },
        {
          "name": "New York",
          "abrev": "NY"
        },
        {
          "name": "North Carolina",
          "abrev": "NC"
        },
        {
          "name": "North Dakota",
          "abrev": "ND"
        },
        {
          "name": "Ohio",
          "abrev": "OH"
        },
        {
          "name": "Oklahoma",
          "abrev": "OK"
        },
        {
          "name": "Oregon",
          "abrev": "OR"
        },
        {
          "name": "Pennsylvania",
          "abrev": "PA"
        },
        {
          "name": "Rhode Island",
          "abrev": "RI"
        },
        {
          "name": "South Carolina",
          "abrev": "SC"
        },
        {
          "name": "South Dakota",
          "abrev": "SD"
        },
        {
          "name": "Tennessee",
          "abrev": "TN"
        },
        {
          "name": "Texas",
          "abrev": "TX"
        },
        {
          "name": "Utah",
          "abrev": "UT"
        },
        {
          "name": "Vermont",
          "abrev": "VT"
        },
        {
          "name": "Virginia ",
          "abrev": "VA"
        },
        {
          "name": "Washington",
          "abrev": "WA"
        },
        {
          "name": "West Virginia",
          "abrev": "WV"
        },
        {
          "name": "Wisconsin",
          "abrev": "WI"
        },
        {
          "name": "Wyoming",
          "abrev": "WY"
        }
      ];

        return states;
    }

    get monthOptions() {
        let monthOptions = [
            { name: "January (1)", number: '01', daysInMonth: 31 },
            { name: "February (2)", number: '02', daysInMonth: 29 },
            { name: "March (3)", number: '03', daysInMonth: 31 },
            { name: "April (4)", number: '04', daysInMonth: 30 },
            { name: "May (5)", number: '05', daysInMonth: 31 },
            { name: "June (6)", number: '06', daysInMonth: 30 },
            { name: "July (7)", number: '07', daysInMonth: 31 },
            { name: "August (8)", number: '08', daysInMonth: 31 },
            { name: "September (9)", number:'09', daysInMonth: 30 },
            { name: "October (10)", number: '10', daysInMonth: 31 },
            { name: "November (11)", number: '11', daysInMonth: 30 },
            { name: "December (12)", number: '12', daysInMonth: 31 }
        ];

        return monthOptions;
    }

    get dobMaxYear() {
        return new Date().getFullYear();
    }

    get dobMinYear() {
        return new Date().getFullYear() - 110;
    }

    get dateFormat() {
        return 'MM-dd-yyyy';
    }
    get dateFormatMoment() {
        return 'MM/DD/YYYY';
    }
    get dateTimeFormat() {
        return 'MM/dd/yyyy hh:mma';
    }
    get dateTimeFormatMoment() {
        return 'MM/DD/YYYY hh:mma z';
    }
}
