using CustomRegionEditor.Handler.Validators;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public interface IValidatorFactory
    {
         CustomRegionValidator CreateCustomRegionValidator(ISession session);
         CustomRegionEntryValidator CreateCustomRegionEntryValidator(ISession session);
         CustomRegionEntrySupersetValidator CreateCustomRegionEntrySupersetValidator();
    }
}
