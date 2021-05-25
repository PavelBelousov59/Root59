using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для ApartmentS.xaml
    /// </summary>
    public partial class SvedsWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<KlassId> KlassIdList { get; set; }

        private IEnumerable<Sveds> _SvedsList;

        public IEnumerable<Sveds> SvedsList
        {
            get
            {
                var res = _SvedsList;

                if (SearchFilter != "")
                    res = res.Where(ai => (ai.FIO.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ai.KlassId.Klassuch.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ai.Ballyuch.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ai.Uspevaemostuch.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ai.Postzanuch.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0) 

                    );

                if (SortAsc) res = res.OrderBy(ai => ai.FIO);
                else res = res.OrderByDescending(ai => ai.FIO);
                if (_StreetFilterValue > 0)
                    res = res.Where(ai => ai.KlassId.Id == _StreetFilterValue);

                return res;
            }
            set
            {
                _SvedsList = value;
                Invalidate();
            }
        }

        private int _StreetFilterValue = 0;
        public int StreetFilterValue
        {
            get
            {
                return _StreetFilterValue;
            }
            set
            {
                _StreetFilterValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SvedsList"));
                }
            }
        }

        private bool _SortAsc = true;
        public bool SortAsc
        {
            get
            {
                return _SortAsc;
            }
            set
            {
                _SortAsc = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SvedsList"));
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortAsc = (sender as RadioButton).Tag.ToString() == "1";
        }

        public SvedsWindow()
        {
            InitializeComponent();
            DataContext = this;
            SvedsList = Core.Diplom.Sveds.ToArray();
            KlassIdList = Core.Diplom.KlassId.ToList();
            KlassIdList.Insert(0, new KlassId { Klassuch = "Все классы" });
        }


        private void Invalidate() {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SvedsList"));
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var NewSvedWindow = new AddSvedsWindow(new Sveds());
            if (NewSvedWindow.ShowDialog() == true)
            {
                SvedsList = Core.Diplom.Sveds.ToArray();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var EditSvedWindow = new AddSvedsWindow(SvedsDataGrid.SelectedItem as Sveds);
            if (EditSvedWindow.ShowDialog() == true)
            {
                SvedsList = Core.Diplom.Sveds.ToArray();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var Sved = SvedsDataGrid.SelectedItem as Sveds;

            // процесс удаления заворачиваем в try..catch
            try
            {
                Core.Diplom.Sveds.Remove(Sved);
                Core.Diplom.SaveChanges();
                SvedsList = Core.Diplom.Sveds.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении объекта недвижимости: {ex.Message}");
            }

        }
        private string SearchFilter = "";
        private void SearchFilterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchFilterTextBox.Text;
            Invalidate();
        }

        private void StreetFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StreetFilterValue = (StreetFilter.SelectedItem as KlassId).Id;
        }

        private void SvedsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

   }







