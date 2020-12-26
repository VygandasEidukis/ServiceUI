using EPS.Administration.Models.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Administration.ServiceUI.Model.Metadata
{
    public class LocationCheckbox
    {
        public bool IsChecked { get; set; }
        public DeviceLocation Location { get; set; }

        public override string ToString()
        {
            return Location?.Name ?? "<EMPTY>";
        }
    }
}
