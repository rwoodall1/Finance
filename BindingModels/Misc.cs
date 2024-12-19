using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ApiBindingModels
{
    public class BankFeedModel
    {
        public IFormFile File { get; set; }
        public string AccountId { get; set; }
    }
    public class IID
	{
		public int Id { get; set; }
	}
	public class EventModel
	{
		public EventModel(string _modifiedBy, string _Service, string _functionName, string _modifiedData, string _schcode, string _Invno)
		{
			this.ModifiedBy = _modifiedBy;
			this.Service = _Service;
			this.Schcode = _schcode;
			this.ModifiedData = _modifiedData;
			this.Invno = _Invno;
			this.FunctionName = _functionName;
		}
		public string ModifiedBy { get; set; }
		public string Service { get; set; }
		public string FunctionName { get; set; }
		public string ModifiedData { get; set; }
		public string Schcode { get; set; }
		public string Invno { get; set; }

	}
	public class OutlookAttachement
	{
		public string Path { get; set; }
		public string Name { get; set; }
	}
	public class EmailModel
	{
		public string ToAddress { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public OutlookAttachement Attachement { get; set; }
	}


	public class EnvironmentInfo
	{
		public string Environment { get; set; }
		public bool IsDeveloperMachine { get; set; }


	}
}
