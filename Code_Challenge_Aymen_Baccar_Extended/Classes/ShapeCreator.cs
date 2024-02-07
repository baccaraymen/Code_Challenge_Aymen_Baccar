using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Code_Challenge_Aymen_Baccar.Classes
{


    public static class ShapeCreator
    {
        public static Shape CreateShape(dynamic shapeData)
        {
            try
            {
                string colorStr = Convert.ToString(shapeData.color); 

                Color color = ParseColor(colorStr);
                string stype = shapeData.type; 

                double? radius = null;
                bool? filled = null;
                Point? center = null;
                var points = new List<Point>();

                if (shapeData.radius != null && shapeData.center != null)
                {
                    radius = shapeData.radius;
                    filled = shapeData.filled;
                    center = new Point((double)shapeData.center.x, (double)shapeData.center.y);
                }
                else if (shapeData.points != null)
                {
                    if (shapeData.filled != null)
                        filled = shapeData.filled;
                    foreach (var pointData in shapeData.points)
                    {
                        double x = pointData.x;
                        double y = pointData.y;
                        points.Add(new Point(x, y));
                    }
                }

                return new Shape(points, color, stype, center, radius, filled);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
                return null;
            }
        }

        public static Shape CreateShapeFromXml(XElement shapeXml)
        {
            try
            {
                string colorStr = (string)shapeXml.Attribute("color");
                Color color = ParseColor(colorStr);
                string stype = (string)shapeXml.Attribute("type");

                double? radius = null;
                bool? filled = null;
                Point? center = null;
                var points = new List<Point>();

                var radiusAttr = shapeXml.Attribute("radius");
                if (radiusAttr != null)
                {
                    radius = (double)radiusAttr;
                    filled = (bool)shapeXml.Attribute("filled");
                    center = new Point((int)shapeXml.Attribute("centerX"), (int)shapeXml.Attribute("centerY"));
                }
                else
                {
                    var pointsXml = shapeXml.Element("Points");
                    if (pointsXml != null)
                    {
                        filled = (bool?)shapeXml.Attribute("filled");
                        foreach (var pointXml in pointsXml.Elements("Point"))
                        {
                            double x = (double)pointXml.Attribute("x");
                            double y = (double)pointXml.Attribute("y");
                            points.Add(new Point(x, y));
                        }
                    }
                }

                return new Shape(points, color, stype, center, radius, filled);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
                return null;
            }
        }



        private static Color ParseColor(string colorStr)
        {
            try
            {
                var colorParts = colorStr.Split(';').Select(byte.Parse).ToArray();
               
                Color shapcolor = Color.FromArgb(colorParts[0], colorParts[1], colorParts[2], colorParts[3]);
                return shapcolor;
            }
            catch (Exception ex)
            {
                return Colors.Black; 
            }
        }
       
    }
}
