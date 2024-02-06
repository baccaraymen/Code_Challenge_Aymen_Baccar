using Code_Challenge_Aymen_Baccar.Classes;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Code_Challenge_Aymen_Baccar
{
    public partial class MainWindow : Window
    {
        public List<Shape> AllShapes;
        public double scale = 1;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MainCanvas.Height = this.Height;
                MainCanvas.Width = this.Width;
                double canvasCenterX = this.Width / 2;
                double canvasCenterY = this.Height / 2;
                LoadAndRenderShapes(canvasCenterX, canvasCenterY);

            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);

            }

        }

        private void LoadAndRenderShapes(double centerX, double centerY)
        {
            try
            {


                var reader = new JsonShapeReader();
                var shapes = reader.ReadShapes(@"..\..\Reading data\shapes -General.json");

                ///////////////////////////////////////////////////////////////////////////
                // Remove the double slashes below  in order to retrieve datas from Xml  //
                ///////////////////////////////////////////////////////////////////////////
                

                //var reader = new XmlReader();
                //var shapesPath = @"..\..\Reading data\Shapes.xml";
                //var shapes = reader.ReadShapes(shapesPath);


                if (shapes == null)
                {
                    MessageBox.Show("Reading shapes from file failed.");
                    return;
                }
                AllShapes = new List<Shape>();
                var drawingVisual = new DrawingVisual();
                using (var context = drawingVisual.RenderOpen())
                {
                    var renderer = new ShapeRenderer();
                     scale = getScale(shapes);

                    foreach (var shape in shapes)
                    {
                        AllShapes.Add(shape);

                        if (scale < 1)
                        {
                            if (shape.center != null && shape.radius != null)
                            {
                                shape.center = new Point(shape.center.X * scale, shape.center.Y * scale);
                                shape.radius *= scale;
                            }

                            for (int i = 0; i < shape.points.Count; i++)
                            {
                                var scaledPoint = new Point(shape.points[i].X * scale, shape.points[i].Y * scale);
                                shape.points[i] = scaledPoint;
                            }

                        }
                        



                        shape.TranslateShape(centerX, centerY);

                        renderer.Render(shape, context);

                    }



                }

                var visualHost = new MyVisualHost(drawingVisual);
                MainCanvas.Children.Add(visualHost);

            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);

            }

        }
        public double getScale(IEnumerable<Shape> shapes)
        {
            try
            {
                double minX = double.MaxValue;
                double minY = double.MaxValue;
                double maxX = double.MinValue;
                double maxY = double.MinValue;

                foreach (Shape shape in shapes)
                {

                    if (shape.radius == null)

                    {
                        foreach (var point in shape.points)
                        {
                            minX = Math.Min(minX, point.X);
                            minY = Math.Min(minY, point.Y);
                            maxX = Math.Max(maxX, point.X);
                            maxY = Math.Max(maxY, point.Y);
                        }
                    }
                    else
                    {
                        minX = Math.Min(minX, shape.radius.Value);
                        minY = Math.Min(minY, shape.radius.Value);
                        maxX = Math.Max(maxX, shape.radius.Value);
                        maxY = Math.Max(maxY, shape.radius.Value);

                    }

                }


                double fullWidth = maxX - minX;
                double fullHeight = maxY - minY;
                // The picture exceeds the the window ( in this  case abs(X) is bigger than the half of the window's width or abs(Y) is bigger than the half of the window's height since the pictures is centered ) 
                double scaleX = (this.Width / 2) / Math.Max(maxX,Math.Abs(minX));
                double scaleY = (this.Height / 2) / Math.Max(maxY,Math.Abs(minY));
                return Math.Min(scaleX, scaleY);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);

                return 1;
            }
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var clickedPoint = e.GetPosition(MainCanvas);
                foreach (var shape in AllShapes)
                {
                    if (IsPointNearShapeBorder(clickedPoint, shape, 5))
                    {
                        ShowShapeDetails(shape);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);

            }
        }


        private bool IsPointNearShapeBorder(Point point, Shape shape, double tolerance)
        {
            try
            {

                if (shape.center !=null && shape.radius != null)
                {
                    // Check if the shape is a circle
                    double distanceFromCenter = (point - shape.center).Length;
                    return Math.Abs(distanceFromCenter - shape.radius.Value) <= tolerance;
                }
                else if (shape.points.Count > 1)
                {
                    // Check if the shape is a polygon
                    for (int i = 0; i < shape.points.Count; i++)
                    {
                        Point currentPoint = shape.points[i];
                        Point nextPoint = shape.points[(i + 1) % shape.points.Count]; 

                        if (PointLineDistance(point, currentPoint, nextPoint) <= tolerance)
                        {
                            return true; 
                        }
                    }
                }


                return false;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
                return false;
            }
        }



        private double PointLineDistance(Point p, Point a, Point b)
        {
            try
            {
                double num = Math.Abs((b.Y - a.Y) * p.X - (b.X - a.X) * p.Y + b.X * a.Y - b.Y * a.X);
                double den = Math.Sqrt(Math.Pow(b.Y - a.Y, 2) + Math.Pow(b.X - a.X, 2));
                double distance = num / den;

                double t = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y)) / (den * den);

                if (t < 0 || t > 1) // The projection of the click Point is not located between the point A and B in the line
                {
                    double distToA = Math.Sqrt(Math.Pow(p.X - a.X, 2) + Math.Pow(p.Y - a.Y, 2));
                    double distToB = Math.Sqrt(Math.Pow(p.X - b.X, 2) + Math.Pow(p.Y - b.Y, 2));
                    distance = Math.Min(distToA, distToB);
                }

                return distance;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
                return double.MaxValue;
            }
        }

        private void ShowShapeDetails(Shape shape)
        {
            try
            {
                string details = "";


                if (shape.center != null && shape.radius != null)
                {
                    // get the initial circle coordinates before the translation and the scaling
                     Point OriginalCenter = shape.GetInitialCoordinates(shape.center, this.Width / 2, this.Height / 2,scale);
                    details = $"Type: Circle\nCenter :  ( {Math.Round(OriginalCenter.X, 1).ToString()} ; {Math.Round(OriginalCenter.Y, 1).ToString()} ) \nRadius : {Math.Round(shape.radius.Value, 1).ToString()}\n";

                }

               

                if (shape.points.Count > 0)
                {
                    details += $"Type : {shape.type}\nPoints:\n";
                    char pointLabel = 'A'; 
                    foreach (var point in shape.points)
                    {
                        // get the initial shape coordinates  before the translation and the scaling
                        Point originalPoint = shape.GetInitialCoordinates(point, this.Width / 2, this.Height / 2, scale);

                        details += $"{pointLabel}: ( {Math.Round(originalPoint.X,1).ToString()} ; {Math.Round(originalPoint.Y, 1).ToString()} )\n";
                        pointLabel++; 
                    }
                }

                details += $"Color: {shape.color.ToString()}";


                MessageBox.Show(details, "Shape Details");
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
            }
        }
    }

    public class MyVisualHost : FrameworkElement
    {
        private readonly Visual _visual;

        public MyVisualHost(Visual visual)
        {
            try
            {
                _visual = visual;
                AddVisualChild(_visual);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
            }
            
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            try
            {
                return _visual;

            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
                return null;    
            }
        }
    }
}

