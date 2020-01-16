using CustomRegionEditor.Handler.Converters;
using CustomRegionEditor.Handler.Factories;
using CustomRegionEditor.Handler.Interfaces;
using Unity;

namespace CustomRegionEditor.Handler
{
    public static class UnityDatabaseConfig
    {

        public static UnityContainer RegisterComponents(UnityContainer container)
        {
            container.RegisterType<IValidatorFactory, ValidatorFactory>();

            container.RegisterType<IConverterFactory, ConverterFactory>();

            return container;
        }
    }
}