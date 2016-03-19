using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.TextEditor.Document;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using SQL_Parser;
using SQL_Parser.Grammar;
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
            Editor.Focus();

            Editor.ShowLineNumbers = true;
            Editor.Text = "select * from t_user where lol;";

            // load syntax theme
            using (var s = new StreamReader(@"C:\Users\user\Documents\GitHubVisualStudio\Formal-Language-Compilers\SQL\SQL\Resources\Colorizer.xshd"))
            {
                var line = s.ReadLine();
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    Editor.SyntaxHighlighting = HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }

            Console.ShowLineNumbers = true;
            // load syntax theme
            using (var s = new StreamReader(@"C:\Users\user\Documents\GitHubVisualStudio\Formal-Language-Compilers\SQL\SQL\Resources\Colorizer.xshd"))
            {
                var line = s.ReadLine();
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    Console.SyntaxHighlighting = HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }
        }

        private void Colorize_Key(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.F5)
        }

        private void Colorize_Button_Click(object sender, RoutedEventArgs e)
        {
        }




        long waiter = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            ddddd();
        }

        private void Editor_KeyDown(object sender, EventArgs e)
        {
            ddddd();
        }

        private void ddddd()
        {
            //var ddd = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - waiter;
            //if (ddd > 300)
            //{
            //Debug.WriteLine("VALIDATE " + ddd);
            var errors = new ParserSql().ValidateSQL(Editor.Text.ToString());

            Editor.TextArea.TextView.LineTransformers.Add(new ColorizeAvalonEdit(errors));

            Console.Text = "";
            foreach (var error in errors)
            {
                Console.Text += error + "\n";
            }
            // }
            // waiter = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }

    public class ColorizeAvalonEdit : DocumentColorizingTransformer
    {
        private List<GrammarError> errors { get; set; }

        public ColorizeAvalonEdit(List<GrammarError> errors)
        {
            this.errors = errors;
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            foreach (var error in errors)
            {
                var index = error.Token.Index;
                var length = error.Token.Length;

                if (index >= line.Offset && index < line.Offset + line.Length)
                    base.ChangeLinePart(index, index + length, (VisualLineElement element) => { Typeface tf = element.TextRunProperties.Typeface; element.BackgroundBrush = new SolidColorBrush(Colors.Red); });
            }
        }

    }

}
