using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditProfesorView : Window
{
    public Profesor EditProfesor;
    public EditProfesorView(Profesor selectedProf)
    {
        InitializeComponent();
        EditProfesor = selectedProf;
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