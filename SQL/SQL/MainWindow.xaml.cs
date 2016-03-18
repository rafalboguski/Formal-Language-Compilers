using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.TextEditor.Document;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using SQL_Parser;
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
            Editor.Text = "select * from t_user where id = '34';\n\n\n";

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

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            var parser = new ParserSql();
            Console.Text = parser.ValidateSQL(Editor.Text.ToString());
        }

        private void Editor_KeyDown(object sender, EventArgs e)
        {
            var parser = new ParserSql();
            Console.Text = parser.ValidateSQL(Editor.Text.ToString());
        }
    }
}
