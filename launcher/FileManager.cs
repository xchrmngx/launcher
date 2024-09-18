using launcher;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public class FileManager
{
    private string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "savedApps.json");

    public void SaveData(ObservableCollection<FileItem> fileItems)
    {
        try
        {
            var filesToSave = new ObservableCollection<SavedFileItem>();

            foreach (var file in fileItems)
            {
                filesToSave.Add(new SavedFileItem
                {
                    FileName = file.FileName,
                    FileNameWithoutExtension = file.FileNameWithoutExtension
                });
            }

            string json = JsonSerializer.Serialize(filesToSave);
            File.WriteAllText(saveFilePath, json);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
        }
    }

    public void LoadData(ObservableCollection<FileItem> fileItems)
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                var savedItems = JsonSerializer.Deserialize<ObservableCollection<SavedFileItem>>(json);

                foreach (var savedItem in savedItems)
                {
                    ImageSource icon = FileIconHelper.GetFileIcon(savedItem.FileName);
                    fileItems.Add(new FileItem
                    {
                        FileName = savedItem.FileName,
                        FileNameWithoutExtension = savedItem.FileNameWithoutExtension,
                        FileIcon = icon
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
    }
}
