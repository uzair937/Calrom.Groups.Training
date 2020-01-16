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

            var validationModel = entryValidator.IsValid(customRegion); //Checks and gets Entry info such as Id

            validationModel = supersetValidator.IsValid(validationModel); //Checks for existing super regions


            return validationModel;
        }
    }
}
