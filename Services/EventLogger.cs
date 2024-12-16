//using NLog;
using SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiBindingModels;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Services
{
    static public class EventLogger{
        private static SQLCustomClient _client { get; set; } = new SQLCustomClient();
        //private static Logger Log = LogManager.GetCurrentClassLogger();
        public static void AddEvent(EventModel model)
        {
            _client.ClearParameters();
            _client.CommandText(@"Insert Into OpyEventLog (ModifiedBy,Service,FunctionName,ModifiedData,Schcode,Invno) Values(@ModifiedBy,@Service,@FunctionName,@ModifiedData,@Schcode,@Invno)");
            _client.AddParameter("@ModifiedBy",model.ModifiedBy);
            _client.AddParameter("@Service",model.Service);
            _client.AddParameter("@FunctionName",model.FunctionName);
            _client.AddParameter("@ModifiedData",model.ModifiedData);
            _client.AddParameter("@Schcode",model.Schcode);
            _client.AddParameter("@Invno", model.Invno);
            var insertResult = _client.Insert();
            if (insertResult.IsError)
            {
                string jsonString = JsonSerializer.Serialize(model);
               // Log.Error("Failed to insert OPYEventLog:" + insertResult.Errors[0].DeveloperMessage + " | Data:" + jsonString);
            }

        }
    }   
}
