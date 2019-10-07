using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JobLibrary
{
    public class ManageJson
    {
        private const string jsonFileOut = "C:/Users/wbooth/Documents/JobTask/json-out.json";
        public static void AddJson(SchedulerDatabase db)
        {
            var fs = new FileStream(jsonFileOut, FileMode.Create);
            var jsonData = JsonConvert.SerializeObject(db);
            var info = new UTF8Encoding(true).GetBytes(jsonData);
            fs.Write(info, 0, info.Length);
            fs.Close();
        }
    }
}
