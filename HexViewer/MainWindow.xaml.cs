using ProbyteEdit_Client;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace HexViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseBinaryFile_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false
            };

            var bytes = Array.Empty<byte>();
            if (openFileDialog.ShowDialog() == true)
            {
                bytes = File.ReadAllBytes(openFileDialog.FileName);
            }


            var viewer = new HexViewerWindow(bytes);
            viewer.Show();
        }
    }
}
