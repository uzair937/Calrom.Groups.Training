using CustomRegionEditor.Handler.Interfaces;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public interface IConverterFactory
    {
        IModelConverter CreateModelConverter(ISession session);
    }
}
