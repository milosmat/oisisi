using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CLI.Service;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditPredmetView : Window, INotifyPropertyChanged
{
    private Predmet _predmet;

    public Predmet EditPredmet
    {
        get => _predmet;
        set
        {
            _predmet = value;
            OnPropertyChanged();
        }
    }
    public event EventHandler? OnFinish;
    private Profesor? _profesor;

    public Profesor? SelectedProfesor
    {
        get => _profesor;
        set
        {
            _profesor = value;
            EditPredmet.PredmetniProfesor = value;
            OnPropertyChanged();
        }
    }

    private List<Profesor> _profesors;

    public List<Profesor> Profesors
    {
        get => _profesors;
        set
        {
            _profesors = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public EditPredmetView()
    {
        InitializeComponent();
        DataContext = this;
    }

    public EditPredmetView(Predmet predmet)
    {
        InitializeComponent();
        EditPredmet = predmet;
        CmbSemestar.SelectedIndex = EditPredmet.Semestar == SemestarEnum.Letnji ? 0 : 1;
        _profesors = ProfesorService.GetProfesors();
        _profesor = EditPredmet.PredmetniProfesor;
        Profesors = _profesors;
        SelectedProfesor = Profesors.Find(p => p.Id == _profesor?.Id);
        DataContext = this;
        ValidateInputs(null, null);
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(EditPredmet.ToString());
        if (CRUDEntitetaService.IzmeniPredmet(EditPredmet))
        {
            MessageBox.Show(EditPredmet.ToString());
            EditPredmet = PredmetService.GetByid(EditPredmet.SifraPredmeta);
            OnFinish?.Invoke(this, EventArgs.Empty); // Poziv OnFinish događaja
            this.DialogResult = true;
        }
        else
        {
            this.DialogResult = false;
        }
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
        if (string.IsNullOrWhiteSpace(TxtGodinaStudija.Text) || !int.TryParse(TxtGodinaStudija.Text, out int godinaStudija) || godinaStudija < 1 || godinaStudija > 8)
        {
            isValid = false;
            LblGodinaStudijaError.Content = "Godina studija mora biti između 1 i 8.";
        }
        else
        {
            LblGodinaStudijaError.Content = string.Empty;
        }

        // Broj ESPB
        if (string.IsNullOrWhiteSpace(TxtEspb.Text) || !int.TryParse(TxtEspb.Text, out int espb) || espb < 1)
        {
            isValid = false;
            LblEspbError.Content = "Broj ESPB mora biti pozitivan broj.";
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

        BtnPotvrdi.IsEnabled = isValid;
    }

    private void RemoveProfessor_Click(object sender, RoutedEventArgs e)
    {
        if(SelectedProfesor == null) return;
        SelectedProfesor = null;
    }
}
