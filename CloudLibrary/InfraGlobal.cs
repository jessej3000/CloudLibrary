using System;
namespace CloudLibrary
{
    /// <summary>
    /// This class only holds global static variables.
    /// </summary>
    public static class InfraGlobal
    {
        public enum EnvironmentTypes
        {
            _,
            Staging,
            UAT,
            Production
        }

        public enum DatabaseTypes
        {
            _,
            MySQL,
            SQL
        }

        public enum OSPlatform
        {
            _,
            Linux,
            Windows
        }

        /// <summary>
        /// GenerateID create unique ID for virtual machine
        /// </summary>
        /// <returns></returns>
        public static string GenerateID()
        {
            Guid id = Guid.NewGuid();
            return id.ToString().Split('-')[0];
        }


        public static OSPlatform WhichOS(string name)
        {
            switch(name.ToLower())
            {
                case "linux": return OSPlatform.Linux;
                case "windows": return OSPlatform.Windows;
            }

            return OSPlatform._;
        }

        public static DatabaseTypes WhichDatabase(string name)
        {
            switch (name.ToLower())
            {
                case "mysql": return DatabaseTypes.MySQL;
                case "sql": return DatabaseTypes.SQL;
            }

            return DatabaseTypes._;
        }
        public static MachineModel ParseVM(string json)
        {
            string clean = json.Replace("{", "").Replace("}", "").Replace("\"", "");
            string[] cleanArr = clean.Split(',');

            MachineModel m = new MachineModel();

            string[] idArr = cleanArr[0].Split(':');
            m.ID = idArr[1].Trim();
            string[] osArr = cleanArr[1].Split(':');
            m.OS = WhichOS(osArr[1].Trim());
            string[] cpuArr = cleanArr[2].Split(':');
            m.CPU = float.Parse(cpuArr[1].Trim());
            string[] memArr = cleanArr[3].Split(':');
            m.Memory = float.Parse(memArr[1].Trim());
            return m;
        }
    }
}
