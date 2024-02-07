## Overview

This .NET C# application is designed for the visualization of vector graphics. It reads vector graphics data from both JSON and XML files, displaying shapes such as lines, circles, triangles, and polygons (including rectangles) on the screen. The application features dynamic scaling and centering of graphics to fit the viewing window, alongside interactive functionalities.
## Key Features

- **Multiple File Formats**: This application supports vector graphics defined in both JSON and XML formats, offering flexibility in data usage and integration.
To switch between JSON and XML file formats, adjust the code in the `LoadAndRenderShapes` function within `mainwindow.xaml.cs` by commenting/uncommenting the relevant lines as documented in the source code. This flexibility allows for easy   
integration with various data sources and formats.
The xml and json files are located in the Reading Data folder.
  
- **Extensive Shape Support**: Beyond basic geometric shapes like lines, circles, and triangles, the application is capable of displaying complex polygons, allowing for a broader range of graphics visualization. You can define and add custom shapes to the JSON or XML file (such as rectangles). Simply specify the color, type, and an array of points that define the shape.
- **Dynamic Visualization**: Graphics are automatically scaled and centered within the application window, ensuring optimal visibility regardless of the original size. The Y-axis is inverted to match conventional Cartesian coordinates, enhancing the intuitive understanding of displayed data.
- **Interactive Elements**: Clicking on the border of any shape brings up a popup displaying detailed information about it, such as coordinates, color, and type. 
## Getting Started

To use the application:

1. **Clone or download the repository** to your local system.
2. **Open the solution** in Visual Studio.
3. **Compile the project** to ensure all dependencies are correctly resolved.
4. **Launch the application**.


