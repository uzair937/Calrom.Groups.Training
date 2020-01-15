using CustomRegionEditor.Handler.Interfaces;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public interface IConverterFactory
    {
        IModelConverter CreateModelConverterManager(ISession session);
    }
}
