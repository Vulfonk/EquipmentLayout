﻿using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipmentLayout.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceTemplateViewModel> DeviceTemplateViewModels { get; set; }

        private DeviceTemplateViewModel _selectedDeviceTemplate;

        public ObservableCollection<Device> RectItems 
        { 
            get; 
            set; 
        }

        public DeviceTemplateViewModel SelectedDeviceTemplate 
        {
            get => _selectedDeviceTemplate;
            set
            {
                if (_selectedDeviceTemplate == value) return;
                _selectedDeviceTemplate = value;
                UpdateProperties();
                OnPropertyChanged();
            }
        }

        private void UpdateProperties()
        {
            var model = _selectedDeviceTemplate;
            var properties = new List<Property>
            {
                new Property( "Имя", model.Name ),
                new Property( "Ширина", model.Width),
                new Property( "Высота", model.Height ),
            };
            OnPropertyChanged(nameof(Properties));
            this.Properties = new ObservableCollection<Property>(properties);
        }

        public ObservableCollection<Property> Properties { get; set; }

        public MainWindowViewModel()
        {
            DeviceTemplateViewModels = new ObservableCollection<DeviceTemplateViewModel>();
            RectItems = new ObservableCollection<Device>();

            var template = new DeviceTemplate(100, 100, "MyDevice");
            var vm_template = new DeviceTemplateViewModel(template);

            DeviceTemplateViewModels.Add(vm_template);

            var template2 = new DeviceTemplate(200, 100, "MyDevice2");
            var vm_template2 = new DeviceTemplateViewModel(template2);

            var factory = new DeviceFactory();
            var position = new Point(100, 50);
            var device1 = factory.GetDevice(position, template2);

            var position2 = new Point(20, 30);
            var device2 = factory.GetDevice(position2, template);

            RectItems.Add(device1);
            RectItems.Add(device2);

            DeviceTemplateViewModels.Add(vm_template2);

        }

    }

    public class Property
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Property(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

}