using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingModels
{
    public class ListStates
    {
         static string[,] states = new string[51,2] {
                        { "ALABAMA", "AL" },
                        { "ALASKA", "AK" },
                        { "ARIZONA", "AZ" },
                        { "ARKANSAS", "AR" },
                        { "CALIFORNIA", "CA" },
                        { "COLORADO", "CO" },
                        { "CONNECTICUT", "CT" },
                        { "DC", "DC" },
                        { "DELAWARE", "DE" },
                        { "FLORIDA", "FL" },
                        { "GEORGIA", "GA" },
                        { "HAWAII", "HI" },
                        { "IDAHO", "ID" },
                        { "ILLINOIS", "IL" },
                        { "INDIANA", "IN" },
                        { "IOWA", "IA" },
                        { "KANSAS", "KS" },
                        { "KENTUCKY", "KY" },
                        { "LOUISIANA", "LA" },
                        { "MAINE", "ME" },
                        { "MARYLAND", "MD" },
                        { "MASSACHUSETTS", "MA" },
                        { "MICHIGAN", "MI" },
                        { "MINNESOTA", "MN" },
                        { "MISSISSIPPI", "MS" },
                        { "MISSOURI", "MO" },
                        { "MONTANA", "MT" },
                        { "NEBRASKA", "NE" },
                        { "NEVADA", "NV" },
                        { "NEW HAMPSHIRE", "NH" },
                        { "NEW JERSEY", "NJ" },
                        { "NEW MEXICO", "NM" },
                        { "NEW YORK", "NY" },
                        { "NORTH CAROLINA", "NC" },
                        { "NORTH DAKOTA", "ND" },
                        { "OHIO", "OH" },
                        { "OKLAHOMA", "OK" },
                        { "OREGON", "OR" },
                        { "PENNSYLVANIA", "PA" },
                        { "RHODE ISLAND", "RI" },
                        { "SOUTH CAROLINA", "SC" },
                        { "SOUTH DAKOTA", "SD" },
                        { "TENNESSEE", "TN" },
                        { "TEXAS", "TX" },
                        { "UTAH", "UT" },
                        { "VERMONT", "VT" },
                        { "VIRGINIA", "VA" },
                        { "WASHINGTON", "WA" },
                        { "WEST VIRGINIA", "WV" },
                        { "WISCONSIN", "WI" },
                        { "WYOMING", "WY" }
        };
        
      
        public static List<State> GetStates()
        {

            List<State> States = new List<State>();
            for (int i = 0; i < 51; i++)
            {
                var vstate = new State()
                {
                    Name = states[i, 0].ToString(),
                    Abrev = states[i, 1].ToString()
                };
                States.Add(vstate);

            };
            return States ;
        }
       
    }
     

   public class State
    {
       public string Name { get; set; }
       public string Abrev { get; set; }
    }


}
