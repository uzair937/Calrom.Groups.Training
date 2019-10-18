using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionNamespace;

namespace JobLibrary
{
    public class EmailSubscription : IEntity
    {
        public override int Id { get; set; }
        public string EmailAddress { get; set; }
        public string LogLevel { get; set; }
        public List<int> JobIds { get; set; }

        public override List<string> GetData()
        {
            var returnList = new List<string>
            {
                Id.ToString(),
                EmailAddress,
                LogLevel
            };
            foreach (var id in JobIds.ListToString())
            {
                returnList.Add(id);
            }
            return returnList;
        }

        public override void SetValues(List<string> args)
        {
            Id = int.Parse(args[0]);
            EmailAddress = args[1];
            LogLevel = args[2];
            for (int x = 3; x < args.Count; x++)
            {
                JobIds.Add(int.Parse(args[x]));
            }
        }
    }
}
