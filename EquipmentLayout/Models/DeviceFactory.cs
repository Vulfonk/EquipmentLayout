using System.Windows;

namespace EquipmentLayout.Models
{
    public class DeviceFactory
    {
        private class DeviceBuild : Device
        {
            public DeviceBuild(DeviceTemplate deviceTemplate, Point position)
                : base(deviceTemplate, position) { }
        }

        public Device GetDevice(Point position, DeviceTemplate deviceTemplate)
        {
            return new DeviceBuild(deviceTemplate, position);
        }

    }

}
