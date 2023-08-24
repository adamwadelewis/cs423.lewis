using System.Windows;
using System.Windows.Media;

namespace M03Brushes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            SolidColorBrush brush = new SolidColorBrush();
            // Predefined brush in Brushes Class:
            brush = Brushes.Red;
            rect1.Fill = brush;
            // From predefined color name in the Colors class:
            brush = new SolidColorBrush(Colors.Green);
            rect2.Fill = brush;
            // From sRGB values in the Color strutcure:
            brush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
            rect3.Fill = brush;
            // From ScRGB values in the Color structure:
            brush = new SolidColorBrush(Color.FromScRgb(0.5f, 0.7f, 0.0f, 0.5f));
            rect4.Fill = brush;
            // From a Hex string using ColorConverter:
            brush = new
            SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBFFFFAA"));
            rect5.Fill = brush;
        }

    }
}
