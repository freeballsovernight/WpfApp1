using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Car : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string manufacturerString; //имя автомобиля
        public string Manufacturer
        {
            get { return manufacturerString; }
            set
            {
                manufacturerString = value;
                OnPropertyChanged("Manufacturer");
            }
        }
        private string modelString; //фамилия автомобиля
        public string Model
        {
            get { return modelString; }
            set
            {
                modelString = value;
                OnPropertyChanged("Model");
            }
        }
        private int quantity; // должность автомобиля
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        private DateTime arrivalDate; //дата рождения автомобиля
        public DateTime Arrival
        {
            get { return arrivalDate; }
            set
            {
                arrivalDate = value;
                OnPropertyChanged("Arrival");
            }
        }
        public Car() { }
        public Car(int id, string manufacturerString, string modelString, int quantity, DateTime
       arrivalDate)
        {
            this.Id = id;
            this.Manufacturer = manufacturerString;
            this.Model = modelString;
            this.Quantity = quantity;
            this.Arrival = arrivalDate;
        }
        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
