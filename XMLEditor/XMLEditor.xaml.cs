using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Xml.Linq;

namespace XMLEditor
{
    /// <summary>
    /// Interaction logic for XMLEditor.xaml
    /// </summary>
    public partial class XMLEditor : UserControl
    {
        XElement Element;

        TextBox tbox;
        TextBlock lastClicked;

        string location;

        int level;

        public XMLEditor()
        {
            this.InitializeComponent();
            this.Loaded += XMLEditor_Loaded;
        }

        void XMLEditor_Loaded(object sender, RoutedEventArgs e)
        {
            tbox = new TextBox();
            tbox.LostFocus += tbox_LostFocus;
            tbox.KeyDown += tbox_KeyDown;
        }

        void tbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RemoveTbox();
            }
        }

        void tbox_LostFocus(object sender, RoutedEventArgs e)
        {
            RemoveTbox();
        }

        public void LoadDocument(XElement element, string location, int level)
        {
            this.location = location;
            this.level = level;
            Element = element;
            PopulateRows();
        }

        Dictionary<TextBlock, XElement> dictionary = new Dictionary<TextBlock, XElement>();

        private void PopulateRows()
        {
            var elements = Element.Elements();

            Thickness margin = new Thickness(2, 2, 4, 2);

            int row = 0;
            foreach (XElement element in elements)
            {
                this.MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

                StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
                sp.SetValue(Grid.RowProperty, row);
                sp.SetValue(Grid.ColumnProperty, 0);

                TextBlock elename = new TextBlock();
                elename.Text = element.Name.ToString();
                elename.Margin = margin;
                elename.FontWeight = FontWeights.Bold;
                sp.Children.Add(elename);

                XAttribute description = element.Attribute("description");
                if (description != null)
                {
                    TextBlock desc = new TextBlock();
                    desc.Text = description.Value;
                    desc.Margin = margin;
                    desc.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    desc.FontSize -= 1;
                    sp.Children.Add(desc);
                }
                this.MainGrid.Children.Add(sp);


                Rectangle rect = new Rectangle();
                rect.Fill = new SolidColorBrush(Color.FromRgb(140, 152, 161));
                rect.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                rect.Width = 1;
                rect.SetValue(Grid.RowProperty, row);
                rect.SetValue(Grid.ColumnProperty, 0);
                rect.Margin = new Thickness(2, 2, 0, 2);
                this.MainGrid.Children.Add(rect);

                if (level == 0)
                {
                    Rectangle rect2 = new Rectangle();
                    rect2.Fill = Brushes.Black;
                    rect2.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    rect2.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    rect2.Height = 1;
                    rect2.SetValue(Grid.RowProperty, row);
                    rect2.SetValue(Grid.ColumnProperty, 0);
                    rect2.SetValue(Grid.ColumnSpanProperty, 2);
                    //rect2.Margin = new Thickness(2, 0, 2, 0);

                    this.MainGrid.Children.Add(rect2);
                }


                if (element.HasElements)
                {
                    XMLEditor xmleditor = new XMLEditor();
                    xmleditor.LoadDocument(element, location, level + 1);
                    xmleditor.SetValue(Grid.RowProperty, row);
                    xmleditor.SetValue(Grid.ColumnProperty, 1);
                    xmleditor.Margin = new Thickness(0, 2, 0, 2);

                    this.MainGrid.Children.Add(xmleditor);
                }
                else
                {
                    TextBlock elevalue = new TextBlock();
                    elevalue.Text = element.Value;
                    elevalue.SetValue(Grid.RowProperty, row);
                    elevalue.SetValue(Grid.ColumnProperty, 1);
                    elevalue.Margin = margin;
                    elevalue.MinWidth = 100;
                    elevalue.Background = Brushes.AntiqueWhite;

                    XAttribute type = element.Attribute("type");

                    if (type == null)
                    {
                        elevalue.MouseLeftButtonUp += TypeText_MouseLeftButtonUp;
                    }
                    else
                    {
                        if (type.Value == "text")
                        {
                            elevalue.MouseLeftButtonUp += TypeText_MouseLeftButtonUp;
                        }
                        else if (type.Value == "folderbrowse")
                        {
                            elevalue.MouseLeftButtonUp += TypeFolderBrowse_MouseLeftButtonUp;
                        }
                        else if (type.Value == "folderbrowserelative")
                        {
                            elevalue.MouseLeftButtonUp += TypeFolderBrowseRelative_MouseLeftButtonUp;
                        }
                    }
                    dictionary.Add(elevalue, element);

                    this.MainGrid.Children.Add(elevalue);
                }

                row++;
            }
        }

        void TypeFolderBrowseRelative_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lastClicked = (TextBlock)sender;

        }

        void TypeFolderBrowse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lastClicked = (TextBlock)sender;

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.SelectedPath = System.Windows.Forms.Application.StartupPath;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lastClicked.Text = fbd.SelectedPath;

                dictionary[lastClicked].Value = lastClicked.Text;
                dictionary[lastClicked].Document.Save(location);
            }
        }


        //private void PopulateRows()
        //{
        //    var elements = Element.Elements();

        //    Thickness margin = new Thickness(2, 2, 4, 2);

        //    int row = 0;
        //    foreach (XElement element in elements)
        //    {
        //        this.MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

        //        TextBlock elename = new TextBlock();
        //        elename.Text = element.Name.ToString();
        //        elename.SetValue(Grid.RowProperty, row);
        //        elename.SetValue(Grid.ColumnProperty, 0);
        //        elename.Margin = margin;
        //        elename.FontWeight = FontWeights.Bold;



        //        Rectangle rect = new Rectangle();
        //        rect.Fill = new SolidColorBrush(Color.FromRgb(140, 152, 161));
        //        rect.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        //        rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        //        rect.Width = 1;
        //        rect.SetValue(Grid.RowProperty, row);
        //        rect.SetValue(Grid.ColumnProperty, 0);
        //        rect.Margin = new Thickness(2, 2, 0, 2);

        //        if (level == 0)
        //        {
        //            Rectangle rect2 = new Rectangle();
        //            rect2.Fill = Brushes.Black;
        //            rect2.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        //            rect2.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        //            rect2.Height = 1;
        //            rect2.SetValue(Grid.RowProperty, row);
        //            rect2.SetValue(Grid.ColumnProperty, 0);
        //            rect2.SetValue(Grid.ColumnSpanProperty, 2);
        //            //rect2.Margin = new Thickness(2, 0, 2, 0);

        //            this.MainGrid.Children.Add(rect2);
        //        }

        //        this.MainGrid.Children.Add(elename);
        //        this.MainGrid.Children.Add(rect);

        //        if (element.HasElements)
        //        {
        //            XMLEditor xmleditor = new XMLEditor();
        //            xmleditor.LoadDocument(element, location, level + 1);
        //            xmleditor.SetValue(Grid.RowProperty, row);
        //            xmleditor.SetValue(Grid.ColumnProperty, 1);
        //            xmleditor.Margin = new Thickness(0, 2, 0, 2);

        //            this.MainGrid.Children.Add(xmleditor);
        //        }
        //        else
        //        {
        //            TextBlock elevalue = new TextBlock();
        //            elevalue.Text = element.Value;
        //            elevalue.SetValue(Grid.RowProperty, row);
        //            elevalue.SetValue(Grid.ColumnProperty, 1);
        //            elevalue.Margin = margin;
        //            elevalue.MinWidth = 100;
        //            elevalue.Background = Brushes.AntiqueWhite;
        //            elevalue.MouseLeftButtonUp += elevalue_MouseLeftButtonUp;
        //            dictionary.Add(elevalue, element);
        //            this.MainGrid.Children.Add(elevalue);
        //        }
        //        row++;
        //    }
        //}

        void TypeText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (tbox.Parent != null)
                RemoveTbox();

            lastClicked = (TextBlock)sender;

            ShowTbox();
        }

        private void ShowTbox()
        {
            Grid parent = (Grid)lastClicked.Parent;
            parent.Children.Add(tbox);

            tbox.Text = lastClicked.Text;
            int row = (int)lastClicked.GetValue(Grid.RowProperty);
            int column = (int)lastClicked.GetValue(Grid.ColumnProperty);
            tbox.SetValue(Grid.RowProperty, row);
            tbox.SetValue(Grid.ColumnProperty, column);

            tbox.Focus();
            tbox.SelectAll();
        }

        public void RemoveTbox()
        {
            if (tbox.Parent != null)
            {
                lastClicked.Text = tbox.Text;
                Grid parent = (Grid)tbox.Parent;
                parent.Children.Remove(tbox);

                dictionary[lastClicked].Value = lastClicked.Text;
                dictionary[lastClicked].Document.Save(location);
            }
        }

        private List<XElement> ToList(IEnumerable<XElement> enumerable)
        {
            List<XElement> list = new List<XElement>();

            foreach (XElement element in enumerable)
            {
                list.Add(element);
            }

            return list;
        }
    }
}
