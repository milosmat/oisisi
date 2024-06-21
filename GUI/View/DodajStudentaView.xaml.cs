using StudentskaSluzba.Model;
using StudentskaSluzba.Service;
using System;
using System.Globalization;
using System.Linq;
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
        // Parsiranje adrese
        var adrParts = TxtAdresaStanovanja.Text.Split(", ");
        Adresa adresaStanovanja = new Adresa
        {
            Ulica = adrParts.Length > 0 ? adrParts[0] : string.Empty,
            Broj = adrParts.Length > 1 && int.TryParse(adrParts[1], out int broj) ? broj : 0,
            Grad = adrParts.Length > 2 ? adrParts[2] : string.Empty,
            Drzava = adrParts.Length > 3 ? adrParts[3] : string.Empty
        };

        // Parsiranje indeksa
        var indeksParts = TxtBrojIndeksa.Text.Split(' ', '/');
        Indeks brojIndeksa = new Indeks
        {
            OznakaSmera = indeksParts.Length > 0 ? indeksParts[0] : string.Empty,
            BrojUpisa = indeksParts.Length > 1 && int.TryParse(indeksParts[1], out int brojUpisa) ? brojUpisa : 0,
            GodinaUpisa = indeksParts.Length > 2 && int.TryParse(indeksParts[2], out int godinaUpisa) ? godinaUpisa : 0
        };

        Student s = new()
        {
            AdresaStanovanja = CRUDEntitetaService.DodajAdresu(adresaStanovanja),
            Ime = TxtIme.Text,
            Prezime = TxtPrezime.Text,
            DatumRodjenja = DateTime.ParseExact(TxtDatumRodjenja.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture),
            EmailAdresa = TxtEmailAdresa.Text,
            KontaktTelefon = TxtBrojTelefona.Text,
            BrojIndeksa = CRUDEntitetaService.DodajIndeks(brojIndeksa),
            TrenutnaGodinaStudija = int.Parse("" + ((ComboBoxItem)CmbTrenutnaGodinaStudija.SelectedItem).Tag),
            Status = ((ComboBoxItem)CmbNacinFinansiranja.SelectedItem).Tag is StatusEnum
                ? (StatusEnum)((ComboBoxItem)CmbNacinFinansiranja.SelectedItem).Tag
                : StatusEnum.Budzet
        };

        string? title = FindResource("AddTitle") as string;
        string? success = FindResource("AddSuccess") as string;
        string? fail = FindResource("AddFail") as string;

        bool result = CRUDEntitetaService.DodajStudenta(s);
        MessageBox.Show(result ? success : fail, title);

        if (result)
        {
            OnFinish?.Invoke(sender, e);
        }

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
            LblDatumRodjenjaError.Content = "Datum rođenja nije validan. Mora biti oblika dd/MM/yyyy";
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
