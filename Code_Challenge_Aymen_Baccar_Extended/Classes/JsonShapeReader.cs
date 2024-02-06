using Code_Challenge_Aymen_Baccar.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class JsonShapeReader
{
    public IEnumerable<Shape> ReadShapes(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            var shapes = JsonConvert.DeserializeObject<List<dynamic>>(json);
            return shapes.Select(ShapeCreator.CreateShape);
        }
        catch(Exception ex)
        {
            Logging.WriteLog(ex);   
            return null;
        }
        
    }
}