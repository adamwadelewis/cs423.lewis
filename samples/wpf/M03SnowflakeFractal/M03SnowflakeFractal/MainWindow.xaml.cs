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

namespace M03SnowflakeFractal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private double distanceScale = 1.0 / 3;
        double[] dTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };
        Polyline pl = new Polyline();
        private Point SnowflakePoint = new Point();
        private double SnowflakeSize;
        private int animationCounter = 0;
        private int i = 0;
        public MainWindow()
        {
            InitializeComponent();
            // determine the size of the snowflake:
            double ysize = 0.8 * canvas1.Height /
            (Math.Sqrt(3) * 4 / 3);
            double xsize = 0.8 * canvas1.Width / 2;
            double size = 0;
            if (ysize < xsize)
                size = ysize;
            else
                size = xsize;
            SnowflakeSize = 2 * size;
            pl.Stroke = Brushes.Blue;
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();

            tbLabel.Text = "";
            i = 0;
            animationCounter = 0;
            canvas1.Children.Add(pl);
            CompositionTarget.Rendering += StartAnimation;
        }
        private void StartAnimation(object sender, EventArgs e)
        {
            i += 1;
            if (i % 60 == 0)
            {
                pl.Points.Clear();
                DrawSnowFlake(canvas1, SnowflakeSize, animationCounter);
                string str = "Snow Flake - Depth = " +
                animationCounter.ToString();
                tbLabel.Text = str;
                animationCounter += 1;
                if (animationCounter > 5)
                {
                    tbLabel.Text = "Snow Flake - Depth = 5. Finished";
                    CompositionTarget.Rendering -=
                    StartAnimation;
                }
            }
        }
        private void SnowFlakeEdge(Canvas canvas,
                int depth, double theta, double distance)
        {
            Point pt = new Point();
            if (depth <= 0)
            {
                pt.X = SnowflakePoint.X +
                distance * Math.Cos(theta);
                pt.Y = SnowflakePoint.Y +
                distance * Math.Sin(theta);
                pl.Points.Add(pt);
                SnowflakePoint = pt;
                return;
            }
            distance *= distanceScale;
            for (int j = 0; j < 4; j++)
            {
                theta += dTheta[j];
                SnowFlakeEdge(canvas, depth - 1,
                theta, distance);
            }
        }

        private void DrawSnowFlake(Canvas canvas, double length, int depth)
        {
            double xmid = canvas.Width / 2;
            double ymid = canvas.Height / 2;
            Point[] pta = new Point[4];
            pta[0] = new Point(xmid, ymid + length / 2 *
            Math.Sqrt(3) * 2 / 3);
            pta[1] = new Point(xmid + length / 2,
            ymid - length / 2 * Math.Sqrt(3) / 3);
            pta[2] = new Point(xmid - length / 2,
            ymid - length / 2 * Math.Sqrt(3) / 3);
            pta[3] = pta[0];
            pl.Points.Add(pta[0]);
            for (int j = 1; j < pta.Length; j++)
            {
                double x1 = pta[j - 1].X;
                double y1 = pta[j - 1].Y;
                double x2 = pta[j].X;
                double y2 = pta[j].Y;
                double dx = x2 - x1;
                double dy = y2 - y1;
                double theta = Math.Atan2(dy, dx);
                SnowflakePoint = new Point(x1, y1);
                SnowFlakeEdge(canvas, depth, theta, length);
            }
        }
    }
}
