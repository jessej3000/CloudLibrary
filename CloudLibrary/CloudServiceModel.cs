using System;
using System.Collections.Generic;
using System.IO;

namespace CloudLibrary
{
    public class CloudServiceModel
    {
        private string _Name;
        private string _BasePath;
        private List<EnvironmentModel> _Environments;
        /// <summary>
        /// Name property - holds CloudService name
        /// </summary>
        public string Name { get { return _Name; } }
        public List<EnvironmentModel> Environments { get { return _Environments; } }

        public CloudServiceModel(string basePath, string name, bool createNew = true)
        {
            _Name = name;
            _Environments = new List<EnvironmentModel>();
            _BasePath = basePath;

            if (createNew)
            {
                Directory.CreateDirectory(basePath + "/" + name);
                Console.WriteLine("New service provider created: {0:G}", name);
            }
            else
            {
                string[] names = Directory.GetDirectories(_BasePath + "/" + _Name);

                foreach (string x in names)
                {
                    string[] xArr = x.Split('/');
                    string xf = xArr[xArr.Length - 1];
                    InfraGlobal.EnvironmentTypes envType = WhichType(xf);
                    string[] idNames = Directory.GetDirectories(_BasePath + "/" + _Name + "/" + xf);

                    foreach (string y in idNames)
                    {
                        string[] yArr = y.Split('/');
                        string yf = yArr[yArr.Length - 1];
                        EnvironmentModel item = new EnvironmentModel(
                            _BasePath, _Name, envType, false, yf);
                        item.ID = yf;
                        item.EnvPath = _BasePath + "/" + _Name + "/" + xf + "/" + yf;
                        _Environments.Add(item);
                    }
                }
            }

        }

        /// <summary>
        /// CreateEnvironment - creates a new environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="env"></param>
        public void CreateEnvironment(InfraGlobal.EnvironmentTypes env)
        {
            EnvironmentModel environment = new EnvironmentModel(_BasePath, _Name, env);
            _Environments.Add(environment);
        }

        /// <summary>
        /// Deletes provider
        /// </summary>
        public void ExpireProvider()
        {
            for (var i = 0; i < _Environments.Count; i++)
            {
                _Environments[i].Destroy = true;
            }

            _Environments.Clear();
        }

        private InfraGlobal.EnvironmentTypes WhichType(string name)
        {
            switch (name.ToLower())
            {
                case "uat": return InfraGlobal.EnvironmentTypes.UAT;
                case "staging": return InfraGlobal.EnvironmentTypes.Staging;
                case "production": return InfraGlobal.EnvironmentTypes.Production;
            }
            return InfraGlobal.EnvironmentTypes._;
        }
    }
}
