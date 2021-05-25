using System;
using System.Collections.Generic;
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
   
    public partial class AddSvedsWindow : Window
    {
        public Sveds CurrentSved { get; set; }
        public IEnumerable<KlassId> KlassIdList { get; set; }
       

        public AddSvedsWindow(Sveds Sved)
        {

            InitializeComponent();
            DataContext = this;
            CurrentSved = Sved;
            KlassIdList = Core.Diplom.KlassId.ToArray();
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // опять же всю работу с БД заворачиваем в try..catch
            try
            {
                if (CurrentSved.KlassId == null)
                    throw new Exception("Не выбран класс");

                if (CurrentSved.Id == 0)
                    Core.Diplom.Sveds.Add(CurrentSved);

                Core.Diplom.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");



            }
        }
    }
}
