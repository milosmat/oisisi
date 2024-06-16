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
                return $"{adresa.Ulica} {adresa.Broj}, {adresa.Grad}, {adresa.Drzava}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not implemented for AddressConverter.");
        }
    }

    public class IndeksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Indeks indeks) return indeks.ToString();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string parms)
            {
                var args = parms.Split(" ");
                var upis = args[1].Split("/");
                try
                {
                    return new Indeks()
                    {
                        OznakaSmera = args[0],
                        BrojUpisa = int.Parse(upis[0]),
                        GodinaUpisa = int.Parse(upis[1])
                    };
                }
                catch (Exception e)
                {
                    return new object();
                }
            }

            return new object();
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