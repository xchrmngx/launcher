using Microsoft.Win32; // Для OpenFileDialog
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices; // Для вызова SHGetFileInfo
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop; // Для создания изображения из HIcon
using System.Diagnostics;
using System.Text.Json;


namespace launcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<FileItem> fileItems;
        private FileManager fileManager;

        public MainWindow()
        {
            InitializeComponent();
            fileItems = new ObservableCollection<FileItem>();
            FilesDataGrid.ItemsSource = fileItems;

            fileManager = new FileManager();
            fileManager.LoadData(fileItems);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                ImageSource icon = FileIconHelper.GetFileIcon(fileName);

                fileItems.Add(new FileItem
                {
                    FileName = fileName,
                    FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName),
                    FileIcon = icon
                });

                fileManager.SaveData(fileItems);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (FilesDataGrid.SelectedItem is FileItem selectedFile)
            {
                string filePath = selectedFile.FileName;
                if (File.Exists(filePath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при запуске приложения: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден.");
                }
            }
            else
            {
                MessageBox.Show("Выберите файл для запуска.");
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли элемент в DataGrid
            if (FilesDataGrid.SelectedItem is FileItem selectedFile)
            {
                // Удаление элемента из коллекции
                fileItems.Remove(selectedFile);

                // Сохранение обновленных данных в файл .json
                fileManager.SaveData(fileItems);

                MessageBox.Show($"Файл успешно удален из списка.");
            }
            else
            {
                MessageBox.Show("Выберите файл для удаления.");
            }
        }
    }
}
