using System.Windows;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditStudentView : Window
{
    public Student EditStudent;
    
    public EditStudentView(Student selectedStud)
    {
        InitializeComponent();
        EditStudent = selectedStud;
        DataContext = this;
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = CRUDEntitetaService.IzmeniStudenta(EditStudent);
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        Close();
    }
}