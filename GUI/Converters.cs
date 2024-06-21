using System;
using System.Globalization;
using System.Windows.Data;
using StudentskaSluzba.Model;

namespace GUI
{
    public class AddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Adresa adresa)
            {
                return $"{adresa.Ulica}, {adresa.Broj}, {adresa.Grad}, {adresa.Drzava}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string addressString)
            {
                if (string.IsNullOrWhiteSpace(addressString))
                {
                    return new Adresa
                    {
                        Ulica = string.Empty,
                        Broj = 0,
                        Grad = string.Empty,
                        Drzava = string.Empty
                    };
                }

                var parts = addressString.Split(new[] { ", " }, StringSplitOptions.None);
                return new Adresa
                {
                    Ulica = parts.Length > 0 ? parts[0] : string.Empty,
                    Broj = parts.Length > 1 && int.TryParse(parts[1], out var broj) ? broj : 0,
                    Grad = parts.Length > 2 ? parts[2] : string.Empty,
                    Drzava = parts.Length > 3 ? parts[3] : string.Empty
                };
            }
            return new Adresa();
        }
    }




    public class IndeksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Indeks indeks)
            {
                return $"{indeks.OznakaSmera} {indeks.BrojUpisa}/{indeks.GodinaUpisa}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string indeksString)
            {
                if (string.IsNullOrWhiteSpace(indeksString))
                {
                    return new Indeks
                    {
                        OznakaSmera = string.Empty,
                        BrojUpisa = 0,
                        GodinaUpisa = 0
                    };
                }

                var parts = indeksString.Split(new[] { ' ', '/' }, StringSplitOptions.None);
                return new Indeks
                {
                    OznakaSmera = parts.Length > 0 ? parts[0] : string.Empty,
                    BrojUpisa = parts.Length > 1 && int.TryParse(parts[1], out var brojUpisa) ? brojUpisa : 0,
                    GodinaUpisa = parts.Length > 2 && int.TryParse(parts[2], out var godinaUpisa) ? godinaUpisa : 0
                };
            }
            return new Indeks();
        }
    }



    public class EnrollmentYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Indeks indeks) return indeks.GodinaUpisa.ToString();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}