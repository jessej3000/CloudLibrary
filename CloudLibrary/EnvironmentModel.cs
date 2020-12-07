using System;
using System.Collections.Generic;
using System.IO;
using static CloudLibrary.InfraGlobal;

namespace CloudLibrary
{
    public class EnvironmentModel
    {
        private string _ID;
        private EnvironmentTypes _Type;
        private List<VirtualMachineModel> _VirtualMachines;
        private List<DatabaseServerModel> _Databases;
        private string _EnvPath;
        private string _BaseEnvPath;
        private bool _Destroy = false;

        public string ID { get { return _ID; } set { _ID = value; } }
        public EnvironmentTypes Type { get { return _Type; } set { _Type = value; } }
        public List<VirtualMachineModel> VirtualMachines { get{ return _VirtualMachines; } }
        public List<DatabaseServerModel> Databases { get { return _Databases; } }
        public string EnvPath { get { return _EnvPath; } set { _EnvPath = value; } }
        public bool Destroy { set { _Destroy = value; } }

        public EnvironmentModel(
            string basePath,
            string provider,
            EnvironmentTypes env,
            bool createNew = true,
            string eId = "")
        {
            // Initialize resources
            _VirtualMachines = new List<VirtualMachineModel>();
            _Databases = new List<DatabaseServerModel>();

            _Type = env;

            if (createNew)
            {
                string Enviroment = string.Empty;

                _ID = GenerateID();
                Enviroment = Enum.GetName(typeof(EnvironmentTypes), env);
                _EnvPath = basePath + "/" + provider + "/" + Enviroment + "/" + _ID;

                // Create folder for environment under provider directory
                _BaseEnvPath = basePath + "/" + provider + "/" + Enviroment;
                Directory.CreateDirectory(_BaseEnvPath);
                Directory.CreateDirectory(_EnvPath);
                Console.WriteLine("Successfully create environment {0:G} with ID: {1:G}.", Enviroment, _ID);
            } else
            {
                LoadVirtualMachines(basePath,
                            provider,
                            env,
                            eId);
                LoadDatabases(basePath,
                            provider,
                            env,
                            eId);
            }
        }

        ~EnvironmentModel()
        {
            if (_Destroy)
            {
                for(var i = 0; i < _Databases.Count; i++)
                {
                    _Databases[i].Destroy = true;
                }
                for (var i = 0; i < _VirtualMachines.Count; i++)
                {
                    _VirtualMachines[i].Destroy = true;
                }

                Directory.Delete(_BaseEnvPath, true);
            }
        }

        /// <summary>
        /// Loads existing databses
        /// </summary>
        /// <param name="basePath">Database path</param>
        /// <param name="provider">Provider name</param>
        /// <param name="env">Environment type</param>
        /// <param name="eId">Envinronment ID</param>
        /// <param name="vId">VM ID</param>
        private void LoadDatabases(
            string basePath,
            string provider,
            EnvironmentTypes env,
            string eId = "",
            string vId = "")
        {
            {
                string environment = Enum.GetName(typeof(EnvironmentTypes), env);
                string[] names = Directory.GetDirectories(basePath + "/" + provider + "/" + environment + "/" + eId);

                foreach (string x in names)
                {
                    string[] xArr = x.Split('/');
                    string xf = xArr[xArr.Length - 1];
                    if ((xf.ToLower().IndexOf("sql") > -1))
                    {
                        string fileName = basePath + "/" + provider + "/" + environment + "/" + eId + "/" + xf;
                        string[] fileNames = Directory.GetFiles(fileName);

                        foreach (string fn in fileNames)
                        {
                            string[] fnArr = fn.Split('/');
                            string fnf = fnArr[fnArr.Length - 1];
                            DatabaseTypes dbType = WhichDatabase(xf);
                            DatabaseServerModel db = new DatabaseServerModel(fn, provider, env, eId, dbType, false, fnf.Replace(".mdb", ""));

                            _Databases.Add(db);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads virtual machines
        /// </summary>
        /// <param name="basePath">Database path</param>
        /// <param name="provider">Provider name</param>
        /// <param name="env">Environment type</param>
        /// <param name="eId">Envinronment ID</param>
        /// <param name="vId">VM ID</param>
        private void LoadVirtualMachines(
            string basePath,
            string provider,
            EnvironmentTypes env,
            string eId = "",
            string vId = "")
        {
            {
                string environment = Enum.GetName(typeof(EnvironmentTypes), env);
                string[] names = Directory.GetDirectories(basePath + "/" + provider + "/" + environment + "/" + eId);

                foreach (string x in names)
                {
                    string[] xArr = x.Split('/');
                    string xf = xArr[xArr.Length - 1];
                    if (!(xf.ToLower().IndexOf("sql") > 0))
                    {
                        string fileName = basePath + "/" + provider + "/" + environment + "/" + eId + "/" + xf + "/config.json";

                        if (File.Exists(fileName))
                        {
                            string jsonString = File.ReadAllText(fileName);
                            MachineModel mm = ParseVM(jsonString);
                            VirtualMachineModel vm = new VirtualMachineModel(
                                basePath + "/" + provider + "/" + environment + "/" + eId,
                                mm.OS,
                                mm.CPU,
                                mm.Memory,
                                false,
                                xf);

                            _VirtualMachines.Add(vm);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// AddVirtualMachine creates a virtual machine and add to the list _VirtualMachines
        /// </summary>
        /// <param name="os">Virtual Machine operating system</param>
        /// <param name="cpu">CPU of the virtual machine</param>
        /// <param name="memory">Virtual machine physical memory</param>
        public void AddVirtualMachine(OSPlatform os, float cpu, float memory)
        {
            VirtualMachineModel vm = new VirtualMachineModel(_EnvPath, os, cpu, memory);
            _VirtualMachines.Add(vm);
        }

        /// <summary>
        /// AddDatabase creates a new database and add it to the list _Databases
        /// </summary>
        /// <param name="provider">Name of provider</param>
        /// <param name="env">Type of environment: UAT, Staging, Production</param>
        /// <param name="db">Type of database MySQL or SQL</param>
        public void AddDatabase(string provider, InfraGlobal.EnvironmentTypes env, InfraGlobal.DatabaseTypes db) 
        {
            DatabaseServerModel rdb = new DatabaseServerModel(_EnvPath, provider, env, _ID, db);
            _Databases.Add(rdb);
        }

        /// <summary>
        /// RemoveVirtualMachine - Removes a virtural machine with id
        /// </summary>
        /// <param name="id">Unique identifier</param>
        public void RemoveVirtualMachine(string id)
        {
            VirtualMachineModel vm = _VirtualMachines.Find(x => x.ID == id);
            if (vm != null)
            {
                vm.Destroy = true;
                _VirtualMachines.Remove(vm);
                vm.Dispose();
            }
        }
    }
}
