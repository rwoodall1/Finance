//using CsvHelper;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Utilities
{
    public class ValidatedDevice
    {
        public bool IsValid { get; set; }
        public string Type { get; set; }
        public string HEX { get; set; }
        public string DEC { get; set; }
    }
    public class paramObj
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class Utils
    {
        public static string ToDelmitedString(List<string> myStringList)
        {
            if (myStringList == null)
            {
                return null;
            }
            return string.Join(",", myStringList);
        }

        public static List<paramObj> CommandParametersToObj(SqlParameter[] commandParameters)
        {
            List<paramObj> vParams = new List<paramObj>();
            foreach (var param in commandParameters)
            {
                paramObj item = new paramObj();
                item.Value = param.Value == null ? "null" : param.Value.ToString();
                item.Name = param.ParameterName.ToString();
                vParams.Add(item);
            }
            return vParams;
        }
        public static void SetObjectProperty(string field, DateTime val, object model)
        {
            var propertyInfo = model.GetType().GetProperty(field);
            // make sure object has the property we are after
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(model, val, null);
            }

        }
        public static void SetObjectProperty(string field, string val, object model)
        {
            var propertyInfo = model.GetType().GetProperty(field);
            // make sure object has the property we are after
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(model, val, null);
            }

        }
        public static void SetObjectProperty(string field, int val, object model)
        {
            var propertyInfo = model.GetType().GetProperty(field);
            // make sure object has the property we are after
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(model, val, null);
            }

        }
        public static void SetObjectProperty(string field, bool val, object model)
        {
            var propertyInfo = model.GetType().GetProperty(field);
            // make sure object has the property we are after
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(model, val, null);
            }

        }
        public static List<List<object>> Batch(List<object> source, int nSize = 30)
        {
            var list = new List<List<object>>();

            for (int i = 0; i < source.Count; i += nSize)
            {
                list.Add(source.GetRange(i, Math.Min(nSize, source.Count - i)));
            }

            return list;
        }
        public static bool IsJSON(string input)
        {
            input.Trim();
            return input.StartsWith("{") && input.EndsWith("}") || input.StartsWith("[") && input.EndsWith("]");
        }

        public static Guid RandomGuid()
        {
            return Guid.NewGuid();
        }

        public static string RandomGuidString()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool IsDeveloperMachine()
        {
            if (ApplicationConfig.IsDeveloperMachine == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDev()
        {
            if (ApplicationConfig.Environment == "DEV")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsStaging()
        {
            if (ApplicationConfig.Environment == "STAGING")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsProd()
        {
            if (ApplicationConfig.Environment == "PROD")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsBusinessHours()
        {
            return (DateTime.UtcNow.Hour >= 12 || DateTime.UtcNow.Hour < 4);
        }

        public static bool IsWakingHours()
        {
            return (DateTime.UtcNow.Hour >= 16);
        }

 






        public static string RandomAlphanumericKey(int length)
        {
            var _charsForAlphanumericKeys = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
            var chars = _charsForAlphanumericKeys.ToCharArray();
            var data = new byte[1];

            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[length];
                crypto.GetNonZeroBytes(data);
            }
            var result = new StringBuilder(length);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            return result.ToString();
        }

        public static bool IsEmail(string pEmailAddress)
        {
            bool addressIsValid = true;
            try
            {
                MailAddress mailAddress = new MailAddress(pEmailAddress);
            }
            catch
            {
                addressIsValid = false;
            }

            return addressIsValid;
        }

        public static string PhoneFormat(string PhoneNumber, string formatType = "default")
        {
            var returnedPhoneNumber = PhoneNumber;
            if (string.IsNullOrEmpty(returnedPhoneNumber))
            {
                return returnedPhoneNumber;
            }

            returnedPhoneNumber = PhoneUnFormat(returnedPhoneNumber);
            if (returnedPhoneNumber.Length != 10)
            {
                return returnedPhoneNumber;
            }

            var areacode = returnedPhoneNumber.Substring(0, 3);
            var prefix = returnedPhoneNumber.Substring(3, 3);
            var lastfour = returnedPhoneNumber.Substring(6, 4);

            if (formatType == "dots")
            {
                returnedPhoneNumber = areacode + "." + prefix + "." + lastfour;
            }
            else if (formatType == "dashes")
            {
                returnedPhoneNumber = areacode + "-" + prefix + "-" + lastfour;
            }
            else if (formatType == "spacedash")
            {
                returnedPhoneNumber = areacode + " " + prefix + "-" + lastfour;
            }
            else
            {
                returnedPhoneNumber = "(" + areacode + ") " + prefix + "-" + lastfour;
            }

            return returnedPhoneNumber;
        }

        public static bool ConvertToBooleen(string value)
        {
            var returnValue = false;
            if (value.ToString() == "1" || value.ToUpper() == "YES" || value.ToUpper() == "TRUE" || value.ToUpper() == "Y")
            {
                returnValue = true;
            }
            else if (value.ToString() == "0" || value.ToUpper() == "NO" || value.ToUpper() == "FALSE" || value.ToUpper() == "N")
            {
                returnValue = false;
            }
            else
            {
                bool boolValue;

                bool isValid = bool.TryParse(value.ToString(), out boolValue);
                returnValue = boolValue;
            }

            return returnValue;
        }

        public static string PhoneUnFormat(string PhoneNumber, string formatType = "default")
        {
            var returnedPhoneNumber = PhoneNumber;
            if (string.IsNullOrEmpty(returnedPhoneNumber))
            {
                return returnedPhoneNumber;
            }

            return Regex.Replace(returnedPhoneNumber, "[^0-9]", "NET");
        }

        public static string GeneratePassword(int passwordLength)
        {

            int iZero = 0, iNine = 0, iA = 0, iZ = 0, iCount = 0, iRandNum = 0;
            string sRandomString = string.Empty;

            //' we'll need random characters, so a Random object 
            //' should probably be created...
            Random rRandom = new Random(System.DateTime.Now.Millisecond);

            //' convert characters into their integer equivalents (their ASCII values)
            iZero = Convert.ToInt32("0");
            iNine = Convert.ToInt32("9");
            iA = Convert.ToInt32('A');
            iZ = Convert.ToInt32('Z');

            //' initialize our return string for use in the following loop
            sRandomString = string.Empty;


            //' now we loop as many times as is necessary to build the string 
            //' length we want
            while (iCount < passwordLength)
            {
                //' we fetch a random number between our high and low values
                iRandNum = rRandom.Next(iZero, iZ);

                // ' here's the cool part: we inspect the value of the random number, 
                // ' and if it matches one of the legal values that we've decided upon,  
                // ' we convert the number to a character and add it to our string
                if ((iRandNum >= iZero) && (iRandNum <= iNine) || (iRandNum >= iA) && (iRandNum <= iZ))
                {
                    if (iRandNum >= iZero && iRandNum <= iNine)
                        sRandomString = sRandomString + iRandNum.ToString();
                    else
                        sRandomString = sRandomString + Convert.ToChar(iRandNum);

                    iCount = iCount + 1;
                }

            }
            //' finally, our random character string should be built, so we return it
            return sRandomString;
        }

        public static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);
            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDate(string date)
        {
            DateTime temp;
            if (DateTime.TryParse(date, out temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidLuhn(string number)
        {
            //string[,] sumTable = new string[2,10] {{ "0","1","2","3","4","5", "6", "7", "8", "9" }, { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" } };
            //int sum = 0;
            //int flip = 0;

            //for (int i = number.Length - 1; i >= 0; i--) {
            //    sum += sumTable[flip++, 0x1][number[i]];
            //}
            //return sum % 10 == 0;

            //return number.All(char.IsDigit) && number.Reverse()
            //    .Select(c => c - 48)
            //    .Select((thisNum, i) => i % 2 == 0
            //    ? thisNum
            //    : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
            //    ).Sum() % 10 == 0;

            int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
            int checksum = 0;
            char[] chars = number.ToCharArray();
            for (int i = chars.Length - 1; i > -1; i--)
            {
                int j = ((int)chars[i]) - 48;
                checksum += j;
                if (((i - chars.Length) % 2) == 0)
                    checksum += DELTAS[j];
            }

            return ((checksum % 10) == 0);
        }

        public static bool IsValidSIM(string SIM)
        {
            // 89 = telecom
            // 01 = united states
            // {14 or 15 numbers} = carrier(first 2 digits)/sim account
            // {1 number} = luhn check digit
            Regex pattern = new Regex(@"^(89)(01)(\d{14,15})(\d{1})$");

            // check to see if the pattern is valid followed by the Luhn checksum
            var isValidPattern = pattern.Match(SIM).Success;
            var isValidLuhn = IsValidLuhn(SIM);
            if (isValidPattern && isValidLuhn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidIMEI(string imei)
        {
            if (imei.Length != 15)
            {
                return false;
            }
            int[] posImei = new int[15];
            for (int innlop = 0; innlop < 15; innlop++)
            {
                posImei[innlop] = Convert.ToInt32(imei.Substring(innlop, 1));
                if (innlop % 2 != 0) posImei[innlop] = posImei[innlop] * 2;
                while (posImei[innlop] > 9) posImei[innlop] = (posImei[innlop] % 10) + (posImei[innlop] / 10);
            }

            var totalval = 0;
            foreach (int v in posImei) totalval += v;
            if (0 == totalval % 10)
            {
                return true;
            }
            return false;
        }

        public static ValidatedDevice IsValidDevice(string input)
        {
            var result = new ValidatedDevice();
            var deviceValidationDetails = new ValidatedDevice();

            if (string.IsNullOrEmpty(input))
            {
                deviceValidationDetails.IsValid = false;
                deviceValidationDetails.Type = "Unknown";
                result = deviceValidationDetails;
                return result;
            }

            input = input.Trim();

            try
            {
                deviceValidationDetails.IsValid = false;
                deviceValidationDetails.Type = "Unknown";
                deviceValidationDetails.HEX = input;
                deviceValidationDetails.DEC = input;

                if (IsValidIMEI(input))
                {
                    deviceValidationDetails.IsValid = true;
                    deviceValidationDetails.Type = "IMEI";
                }
                else
                {
                    Regex regexSpecChar = new Regex("[^a-zA-Z0-9]+");
                    Regex regexLetters = new Regex("[a-zA-Z]+");

                    if (!regexSpecChar.IsMatch(input))
                    {
                        if (input.Length == 11 && regexLetters.IsMatch(input))
                        {
                            input = input.Substring(0, 8);
                        }

                        if (input.Length == 8 || input.Length == 11)
                        {
                            deviceValidationDetails.Type = "ESN";
                            if (input.Length == 8)
                            {
                                deviceValidationDetails.HEX = input;

                                var firstPart = Convert.ToInt32(input.Substring(0, 2), 16).ToString("D3");
                                var lastPart = Convert.ToInt32(input.Substring(2), 16).ToString("D8");

                                deviceValidationDetails.DEC = firstPart + lastPart;
                                deviceValidationDetails.IsValid = true;
                            }
                            else
                            {
                                deviceValidationDetails.DEC = input;

                                var firstPart = Convert.ToInt32(input.Substring(0, 3)).ToString("X2");
                                var lastPart = Convert.ToInt32(input.Substring(3)).ToString("X6");

                                deviceValidationDetails.HEX = firstPart + lastPart;
                                deviceValidationDetails.IsValid = true;
                            }
                        }
                        else if (input.Length == 14 || input.Length == 18)
                        {
                            deviceValidationDetails.Type = "MEID";

                            if (input.Length == 14)
                            {
                                deviceValidationDetails.HEX = input;

                                var firstPart = Convert.ToInt64(input.Substring(0, 8), 16).ToString("D10");
                                var lastPart = Convert.ToInt64(input.Substring(8), 16).ToString("D8");

                                deviceValidationDetails.DEC = firstPart + lastPart;
                                deviceValidationDetails.IsValid = true;
                            }
                            else
                            {
                                deviceValidationDetails.DEC = input;

                                var firstPart = Convert.ToInt64(input.Substring(0, 10)).ToString("X8");
                                var lastPart = Convert.ToInt64(input.Substring(10)).ToString("X6");

                                deviceValidationDetails.HEX = firstPart + lastPart;
                                deviceValidationDetails.IsValid = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                deviceValidationDetails.IsValid = false;
            }

            result = deviceValidationDetails;
            return result;
        }

        public static ValidatedDevice ValidateDeviceOrSIM(string input)
        {
            var result = new ValidatedDevice();
            var deviceValidationDetails = new ValidatedDevice();

            if (string.IsNullOrEmpty(input))
            {
                deviceValidationDetails.IsValid = false;
                deviceValidationDetails.Type = "Unknown";
                result = deviceValidationDetails;
                return result;
            }
            input = input.Trim();

            try
            {
                deviceValidationDetails.IsValid = false;
                deviceValidationDetails.Type = "Unknown";
                deviceValidationDetails.HEX = input;
                deviceValidationDetails.DEC = input;

                var deviceResult = IsValidDevice(input);
                if (deviceResult.IsValid)
                {
                    deviceValidationDetails = deviceResult;
                }
                else
                {
                    bool simResult = IsValidSIM(input);
                    if (simResult)
                    {
                        deviceValidationDetails.IsValid = true;
                        deviceValidationDetails.Type = "ICCID";
                    }
                }
            }
            catch
            {
                deviceValidationDetails.IsValid = false;
            }

            result = deviceValidationDetails;
            return result;
        }

        public static string Encrypt(string input)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string Encrypt(string Message, string Passphrase, bool Base64Encode = true)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            var returnVar = Results.ToString();
            if (Base64Encode)
            {
                returnVar = Convert.ToBase64String(Results);
            }
            // Step 6. Return the encrypted string as a base64 encoded string
            return returnVar;
        }

        public static string CreateHMAC(string Data, string Passphrase, string EncType = "SHA256", string OutputType = "HEX")
        {
            Passphrase = Passphrase ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(Passphrase);
            byte[] messageBytes = encoding.GetBytes(Data);
            var hashmessage = new byte[0];
            if (EncType == "SHA1")
            {
                using (var keyBytes = new HMACSHA1(keyByte))
                {
                    hashmessage = keyBytes.ComputeHash(messageBytes);
                }
            }
            else
            {
                using (var keyBytes = new HMACSHA256(keyByte))
                {
                    hashmessage = keyBytes.ComputeHash(messageBytes);
                }
            }

            if (OutputType == "HEX")
            {
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
            else
            {
                return Convert.ToBase64String(hashmessage);
            }
        }

        public static string EncryptText(string input, string password)
        {
            byte[] plainText = Encoding.UTF8.GetBytes(input);
            byte[] key = Convert.FromBase64String(password);
            RijndaelManaged algorithm = new RijndaelManaged();
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;
            algorithm.BlockSize = 128;
            algorithm.KeySize = 128;
            algorithm.Key = key;
            string result;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        result = Convert.ToBase64String(memoryStream.ToArray()); //Base64 String
                        //UNREM if you want HEX result = BitConverter.ToString(memoryStream.ToArray()).Replace("-", string.Empty); //HEX string
                    }
                }
            }

            return result;
        }


        public static string Decrypt(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt;
            try
            {
                DataToDecrypt = Convert.FromBase64String(Message);
            }
            catch
            {
                return "ERROR";
            }

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        //public static DataTable ExcelToDataTable(ExcelPackage package, string Worksheet)
        //{
        //    DataTable table = new DataTable();
        //    ExcelWorksheet workSheet = null;
        //    //try {
        //    //    workSheet = package.Workbook.Worksheets[Worksheet];
        //    //} catch {
        //    workSheet = package.Workbook.Worksheets.OrderBy(x => x.Name.ToString()).First();
        //    //}

        //    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //    {
        //        table.Columns.Add(firstRowCell.Text);
        //    }
        //    for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //    {
        //        var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //        var newRow = table.NewRow();
        //        foreach (var cell in row)
        //        {
        //            //newRow[cell.Start.Column - 1] = cell.Text;
        //            newRow[cell.Start.Column - 1] = cell.Value;
        //        }
        //        table.Rows.Add(newRow);
        //    }

        //    return table;
        //}

        //public static string DataTableToCSV(DataTable dt, bool IncludeHeader = true)
        //{
        //    StringBuilder myCSV = new StringBuilder();
        //    StringWriter myCSVWriter = new StringWriter(myCSV);

        //    var csv = new CsvWriter(myCSVWriter);
        //    using (dt)
        //    {
        //        if (IncludeHeader)
        //        {
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                csv.WriteField(column.ColumnName);
        //            }
        //            csv.NextRecord();
        //        }

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            for (var i = 0; i < dt.Columns.Count; i++)
        //            {
        //                csv.WriteField(row[i]);
        //            }
        //            csv.NextRecord();
        //        }
        //    }

        //    return myCSV.ToString();
        //}

    }
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); }
        }
    }
    public static class Serialize
    {

        public static string ToXml(object data)
        {

            using (var stringwriter = new Utf8StringWriter())
            {
                try
                {
                    var serializer = new XmlSerializer(data.GetType());
                    serializer.Serialize(stringwriter, data);
                    return stringwriter.ToString();
                }
                catch (Exception ex)
                {
                    return "";
                }
            }

        }
        public static T FromXml<T>(string data) where T : class, new()
        {

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var stringreader = new StringReader(data);
                var vObj = (T)serializer.Deserialize(stringreader);
                return vObj;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }
}
