using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CLI.DAO;
using CLI.Service;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View
{
    public partial class EditStudentView : Window, INotifyPropertyChanged
    {
        public event EventHandler? OnFinish;
        private Student _student;
        public Student EditStudent
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Predmet> _predmets;
        public ObservableCollection<Predmet> PredmetsNePolozeni
        {
            get => _predmets;
            set
            {
                EditStudent.SpisakNepolozenihPredmeta = value.ToList();
                _predmets = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Predmet> _predmetsPolozeni;
        public ObservableCollection<Predmet> PredmetsPolozeni
        {
            get => _predmetsPolozeni;
            set
            {
                EditStudent.SpisakPolozenihIspita = value.ToList();
                _predmetsPolozeni = value;
                OnPropertyChanged();
            }
        }

        private List<Profesor> _nepolozeniProfesori;
        public List<Profesor> NepolozeniProfesori
        {
            get => _nepolozeniProfesori;
            set
            {
                _nepolozeniProfesori = value;
                OnPropertyChanged();
            }
        }

        private Predmet? _predmet;
        public Predmet? SelectedPredmet
        {
            get => _predmet;
            set
            {
                _predmet = value;
                OnPropertyChanged();
            }
        }
        public int BrojcanaVrednostOcene { get; set; }
        public DateTime DatumPolaganjaIspita { get; set; }
        private List<Profesor> _professori;
        public List<Profesor> Professori
        {
            get => _professori;
            set
            {
                _professori = value;
                OnPropertyChanged();
            }
        }

        private int _totalESPB;
        public int TotalESPB
        {
            get => _totalESPB;
            set
            {
                _totalESPB = value;
                OnPropertyChanged();
            }
        }
        

        private double _averageGrade;
        public double AverageGrade
        {
            get => _averageGrade;
            set
            {
                _averageGrade = value;
                OnPropertyChanged();
            }
        }

            public string TempIme { get; set; }
            public string TempPrezime { get; set; }
            public DateTime TempDatumRodjenja { get; set; }
            public string TempAdresa { get; set; }
            public string TempTelefon { get; set; }
            public string TempEmail { get; set; }
            public string TempIndeks { get; set; }
            public int TempTrenutnaGodinaStudija { get; set; }
            public StatusEnum TempStatus { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EditStudentView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public EditStudentView(Student selectedStud)
        {
            InitializeComponent();
            _student = selectedStud;
            EditStudent = _student;
            DataContext = this;

            TempIme = EditStudent.Ime;
            TempPrezime = EditStudent.Prezime;
            TempDatumRodjenja = EditStudent.DatumRodjenja;
            TempAdresa = $"{EditStudent.AdresaStanovanja.Ulica}, {EditStudent.AdresaStanovanja.Broj}, {EditStudent.AdresaStanovanja.Grad}, {EditStudent.AdresaStanovanja.Drzava}";
            TempTelefon = EditStudent.KontaktTelefon;
            TempEmail = EditStudent.EmailAdresa;
            TempIndeks = $"{EditStudent.BrojIndeksa.OznakaSmera} {EditStudent.BrojIndeksa.BrojUpisa}/{EditStudent.BrojIndeksa.GodinaUpisa}";
            TempTrenutnaGodinaStudija = EditStudent.TrenutnaGodinaStudija;
            TempStatus = EditStudent.Status;

            CmbTrenutnaGodinaStudija.SelectedIndex = TempTrenutnaGodinaStudija - 1;
            CmbNacinFinansiranja.SelectedIndex = TempStatus.Equals(StatusEnum.Budzet) ? 0 : 1;

            PredmetsNePolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakNepolozenihPredmeta); // Inicijalizacija
            PredmetsPolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakPolozenihIspita); // Inicijalizacija
            Professori = UzmiProfesoreZaStudenta(selectedStud);
            NepolozeniProfesori = UzmiNepolozeneProfesore(selectedStud);

            // Dodaj kod za postavljanje vrednosti za prosečnu ocenu i ESPB
            TotalESPB = CalculateTotalESPB(PredmetsPolozeni.ToList());
            AverageGrade = CalculateAverageGrade(PredmetsPolozeni.ToList());
            var dao = new OcenaNaIspituDAO();
            foreach (var ocena in dao.UzmiSveOceneNaIspitu())
            {
                if (ocena.StudentKojiJePolozio.Id == EditStudent.Id && PredmetsPolozeni.Any(p => p.SifraPredmeta.Equals(ocena.Predmet.SifraPredmeta)))
                {
                    var predmet = PredmetsPolozeni.First(p => p.SifraPredmeta.Equals(ocena.Predmet.SifraPredmeta));
                    predmet.BrojcanaVrednostOcene = ocena.BrojcanaVrednostOcene;
                    predmet.DatumPolaganjaIspita = ocena.DatumPolaganjaIspita;
                }
            }
            ValidateInputs(null, null);
        }

        public double CalculateAverageGrade(List<Predmet> polozeniPredmeti)
        {
            if (polozeniPredmeti.Count == 0) return 0;

            var ocenaDAO = new OcenaNaIspituDAO(); // Kreiraj instancu OcenaNaIspituDAO
            var oceneNaIspitu = ocenaDAO.UzmiSveOceneNaIspitu(); // Uzmi sve ocene na ispitima

            double totalGrades = 0;
            int totalSubjects = 0;

            foreach (var pred in polozeniPredmeti)
            {
                var ocena = oceneNaIspitu.FirstOrDefault(o => o.Predmet.SifraPredmeta == pred.SifraPredmeta && o.StudentKojiJePolozio.Id == EditStudent.Id);
                if (ocena != null)
                {
                    totalGrades += ocena.BrojcanaVrednostOcene;
                    totalSubjects++;
                }
            }

            return totalSubjects == 0 ? 0 : totalGrades / totalSubjects;
        }

        public int CalculateTotalESPB(List<Predmet> polozeniPredmeti)
        {
            return polozeniPredmeti.Sum(p => p.BrojESPB);
        }

        public void UpdatePassedAndFailedSubjects()
        {
            var dao = new OcenaNaIspituDAO();
            foreach (var ocena in dao.UzmiSveOceneNaIspitu())
            {
                if (ocena.StudentKojiJePolozio.Id == EditStudent.Id && PredmetsPolozeni.Any(p => p.SifraPredmeta.Equals(ocena.Predmet.SifraPredmeta)))
                {
                    // Pronađite predmet u kolekciji položenih predmeta i ažurirajte vrednosti
                    BrojcanaVrednostOcene = ocena.BrojcanaVrednostOcene;
                    DatumPolaganjaIspita = ocena.DatumPolaganjaIspita;
                }
            }
            PredmetsNePolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakNepolozenihPredmeta);
            PredmetsPolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakPolozenihIspita);
            EditStudent.ProsecnaOcena = CalculateAverageGrade(PredmetsPolozeni.ToList());
            StudentService.AzurirajStudenta(EditStudent);
            AverageGrade = EditStudent.ProsecnaOcena;
            TotalESPB = CalculateTotalESPB(PredmetsPolozeni.ToList());
            OnPropertyChanged(nameof(PredmetsNePolozeni));
            OnPropertyChanged(nameof(PredmetsPolozeni));
        }



        private List<Profesor> UzmiProfesoreZaStudenta(Student selectedStud)
        {
            var predmeti = selectedStud.SpisakNepolozenihPredmeta.Concat(selectedStud.SpisakPolozenihIspita).ToList();
            var profesori = new List<Profesor>();

            foreach (var predmet in predmeti)
            {
                var profesor = predmet.PredmetniProfesor;
                if (profesor != null && !profesori.Contains(profesor))
                {
                    profesor = ProfesorService.GetById(profesor.Id);
                    profesori.Add(profesor);
                }
            }

            return profesori;
        }

        private List<Profesor> UzmiNepolozeneProfesore(Student selectedStud)
        {
            var predmeti = selectedStud.SpisakNepolozenihPredmeta;
            var profesori = new List<Profesor>();

            foreach (var predmet in predmeti)
            {
                var profesor = predmet.PredmetniProfesor;
                if (profesor != null && !profesori.Contains(profesor))
                {
                    profesor = ProfesorService.GetById(profesor.Id);
                    profesori.Add(profesor);
                }
            }

            return profesori;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Ažurirajte model sa privremenim vrednostima kada korisnik potvrdi izmene
            EditStudent.Ime = TempIme;
            EditStudent.Prezime = TempPrezime;
            EditStudent.DatumRodjenja = TempDatumRodjenja;

            var adrParts = TempAdresa.Split(", ");
            EditStudent.AdresaStanovanja = new Adresa
            {
                Ulica = adrParts.Length > 0 ? adrParts[0] : string.Empty,
                Broj = adrParts.Length > 1 && int.TryParse(adrParts[1], out int broj) ? broj : 0,
                Grad = adrParts.Length > 2 ? adrParts[2] : string.Empty,
                Drzava = adrParts.Length > 3 ? adrParts[3] : string.Empty
            };

            var indeksParts = TempIndeks.Split(' ', '/');
            EditStudent.BrojIndeksa = new Indeks
            {
                OznakaSmera = indeksParts.Length > 0 ? indeksParts[0] : string.Empty,
                BrojUpisa = indeksParts.Length > 1 && int.TryParse(indeksParts[1], out int brojUpisa) ? brojUpisa : 0,
                GodinaUpisa = indeksParts.Length > 2 && int.TryParse(indeksParts[2], out int godinaUpisa) ? godinaUpisa : 0
            };

            EditStudent.KontaktTelefon = TempTelefon;
            EditStudent.EmailAdresa = TempEmail;
            EditStudent.TrenutnaGodinaStudija = TempTrenutnaGodinaStudija;
            EditStudent.Status = TempStatus;

            // Osvježi liste položenih i nepoloženih predmeta
            PredmetsPolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakPolozenihIspita);
            PredmetsNePolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakNepolozenihPredmeta);

            // Ažuriraj prosečnu ocenu i ukupne ESPB bodove
            EditStudent.ProsecnaOcena = CalculateAverageGrade(PredmetsPolozeni.ToList());
            TotalESPB = CalculateTotalESPB(PredmetsPolozeni.ToList());

            if (CRUDEntitetaService.IzmeniStudenta(EditStudent))
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

        private void ValidateInputs(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // Validacija za ime
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || !Regex.IsMatch(FirstNameTextBox.Text, @"^[a-zA-Z]+$"))
            {
                isValid = false;
                LblFirstNameError.Content = "Ime je obavezno i mora sadržati samo slova.";
            }
            else
            {
                LblFirstNameError.Content = string.Empty;
            }

            // Validacija za prezime
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) || !Regex.IsMatch(LastNameTextBox.Text, @"^[a-zA-Z]+$"))
            {
                isValid = false;
                LblLastNameError.Content = "Prezime je obavezno i mora sadržati samo slova.";
            }
            else
            {
                LblLastNameError.Content = string.Empty;
            }

            // Validacija za datum rođenja
            if (!DateTime.TryParseExact(BirthDateTextBox.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                isValid = false;
                LblBirthDateError.Content = "Datum rođenja nije validan. Mora biti u formatu dd/MM/yyyy";
            }
            else
            {
                LblBirthDateError.Content = string.Empty;
            }

            // Validacija za adresu stanovanja
            var adrParts = AddressTextBox.Text.Split(", ");
            if (adrParts.Length != 4 || adrParts.Any(string.IsNullOrWhiteSpace))
            {
                isValid = false;
                LblAddressError.Content = "Adresa stanovanja mora biti u formatu: Ulica, Broj, Grad, Država.";
            }
            else
            {
                LblAddressError.Content = string.Empty;
            }

            // Validacija za broj telefona
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text) || !Regex.IsMatch(PhoneTextBox.Text, @"^\d+$"))
            {
                isValid = false;
                LblPhoneError.Content = "Broj telefona nije validan.";
            }
            else
            {
                LblPhoneError.Content = string.Empty;
            }

            // Validacija za email adresu
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                isValid = false;
                LblEmailError.Content = "Email adresa nije validna.";
            }
            else
            {
                LblEmailError.Content = string.Empty;
            }

            // Validacija za broj indeksa
            if (string.IsNullOrWhiteSpace(IndexNumberTextBox.Text) || !Regex.IsMatch(IndexNumberTextBox.Text, @"^[A-Z]{2}\s\d{1,3}/\d{4}$"))
            {
                isValid = false;
                LblIndexError.Content = "Broj indeksa mora biti formata XX 123/1234.";
            }
            else
            {
                LblIndexError.Content = string.Empty;
            }

            // Validacija za trenutnu godinu studija
            if (CmbTrenutnaGodinaStudija.SelectedItem == null)
            {
                isValid = false;
                LblCurrentYearError.Content = "Trenutna godina studija je obavezna.";
            }
            else
            {
                LblCurrentYearError.Content = string.Empty;
            }

            // Validacija za način finansiranja
            if (CmbNacinFinansiranja.SelectedItem == null)
            {
                isValid = false;
                LblFinancingMethodError.Content = "Način finansiranja je obavezan.";
            }
            else
            {
                LblFinancingMethodError.Content = string.Empty;
            }

            BtnPotvrdi.IsEnabled = isValid;
        }

        private void AddNewSubject_Click(object sender, RoutedEventArgs e)
        {
            IzaberiPredmetDialog izaberiPredmetDialog = new IzaberiPredmetDialog(EditStudent);
            if (izaberiPredmetDialog.ShowDialog() == true)
            {
                MessageBox.Show("Predmet uspešno dodat!");
                var noviPredmet = izaberiPredmetDialog.SelectedPredmet;

                if (noviPredmet == null) return;
                if (PredmetsNePolozeni.Any(p => p.SifraPredmeta == noviPredmet.SifraPredmeta)) return;
                PredmetsNePolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakNepolozenihPredmeta);
                OnPropertyChanged(nameof(PredmetsNePolozeni));
            }
        }

        private void UndoPassedExam_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet == null) return;
            var tmp = new Predmet()
            {
                SifraPredmeta = SelectedPredmet.SifraPredmeta,
                BrojESPB = SelectedPredmet.BrojESPB,
                Semestar = SelectedPredmet.Semestar,
                SpisakStudenataPolozili = SelectedPredmet.SpisakStudenataPolozili,
                SpisakStudenataNisuPolozili = SelectedPredmet.SpisakStudenataNisuPolozili,
                GodinaStudija = SelectedPredmet.GodinaStudija,
                NazivPredmeta = SelectedPredmet.NazivPredmeta,
                PredmetniProfesor = SelectedPredmet.PredmetniProfesor,
            };
            MessageBoxResult res = MessageBox.Show("Da li ste sigurni da hoćete da poništite ocenu?", "Upozorenje", MessageBoxButton.OKCancel);
            if (res.Equals(MessageBoxResult.OK))
            {
                // Ponisti ocenu predmeta
                CRUDEntitetaService.PonistiOcenu(SelectedPredmet.SifraPredmeta, EditStudent.Id);

                // Premesti predmet iz položenih u nepoložene
                EditStudent.SpisakPolozenihIspita.Remove(tmp);
                EditStudent.SpisakNepolozenihPredmeta.Add(tmp);
                tmp.SpisakStudenataPolozili.Remove(EditStudent);
                tmp.SpisakStudenataNisuPolozili.Add(EditStudent);

                PredmetsPolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakPolozenihIspita);
                PredmetsNePolozeni = new ObservableCollection<Predmet>(EditStudent.SpisakNepolozenihPredmeta);

                TotalESPB = CalculateTotalESPB(PredmetsPolozeni.ToList());
                EditStudent.ProsecnaOcena = CalculateAverageGrade(PredmetsPolozeni.ToList());
                AverageGrade = EditStudent.ProsecnaOcena;
                // Ažuriraj studenta
                StudentService.AzurirajStudenta(EditStudent);
                PredmetService.Azuriraj(tmp);

                // Osveži prikaz
                OnPropertyChanged(nameof(PredmetsNePolozeni));
                OnPropertyChanged(nameof(PredmetsPolozeni));

                MessageBox.Show("Ocena uspešno poništena.");
            }
        }




        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet == null) return;
            var tmp = new Predmet()
            {
                SifraPredmeta = SelectedPredmet.SifraPredmeta,
                BrojESPB = SelectedPredmet.BrojESPB,
                Semestar = SelectedPredmet.Semestar,
                SpisakStudenataPolozili = SelectedPredmet.SpisakStudenataPolozili,
                SpisakStudenataNisuPolozili = SelectedPredmet.SpisakStudenataNisuPolozili,
                GodinaStudija = SelectedPredmet.GodinaStudija,
                NazivPredmeta = SelectedPredmet.NazivPredmeta,
                PredmetniProfesor = SelectedPredmet.PredmetniProfesor,
            };
            AddGradeView gradeView = new AddGradeView(this, SelectedPredmet, _student);
            if (gradeView.ShowDialog() == true)
            {
                MessageBox.Show("Ocena uspešno upisana");

                // Premesti predmet iz nepoloženih u položene
                StudentService.AzurirajStudenta(EditStudent);
                PredmetService.Azuriraj(tmp);

                
                // Osveži prikaz
                UpdatePassedAndFailedSubjects();
            }
        }


        private void TabChangedEvent(object sender, RoutedEventArgs e)
        {
            SelectedPredmet = null;

        }

        private void RemoveSubject_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet == null) return;
            MessageBoxResult res = MessageBox.Show("Da li ste sigurni da hoćete da uklonite predmet?", "Upozorenje", MessageBoxButton.OKCancel);
            if (res.Equals(MessageBoxResult.OK))
            {
                var tmp = new Predmet()
                {
                    SifraPredmeta = SelectedPredmet.SifraPredmeta,
                    BrojESPB = SelectedPredmet.BrojESPB,
                    Semestar = SelectedPredmet.Semestar,
                    SpisakStudenataPolozili = SelectedPredmet.SpisakStudenataPolozili,
                    SpisakStudenataNisuPolozili = SelectedPredmet.SpisakStudenataNisuPolozili,
                    GodinaStudija = SelectedPredmet.GodinaStudija,
                    NazivPredmeta = SelectedPredmet.NazivPredmeta,
                    PredmetniProfesor = SelectedPredmet.PredmetniProfesor,
                };
                // Ukloni predmet iz nepoloženih ili položenih
                if (PredmetsNePolozeni.Contains(SelectedPredmet))
                {
                    SelectedPredmet.SpisakStudenataNisuPolozili.Remove(EditStudent);
                    PredmetsNePolozeni.Remove(tmp);
                }
                else if (PredmetsPolozeni.Contains(SelectedPredmet))
                {
                    SelectedPredmet.SpisakStudenataPolozili.Remove(EditStudent);
                    PredmetsPolozeni.Remove(tmp);
                }

                // Ažuriraj studenta
                EditStudent.SpisakNepolozenihPredmeta = PredmetsNePolozeni.ToList();
                EditStudent.SpisakPolozenihIspita = PredmetsPolozeni.ToList();
                StudentService.AzurirajStudenta(EditStudent);
                PredmetService.Azuriraj(tmp);

                // Osveži prikaz
                OnPropertyChanged(nameof(PredmetsNePolozeni));
                OnPropertyChanged(nameof(PredmetsPolozeni));

            }
        }

    }
}
