XMLEditor is a control for WPF-based applications and is written in C#.

To use, simply add the control to your WPF-based application and call the "LoadDocument" method. The element parameter will be the root. The location parameter will be where the document is saved when edited (just supply it with the original location to overwrite the old document). The level parameter is nothing you have to worry about; if level is 0 it will draw a line under each element to seperate it from the other.

It behaves differently based on attributes in the source XML-document. For example, if a loaded XML-element have the attribute "description", the attributes value will be shown as a non-bold text as a description.
The only other behaviour I've added so far is the type of input. To change the type of input add a attribute of "type" with either values "folderbrowse" or "text". The folderbrowse value does what you'd expect, it opens a folder browser dialog for you to browse a path and insert it as value, or text, which is the standard if no type is set, simply allows you to enter text.

The loaded document is automatically saved.

Examples
![](http://imgur.com/cQc7ks1.png)
Simple look with description on some elements

![](http://i.imgur.com/hY1cSDK.png)
Simple text input

![](http://i.imgur.com/UaIRgVg.png?1)
"folderbrowse"-type