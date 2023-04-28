using libs;
using libs;
using Microsoft.Win32;
using Microsoft.Win32;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Globalization;
using System.Globalization;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shapes;
using WPF_UI;

internal static class MainWindowHelpers
{
    public static RoutedCommand LoadFromControlsCommand = new("LoadFromControls", typeof(MainWindow));
    public static RoutedCommand LoadFromFileCommand = new("LoadFromFile", typeof(MainWindow));
}