using StudentskaSluzba.Model;
using StudentskaSluzba.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GUI.View;

public partial class EditStudentView : Window, INotifyPropertyChanged
{
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

    private List<Predmet> _predmets;
    public List<Predmet> PredmetsNePolozeni
    {
        get => _predmets;
        set
        {
            EditStudent.SpisakNepolozenihPredmeta = value;
            _predmets = value;
            OnPropertyChanged();
        }
    }

    private List<Predmet> _predmetsPolozeni;
    public List<Predmet> PredmetsPolozeni
    {
        get => _predmetsPolozeni;
        set
        {
            EditStudent.SpisakPolozenihIspita = value;
            _predmetsPolozeni = value;
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
        CmbTrenutnaGodinaStudija.SelectedIndex = EditStudent.TrenutnaGodinaStudija - 1;
        CmbNacinFinansiranja.SelectedIndex = EditStudent.Status.Equals(StatusEnum.Budzet) ? 0 : 1;
        PredmetsNePolozeni = EditStudent.SpisakNepolozenihPredmeta;
        PredmetsPolozeni = EditStudent.SpisakPolozenihIspita;
        ValidateInputs(null, null);
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = CRUDEntitetaService.IzmeniStudenta(EditStudent);
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

        // Ime
        if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || !Regex.IsMatch(FirstNameTextBox.Text, @"^[a-zA-Z]+$"))
        {
            isValid = false;
            LblFirstNameError.Content = "Ime je obavezno i mora sadržati samo slova.";
        }
        else
        {
            LblFirstNameError.Content = string.Empty;
        }

        // Prezime
        if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) || !Regex.IsMatch(LastNameTextBox.Text, @"^[a-zA-Z]+$"))
        {
            isValid = false;
            LblLastNameError.Content = "Prezime je obavezno i mora sadržati samo slova.";
        }
        else
        {
            LblLastNameError.Content = string.Empty;
        }

        // Datum rođenja
        if (!DateTime.TryParseExact(BirthDateTextBox.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            isValid = false;
            LblBirthDateError.Content = "Datum rođenja nije validan.";
        }
        else
        {
            LblBirthDateError.Content = string.Empty;
        }

        // Adresa stanovanja
        if (string.IsNullOrWhiteSpace(AddressTextBox.Text) || AddressTextBox.Text.Split(" ").Length < 3)
        {
            isValid = false;
            LblAddressError.Content = "Adresa stanovanja nije validna.";
        }
        else
        {
            LblAddressError.Content = string.Empty;
        }

        // Broj telefona
        if (string.IsNullOrWhiteSpace(PhoneTextBox.Text) || !Regex.IsMatch(PhoneTextBox.Text, @"^\d+$"))
        {
            isValid = false;
            LblPhoneError.Content = "Broj telefona nije validan.";
        }
        else
        {
            LblPhoneError.Content = string.Empty;
        }

        // Email adresa
        if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            isValid = false;
            LblEmailError.Content = "Email adresa nije validna.";
        }
        else
        {
            LblEmailError.Content = string.Empty;
        }

        // Broj indeksa
        if (string.IsNullOrWhiteSpace(IndexNumberTextBox.Text) || !Regex.IsMatch(IndexNumberTextBox.Text, @"^[A-Z]{2}\s\d{1,3}/\d{4}$"))
        {
            isValid = false;
            LblIndexError.Content = "Broj indeksa mora biti formata XX 123/1234.";
        }
        else
        {
            LblIndexError.Content = string.Empty;
        }

        // Godina upisa
        if (string.IsNullOrWhiteSpace(EnrollmentYearTextBox.Text) || !int.TryParse(EnrollmentYearTextBox.Text, out int godinaUpisa) || godinaUpisa <= 0)
        {
            isValid = false;
            LblEnrollmentYearError.Content = "Godina upisa mora biti validan pozitivan broj.";
        }
        else
        {
            LblEnrollmentYearError.Content = string.Empty;
        }

        // Trenutna godina studija
        if (CmbTrenutnaGodinaStudija.SelectedItem == null)
        {
            isValid = false;
            LblCurrentYearError.Content = "Trenutna godina studija je obavezna.";
        }
        else
        {
            LblCurrentYearError.Content = string.Empty;
        }

        // Način finansiranja
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
        }
    }

    private void UndoPassedExam_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedPredmet == null) return;
        MessageBoxResult res = MessageBox.Show("Da li ste sigurni da hoćete da poništite ocenu?", "Upozorenje", MessageBoxButton.OKCancel);
        if (res.Equals(MessageBoxResult.OK))
        {
            CRUDEntitetaService.PonistiOcenu(SelectedPredmet.SifraPredmeta, EditStudent.Id);
        }
    }

    private void AddGrade_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedPredmet == null) return;
        AddGradeView gradeView = new AddGradeView(SelectedPredmet, _student);
        if (gradeView.ShowDialog() == true)
        {
            MessageBox.Show("Ocena uspešno upisana");
        }
    }

    private void TabChangedEvent(object sender, RoutedEventArgs e)
    {
        SelectedPredmet = null;
    }

    private void RemoveSubject_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedPredmet == null) return;
        MessageBoxResult res = MessageBox.Show("Da li ste sigurni da hoćete da poništite ocenu?", "Upozorenje", MessageBoxButton.OKCancel);
        if (res.Equals(MessageBoxResult.OK))
        {
            CRUDEntitetaService.PonistiOcenu(SelectedPredmet.SifraPredmeta, EditStudent.Id);
        }
    }
}
