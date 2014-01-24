# What it is
XMLEditor is a control for WPF-based applications and is written in C#. I've threw it together pretty quick just to my current projects needs but I figured it could be quite useful in the future. Therefor the functionality is pretty limited as of now but it's quite easy to expand

# How to use
To use, simply add the control to your WPF-based application and call the "LoadDocument" method. The element parameter will be the root. The location parameter will be where the document is saved when edited (just supply it with the original location to overwrite the old document). The level parameter is nothing you have to worry about; if level is 0 it will draw a line under each element to seperate it from the other.

It behaves differently based on attributes in the source XML-document. For example, if a loaded XML-element have the attribute "description", the attributes value will be shown as a non-bold text as a description.
The only other behaviour I've added so far is the type of input. To change the type of input add a attribute of "type" with either values "folderbrowse" or "text". The folderbrowse value does what you'd expect, it opens a folder browser dialog for you to browse a path and insert it as value, or text, which is the standard if no type is set, simply allows you to enter text.

The loaded document is automatically saved.

## Example code
Code from `MainWindow.xaml.cs`
```csharp
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
```

## Screenshots

![](http://i.imgur.com/Pw0N9cr.png)

Simple look with description on some elements

![](http://i.imgur.com/NvuSZyn.png)

Simple text input

![](http://i.imgur.com/drR6Wdf.png?1)

"folderbrowse"-type
