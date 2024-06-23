using System;
using System.Globalization;
using System.Windows.Data;
using CLI.DTO;
using CLI.Service;
using StudentskaSluzba.Model;

namespace GUI.Converters
{
    public class PolozeniPredmetiDTOToPredmetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PolozeniPredmetiDTO dto)
            {
                return PredmetService.GetByid(dto.SifraPredmeta);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Predmet predmet)
            {
                return new PolozeniPredmetiDTO
                {
                    SifraPredmeta = predmet.SifraPredmeta,
                    NazivPredmeta = predmet.NazivPredmeta,
                    ESPB = predmet.BrojESPB
                    // Dodaj druge potrebne vrednosti ako je potrebno
                };
            }

            return null;
        }
    }

}
