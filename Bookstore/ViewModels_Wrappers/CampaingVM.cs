using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class CampaingVM : NotifyPropertyChangeHandler
    {
        public Campaing Model { get; set; }

        public int Id { get => Model.Id; }

        public string Name
        {
            get => Model.Name;
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public double DiscountPercent
        {
            get => Model.DiscountPercent;
            set
            {
                if (Model.DiscountPercent != value)
                {
                    Model.DiscountPercent = value;
                    NotifyPropertyChanged(nameof(DiscountPercent));
                }
            }
        }


        public CampaingVM(Campaing campaing)
        {
            Model = campaing;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not CampaingVM campaingVM || campaingVM.Model == null)
            {
                return false;
            }
            return Model.Id.Equals((obj as CampaingVM)!.Model.Id);
        }
    }
}
