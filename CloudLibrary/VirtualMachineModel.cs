using System;
using System.IO;
using static CloudLibrary.InfraGlobal;

namespace CloudLibrary
{
    public class VirtualMachineModel:IDisposable
    {
        private string _ID;
        private OSPlatform _OS;
        private float _CPU;
        private float _Memory;
        private string _VMPath;
        private bool _Destroy = false;

        public string ID { get { return _ID; } }
        public OSPlatform OS { get { return _OS; } }
        public float CPU { get { return _CPU; } }
        public float Memory { get { return _Memory; } }
        public bool Destroy { set { _Destroy = value; } }

        public VirtualMachineModel(
            string environment,
            OSPlatform os,
            float cpu,
            float memory,
            bool createNew = true,
            string vmID = "")
        {
            _OS = os;
            _CPU = cpu;
            _Memory = memory;
            if (createNew)
            {
                _ID = GenerateID();
                _OS = os;
                _CPU = cpu;
                _Memory = memory;
                _VMPath = environment + "/" + _ID;
                Directory.CreateDirectory(_VMPath); // Create virtual machine directory
                var jsonString = "{" +
                    "\"id\": \"" + _ID + "\"," +
                    "\"os\": \"" + _OS + "\"," +
                    "\"cpu\": " + _CPU + "," +
                    "\"memory\": " + _Memory + "," +
                    "\"path\": \"" + _VMPath + "\"" +
                    "}";
                File.WriteAllText(_VMPath + "/config.json", jsonString);
                var hd = "{" +
                    "\"size\": \"1TB\"" +
                    "}";
                File.WriteAllText(_VMPath + "/harddisk.json", hd);

                Console.WriteLine("New virtual machine created: ");
                Console.WriteLine("ID: {0:G}", _ID);
                Console.WriteLine("OS: {0:G}", _OS);
                Console.WriteLine("CPU: {0:F}", _CPU);
                Console.WriteLine("Memory: {0:F}", _Memory);
            } else {
                _ID = vmID;
                _VMPath = environment + "/" + _ID;
            }
        }


        ~VirtualMachineModel()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            if (_Destroy)
            {
                // Delete hardisk first
                File.Delete(_VMPath + "/harddisk.json");
                // Delete directory
                Directory.Delete(_VMPath, true);
            }
        }
    }
}
