using System;
using static CloudLibrary.InfraGlobal;
namespace CloudLibrary
{
    public class MachineModel
    {
        public string ID { get; set; }
        public OSPlatform OS { get; set; }
        public float CPU { get; set; }
        public float Memory { get; set; }
    }
}
