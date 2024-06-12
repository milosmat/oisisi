using System;
using System.Windows;

namespace GUI.View;

public partial class DodajProfesoraView : Window
{
    public event EventHandler? OnFinish;
    public DodajProfesoraView()
    {
        InitializeComponent();
    }

    private void BtnPotvrdi_Action(object sender, RoutedEventArgs e)
    {
        
        Close();
    }
    
    private void BtnOdustani_Action(object sender, RoutedEventArgs e)
    {
        Close();
    }
}