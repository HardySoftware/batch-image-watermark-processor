# Translate into More Languages #

Anybody who can speak other languages are welcome to contribute to the translations. An online collaborative translation service has been setup at http://crowdin.net/project/sea-turtle-batch-image-process, it is FREE and open to any registered users.

![https://raw.githubusercontent.com/hardywang/batch-image-watermark-processor/wiki/screenshots/translation.png](https://raw.githubusercontent.com/hardywang/batch-image-watermark-processor/wiki/screenshots/translation.png)

## Translation Guide ##
From the site there are 2 files available HelpContent.resx and LanguageContent.resx.

### LanguageContent.resx ###
This file is the resource file used by application to display all localized texts/labels/buttons/tabs wording throughout the tool.

Many phrases of this file are short and are designed to fit into small display area, thus you may have different ways to interpret the actual meaning. If you are not 100% sure please simply drop me an email with the “Resource Key Label” as marked in the screenshot above, I will get to you with detailed explanation.

Special resource key to pay attention to:
  * You will see some key begins with "Style", and the actual content is a list of font name. These keys are used to control the tool what fonts to use when some certain areas are displayed. If you are not sure what window each key refers to, I will be happy to help you. It is encouraged to use the font name optimized to your language and operating system.

### HelpContent.resx ###
This file is the resource file used by application to display all tooltip text which you can see when clicking on small blue question marks beside each section's label.

This is a more complicated format to deal with, I have prepared a tool named OpenXML Writer, which could be downloaded at http://code.google.com/p/batch-image-watermark-processor/downloads/detail?name=OpenXMLWriter.exe&can=2&q=. This is a rich text editor tool to open/edit/save [Xaml FlowDocument](http://msdn.microsoft.com/en-us/library/aa970909.aspx) files.

What you need to do is to follow the steps:
  * Click on the entry you want to translate;
  * Get the text with all tags (e.g. `<FlowDocument PagePadding="5,0,5,0" AllowDrop="True" NumberSubstitution.CultureSource="User" xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"><Paragraph> ... </Paragraph></FlowDocument>`), copy and paste to a text editor. Save it to a file with extention .xaml.
  * Run the Open Xml Writer and open the file you have just saved.
  * Do your editing.
  * Click "Show XAML" button at far right of the toolbar, a window will popup to display the modifed FlowDocument content.
  * Copy and paste it into [crowdin](http://crowdin.net/project/sea-turtle-batch-image-process)'s Your Language area as marked in screenshot.

Since the text here is actually a format of rich text, it is encouraged to use the font name optimized to your language and operating system.