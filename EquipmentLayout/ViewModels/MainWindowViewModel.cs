using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EquipmentLayout.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceTemplateViewModel> DeviceTemplateViewModels { get; set; }

        private DeviceTemplateViewModel _selectedDeviceTemplate;

        public DelegateCommand CalcCommand { get; set; }

        public List<Device> InputItems 
        { 
            get
            {
                var devices = new List<Device>();
                var factory = new DeviceFactory();
                foreach (var temp in DeviceTemplateViewModels)
                {
                    for(int i = 0; i < temp.Count; i++)
                    {
                        var device = factory.GetDevice(new Point(), temp.Model);
                        devices.Add(device);
                    }
                }
                return devices;
            }
        }

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

        private void CalcCommand_Executed()
        {
            var csvWriter = new CsvDeviceSerializer();
            csvWriter.Write(InputItems, "input.csv");
           
            MessageBox.Show("Расчет начат.", "", MessageBoxButton.OK, MessageBoxImage.Information);

            var process = new Process();
            var path = "stock_cutter.exe";

            process.Exited += ProcessExited;

            process.StartInfo.FileName = path;
            process.EnableRaisingEvents = true;
            process.Start();

        }

        private void ProcessExited(object sender, EventArgs e)
        {
            var csvReader = new CsvDeviceDeserializer();
            var devices = csvReader.Read("output.csv");

            RectItems = new ObservableCollection<Device>(devices);
            OnPropertyChanged(nameof(RectItems));
            MessageBox.Show("Расчет завершен.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MainWindowViewModel()
        {
            DeviceTemplateViewModels = new ObservableCollection<DeviceTemplateViewModel>();
            RectItems = new ObservableCollection<Device>();
            CalcCommand = new DelegateCommand(CalcCommand_Executed);


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

            //RectItems.Add(device1);
            //RectItems.Add(device2);

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
