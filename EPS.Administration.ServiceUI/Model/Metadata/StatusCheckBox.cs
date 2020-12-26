using EPS.Administration.Models.Device;

namespace EPS.Administration.ServiceUI.Model.Metadata
{
    public class StatusCheckBox
    {
        public bool IsChecked { get; set; }
        public DetailedStatus Status { get; set; }

        public override string ToString()
        {
            return Status?.Status ?? "<EMPTY>";
        }
    }
}
