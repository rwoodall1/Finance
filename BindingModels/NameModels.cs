using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingModels
{
    public class NameModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber{get;set;}
        public string MobilePhone { get; set; } 
        public string TaxId { get; set; }   
        public string Notes { get; set; }  
        public string NameType { get; set; }
        public string EmailAddress { get; set; }
        public string AccountNo { get; set; }   


    }
    public class NameListModel
    {
        public string FullName { get; set; }  
        public string Notes { get; set; }
        public int Id { get; set; }
    }
    public class NameLkpModel
    {
        public string FullName { get; set; }
        public string NameType { get; set; }
        public int Id { get; set; }
    }
    public class DropDownNames
    {
        public string NameType { get; set; }
        public List<NameList> Names { get; set; }
    }
    public class NameList
    {
        public string FullName { get; set; }
        public int Id { get; set; }
    }
}
