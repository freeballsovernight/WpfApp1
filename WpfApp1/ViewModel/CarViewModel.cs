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
            // Читаем данные из базы данных в коллекцию
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
                foreach (var car in db.Cars.ToList())
                    ListCar.Add(car);
            }
        }
        // Нахождение максимального Id в коллекции данных
        public int MaxId()
        {
            int max = 0;
            foreach (var per in this.ListCar)
            {
                if (max < per.carId)
                {
                    max = per.carId;
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
                    Car car = new Car
                    {
                        carId = maxIdCar,
                        Arrival = DateTime.Now
                    };
                    wnCar.DataContext = car;
                    if (wnCar.ShowDialog() == true)
                    {
                        // Добавляем данные в базу данных
                        using (var db = new AppDbContext())
                        {
                            db.Cars.Add(car);
                            db.SaveChanges();
                        }
                        // Добавляем данные в коллекцию
                        ListCar.Add(car);
                    }
                    SelectedCar = car;
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
                    Car car = SelectedCar;
                    Car tempCar = car;
                    wnCar.DataContext = tempCar;
                    if (wnCar.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        car = tempCar;
                        // Обновляем данные в базе данных
                        using (var db = new AppDbContext())
                        {
                            db.Cars.Update(car);
                            db.SaveChanges();
                        }
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
                    Car car = SelectedCar;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по автомобилю: \n" + car.Model + " " + car.Manufacturer, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        // Удаляем данные из базы данных
                        using (var db = new AppDbContext())
                        {
                            db.Cars.Remove(car);
                            db.SaveChanges();
                        }
                        // удаление данных в коллекции
                        ListCar.Remove(car);
                    }
                }, (obj) => SelectedCar != null && ListCar.Count > 0));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        // [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
