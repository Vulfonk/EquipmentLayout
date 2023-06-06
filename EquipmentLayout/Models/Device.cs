using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipmentLayout.Models
{
    public class DeviceTemplate : IArea
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public string Name { get; set; }

        Area WorkArea { get; set; }
        Area ServiceArea { get; set; }

        class Area
        {
            public uint Width { get; set; }
            public uint Height { get; set; }

            public Point Position { get; set; }

            public Area Clone()
            {
                var clone = new Area();
                clone.Width = Width;
                clone.Height = Height;
                clone.Position = Position;
                return clone;
            }
        }

        public DeviceTemplate(uint width, uint height, string name)
        {
            this.Width = width;
            this.Height = height;
            this.Name = name;
        }

        public DeviceTemplate Clone()
        {
            var clone = new DeviceTemplate(Width, Height, Name);
            clone.WorkArea = this.WorkArea.Clone();
            clone.ServiceArea = this.WorkArea.Clone();
            return clone;
        }

    }

    public interface IArea
    {
        uint Width { get; }
        uint Height { get; }
    }

    public class Device : IArea
    {
        DeviceTemplate deviceTemplate;
        Point Position { get; set; }
        public uint Width { get => deviceTemplate.Width;}
        public uint Height { get => deviceTemplate.Height;}

        public int X => (int)Position.X;

        public int Y => (int)Position.Y;
        protected Device(DeviceTemplate deviceTemplate, Point position)
        {
            this.Position = position;
            this.deviceTemplate = deviceTemplate;
        }
    }

}
