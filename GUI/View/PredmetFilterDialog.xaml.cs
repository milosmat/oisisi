using System.Collections.ObjectModel;
using System.Windows;
using CLI.Service;
using System.Windows.Controls;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View
{
    public partial class PredmetFilterDialog : Window
    {
        public ObservableCollection<Predmet> Predmeti { get; set; }
        public Predmet FirstSubject { get; set; }
        public Predmet SecondSubject { get; set; }
        public string FilterType { get; set; }

        public PredmetFilterDialog()
        {
            InitializeComponent();
            Predmeti = new ObservableCollection<Predmet>(PredmetService.GetPredmets());
            DataContext = this;
        }

        private void FilterStudents_Click(object sender, RoutedEventArgs e)
        {
            FirstSubject = CmbFirstSubject.SelectedItem as Predmet;
            SecondSubject = CmbSecondSubject.SelectedItem as Predmet;
            if (FirstSubject != null && SecondSubject != null && CmbFilterType.SelectedItem != null)
            {
                FilterType = (CmbFilterType.SelectedItem as ComboBoxItem).Tag.ToString();
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please select both subjects and a filter type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
