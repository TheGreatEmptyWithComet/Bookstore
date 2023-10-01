using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class CustomerVM : NotifyPropertyChangeHandler
    {
        public Customer Model { get; set; }

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
        public string PhoneNumber
        {
            get => Model.PhoneNumber;
            set
            {
                if (Model.PhoneNumber != value)
                {
                    Model.PhoneNumber = value;
                    NotifyPropertyChanged(nameof(PhoneNumber));
                }
            }
        }


        public CustomerVM(Customer customer)
        {
            Model = customer;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not CustomerVM customerVM || customerVM.Model == null)
            {
                return false;
            }
            return Model.Id.Equals((obj as CustomerVM)!.Model.Id);
        }
    }
}
