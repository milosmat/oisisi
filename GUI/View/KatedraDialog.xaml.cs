using CLI.Service;
using StudentskaSluzba.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for KatedraDialog.xaml
    /// </summary>
    public partial class KatedraDialog : Window
    {
        public ObservableCollection<Katedra> Katedre { get; set; }
        public ObservableCollection<Profesor> Profesori { get; set; }
        public ObservableCollection<Predmet> Predmeti { get; set; }
        public Katedra? SelectedKatedra { get; set; }
        public Profesor? SelectedProfesor { get; set; }

        public KatedraDialog()
        {
            InitializeComponent();
            Katedre = new ObservableCollection<Katedra>(KatedraService.GetKatedre());
            Profesori = new ObservableCollection<Profesor>();
            Predmeti = new ObservableCollection<Predmet>();
            CmbKatedre.ItemsSource = Katedre;
        }

        private void CmbKatedre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbKatedre.SelectedItem is Katedra katedra)
            {
                Profesori.Clear();
                foreach (var profesor in katedra.SpisakProfesora)
                {
                    Profesori.Add(profesor);
                }
                DgProfesori.ItemsSource = Profesori;
                SelectedKatedra = katedra;

                BtnPrikaziPredmete.IsEnabled = true; // Enable the button when a department is selected
            }
        }

        private void DgProfesori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgProfesori.SelectedItem is Profesor profesor)
            {
                SelectedProfesor = profesor;
            }
        }

        private void BtnPostaviSefa_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProfesor != null && SelectedKatedra != null && IsEligibleForHead())
            {
                SelectedKatedra.SefKatedre = SelectedProfesor;
                MessageBox.Show($"{SelectedProfesor.Ime} {SelectedProfesor.Prezime} je postavljen za šefa katedre {SelectedKatedra.NazivKatedre}.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Odabrani profesor ne zadovoljava uslove za šefa katedre.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
            Close();
        }

        public bool IsEligibleForHead()
        {
            return (SelectedProfesor.Zvanje.Contains("Redovni profesor") || SelectedProfesor.Zvanje.Contains("Vanredni profesor")) && SelectedProfesor.GodineStaza >= 5;
        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void BtnPrikaziPredmete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedKatedra != null)
            {
                var uniquePredmeti = new List<Predmet>();

                foreach (var profesor in SelectedKatedra.SpisakProfesora)
                {
                    foreach (var predmet in profesor.SpisakPredmeta)
                    {
                        if (predmet.SifraPredmeta != "" && !uniquePredmeti.Any(p => p.SifraPredmeta == predmet.SifraPredmeta))
                        {
                            var pr = PredmetService.GetByid(predmet.SifraPredmeta);
                            uniquePredmeti.Add(pr);
                        }
                    }
                }

                Predmeti = new ObservableCollection<Predmet>(uniquePredmeti);
                DgPredmeti.ItemsSource = Predmeti;
            }
        }
    }
}
