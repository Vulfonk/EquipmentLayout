using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace EquipmentLayout.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceTemplateViewModel> DeviceTemplateViewModels { get; set; }

        private DeviceTemplateViewModel _selectedDeviceTemplate;

        public DelegateCommand CalcCommand { get; set; }

        public DelegateCommand DeleteTemplateCommand { get; set; }

        public DelegateCommand AddTemplateCommand { get; set; }

        private Rectangle _zone;

        public Rectangle Zone 
        { 
            get => _zone;
            set
            {
                this._zone = value;
                OnPropertyChanged(nameof(Zone));
            }
        }

        public List<Device> InputItems
        {
            get
            {
                var devices = new List<Device>();
                var factory = new DeviceFactory();
                foreach (var temp in DeviceTemplateViewModels)
                {
                    for (int i = 0; i < temp.Count; i++)
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
            Action<DeviceTemplateViewModel, object> setter = (x, v) => (x as DeviceTemplateViewModel).Name = (string)v;
            Func<DeviceTemplateViewModel, object> getter = (x) => (x as DeviceTemplateViewModel).Name;

            var model = _selectedDeviceTemplate;

            if (model == null)
            {
                OnPropertyChanged(nameof(Properties));
                return;
            }

            var properties = new List<Property<DeviceTemplateViewModel>>
            {
                new Property<DeviceTemplateViewModel>( "Имя", model.Name, model,
                (x, v) => x.Name = (string)v,
                (x) => x.Name),

                new Property<DeviceTemplateViewModel>( "Ширина", model.Width, model,
                (x, v) => x.Width = int.Parse(v.ToString()),
                (x) => x.Width),

                new Property<DeviceTemplateViewModel>( "Высота", model.Height, model,
                (x, v) => x.Height = int.Parse(v.ToString()),
                (x) => x.Height)
            };

            this.Properties = new ObservableCollection<Property<DeviceTemplateViewModel>>(properties);
            OnPropertyChanged(nameof(Properties));
        }

        public ObservableCollection<Property<DeviceTemplateViewModel>> Properties { get; set; }

        private void CalcCommand_Executed()
        {
            var csvWriter = new CsvDeviceSerializer();
            csvWriter.Write(Zone, InputItems, "input.csv");

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
        }

        public MainWindowViewModel()
        {
            DeviceTemplateViewModels = new ObservableCollection<DeviceTemplateViewModel>();
            RectItems = new ObservableCollection<Device>();
            CalcCommand = new DelegateCommand(CalcCommand_Executed);
            AddTemplateCommand = new DelegateCommand(AddTemplateCommand_Executed);
            DeleteTemplateCommand = new DelegateCommand(DeleteTemplateCommand_Executed);
            Zone = new Rectangle()
            {
                Width = 460,
                Height = 330,
            };
           

            var factory = new DeviceFactory();

            var template = new DeviceTemplate(100, 100, "MyDevice");
            var vm_template = new DeviceTemplateViewModel(template);

            DeviceTemplateViewModels.Add(vm_template);

            var template2 = new DeviceTemplate(200, 100, "MyDevice2");
            var vm_template2 = new DeviceTemplateViewModel(template2);

            var position = new Point(100, 50);
            var device1 = factory.GetDevice(position, template2);

            var position2 = new Point(20, 30);
            var device2 = factory.GetDevice(position2, template);

            //RectItems.Add(device1);
            //RectItems.Add(device2);

            DeviceTemplateViewModels.Add(vm_template2);

        }

        private void DeleteTemplateCommand_Executed()
        {
            this.DeviceTemplateViewModels.Remove(this.SelectedDeviceTemplate);
        }

        private void AddTemplateCommand_Executed()
        {
            if(this.SelectedDeviceTemplate != null)
                this.DeviceTemplateViewModels.Add(this.SelectedDeviceTemplate.Clone());
            else
            {
                var template2 = new DeviceTemplate(200, 100, "Name");
                var vm_template2 = new DeviceTemplateViewModel(template2);
                this.DeviceTemplateViewModels.Add(vm_template2);
            }


        }
    }

    public class Property<T>
    {
        private T _model;
        public string Name { get; set; }
        public object Value
        {
            get => _getter(_model);
            set => _setter(_model, value);
        }

        private Action<T, object> _setter;
        private Func<T, object> _getter;

        public Property(string name, object value, T model, Action<T, object> setter, Func<T, object> getter)
        {
            this._model = model;
            this._setter = setter;
            this._getter = getter;
            Name = name;
            Value = value;
        }
    }

}
