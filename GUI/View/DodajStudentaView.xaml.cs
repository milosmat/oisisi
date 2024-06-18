using StudentskaSluzba.Model;
using StudentskaSluzba.Service;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GUI.View;

public partial class DodajStudentaView : Window
{
    public event EventHandler? OnFinish;

    public DodajStudentaView()
    {
        InitializeComponent();
        ValidateInputs(null, null);
    }

    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        var adr = TxtAdresaStanovanja.Text.Split(" ");
        Student s = new()
        {
            AdresaStanovanja = CRUDEntitetaService.DodajAdresu(new Adresa
            {
                Drzava = adr.Length > 2 ? adr[2] : string.Empty,
                Ulica = adr.Length > 0 ? adr[0] : string.Empty,
                Broj = adr.Length > 1 ? int.Parse(adr[1]) : 0
            }),
            Ime = TxtIme.Text,
            Prezime = TxtPrezime.Text,
            DatumRodjenja = DateTime.ParseExact(TxtDatumRodjenja.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture),
            EmailAdresa = TxtEmailAdresa.Text,
            KontaktTelefon = TxtBrojTelefona.Text,
            BrojIndeksa = CRUDEntitetaService.DodajIndeks(new Indeks
            {
                GodinaUpisa = int.Parse(TxtGodinaUpisa.Text),
                OznakaSmera = TxtBrojIndeksa.Text
            }),
            TrenutnaGodinaStudija = int.Parse("" + ((ComboBoxItem)CmbTrenutnaGodinaStudija.SelectedItem).Tag),
            Status = ((ComboBoxItem)CmbNacinFinansiranja.SelectedItem).Tag is StatusEnum
                ? (StatusEnum)((ComboBoxItem)CmbNacinFinansiranja.SelectedItem).Tag
                : StatusEnum.Budzet
        };
        string? title = FindResource("AddTitle") as string;
        string? success = FindResource("AddSuccess") as string;
        string? fail = FindResource("AddFail") as string;

        MessageBox.Show(CRUDEntitetaService.DodajStudenta(s) ? success : fail,
            title);
        OnFinish?.Invoke(sender, e);
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
        if (string.IsNullOrWhiteSpace(TxtAdresaStanovanja.Text) || TxtAdresaStanovanja.Text.Split(" ").Length < 3)
        {
            isValid = false;
            LblAdresaStanovanjaError.Content = "Adresa stanovanja nije validna.";
        }
        else
        {
            LblAdresaStanovanjaError.Content = string.Empty;
        }

        // Broj telefona
        if (string.IsNullOrWhiteSpace(TxtBrojTelefona.Text) || !Regex.IsMatch(TxtBrojTelefona.Text, @"^\d+$"))
        {
            isValid = false;
            LblBrojTelefonaError.Content = "Broj telefona nije validan.";
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

        // Broj indeksa
        if (string.IsNullOrWhiteSpace(TxtBrojIndeksa.Text) || !Regex.IsMatch(TxtBrojIndeksa.Text, @"^[A-Z]{2}\s\d{1,3}/\d{4}$"))
        {
            isValid = false;
            LblBrojIndeksaError.Content = "Broj indeksa mora biti formata XX 123/1234.";
        }
        else
        {
            LblBrojIndeksaError.Content = string.Empty;
        }

        // Godina upisa
        if (string.IsNullOrWhiteSpace(TxtGodinaUpisa.Text) || !int.TryParse(TxtGodinaUpisa.Text, out int godinaUpisa) || godinaUpisa <= 0)
        {
            isValid = false;
            LblGodinaUpisaError.Content = "Godina upisa mora biti validan pozitivan broj.";
        }
        else
        {
            LblGodinaUpisaError.Content = string.Empty;
        }

        // Trenutna godina studija
        if (CmbTrenutnaGodinaStudija.SelectedItem == null)
        {
            isValid = false;
            LblTrenutnaGodinaError.Content = "Trenutna godina studija je obavezna.";
        }
        else
        {
            LblTrenutnaGodinaError.Content = string.Empty;
        }

        // Način finansiranja
        if (CmbNacinFinansiranja.SelectedItem == null)
        {
            isValid = false;
            LblNacinFinansiranjaError.Content = "Način finansiranja je obavezan.";
        }
        else
        {
            LblNacinFinansiranjaError.Content = string.Empty;
        }

        BtnPotvrdi.IsEnabled = isValid;
    }
}
