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

public partial class EditProfesorView : Window, INotifyPropertyChanged
{
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public EditProfesorView(Profesor selectedProf)
    {
        InitializeComponent();
        _profesor = selectedProf;
        EditProfesor = _profesor;
        DataContext = this;
        ValidateInputs(null, null);
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = CRUDEntitetaService.IzmeniProfestora(EditProfesor);
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        Close();
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
            LblDatumRodjenjaError.Content = "Datum rođenja nije validan.";
        }
        else
        {
            LblDatumRodjenjaError.Content = string.Empty;
        }

        // Adresa stanovanja
        if (string.IsNullOrWhiteSpace(TxtAdresaStanovanja.Text) || TxtAdresaStanovanja.Text.Split(" ").Length < 4)
        {
            isValid = false;
            LblAdresaStanovanjaError.Content = "Adresa stanovanja nije validna.";
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
