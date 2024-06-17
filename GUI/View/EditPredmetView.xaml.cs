using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CLI.Service;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditPredmetView : Window
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

    private Profesor _profesor;

    public Profesor SelectedProfesor
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
    public EditPredmetView(Predmet predmet)
    {
        InitializeComponent();
        EditPredmet = predmet;
        CmbSemestar.SelectedIndex = EditPredmet.Semestar == SemestarEnum.Letnji ? 0 : 1;
        _profesors = ProfesorService.GetProfesors();
        _profesor = EditPredmet.PredmetniProfesor;
        Profesors = _profesors;
        SelectedProfesor = Profesors.Find(p => p.Id == _profesor.Id);
        DataContext = this;

    }
    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = CRUDEntitetaService.IzmeniPredmet(EditPredmet);
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        Close();
    }
}