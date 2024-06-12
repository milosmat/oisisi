using StudentskaSluzba.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CLI.Service;
using GUI.View;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Student> students;
        public ObservableCollection<Student> Students
        {
            get => students;
            set
            {
                students = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<Predmet> predmets;
        public ObservableCollection<Predmet> Predmets
        {
            get => predmets;
            set
            {
                predmets = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<Profesor> profesors;
        public ObservableCollection<Profesor> Profesors
        {
            get => profesors;
            set
            {
                profesors = value;
                OnPropertyChanged();
            }
        }

        private TabItem? selected;

        public TabItem? Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged();
            }
        }

        private Predmet? _predmet;

        public Predmet? SelectedPredmet
        {
            get => _predmet;
            set
            {
                _predmet = value;
                OnPropertyChanged();
            }
        }

        private Profesor? _profesor;

        public Profesor? SelectedProfesor
        {
            get => _profesor;
            set
            {
                _profesor = value;
                OnPropertyChanged();
            }
        }

        private Student? _student;

        public Student? SelectedStudent
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

        public MainWindow()
        {
            InitializeComponent();
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            students = new ObservableCollection<Student>(StudentService.GetStudents());
            profesors = new ObservableCollection<Profesor>(ProfesorService.GetProfesors());
            predmets = new ObservableCollection<Predmet>(PredmetService.GetPredmets());
            Students = students;
            Profesors = profesors;
            Predmets = predmets;
            DataContext = this;
            SetMenuIcons();
            selected = Tabs1.SelectedItem as TabItem;
            StatusBarText.Content += " - " + selected?.Header;
            StatusDateText.Content = DateTime.Now.ToString("hh:mm dd:MM:yyyy");
            TxtSearch.Margin = TxtSearch.Margin with { Left = TTray.Width - 270 };
        }
        
        private void SetMenuIcons()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var imagesFolder = Path.Combine(baseDirectory, "..", "..", "..", "Resources", "images");

            SetMenuItemIcon(NewMenuItem, Path.Combine(imagesFolder, "add.png"));
            SetMenuItemIcon(SaveMenuItem, Path.Combine(imagesFolder, "diskette.png"));
            SetMenuItemIcon(OpenMenuItem, Path.Combine(imagesFolder, "folder.png"));
            SetMenuItemIcon(CloseMenuItem, Path.Combine(imagesFolder, "close.png"));
            SetMenuItemIcon(EditMenuItem, Path.Combine(imagesFolder, "edit.png"));
            SetMenuItemIcon(DeleteMenuItem, Path.Combine(imagesFolder, "delete.png"));
            SetMenuItemIcon(AboutMenuItem, Path.Combine(imagesFolder, "info.png"));
            SetButtonIcon(BtnNew, Path.Combine(imagesFolder, "add.png"));
            SetButtonIcon(BtnEdit, Path.Combine(imagesFolder, "edit.png"));
            SetButtonIcon(BtnDelete, Path.Combine(imagesFolder, "delete.png"));
            SetButtonIcon(BtnSearch, Path.Combine(imagesFolder, "info.png"));
        }

        private void SetMenuItemIcon(MenuItem menuItem, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                var uri = new Uri(imagePath, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                var image = new Image { Source = bitmap };
                menuItem.Icon = image;
            }
            else
            {
                MessageBox.Show($"Image not found: {imagePath}");
            }
        }
        
        private void SetButtonIcon(Button button, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                var uri = new Uri(imagePath, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                var image = new Image { Source = bitmap };
                button.Content = image;
            }
            else
            {
                MessageBox.Show($"Image not found: {imagePath}");
            }
        }

        private void SwitchLanguage(string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var dict = new ResourceDictionary();
            var resourcesPath = Path.Combine("Resources", "l10n", $"StringResources.{cultureCode}.xaml");
            dict.Source = new Uri(resourcesPath, UriKind.Relative);

            this.Resources.MergedDictionaries[0] = dict;
        }

        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            var selectedLanguage = (e.AddedItems[0] as ComboBoxItem)?.Tag.ToString();
            if (selectedLanguage != null) SwitchLanguage(selectedLanguage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Tabs1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = Tabs1.SelectedItem as TabItem;
            var text = StatusBarText.Content as String;
            text = text?[..text.LastIndexOf('-')];
            StatusBarText.Content = text + "- " + selected?.Header;
        }

        private void RefreshData(object? sender, EventArgs e)
        {
            var lista = StudentService.GetStudents();
            var listaProf = ProfesorService.GetProfesors();
            var listaPred = PredmetService.GetPredmets();
            Students.Clear();
            Predmets.Clear();
            Profesors.Clear();
            lista.ForEach(i => Students.Add(i));
            listaPred.ForEach(i => Predmets.Add(i));
            listaProf.ForEach(i => Profesors.Add(i));
        }

        private void NewEntityBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (Selected?.Tag)
            {
                case "Studenti":
                    DodajStudentaView dsv = new DodajStudentaView();
                    dsv.OnFinish += RefreshData;
                    dsv.Show();
                    break;
                case "Profesori":
                    DodajProfesoraView dpv = new DodajProfesoraView();
                    dpv.OnFinish += RefreshData;
                    dpv.Show();
                    break;
                case "Predmeti":
                    DodajPredmetView dprv = new DodajPredmetView();
                    dprv.OnFinish += RefreshData;
                    dprv.Show();
                    break;
                 default:
                    MessageBox.Show("Greška", "Greška");
                     break;
            }
        }

        private void SaveBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CloseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void EditBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Tabs1.SelectedItem is not TabItem selectedTab) return;
            switch (selectedTab.Tag.ToString())
            {
                case "Studenti":
                    if (SelectedStudent == null)
                    {
                        MessageBox.Show("Nijedan student nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        EditStudentView esv = new EditStudentView(SelectedStudent);
                        MessageBox.Show(esv.DialogResult == true ? "Uspesno izmenjen student!": "Izmene nisu sacuvane!", "Izmena studenta");
                    }
                    break;
            
                case "Profesori":
                    if (SelectedProfesor == null)
                    {
                        MessageBox.Show("Nijedan profesor nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        EditProfesorView esv = new EditProfesorView(SelectedProfesor);
                        // MessageBox.Show(esv.DialogResult == true ? "Uspesno izmenjen student!": "Izmene nisu sacuvane!", "Izmena studenta");
                    }
                    break;

                case "Predmeti":
                    if (SelectedPredmet == null)
                    {
                        MessageBox.Show("Nijedan predmet nije izabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Proceed with editing the selected course
                    }
                    break;

                default:
                    MessageBox.Show("Nepoznata opcija.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

        }

        private void DeleteBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HelpBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Verzija: \t0.1\nAutor: \tMiloš Matunović\nStudent FTN-a E2 smer.");
        }
    }
}
