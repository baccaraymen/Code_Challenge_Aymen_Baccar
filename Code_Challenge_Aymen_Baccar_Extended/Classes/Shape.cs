using Code_Challenge_Aymen_Baccar.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;


public class Shape
{
    public string type { get; set; }
    public dynamic points { get; set; }
    public Color color { get; set; }
    public double? radius { get; set; }
    public bool? filled { get; set; }
    public dynamic center { get; set; }

    public Shape(List<Point> _points, Color _color, string _type, Point? _Center, double? _radius , bool? _filled)

    {
        radius = _radius;
        filled = _filled;
        color = _color;
        points = _points;
        type = _type;
        center = _Center;   


    }
    public void TranslateShape( double translateX, double translateY)
    {
        try
        {



            if (this.center != null)
            {
                
                this.center = TranslatePoint(this.center, translateX, translateY);
            }
            for (int i = 0; i < this.points.Count; i++)
            {
                this.points[i] = TranslatePoint(this.points[i], translateX, translateY);
            }

        }
        catch (Exception ex)
        {
            Logging.WriteLog(ex);
        }
    }

    public Point TranslatePoint(Point point, double translateX, double translateY)
    {
        return new Point(point.X + translateX, -point.Y + translateY);
    }
   
    public Point GetInitialCoordinates(Point originalPoint, double centerX, double centerY, double scale)
    {
        try
        {
            return new Point((originalPoint.X - centerX)/scale, -(originalPoint.Y - centerY)/scale);


        }
        catch (Exception ex)
        {
            Logging.WriteLog(ex);

            return new Point();


        }
    }


}
