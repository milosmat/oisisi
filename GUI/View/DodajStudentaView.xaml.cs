using StudentskaSluzba.Model;
using StudentskaSluzba.Service;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace GUI.View;

public partial class DodajStudentaView : Window
{

    public event EventHandler? OnFinish;
    
    public DodajStudentaView()
    {
        InitializeComponent();
    }

    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        var adr = TxtAdresaStanovanja.Text.Split(" ");
        Student s = new()
        {
            AdresaStanovanja = CRUDEntitetaService.DodajAdresu(new Adresa
            {
                Drzava = adr[2],
                Ulica = adr[0],
                Broj = int.Parse(adr[1])
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
            Status = ((ComboBoxItem)CmbTrenutnaGodinaStudija.SelectedItem).Tag is StatusEnum ? (StatusEnum)((ComboBoxItem)CmbTrenutnaGodinaStudija.SelectedItem).Tag : StatusEnum.Budzet
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
}