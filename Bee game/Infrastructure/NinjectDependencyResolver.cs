using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Web.Common;
using Bee_game.Service;
using Bee_game.DAL;
using Bee_game.Models;
using System.Configuration;

namespace Bee_game.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public enum Storages
        {
            MongoDB, MSSQL
        }

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IService>().To<BeeGameService>();
            //Binding to storage from config
            switch ((Storages)Enum.Parse(typeof(Storages), ConfigurationManager.AppSettings["DefaultStorage"]))
            {
                case Storages.MongoDB:
                    kernel.Bind<IRepository<List<IBee>>>().To<MongoBeeRepository>();
                    break;
                case Storages.MSSQL:
                    kernel.Bind<IRepository<List<IBee>>>().To<SQLBeeRepository>();
                    break;
                default:
                    kernel.Bind<IRepository<List<IBee>>>().To<SQLBeeRepository>();
                    break;
            }
            Logger.Log.Debug("Bindings added.");
        }
    }
}