using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Helper;
using WpfApp1.Model;
using WpfApp1.View;


namespace WpfApp1.ViewModel
{
    public class CarViewModel : INotifyPropertyChanged
    {
        // Нахождение максимального Id в коллекции данных
        public int MaxId()
        {
            int max = 0;
            foreach (var per in this.ListCar)
            {
                if (max < per.Id)
                {
                    max = per.Id;
                };
            }
            return max;
        }
        // добавление автомобиля
        private RelayCommand addCar;
        public RelayCommand AddCar
        {
            get
            {
                return addCar ??
                (addCar = new RelayCommand(obj =>
                {
                    WindowNewCar wnCar = new WindowNewCar
                    {
                        Title = "Новый автомобиль"
                    };
                    // формирование кода нового автомобиля
                    int maxIdCar = MaxId() + 1;
                    Car person = new Car
                    {
                        Id = maxIdCar,
                        Arrival = DateTime.Now
                    };
                    wnCar.DataContext = person;
                    if (wnCar.ShowDialog() == true)
                    {
                        ListCar.Add(person);
                    }
                    SelectedCar = person;
                }));
            }
        }
        // команда редактирования данных по автомобилю
        private RelayCommand editCar;
        public RelayCommand EditCar
        {
            get
            {
                return editCar ??
                (editCar = new RelayCommand(obj =>
                {
                    WindowNewCar wnCar = new WindowNewCar()
                    {
                        Title = "Редактирование данных автомобиля",
                    };
                    Car person = SelectedCar;
                    Car tempCar = person;
                    wnCar.DataContext = tempCar;
                    if (wnCar.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        person = tempCar;
                    }
                }, (obj) => SelectedCar != null && ListCar.Count > 0));
            }
        }
        // Удаление данных по автомобилю
        private RelayCommand deleteCar;
        public RelayCommand DeleteCar
        {
            get
            {
                return deleteCar ??
                (deleteCar = new RelayCommand(obj =>
                {
                    Car person = SelectedCar;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по автомобилю: \n" +
    person.Model + " " + person.Manufacturer,
    "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        // удаление данных в списке отображения данных
                        ListCar.Remove(person);
                    }
                }, (obj) => SelectedCar != null && ListCar.Count > 0));
            }
        }

        private Car selectedCar; //выделенные в списке данные по автомобилю
        public Car SelectedCar
        {
            get { return selectedCar; }
            set
            {
                selectedCar = value;
                OnPropertyChanged("SelectedCar");
            }
        }
        // коллекция данных по автомобилям
        public ObservableCollection<Car> ListCar { get; set; } =
        new ObservableCollection<Car>();
        public CarViewModel()
        {
            this.ListCar.Add(new Car
            {
                Id = 1,
                Manufacturer = "Lixiang",
                Model = "L9",
                Quantity = 6,
                Arrival = new DateTime(2024, 02, 28)
            });
            this.ListCar.Add(new Car
            {
                Id = 2,
                Manufacturer = "BMW",
                Model = "M3 GTR",
                Quantity = 1,
                Arrival = new DateTime(2024, 03, 20)
            });
            this.ListCar.Add(new Car
            {
                Id = 3,
                Manufacturer = "Changan",
                Model = "UNI-V",
                Quantity = 4,
                Arrival = new DateTime(2024, 04, 15)
            });
            this.ListCar.Add(new Car
            {
                Id = 4,
                Manufacturer = "Xiaomi",
                Model = "SU7",
                Quantity = 2,
                Arrival = new DateTime(2025, 05, 10)
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]
string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
