using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics; 

namespace M03Mandelbrot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rect area = new Rect(new Point(-2.4, -1.5), new Point(0.8, 1.5));
        private Rectangle selection =
            new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Visibility = Visibility.Collapsed
            };

        private bool mouseDown = false;
        private Point mouseDownPos;

        public MainWindow()
        {
            InitializeComponent();
            drawImage.Source = drawMandelbrotSet(area);
        }

        Int32 mandelbrot(Complex c)
        {
            Int32 count = 0;
            Complex z = Complex.Zero;
            while (count < 1000 && z.Magnitude < 4)
            {
                z = z * z + c;
                count++;
            }
            return count;
        }

        private Color colorMap(int count)
        {
            Color C = new Color();
            C.B = (byte)(count / 100 * 25);
            count = count % 100;
            C.G = (byte)(count / 10 * 25);
            C.R = (byte)(count % 10 * 25);
            C.A = 255;
            return C;
        }
        private WriteableBitmap drawMandelbrotSet(Rect area)
        {
            tbWorking.Text = "Computing Mandelbrot Set";
            int pixelHeight = (int)drawImage.Height;
            int pixelWidth = (int)drawImage.Width;
            WriteableBitmap wbmap = new WriteableBitmap(
                pixelWidth,
                pixelHeight,
                96, 96,
                PixelFormats.Bgra32,
                null
                );
            int bytesPerPixel = wbmap.Format.BitsPerPixel / 8;
            byte[] pixels = new byte[pixelHeight * pixelWidth * bytesPerPixel];
            int stride = pixelWidth * bytesPerPixel;
            double xscale = (area.Right - area.Left) / pixelWidth;
            double yscale = (area.Top - area.Bottom) / pixelHeight;
            for (int i = 0; i < pixels.Length; i += bytesPerPixel)
            {
                int py = i / stride;
                int px = i % stride / bytesPerPixel;
                double x = area.Left + px * xscale;
                double y = area.Top - py * yscale;
                Complex c = new Complex(x, y);
                int count = mandelbrot(c);
                Color iteratedColor = colorMap(count);
                pixels[i] = iteratedColor.B;
                pixels[i + 1] = iteratedColor.G;
                pixels[i + 2] = iteratedColor.R;
                pixels[i + 3] = iteratedColor.A;
                wbmap.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixels, stride, 0);
            }
            tbWorking.Text = "";
            return wbmap;
        }

        private void btnRedraw_Click(object sender, RoutedEventArgs e)
        {
            area = new Rect(new Point(-2.4, -1.5), new Point(0.8, 1.5));
            drawImage.Source = drawMandelbrotSet(area);
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void cvsMandel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            selection.Visibility = Visibility.Collapsed;
            double xscale = (area.Right - area.Left) /
                                          drawImage.Width;
            double yscale = (area.Top - area.Bottom) /
                                          drawImage.Height;
            Point TopLeft = new Point(area.Left +  Canvas.GetLeft(selection) * xscale,
                                        area.Top - Canvas.GetTop(selection)  * yscale);
            Point BottomRight = TopLeft 
                                + new Vector(selection.Width * xscale, -selection.Height * yscale);
            area = new Rect(TopLeft, BottomRight);
            drawImage.Source = drawMandelbrotSet(area);
        }

        private void cvsMandel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point mousepos = e.GetPosition(cvsMandel);
                Vector diff = mousepos - mouseDownPos;
                Point TopLeft = mouseDownPos;
                if (diff.X < 0)
                {
                    TopLeft.X = mousepos.X;
                    diff.X = -diff.X;
                }
                if (diff.Y < 0)
                {
                    TopLeft.Y = mousepos.Y;
                    diff.Y = -diff.Y;
                }
                selection.Width = diff.X;
                selection.Height = diff.Y;
                Canvas.SetLeft(selection, TopLeft.X);
                Canvas.SetTop(selection, TopLeft.Y);
            }
        }

        private void cvsMandel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;
            mouseDownPos = e.GetPosition(cvsMandel);
            Canvas.SetLeft(selection, mouseDownPos.X);
            Canvas.SetTop(selection, mouseDownPos.Y);
            selection.Width = 0;
            selection.Height = 0;
            selection.Visibility = Visibility.Visible;
        }
    }
}
