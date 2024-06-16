using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using StudentskaSluzba.Model;
using StudentskaSluzba.Service;

namespace GUI.View;

public partial class EditStudentView : Window
{
    private Student _student;
    public Student EditStudent
    {
        get => _student;
        set
        {
            _student = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public EditStudentView(Student selectedStud)
    {
        InitializeComponent();
        _student = selectedStud;
        EditStudent = _student;
        DataContext = this;
        CmbTrenutnaGodinaStudija.SelectedIndex = EditStudent.TrenutnaGodinaStudija - 1;
        CmbNacinFinansiranja.SelectedIndex = EditStudent.Status.Equals(StatusEnum.Budzet) ? 0 : 1;
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