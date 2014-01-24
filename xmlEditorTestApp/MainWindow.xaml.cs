using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using XMLEditor;

namespace xmlEditorTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string location = System.Windows.Forms.Application.StartupPath + @"\Language.xml";
            XDocument doc = XDocument.Load(location);

            foreach (XElement element in doc.Element("languages").Elements("language"))
            {
                Grid grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

                TextBlock tb = new TextBlock();
                tb.Text = element.Attribute("lang").Value;
                tb.SetValue(Grid.RowProperty, 0);
                tb.FontSize += 4;
                tb.FontWeight = FontWeights.Bold;
                tb.Margin = new Thickness(4);

                Rectangle rect = new Rectangle();
                rect.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                rect.Height = 1;
                rect.Fill = Brushes.Black;

                Rectangle rect2 = new Rectangle();
                rect2.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                rect2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                rect2.Width = 1;
                rect2.Fill = Brushes.Black;

                XMLEditor.XMLEditor editor = new XMLEditor.XMLEditor();
                editor.LoadDocument(element, location, 0);
                editor.SetValue(Grid.RowProperty, 1);

                grid.Children.Add(tb);
                grid.Children.Add(rect);
                grid.Children.Add(editor);
                itemscontrol.Children.Add(rect2);
                itemscontrol.Children.Add(grid);
            }
        }
    }
}
