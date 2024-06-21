using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CLI.Service;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class DodajPredmetView : Window, INotifyPropertyChanged
{
    public event EventHandler? OnFinish;

    private List<Profesor> _profesors = ProfesorService.GetProfesors();

    public List<Profesor> Profesors
    {
        get => _profesors;
        set
        {
            _profesors = value;
            OnPropertyChanged();
        }
    }

    private Profesor _profesor = ProfesorService.GetProfesors().FirstOrDefault() ?? throw new InvalidOperationException();

    public Profesor SelectedProfesor
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

    public DodajPredmetView()
    {
        _profesors = ProfesorService.GetProfesors();
        Profesors = _profesors;
        InitializeComponent();
        DataContext = this;
        ValidateInputs(null, null);
    }

    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtEspb.Text, out int espb) || espb <= 0)
        {
            MessageBox.Show("Broj ESPB bodova nije validan.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Predmet p = new Predmet()
        {
            SifraPredmeta = TxtSifra.Text,
            NazivPredmeta = TxtNaziv.Text,
            PredmetniProfesor = SelectedProfesor,
            Semestar = ((ComboBoxItem)CmbSemestar.SelectedItem).Tag is SemestarEnum
                ? (SemestarEnum)((ComboBoxItem)CmbSemestar.SelectedItem).Tag
                : SemestarEnum.Letnji,
            GodinaStudija = int.Parse(TxtGodinaStudija.Text),
            BrojESPB = espb
        };

        string? title = FindResource("AddTitle") as string;
        string? success = FindResource("AddSuccess") as string;
        string? fail = this.FindResource("AddFail") as string;

        MessageBox.Show(CRUDEntitetaService.DodajPredmet(p) ? success : fail, title);
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

        // Šifra predmeta
        if (string.IsNullOrWhiteSpace(TxtSifra.Text))
        {
            isValid = false;
            LblSifraError.Content = "Šifra predmeta je obavezna.";
        }
        else
        {
            LblSifraError.Content = string.Empty;
        }

        // Naziv predmeta
        if (string.IsNullOrWhiteSpace(TxtNaziv.Text))
        {
            isValid = false;
            LblNazivError.Content = "Naziv predmeta je obavezan.";
        }
        else
        {
            LblNazivError.Content = string.Empty;
        }

        // Godina studija
        if (string.IsNullOrWhiteSpace(TxtGodinaStudija.Text) || !int.TryParse(TxtGodinaStudija.Text, out int godina) || godina <= 0 || godina >= 9)
        {
            isValid = false;
            LblGodinaStudijaError.Content = "Godina studija u kojoj se predmet izvodi mora biti validan pozitivan broj od 1 do 8.";
        }
        else
        {
            LblGodinaStudijaError.Content = string.Empty;
        }

        // ESPB bodovi
        if (string.IsNullOrWhiteSpace(TxtEspb.Text) || !int.TryParse(TxtEspb.Text, out int espb) || espb <= 0)
        {
            isValid = false;
            LblEspbError.Content = "Broj ESPB bodova mora biti validan pozitivan broj.";
        }
        else
        {
            LblEspbError.Content = string.Empty;
        }

        // Semestar
        if (CmbSemestar.SelectedItem == null)
        {
            isValid = false;
            LblSemestarError.Content = "Semestar je obavezan.";
        }
        else
        {
            LblSemestarError.Content = string.Empty;
        }

        // Profesor
        if (CmbProfesor.SelectedItem == null)
        {
            isValid = false;
            LblProfesorError.Content = "Profesor je obavezan.";
        }
        else
        {
            LblProfesorError.Content = string.Empty;
        }

        BtnPotvrdi.IsEnabled = isValid;
    }
}
