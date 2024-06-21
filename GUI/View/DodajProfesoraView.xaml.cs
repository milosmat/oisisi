using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View
{
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
            // Parsiranje adrese
            var adrParts = TxtAdresaStanovanja.Text.Split(", ");
            Adresa adresaStanovanja = new Adresa
            {
                Ulica = adrParts.Length > 0 ? adrParts[0] : string.Empty,
                Broj = adrParts.Length > 1 && int.TryParse(adrParts[1], out int broj) ? broj : 0,
                Grad = adrParts.Length > 2 ? adrParts[2] : string.Empty,
                Drzava = adrParts.Length > 3 ? adrParts[3] : string.Empty
            };

            var p = new Profesor()
            {
                Ime = TxtIme.Text,
                Prezime = TxtPrezime.Text,
                DatumRodjenja = DateTime.ParseExact(TxtDatumRodjenja.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                KontaktTelefon = TxtBrojTelefona.Text,
                BrojLicneKarte = TxtLicnaKarta.Text,
                GodineStaza = int.Parse(TxtGodineStaza.Text),
                EmailAdresa = TxtEmailAdresa.Text,
                AdresaStanovanja = adresaStanovanja,
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
