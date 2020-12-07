using System;
using System.IO;
using static CloudLibrary.InfraGlobal;
namespace CloudLibrary
{
    public class DatabaseServerModel:IDisposable
    {
        private string _ID;
        private string _DBPath;
        private string _DBFolderPath;
        private DatabaseTypes _DBType;
        private bool _Destroy = false;

        public string ID { get { return _ID; } }
        public DatabaseTypes DBType { get { return _DBType; } }
        public bool Destroy { set { _Destroy = value; } }

        public DatabaseServerModel(
            string envPath,
            string provider,
            EnvironmentTypes env,
            string envId,
            DatabaseTypes dbType,
            bool createNew = true,
            string oldID = "")
        {
            var dbDescription = Enum.GetName(typeof(DatabaseTypes), dbType);
            if (createNew)
            {
                var dbID = GenerateID();

                _DBPath = envPath + "/" + dbDescription + "/" + dbID + ".mdb";
                _DBFolderPath = envPath + "/" + dbDescription;
                _ID = dbID;
                _DBType = dbType;

                // Write to file
                Directory.CreateDirectory(envPath + "/" + dbDescription);
                var mdb = "{" +
                    "\"type\": \"" + dbDescription + "\"" +
                    "}";
                File.WriteAllText(_DBPath, mdb);
                Console.WriteLine("Successfully created database");
            } else
            {
                _DBPath = envPath; // + "/" + dbDescription + "/" + oldID + ".mdb";
                _ID = oldID;
                _DBType = dbType;
            }
        }

        ~DatabaseServerModel()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (_Destroy)
            {
                // Delete directory
                Directory.Delete(Path.GetDirectoryName(_DBPath), true);
            }
        }
    }
}
