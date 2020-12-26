using EPS.Administration.Models.Device;

namespace EPS.Administration.ServiceUI.Model.Metadata
{
    public class ClassificationCheckBox
    {
        public bool IsChecked { get; set; }
        public Classification Classification { get; set; }

        public override string ToString()
        {
            return Classification?.Model ?? "<Empty>";
        }
    }
}
