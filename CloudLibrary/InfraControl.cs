using System;
using System.Collections.Generic;
using System.IO;
using static CloudLibrary.InfraGlobal;

namespace CloudLibrary
{
    /// <summary>
    /// Main controller class. Manages all transactions in the cloud
    /// Code can still be improved
    /// </summary>
    public class InfraControl
    {
        private List<CloudServiceModel> _CloudServices;
        private string _BasePath;

        public List<CloudServiceModel> CloudServices { get { return _CloudServices; } }
        public string BasePath { get { return _BasePath; } set { _BasePath = value; } }

        public InfraControl(string basePath)
        {
            _BasePath = basePath;
            // Initialize list of cloud service provider
            _CloudServices = new List<CloudServiceModel>();

            string[] names = Directory.GetDirectories(basePath);

            foreach(string n in names)
            {
                string[] nArr = n.Split('/');
                string nf = nArr[nArr.Length - 1];
                CloudServiceModel item = new CloudServiceModel(basePath, nf, false);
                _CloudServices.Add(item);
            }
        }

        /// <summary>
        /// CreateProvider - initializes a new Cloud Service Provider
        /// </summary>
        /// <param name="name">Name of service provider</param>
        public void CreateProvider(string name)
        {
            // If not exist create new service and add to List cloudServices
            if (!_CloudServices.Exists(x => x.Name == name))
            {
                CloudServiceModel CloudProvider = new CloudServiceModel(_BasePath, name);
                _CloudServices.Add(CloudProvider);
                return;
            }
            Console.WriteLine("Provider {0:G} already exist.", name);
        }

        /// <summary>
        /// GetProvider gets the specific provider
        /// </summary>
        /// <param name="name">Name of provider</param>
        /// <returns></returns>
        public CloudServiceModel GetProvider(string name)
        {
            return _CloudServices.Find(x => x.Name == name);
        }

        /// <summary>
        /// ProviderExists checks if provider in the list
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ProviderExist(string name)
        {
            return _CloudServices.Exists(x => x.Name == name);
        }

        /// <summary>
        /// ShowProviders list down all existing providers
        /// </summary>
        public void ShowProviders()
        {
            Console.WriteLine("Available Providers");
            foreach (CloudServiceModel x in _CloudServices)
            {
                Console.WriteLine(x.Name);
            }
        }

        /// <summary>
        /// Check if environment exist
        /// </summary>
        /// <param name="provider">Provider name</param>
        /// <param name="env">Environment type</param>
        /// <returns></returns>
        public bool EnvironmentExist(string provider, InfraGlobal.EnvironmentTypes env)
        {
            CloudServiceModel pro = _CloudServices.Find(i => i.Name == provider);
            return pro.Environments.Exists(x => x.Type == env);
        }

        /// <summary>
        /// Checks if environment exist using ID
        /// </summary>
        /// <param name="provider">Provider name</param>
        /// <param name="env">Environmnt type</param>
        /// <param name="id">Environment ID</param>
        /// <returns></returns>
        public bool EnvironmentExistByID(string provider, InfraGlobal.EnvironmentTypes env, string id)
        {
            CloudServiceModel pro = _CloudServices.Find(i => i.Name == provider);
            return pro.Environments.Exists(x => (x.Type == env) && (x.ID == id));
        }

        /// <summary>
        /// Show list of environment under a provider
        /// </summary>
        /// <param name="provider">Provider name</param>
        public void ShowEnvironments(string provider)
        {
            Console.WriteLine("Available Environments in {0:G} provider.", provider);
            CloudServiceModel pro = _CloudServices.Find(i => i.Name == provider);
            foreach (EnvironmentModel x in pro.Environments)
            {
                var env = Enum.GetName(typeof(EnvironmentTypes), x.Type);
                var id = x.ID;
                Console.WriteLine("Environment: {0:G} ----- ID: {1:G}", env, id);
            }
        }

        /// <summary>
        /// Display all available virtual machines with the provider and environment
        /// </summary>
        /// <param name="provider">Cloud provider name</param>
        /// <param name="env">Environment: UAT, Staging, Production</param>
        /// <param name="envID">Envinronment ID, one environment may have multipleinstances</param>
        public void ShowVirtualMachines(string provider, InfraGlobal.EnvironmentTypes env, string envID)
        {
            var environment = Enum.GetName(typeof(EnvironmentTypes), env);

            CloudServiceModel pro = _CloudServices.Find(i => i.Name == provider);
            EnvironmentModel environ = pro.Environments.Find(j => ((j.Type == env) && (j.ID == envID)));

            Console.WriteLine(
                "Available virtual machines under provider {0:G} and environment {1:G}:{2:G}.",
                provider, environment, envID);
            foreach (VirtualMachineModel x in environ.VirtualMachines)
            {
                string os = Enum.GetName(typeof(EnvironmentTypes), x.OS);
                Console.WriteLine("ID: {0:G}  |  OS: {1:G}  |  CPU: {2:G}  |  Memory: {3:G}",
                    x.ID, os, string.Format("{0:N2}", x.CPU), string.Format("{0:N2}", x.Memory));
            }
        }

        public bool VirtualMachineExist(string provider, InfraGlobal.EnvironmentTypes env, string envID, string vmID)
        {
            var environment = Enum.GetName(typeof(EnvironmentTypes), env);

            CloudServiceModel pro = _CloudServices.Find(i => i.Name == provider);
            EnvironmentModel environ = pro.Environments.Find(j => ((j.Type == env) && (j.ID == envID)));
            return environ.VirtualMachines.Exists(v => v.ID == vmID);
        }

        /// <summary>
        /// Display all available databases with the provider and environment
        /// </summary>
        /// <param name="provider">Cloud provider name</param>
        /// <param name="env">Environment: UAT, Staging, Production</param>
        /// <param name="envID">Envinronment ID, one environment may have multipleinstances</param>
        public void ShowDatabases(string provider, InfraGlobal.EnvironmentTypes env, string envID)
        {
            var environment = Enum.GetName(typeof(EnvironmentTypes), env);

            CloudServiceModel pro = _CloudServices.Find(i => i.Name == provider);
            EnvironmentModel environ = pro.Environments.Find(j => ((j.Type == env) && (j.ID == envID)));

            if (environ.Databases == null)
            {
                Console.WriteLine("No available database.");
                return;
            }
            Console.WriteLine(
                "Available databases under provider {0:G} and environment {1:G}:{2:G}.",
                provider, environment, envID);
            foreach (DatabaseServerModel x in environ.Databases)
            {
                Console.WriteLine(
                    "ID: {0:G} --- DB: {1:G}",
                    x.ID, Enum.GetName(typeof(DatabaseTypes), x.DBType));
            }
        }

        /// <summary>
        /// Create database under a provider and environment
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void CreateDatabase(
            string provider, InfraGlobal.EnvironmentTypes env, string envID, InfraGlobal.DatabaseTypes db)
        {
            CloudServiceModel pro = _CloudServices.Find(i => i.Name == provider);
            pro.Environments.Find(j => ((j.Type == env) && (j.ID == envID)))
                .AddDatabase(provider, env, db);
        }

        /// <summary>
        /// DeleteProvider - removes provider with the provider name
        /// </summary>
        /// <param name="provider">Provider name</param>
        public void DeleteProvider(string provider)
        {
            CloudServiceModel c = _CloudServices.Find(x => x.Name == provider);
            c.ExpireProvider();
        }

        /// <summary>
        /// Delete environment. Need the provider name and environment
        /// </summary>
        /// <param name="provider">Provider name</param>
        /// <param name="env">Envirnaoment type</param>
        public void DeleteEnvironment(string provider, InfraGlobal.EnvironmentTypes env)
        {
            CloudServiceModel c = _CloudServices.Find(x => x.Name == provider);
            EnvironmentModel e = c.Environments.Find(i => i.Type == env);
            e.Destroy = true;
            c.Environments.Remove(e);
        }

        /// <summary>
        /// Deletes database. 
        /// </summary>
        /// <param name="provider">Provider name</param>
        /// <param name="env">Environment type</param>
        /// <param name="envId">Environment ID</param>
        /// <param name="db">Database type</param>
        public void DeleteDatabase(string provider, InfraGlobal.EnvironmentTypes env, string envId, DatabaseTypes db)
        {
            CloudServiceModel c = _CloudServices.Find(x => x.Name == provider);
            EnvironmentModel e = c.Environments.Find(i => i.Type == env && i.ID == envId);
            DatabaseServerModel d = e.Databases.Find(i => i.DBType == db);
            d.Destroy = true;
            e.Databases.Remove(d);
            d.Dispose();
        }

        /// <summary>
        /// Deletes virtual machine
        /// </summary>
        /// <param name="provider">Provider name</param>
        /// <param name="env">Environment type</param>
        /// <param name="id">Environment ID</param>
        /// <param name="vid">Virtual machine ID</param>
        public void DeleteVirtualMachine(string provider, InfraGlobal.EnvironmentTypes env, string id, string vid)
        {
            CloudServiceModel c = _CloudServices.Find(x => x.Name == provider);
            EnvironmentModel e = c.Environments.Find(i => i.Type == env && i.ID == id);
            e.RemoveVirtualMachine(vid);
        }
    }
}
