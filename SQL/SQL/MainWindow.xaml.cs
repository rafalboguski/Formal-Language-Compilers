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

            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
                return;
            editor.HighlightSyntax();
        }
    }


    public static class RichTextBoxExtensions
    {

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
        }

        public static void HighlightSyntax(this RichTextBox richTextBox)
        {
            var text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.ToString();
            var textRange = richTextBox.Selection;

            var savedCaretPosition = richTextBox.CaretPosition;

            Colorize(richTextBox, 0, text.Length, Colors.White);


            var i = text.IndexOf("select");

            if (i >= 0)
                Colorize(richTextBox, i, "select".Length, Colors.Red);


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
                string stringSoFar = new TextRange(ret, ret.GetPositionAtOffset(i, LogicalDirection.Forward)).Text;
                if (stringSoFar.Length == x)
                    break;
                i++;
                if (ret.GetPositionAtOffset(i, LogicalDirection.Forward) == null)
                    return ret.GetPositionAtOffset(i - 1, LogicalDirection.Forward);

            }
            ret = ret.GetPositionAtOffset(i, LogicalDirection.Forward);
            return ret;
        }

    }
}
