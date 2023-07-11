using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, nint hToken = 0);

        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
        }
        public static string GetPathToDownload()
        {
            return SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0);
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1)
            };

            timer.Tick -= Tick;
            timer.Tick += Tick;

            timer.Start();
        }

        private async void Tick(object? sender, EventArgs e)
        {
            try
            {
                using var sr = new StreamReader(GetPathToDownload() + "\\logger.txt");

                var text = await sr.ReadToEndAsync();

                if (text == txtTerminal.Text) return;

                txtTerminal.Text = text;
                Scroller.ScrollToBottom();
            }
            catch (Exception)
            { }
        }

        private void CmdReduce_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CmdClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
