using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.ViewModels
{
    public class DeviceTemplateViewModel : BaseViewModel
    {
        private DeviceTemplate _model;

        public uint Width 
        { 
            get => _model.Width;
            set
            {
                if (_model.Width == value) return; else _model.Width = value; OnPropertyChanged();
            }
        }

        public uint Height
        {
            get => _model.Height;
            set
            {
                if (_model.Height == value) return; else _model.Height = value; OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _model.Name;
            set
            {
                if (_model.Name == value) return; else _model.Name = value; OnPropertyChanged();
            }
        }


        public DeviceTemplateViewModel(DeviceTemplate model)
        {
            _model = model;
        }
    }
}
