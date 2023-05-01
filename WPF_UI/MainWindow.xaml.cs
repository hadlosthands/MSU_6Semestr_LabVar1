using libs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WPF_UI
{
    public partial class MainWindow : Window
    {
        readonly ViewData viewData = new();
        OxyPlotModel Plot;
        public static RoutedCommand LoadFromControlsCommand = new("LoadFromControls", typeof(MainWindow));
        public static RoutedCommand LoadFromFileCommand = new("LoadFromFile", typeof(MainWindow));
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewData;
            functions.ItemsSource = Enum.GetValues(typeof(FRawEnum));
            functions.SelectedIndex = 0;
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveFileClick, CanSaveCommandHandler));
        }

        private void LoadDataClick(object sender, RoutedEventArgs e)
        {
            try
            {
                viewData.LoadFromControls();
                int result = viewData.ComputeSpline();
                ShowData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void SaveFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new();
                if (saveFileDialog.ShowDialog() == true)
                    viewData.RawData.Save(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new();
                if (openFileDialog.ShowDialog() == true)
                    viewData.LoadFromFile(openFileDialog.FileName);
                int result = viewData.ComputeSpline();
                ShowData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ShowData()
        {
            try
            {
                rawDataListBox.Items.Clear();
                for (int i = 0; i < viewData.NumOfNodes; i++)
                {
                    rawDataListBox.Items.Add($"X = {viewData.RawData.GridNodes[i]:F4};  F(x) = {viewData.RawData.ArrayValues[i]:F4}");
                }
                splineDataListBox.ItemsSource = viewData.SplineData.SplineItemList;
                integralTextBlock.Text = viewData.SplineData.Integral.ToString();
                DrawSpline();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DrawSpline()
        {
            try
            {
                Plot = new OxyPlotModel(viewData.SplineData, viewData.RawData);
                SplinePlot.Model = Plot.plotModel;
            }
            catch (Exception ex)
            {   
                MessageBox.Show($"Ошибка отрисовки сплайна\n" + ex.Message);
            }
        }

        private void LoadFromControlsCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            LoadDataClick(sender, e);
            DrawSpline();
        }
        private void CanLoadFromControlsCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Validation.GetHasError(Bounds) || Validation.GetHasError(NumOfNodes) || Validation.GetHasError(SplineNodes))
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }

        private void LoadFromFileCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new();
                if (openFileDialog.ShowDialog() == true)
                {
                    viewData.LoadFromFile(openFileDialog.FileName);
                    viewData.SplineData = new SplineData(viewData.RawData, viewData.LeftDer, viewData.RightDer, viewData.SplineNodes);             
                    int res = viewData.SplineData.CreateSpline();
                    DrawSpline();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CanLoadFromFileCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Validation.GetHasError(Bounds) || Validation.GetHasError(NumOfNodes) || Validation.GetHasError(SplineNodes))
                e.CanExecute = false;
            else 
                e.CanExecute= true;
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileClick(sender, e);
        }
        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((Validation.GetHasError(Bounds) || Validation.GetHasError(NumOfNodes)) && (viewData.RawData != null))
                e.CanExecute = false;
            else 
                e.CanExecute = true;
        }

    }
}

namespace convs
{
    public class BoundsTextBoxConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string result = values[0].ToString() + ";" + values[1].ToString();
                MessageBox.Show(result.ToString());
                return result;
            }
            catch
            {
                MessageBox.Show("Неправильный ввод!");
                return "0;10";
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            try
            {
                string[] splitValues = ((string)value).Split(';');
                double[] bounds = { System.Convert.ToDouble(splitValues[0]), System.Convert.ToDouble(splitValues[1]) };
                return new object[] { bounds[0], bounds[1] };
            }
            catch
            {
                return new object[] { 0, 1 };
            }
        }
    }
    public class DoubleTextBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? converted = value.ToString();
            if (converted != null)
                return converted;
            else
            {
                MessageBox.Show("Invalid input");
                return "0";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as string;
            if (double.TryParse(input, out double output))
                return output;
            else
                return DependencyProperty.UnsetValue;
        }
    }
    public class IntegerTextBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? converted = value.ToString();
            if (converted != null)
                return converted;
            else
            {
                MessageBox.Show("Invalid input");
                return "0";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as string;
            if (int.TryParse(input, out int output))
                return output;
            else
                return DependencyProperty.UnsetValue;
        }
    }
    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}