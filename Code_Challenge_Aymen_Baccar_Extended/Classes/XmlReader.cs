using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;

namespace Code_Challenge_Aymen_Baccar.Classes
{
    public class XmlReader
    {
       
        
        public  List<Shape> ReadShapes(string filePath)
        {
            try
            {
                var xml = XDocument.Load(filePath);
                var shapes = xml.Descendants("Shape")
                                .Select(shape => ShapeCreator.CreateShapeFromXml(shape))
                                .ToList();
                return shapes;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex);
                return null;
            }
        }

       

       
    }
}
