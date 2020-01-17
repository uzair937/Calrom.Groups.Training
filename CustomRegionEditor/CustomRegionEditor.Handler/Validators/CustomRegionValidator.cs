using CustomRegionEditor.Models;
using CustomRegionEditor.Handler.Factories;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Factories;

namespace CustomRegionEditor.Handler.Validators
{
    public class CustomRegionValidator
    {
        private readonly ISession Session;
        private readonly IValidatorFactory ValidatorFactory;
        private readonly IRepositoryFactory RepositoryFactory;

        public CustomRegionValidator(ISession session, IValidatorFactory validatorFactory, IRepositoryFactory repositoryFactory)
        {
            this.Session = session;
            this.RepositoryFactory = repositoryFactory;
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

        public ErrorModel ValidateName(string name, string id)
        {
            var errorModel = new ErrorModel();
            var customRegionRepo = this.RepositoryFactory.CreateCustomRegionGroupRepository(this.Session);
            if(string.IsNullOrEmpty(name))
            {
                errorModel.Message = "Enter a name";
                errorModel.Warning = true;
                return errorModel;
            }

            var regionList = customRegionRepo.List();
            var matchedName = regionList.FirstOrDefault(a => a.Name == name);

            if (matchedName != null && matchedName.Id.ToString() != id)
            {
                errorModel.Message = "Name already exists";
                errorModel.Warning = true;
                return errorModel;
            }
            return null;
        }
       
    }
}
