using StudentskaSluzba.Model;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Student> students = new()
        {
            new Student { Id = 1, Ime = "Marko", Prezime = "Markovic", Status = StatusEnum.Budzet },
        };
        public ObservableCollection<Student> Students
        {
            get => students;
            set => students = value;
        }

        private TabItem? selected;
        
        public MainWindow()
        {
            InitializeComponent();
            Students = students;
            DataContext = this;
            SetMenuIcons();
            selected = Tabs1.SelectedItem as TabItem;
            StatusBarText.Content += " - " + selected?.Header.ToString();
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
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Tabs1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = Tabs1.SelectedItem as TabItem;
            var text = StatusBarText.Content as String;
            text = text?[..text.LastIndexOf('-')];
            StatusBarText.Content += "- " + selected?.Header.ToString();
        }
    }
}
