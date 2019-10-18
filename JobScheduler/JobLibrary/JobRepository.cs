using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public class JobRepository : IDatabaseRepos<Job>
    {
        Config _jobContext;
        public JobRepository(Config init)
        {
            _jobContext = init;

        }
        public IEnumerable<Job> List
        {
            get
            {
                return _jobContext.Jobs;
            }
        }

        public void Add(Job entity)
        {
            _jobContext.Jobs.Add(entity);
        }

        public void Delete(Job entity)
        {
            _jobContext.Jobs.Remove(entity);
        }

        public Config Update()
        {
            return _jobContext;
        }

        public Job FindById(int Id)
        {
            var result = (from r in _jobContext.Jobs where r.Id == Id select r).FirstOrDefault();
            return result;
        }
    }

}
