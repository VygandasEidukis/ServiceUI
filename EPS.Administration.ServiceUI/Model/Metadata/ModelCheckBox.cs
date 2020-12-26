using EPS.Administration.Models.Device;

namespace EPS.Administration.ServiceUI.Model.Metadata
{
    public class ModelCheckBox
    {
        public bool IsChecked { get; set; }
        public DeviceModel Model { get; set; }

        public override string ToString()
        {
            return Model?.Name ?? "<Empty>";
        }
    }
}
