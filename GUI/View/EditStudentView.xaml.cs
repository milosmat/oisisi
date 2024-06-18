using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public EditStudentView(Student selectedStud)
    {
        InitializeComponent();
        _student = selectedStud;
        EditStudent = _student;
        DataContext = this;
        CmbTrenutnaGodinaStudija.SelectedIndex = EditStudent.TrenutnaGodinaStudija - 1;
        CmbNacinFinansiranja.SelectedIndex = EditStudent.Status.Equals(StatusEnum.Budzet) ? 0 : 1;
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
}
