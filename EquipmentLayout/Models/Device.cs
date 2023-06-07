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
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public int Count { get;set; }

        Area WorkArea { get; set; }
        Area ServiceArea { get; set; }

        class Area
        {
            public int Width { get; set; }
            public int Height { get; set; }

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

        public DeviceTemplate(int width, int height, string name)
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
        int Width { get; }
        int Height { get; }
    }

    public class Device : IArea
    {
        DeviceTemplate deviceTemplate;
        Point Position { get; set; }
        public int Width { get => deviceTemplate.Width;}
        public int Height { get => deviceTemplate.Height;}

        public int X => (int)Position.X;

        public int Y => (int)Position.Y;
        protected Device(DeviceTemplate deviceTemplate, Point position)
        {
            this.Position = position;
            this.deviceTemplate = deviceTemplate;
        }
    }

}
