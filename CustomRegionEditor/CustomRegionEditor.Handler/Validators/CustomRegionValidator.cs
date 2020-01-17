using CustomRegionEditor.Models;
using CustomRegionEditor.Handler.Factories;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Handler.Validators
{
    public class CustomRegionValidator
    {
        private readonly ISession Session;
        private readonly IValidatorFactory ValidatorFactory;

        public CustomRegionValidator(ISession session, IValidatorFactory validatorFactory)
        {
            this.Session = session;
            this.ValidatorFactory = validatorFactory;
        }

        public ValidationModel IsValid(CustomRegionGroupModel customRegion)
        {
            var supersetValidator = this.ValidatorFactory.CreateCustomRegionEntrySupersetValidator();
            var entryValidator = this.ValidatorFactory.CreateCustomRegionEntryValidator(this.Session);

            var basicValidationResult = entryValidator.IsValid(customRegion); //Checks and gets Entry info such as Id

            var supersetValidationResult = supersetValidator.IsValid(customRegion); //Checks for existing super regions

            return basicValidationResult.Merge(supersetValidationResult);
        }
        
        public ErrorModel IsNull(string entry)
        {
            var errorModel = new ErrorModel
            {
                Message = "Entry " + entry + " could not be found.",
                Warning = true
            };

            return errorModel;
        }
    }
}
