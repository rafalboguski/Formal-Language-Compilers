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
          "select","from","where","update","delete","alter",
          "and","or","order by" ,"desc","asc"
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
                foreach (var i in indexes)
                {
                    if (i >= 0)
                        Colorize(richTextBox, i, keyword.Length, Colors.Teal);
                }
            }

            richTextBox.CaretPosition = savedCaretPosition;
        }

        private static void Colorize(RichTextBox richTextBox, int offset, int length, Color color)
        {
            var textRange = richTextBox.Selection;
            var start = richTextBox.Document.ContentStart;
            var startPos = GetPoint(start, offset);
            var endPos = GetPoint(start, offset + length);

            textRange.Select(startPos, endPos);
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
        }

        private static TextPointer GetPoint(TextPointer start, int x)
        {
            var ret = start;
            var i = 0;
            while (ret != null)
            {
                if (new TextRange(ret, ret.GetPositionAtOffset(i, LogicalDirection.Forward)).Text.Length == x)
                    break;
                i++;
                if (ret.GetPositionAtOffset(i, LogicalDirection.Forward) == null)
                    return ret.GetPositionAtOffset(i - 1, LogicalDirection.Forward);
            }
            ret = ret.GetPositionAtOffset(i, LogicalDirection.Forward);
            return ret;
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
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
