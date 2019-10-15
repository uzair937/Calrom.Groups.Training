using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JobLibrary
{
    public class JsonDatabase : GenericDatabase
    {
        private const string jsonFileOut = "C:/Users/wbooth/Documents/JobTask/json-out.json";
        public override bool AddData(SchedulerDatabase db)
        {
            try
            {
                using (var fs = new FileStream(jsonFileOut, FileMode.Create))
                {
                    var jsonData = JsonConvert.SerializeObject(db);
                    var info = new UTF8Encoding(true).GetBytes(jsonData);
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override SchedulerDatabase GetData()
        {
            try
            {
                var db = SchedulerDatabase.GetDb();
                using (var fs = new FileStream(jsonFileOut, FileMode.Open))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        db = (SchedulerDatabase)JsonConvert.DeserializeObject(reader.ReadToEnd());
                    }
                    fs.Close();
                }
                return db;
            }
            catch
            {
                return null;
            }
        }
    }
}
