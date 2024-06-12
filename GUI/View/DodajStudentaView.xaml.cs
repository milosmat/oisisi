using StudentskaSluzba.Model;
using StudentskaSluzba.Service;
using System;
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
            AdresaStanovanja = DodavanjeEntitetaService.DodajAdresu(new Adresa
            {
                Drzava = adr[2],
                Ulica = adr[0],
                Broj = int.Parse(adr[1])
            }),
            Ime = TxtIme.Text,
            Prezime = TxtPrezime.Text,
            DatumRodjenja = DateTime.Parse(TxtDatumRodjenja.Text),
            EmailAdresa = TxtEmailAdresa.Text,
            KontaktTelefon = TxtBrojTelefona.Text,
            BrojIndeksa = DodavanjeEntitetaService.DodajIndeks(new Indeks
            {
                GodinaUpisa = int.Parse(TxtGodinaUpisa.Text),
                OznakaSmera = TxtBrojIndeksa.Text
            }),
            TrenutnaGodinaStudija = int.Parse("" + ((ComboBoxItem)CmbTrenutnaGodinaStudija.SelectedItem).Tag),
            Status = ((ComboBoxItem)CmbTrenutnaGodinaStudija.SelectedItem).Tag is StatusEnum ? (StatusEnum)((ComboBoxItem)CmbTrenutnaGodinaStudija.SelectedItem).Tag : StatusEnum.Budzet
        };
        string? title = (string)Application.Current.FindResource("AddTitle");
        string? success = (string)Application.Current.FindResource("AddSuccess");
        string? fail = (string)Application.Current.FindResource("AddFail");
        
        MessageBox.Show(DodavanjeEntitetaService.DodajStudenta(s) ? success : fail,
            title);
        OnFinish?.Invoke(sender, e);
        Close();
    }

    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }
}