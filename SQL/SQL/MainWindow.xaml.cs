using MahApps.Metro.Controls;
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

namespace SQL
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Editor.Focus();
        }

        private void CodeChanged(object sender, KeyEventArgs e)
        {
            if (Console == null)
                return;

            var editor = ((RichTextBox)sender);
            Console.Text = editor.GetText();

            if (e.Key == Key.F5)

                editor.HighlightSyntax();
        }
    }


    public static class RichTextBoxExtensions
    {

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
        }

        private static List<string> _keywords = new List<string>()
        {
          "select ","from ","where ","update ","delete ","alter ",
          "and ","or ","order by " ," desc"," asc"
        };

        public static void HighlightSyntax(this RichTextBox richTextBox)
        {
            var text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.ToString().ToLower();
            var textRange = richTextBox.Selection;

            var savedCaretPosition = richTextBox.CaretPosition;

            //clear colors
            textRange.Select(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.White));


            foreach (var keyword in _keywords)
            {
                var indexes = text.AllIndexesOf(keyword);
                int inn = 0;
                TextPointer startPos = richTextBox.Document.ContentStart; ;
                for (var i = 0; i < indexes.Count(); i++)
                {
                    inn++;
                    if (indexes[i] >= 0)
                    {
                        if (i > 0)
                        {
                            startPos = GetPoint(startPos, indexes[i] - indexes[i - 1]);
                        }
                        else
                        {
                            startPos = GetPoint(startPos, indexes[i]);
                        }
                        Colorize(richTextBox, startPos, indexes[i], keyword.Length, new SolidColorBrush(Colors.Teal));
                    }
                }
            }

            richTextBox.CaretPosition = savedCaretPosition;
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
