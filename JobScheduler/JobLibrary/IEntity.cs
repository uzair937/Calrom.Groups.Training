
using System.Collections.Generic;

namespace JobLibrary
{
    public abstract class IEntity
    {
        public abstract int Id { get; set; }
        public abstract List<string> GetData();
        public abstract void SetValues(List<string> args);
    }
}
