using FactoryLibrary;
using JobLibrary;

namespace JobConfiguration
{
    interface IUpdateDb
    {
        void UpdateDb(SchedulerDatabase db, GenericDatabaseTools dbTools);
    }
}
