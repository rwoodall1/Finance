using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
   public class NumberToWords
    {
        private NumberToWords()
        {

        }
        private static String[] units = { "Zero", "One", "Two", "Three",
    "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
    "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
    "Seventeen", "Eighteen", "Nineteen" };
        private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
    "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public static String ConvertAmount(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return Convert(amount_int) + "00/100";
                }
                else
                {
                    return Convert(amount_int)  + amount_dec.ToString() + "/100";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }

        public static String Convert(Int64 i)
        {
            if (i < 20)
            {
                return units[i] + " and ";
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + Convert(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred and "
                        + ((i % 100 > 0) ? " " + Convert(i % 100) : "");
            }
            if (i < 100000)
            {
                return Convert(i / 1000).Replace(" and ","") + " Thousand and "
                + ((i % 1000 > 0) ? " " + Convert(i % 1000) : "");
            }
            if (i < 1000000)
            {
                return Convert(i / 100000).Replace(" and ", "") + " Hundred Thousand and "
                        + ((i % 100000 > 0) ? " " + Convert(i % 100000) : "");
            }
            if (i < 10000000)
            {
                return Convert(i / 1000000).Replace(" and ", "") + " Million and "
                        + ((i % 1000000 > 0) ? " " + Convert(i % 1000000) : "");
            }
            if (i < 100000000)
            {
                return Convert(i / 1000000).Replace(" and ","") + "  Million and "
                        + ((i % 1000000 > 0) ? " " + Convert(i % 1000000) : "");
            }


            return Convert(i / 100000000).Replace(" and ", "") + " Hundred Million and "
                    + ((i % 100000000 > 0) ? " " + Convert(i % 100000000) : "");
        }
    }
}
