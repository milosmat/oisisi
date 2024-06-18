using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class DodajProfesoraView : Window
{
    public event EventHandler? OnFinish;
    public DodajProfesoraView()
    {
        InitializeComponent();
        ValidateInputs(null, null);
    }

    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        var adr = TxtAdresaStanovanja.Text.Split(" ");
        var p = new Profesor()
        {
            Ime = TxtIme.Text,
            Prezime = TxtPrezime.Text,
            DatumRodjenja = DateTime.ParseExact(TxtDatumRodjenja.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture),
            KontaktTelefon = TxtBrojTelefona.Text,
            BrojLicneKarte = TxtLicnaKarta.Text,
            GodineStaza = int.Parse(TxtGodineStaza.Text),
            EmailAdresa = TxtEmailAdresa.Text,
            AdresaStanovanja = new Adresa()
            {
                Drzava = adr.Length > 3 ? adr[3] : string.Empty,
                Grad = adr.Length > 2 ? adr[2] : string.Empty,
                Ulica = adr.Length > 0 ? adr[0] : string.Empty,
                Broj = adr.Length > 1 ? int.Parse(adr[1]) : 0
            },
            Zvanje = TxtZvanje.Text
        };
        string? title = FindResource("AddTitle") as string;
        string? success = FindResource("AddSuccess") as string;
        string? fail = FindResource("AddFail") as string;
        var status = CRUDEntitetaService.DodajProfesora(p);
        MessageBox.Show(status ? success : fail, title);
        DialogResult = status;
        Close();
    }

    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ValidateInputs(object sender, RoutedEventArgs e)
    {
        bool isValid = true;

        // Ime
        if (string.IsNullOrWhiteSpace(TxtIme.Text))
        {
            isValid = false;
            LblImeError.Content = "Ime je obavezno.";
        }
        else
        {
            LblImeError.Content = string.Empty;
        }

        // Prezime
        if (string.IsNullOrWhiteSpace(TxtPrezime.Text))
        {
            isValid = false;
            LblPrezimeError.Content = "Prezime je obavezno.";
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
