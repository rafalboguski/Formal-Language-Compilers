using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.TextEditor.Document;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml;

namespace SQL
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //Editor.Focus();

            using (var s = new StreamReader(@"C:\Users\user\Documents\GitHubVisualStudio\Formal-Language-Compilers\SQL\SQL\Resources\Colorizer.xshd"))
            {
                var line = s.ReadLine();
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }
        }

        private void Colorize_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                Colorize();
        }

        private void Colorize_Button_Click(object sender, RoutedEventArgs e)
        {
            Colorize();
        }

        private void Colorize()
        {
            //Editor.HighlightSyntax();
        }
    }


    public static class RichTextBoxExtensions
    {

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
        }

        private static List<string> _keywords = new List<string>
        {
          "select","from","where","update","delete","alter",
          "and","or","order by" ," desc"," asc","insert","into","values" ,"join", "as ","like ","not ","on " ,
            ",", "\"", "\'", ";", "=", "-", "+" ,"(", ")"

        };

        public static List<string> _signs = new List<string>
        {
        };

        public static void HighlightSyntax(this RichTextBox richTextBox)
        {
            var text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.ToString().ToLower();
            var textRange = richTextBox.Selection;

            var savedCaretPosition = richTextBox.CaretPosition;

            //clear colors
            textRange.Select(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.White));

            ColorizeSet(richTextBox, text, _keywords, new SolidColorBrush(Colors.Teal));
            ColorizeSet(richTextBox, text, _signs, new SolidColorBrush(Colors.Yellow));

            richTextBox.CaretPosition = savedCaretPosition;
        }

        private static void ColorizeSet(RichTextBox richTextBox, string text, List<string> keywords, SolidColorBrush color)
        {
            foreach (var keyword in keywords)
            {
                var indexes = text.AllIndexesOf(keyword).ToArray();
                TextPointer startPos = richTextBox.Document.ContentStart; ;
                for (var i = 0; i < indexes.Count(); i++)
                {
                    if (i > 0)
                    {
                        startPos = GetPoint(startPos, indexes[i] - indexes[i - 1]);
                    }
                    else
                    {
                        startPos = GetPoint(startPos, indexes[i]);
                    }
                    Colorize(richTextBox, startPos, indexes[i], keyword.Length, color);
                }
            }
        }


        private static void Colorize(RichTextBox richTextBox, TextPointer from, int offset, int length, SolidColorBrush color)
        {
            var startPos = from;
            var endPos = GetPoint(startPos, length);

            richTextBox.Selection.Select(startPos, endPos);
            richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }

        private static TextPointer GetPoint(TextPointer start, int x)
        {
            var begin = start;
            var i = x;
            while (begin != null)
            {
                var xxx = new TextRange(begin, begin.GetPositionAtOffset(i, LogicalDirection.Forward)).Text.Length;
                if (xxx == x)
                    break;
                i++;
                if (begin.GetPositionAtOffset(i, LogicalDirection.Forward) == null)
                    return begin.GetPositionAtOffset(i - 1, LogicalDirection.Forward);
            }
            begin = begin.GetPositionAtOffset(i, LogicalDirection.Forward);
            return begin;
        }

    }

    public static class StandardExtensions
    {
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes.OrderBy(x => x).ToList();
                indexes.Add(index);
            }
        }
    }

}
