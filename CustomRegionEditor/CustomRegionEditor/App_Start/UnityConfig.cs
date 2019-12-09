using CustomRegionEditor.Database;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace CustomRegionEditor.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<LazyLoader, LazyLoader>();
            container.RegisterType<ICustomRegionGroupRepository, CustomRegionGroupRepo>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}