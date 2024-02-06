using Code_Challenge_Aymen_Baccar.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

public class ShapeRenderer
{
    public void Render(Shape shape, DrawingContext context)
    {
        try
        {
            var pen = new Pen(new SolidColorBrush(shape.color), 1);
            //  var pen = new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)), 1); // Noir, épaisseur 1
            SolidColorBrush brush = shape.filled.HasValue && shape.filled != null
                     ? new SolidColorBrush(shape.color as Color? ?? Colors.Black)
                     : Brushes.Transparent;



            // Handle circle separately due to the need for a center and radius
            if (shape.radius != null && shape.center != null)
            {
                // Draw circle
                context.DrawEllipse(brush, pen, new Point(shape.center.X, shape.center.Y), shape.radius.Value, shape.radius.Value);
            }
            else
            {
                // For other shapes, use the Points list to draw lines 
                if (shape.points.Count > 1)
                {
                    var geometry = new StreamGeometry();
                    using (var geometryContext = geometry.Open())
                    {
                        geometryContext.BeginFigure(shape.points[0], shape.filled.HasValue && shape.filled != null, shape.points.Count > 2);

                        var pointsToUse = new List<Point>();
                        for (int i = 1; i < shape.points.Count; i++)
                        {
                            pointsToUse.Add(shape.points[i]);
                        }
                        geometryContext.PolyLineTo(pointsToUse, true, false);
                    }
                    geometry.Freeze();

                    context.DrawGeometry(shape.filled.HasValue && shape.filled != null ? brush : null, pen, geometry);
                }
            }


        }
        catch (Exception ex)
        {
            Logging.WriteLog(ex);

        }



    }
}