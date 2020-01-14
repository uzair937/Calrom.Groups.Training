using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Database.Factories
{
    public interface IRepositoryFactory
    {
        ICustomRegionGroupRepository CreateCustomRegionGroupRepository();
    }
}
