using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditProfesorView : Window
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
    public EditProfesorView()
    {
        InitializeComponent();
        DataContext = this;
    }
    public EditProfesorView(Profesor selectedProf)
    {
        InitializeComponent();
        _profesor = selectedProf;
        EditProfesor = _profesor;
        DataContext = this;
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
}