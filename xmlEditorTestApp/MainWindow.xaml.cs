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
            string location = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\example.xml";
            string xml = 
@"<?xml version=""1.0"" encoding=""utf-8""?>
<rootelement>
    <folderbrowserexample type=""folderbrowse"" description=""Click to choose a folder"" />
    <textinputexample1 type=""text"" description=""Click to input text"" />
    <textinputexample2 description=""Click to also input text"">
        <subitemexample1 />
        <subitemexample2>
            <subsubexample1 />
            <subsubexample2 />
        </subitemexample2>
    </textinputexample2>
</rootelement>";
            XDocument doc = XDocument.Parse(xml);
            this.editor.LoadDocument(doc.Element("rootelement"), location);
        }
    }
}
