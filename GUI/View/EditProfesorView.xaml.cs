using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CLI.Service;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View
{
    public partial class EditProfesorView : Window, INotifyPropertyChanged
    {
        public event EventHandler? OnFinish;
        private Profesor _profesor;

        public Profesor EditProfesor
        {
            get => _profesor;
            set
            {
                _profesor = value;
                OnPropertyChanged();
            }
        }

        private Predmet? _selectedPredmet;

        public Predmet? SelectedPredmet
        {
            get => _selectedPredmet;
            set
            {
                _selectedPredmet = value;
                OnPropertyChanged();
            }
        }

        public List<Predmet> predmeti { get; set; } = new List<Predmet>();
        public ObservableCollection<Predmet> PredmetDisplays { get; set; } = new ObservableCollection<Predmet>();
        public ObservableCollection<Student> StudentDisplays { get; set; } = new ObservableCollection<Student>();
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EditProfesorView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public EditProfesorView(Profesor selectedProf)
        {
            InitializeComponent();
            _profesor = selectedProf;
            EditProfesor = _profesor;
            DataContext = this;
            LoadPredmeti();
            LoadStudents();

            ValidateInputs(null, null);
        }

        private void LoadPredmeti()
        {
            if (EditProfesor.SpisakPredmeta != null)
            {
                var predmeti = new ObservableCollection<Predmet>();
                foreach (var predmet in EditProfesor.SpisakPredmeta)
                {
                    predmeti.Add(PredmetService.GetByid(predmet.SifraPredmeta));
                }

                EditProfesor.SpisakPredmeta = predmeti
                    .Where(p => !string.IsNullOrWhiteSpace(p.SifraPredmeta) && p.GodinaStudija > 0)
                    .ToList();
            }
            else
            {
                EditProfesor.SpisakPredmeta = new List<Predmet>();
            }

            PredmetDisplays = new ObservableCollection<Predmet>(EditProfesor.SpisakPredmeta);
            GridSubjects.ItemsSource = PredmetDisplays;
        }

        private void LoadStudents()
        {
            var allStudents = StudentService.GetStudents();
            var students = new HashSet<Student>();

            foreach (var student in allStudents)
            {
                foreach (var predmet in EditProfesor.SpisakPredmeta)
                {
                    if (student.SpisakNepolozenihPredmeta.Any(p => p.SifraPredmeta == predmet.SifraPredmeta))
                    {
                        students.Add(student);
                        break;
                    }
                }
            }

            StudentDisplays = new ObservableCollection<Student>(students);
            GridStudents.ItemsSource = StudentDisplays;
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var adrParts = TxtAdresaStanovanja.Text.Split(", ");
            EditProfesor.AdresaStanovanja = new Adresa
            {
                Ulica = adrParts.Length > 0 ? adrParts[0] : string.Empty,
                Broj = adrParts.Length > 1 && int.TryParse(adrParts[1], out int broj) ? broj : 0,
                Grad = adrParts.Length > 2 ? adrParts[2] : string.Empty,
                Drzava = adrParts.Length > 3 ? adrParts[3] : string.Empty
            };

            if (CRUDEntitetaService.IzmeniProfesora(EditProfesor))
            {
                OnFinish?.Invoke(this, EventArgs.Empty); // Poziv OnFinish događaja
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void AddSubject_Click(object sender, RoutedEventArgs e)
        {
            var predmetDialog = new IzaberiPredmetDialog(EditProfesor);
            EditProfesor = ProfesorService.GetById(EditProfesor.Id);
            if (predmetDialog.ShowDialog() == true)
            {
                var selectedPredmet = predmetDialog.SelectedPredmet;

                if (EditProfesor.SpisakPredmeta.Any(p => p.SifraPredmeta == selectedPredmet.SifraPredmeta))
                {
                    MessageBox.Show("Profesor već predaje ovaj predmet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                EditProfesor = ProfesorService.GetById(EditProfesor.Id);

                var predmeti = new ObservableCollection<Predmet>();
                foreach (var predmet in EditProfesor.SpisakPredmeta)
                {
                    predmeti.Add(PredmetService.GetByid(predmet.SifraPredmeta));
                }

                EditProfesor.SpisakPredmeta = predmeti
                    .Where(p => !string.IsNullOrWhiteSpace(p.SifraPredmeta) && p.GodinaStudija > 0)
                    .ToList();

                PredmetDisplays = new ObservableCollection<Predmet>(EditProfesor.SpisakPredmeta);
                GridSubjects.ItemsSource = PredmetDisplays;

                MessageBox.Show("Uspešno dodat predmet!");
            }
        }


        private void RemoveSubject_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet == null) return;
            var dialogRes = MessageBox.Show("Da li ste sigurni?", "Ukloni predmet", MessageBoxButton.OKCancel);
            if (!dialogRes.Equals(MessageBoxResult.OK)) return;
            var res = CRUDEntitetaService.ObrisiPredmetProfesoru(EditProfesor, SelectedPredmet);
            MessageBox.Show(res ? "Uspešno skinut profesor sa predmeta" : "Došlo je do greške prilikom brisanja!");
            EditProfesor = ProfesorService.GetById(EditProfesor.Id);

            var predmeti = new ObservableCollection<Predmet>();
            foreach (var predmet in EditProfesor.SpisakPredmeta)
            {
                predmeti.Add(PredmetService.GetByid(predmet.SifraPredmeta));
            }

            EditProfesor.SpisakPredmeta = predmeti
                .Where(p => !string.IsNullOrWhiteSpace(p.SifraPredmeta) && p.GodinaStudija > 0)
                .ToList();

            PredmetDisplays = new ObservableCollection<Predmet>(EditProfesor.SpisakPredmeta);
            GridSubjects.ItemsSource = PredmetDisplays;
        }
        private void SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearchStudent.Text;
            if (string.IsNullOrWhiteSpace(query))
            {
                LoadStudents();
                return;
            }

            var parts = query.Split(',');

            var filteredStudents = new List<Student>();

            if (parts.Length == 1)
            {
                string lastNamePart = parts[0].Trim().ToLower();
                foreach (var student in StudentDisplays)
                {
                    if (student.Prezime.ToLower().Contains(lastNamePart))
                    {
                        filteredStudents.Add(student);
                    }
                }
            }
            else if (parts.Length == 2)
            {
                string lastNamePart = parts[0].Trim().ToLower();
                string firstNamePart = parts[1].Trim().ToLower();
                foreach (var student in StudentDisplays)
                {
                    if (student.Prezime.ToLower().Contains(lastNamePart) &&
                        student.Ime.ToLower().Contains(firstNamePart))
                    {
                        filteredStudents.Add(student);
                    }
                }
            }
            else if (parts.Length == 3)
            {
                string indexPart = parts[0].Trim().ToLower();
                string firstNamePart = parts[1].Trim().ToLower();
                string lastNamePart = parts[2].Trim().ToLower();
                foreach (var student in StudentDisplays)
                {
                    if (student.BrojIndeksa.ToString().ToLower().Contains(indexPart) &&
                        student.Ime.ToLower().Contains(firstNamePart) &&
                        student.Prezime.ToLower().Contains(lastNamePart))
                    {
                        filteredStudents.Add(student);
                    }
                }
            }

            GridStudents.ItemsSource = new ObservableCollection<Student>(filteredStudents);
        }
        private void ValidateInputs(object sender, TextChangedEventArgs e)
        {
            bool isValid = true;

            // Ime
            if (string.IsNullOrWhiteSpace(TxtIme.Text) || !Regex.IsMatch(TxtIme.Text, @"^[a-zA-Z]+$"))
            {
                isValid = false;
                LblImeError.Content = "Ime je obavezno i mora sadržati samo slova.";
            }
            else
            {
                LblImeError.Content = string.Empty;
            }

            // Prezime
            if (string.IsNullOrWhiteSpace(TxtPrezime.Text) || !Regex.IsMatch(TxtPrezime.Text, @"^[a-zA-Z]+$"))
            {
                isValid = false;
                LblPrezimeError.Content = "Prezime je obavezno i mora sadržati samo slova.";
            }
            else
            {
                LblPrezimeError.Content = string.Empty;
            }

            // Datum rođenja
            if (!DateTime.TryParseExact(TxtDatumRodjenja.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                isValid = false;
                LblDatumRodjenjaError.Content = "Datum rođenja nije validan. Mora biti u formatu dd/MM/yyyy";
            }
            else
            {
                LblDatumRodjenjaError.Content = string.Empty;
            }

            // Adresa stanovanja
            var adrParts = TxtAdresaStanovanja.Text.Split(", ");
            if (adrParts.Length != 4 || adrParts.Any(string.IsNullOrWhiteSpace))
            {
                isValid = false;
                LblAdresaStanovanjaError.Content = "Adresa stanovanja mora biti u formatu: Ulica, Broj, Grad, Država.";
            }
            else
            {
                LblAdresaStanovanjaError.Content = string.Empty;
            }

            // Broj telefona
            if (string.IsNullOrWhiteSpace(TxtBrojTelefona.Text))
            {
                isValid = false;
                LblBrojTelefonaError.Content = "Broj telefona je obavezan.";
            }
            else
            {
                LblBrojTelefonaError.Content = string.Empty;
            }

            // Email adresa
            if (string.IsNullOrWhiteSpace(TxtEmailAdresa.Text) || !Regex.IsMatch(TxtEmailAdresa.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                isValid = false;
                LblEmailAdresaError.Content = "Email adresa nije validna.";
            }
            else
            {
                LblEmailAdresaError.Content = string.Empty;
            }

            // Zvanje
            if (string.IsNullOrWhiteSpace(TxtZvanje.Text))
            {
                isValid = false;
                LblZvanjeError.Content = "Zvanje je obavezno.";
            }
            else
            {
                LblZvanjeError.Content = string.Empty;
            }

            // Broj lične karte
            if (string.IsNullOrWhiteSpace(TxtLicnaKarta.Text) || !Regex.IsMatch(TxtLicnaKarta.Text, @"^\d+$"))
            {
                isValid = false;
                LblLicnaKartaError.Content = "Broj lične karte mora sadržati samo brojeve.";
            }
            else
            {
                LblLicnaKartaError.Content = string.Empty;
            }

            // Godine staža
            if (string.IsNullOrWhiteSpace(TxtGodineStaza.Text) || !int.TryParse(TxtGodineStaza.Text, out _) || int.Parse(TxtGodineStaza.Text) < 0)
            {
                isValid = false;
                LblGodineStazaError.Content = "Godine staža moraju biti validan pozitivan broj.";
            }
            else
            {
                LblGodineStazaError.Content = string.Empty;
            }

            BtnPotvrdi.IsEnabled = isValid;
        }
    }

}
