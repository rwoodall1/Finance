using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Reflection.PortableExecutable;
using System.IO;
namespace BindingModels
{
    public class ImportOfx
    {
        public STMTTRN transaction { get; set; }

        
        public async Task<ApiProcessingResult<List<STMTTRN>>>  OfxToObject(IFormFile file)
        {
            var processingResult = new ApiProcessingResult<List<STMTTRN>>();
            if (file==null)
            {
                processingResult.IsError = true;
                processingResult.Errors.Add(new ApiProcessingError("Bank file is null", "Bank file is null", ""));
                return processingResult;
            }

            var strFile = await file.ReadAsStringAsync();
            System.IO.TextReader Reader = new System.IO.StringReader(strFile);
        ;
            //use LINQ TO GET ONLY THE LINES THAT WE WANT

            var tags = from line in Reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                       where line.Contains("<ACCTID>") ||
                       line.Contains("<STMTTRN>") ||
                       line.Contains("</STMTTRN>") ||
                       line.Contains("<TRNTYPE>") ||
                       line.Contains("<DTPOSTED>") ||
                       line.Contains("<TRNAMT>") ||
                       line.Contains("<FITID>") ||
                       line.Contains("<CHECKNUM>") ||
                       line.Contains("<NAME>") ||
                       line.Contains("<MEMO>")
                       select line;
            

            var vTransactions = new List<STMTTRN>();
            STMTTRN _TransAction;
            string _BankAccountId = "";
            foreach (var l in tags)
            {
                if (l.IndexOf("<ACCTID>") != -1)
                {
                    _BankAccountId = getTagValue(l);

                    continue;
                }

                if (l.IndexOf("<STMTTRN>") != -1)
                {
                    transaction = new STMTTRN() { BankAccountNo = _BankAccountId };


                    continue;
                }
                if (l.IndexOf("</STMTTRN>") != -1)
                {
                    vTransactions.Add(transaction);


                    continue;
                }

                var tagName = getTagName(l);
                switch (tagName)
                {

                    case "TRNTYPE":
                        var trnType = getTagValue(l);
                        transaction.TRNTYPE = getTagValue(l);
                        break;
                    case "DTPOSTED":
                        var stringDate = getTagValue(l);
                        string _date = stringDate.Substring(0, 4) + "-" + stringDate.Substring(4, 2) + "-" + stringDate.Substring(6, 2);
                        transaction.DTPOSTED = DateTime.Parse(_date);
                        break;
                    case "TRNAMT":
                        string rawVal1 = getTagValue(l);
                        decimal val1 = 0;
                        if (decimal.TryParse(rawVal1, out val1))
                        {
                            transaction.TRNAMT = val1;
                        }
                        break;
                    case "FITID":
                        transaction.FITID = getFITIDTagValue(l);


                        break;
                    case "MEMO":
                        transaction.MEMO = getTagValue(l);
                        break;
                    case "NAME":
                        transaction.NAME = getTagValue(l);
                        break;
                    case "CHECKNUM":
                        transaction.CHECKNUM = getTagValue(l);
                        break;

                }

            }
            processingResult.Data = vTransactions;
            return processingResult;
          

        }


        private string getTagName(string line)
        {
            int pos_init = line.IndexOf("<") + 1;
            int pos_end = line.IndexOf(">");
            pos_end = pos_end - pos_init;
            return line.Substring(pos_init, pos_end);
        }
        ///// <summary>
        ///// Get the value of the tag to put on the Xelement
        ///// </summary>
        ///// <param name="line">The line</param>
        ///// <returns></returns>
        private string getTagValue(string line)
        {
            int pos_init = line.IndexOf(">") + 1;
            string retValue = line.Substring(pos_init).Trim();
            if (retValue.IndexOf("[") != -1)
            {
                //date--lets get only the 8 date digits
                retValue = retValue.Substring(0, 8);
            }
            return retValue;
        }
        private string getFITIDTagValue(string line)
        {
            int pos_init = line.IndexOf(">") + 1;
            string retValue = line.Substring(pos_init).Trim();
            //if (retValue.IndexOf("[") != -1)
            //{
            //    //date--lets get only the 8 date digits
            //    retValue = retValue.Substring(0, 8);
            //}
            return retValue;
        }
    }
    public class STMTTRN
    {
        public string BankAccountNo { get; set; }
        public string TRNTYPE { get; set; }
        public DateTime DTPOSTED { get; set; }
        public decimal TRNAMT { get; set; }
        public string FITID { get; set; }
        public string CHECKNUM { get; set; }
        public string NAME { get; set; }
        public string MEMO { get; set; }
        public bool Imported { get; set; }
        public DateTime DateImported { get; set; }
    }
    public static class FormFileExtensions
    {
        public static async Task<string> ReadAsStringAsync(this IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }
        public static string ReadAsList(this IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            return result.ToString();
        }


    }

    }

