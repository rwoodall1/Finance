using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingModels
{
   public class SaleModel
    {
        public int Id { get; set; }
        public int CustId { get; set; }
        public DateTime SaleDate { get; set; }
        public string RefNo { get; set; }
      
        public Decimal SaleTotal { get; set; }

    }
    public class SaleDetailModel
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ItemId { get; set; }
        public string Memo { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal ItemTotal { get; set; }
        
    }
    public class InvoiceModel
    {
        public int Id { get; set; }
        public int CustId { get; set; }
        public DateTime SaleDate { get; set; }
        public string RefNo { get; set; }
      
        public Decimal SaleTotal { get; set; }

    }
    public class InvoiceDetailModel
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public string Memo { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal ItemTotal { get; set; }

    }
}
