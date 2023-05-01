using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlightScoreboard.Converters
{
    public class BoolYesterdayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool flag && targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                Uri uri;
                string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                uri = flag ? new Uri($"avares://{assemblyName}/Assets/yesterday_yes.jpg") : new Uri($"avares://{assemblyName}/Assets/yesterday_no.jpg");

                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                var asset = assets.Open(uri);

                return new Bitmap(asset);
            }
            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
