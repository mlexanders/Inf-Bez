namespace InfBez.Ui.Services
{
    using System.IO.Compression;
    using System.Security.Cryptography;
    using System.Text;

    public class ArchiveManager
    {
        protected string baseExtension;
        protected string encryptionExtension;
        protected string zipExtension;
        public string Pass { get; set; } = "mysmallkey1234551298765134567890";

        public ArchiveManager(string zipExtension, string encryptionExtension, string baseExtension)
        {
            this.zipExtension = zipExtension;
            this.encryptionExtension = encryptionExtension;
            this.baseExtension = baseExtension;
        }

        public string GetBasePath(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + baseExtension);
        }

        public string GetZipPath(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + zipExtension);
        }

        public string GetEncryptionPath(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + encryptionExtension);

        }

        public void CreateArchive(string filePath, string pass = null)
        {
            if (pass == null) pass = Pass;

            FileInfo file = new FileInfo(filePath);

            if (!file.Exists)
            {
                using (FileStream fstream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    var buffer = Encoding.UTF8.GetBytes("");
                    fstream.Write(buffer);
                    fstream.Close();
                }
            }

            try
            {
                // Определяем путь к архиву, добавляя расширение 
                string archivePath = GetZipPath(filePath);

                // Создаем архив
                using (FileStream fileToCompress = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream compressedFileStream = File.Create(archivePath))
                    {
                        using (ZipArchive archive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update))
                        {
                            // Создаем запись в архиве для файла
                            ZipArchiveEntry zipEntry = archive.CreateEntry(Path.GetFileName(filePath));

                            // Копируем содержимое файла в архив
                            using (Stream entryStream = zipEntry.Open())
                            {
                                fileToCompress.CopyTo(entryStream);
                            }
                        }
                    }
                }

                Console.WriteLine($"Архив успешно создан: {archivePath}");
                ProcessFile(archivePath, pass, true);
                File.Delete(GetZipPath(archivePath));
                File.Delete(GetBasePath(filePath)); ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании архива: {ex.Message}");
            }
        }

        public void ExtractFromArchive(string archivePath, string pass = null)
        {
            if (pass == null) pass = Pass;

            var a = GetEncryptionPath(archivePath);
            // Проверяем, существует ли архив
            if (!File.Exists(a))
            {
                Console.WriteLine($"Архив {archivePath} не существует.");
                return;
            }

            try
            {
                ProcessFile(a, pass, false);
                // Определяем путь к извлекаемому файлу, убирая расширение
                string extractedFilePath = GetBasePath(archivePath);

                // Извлекаем содержимое архива
                using (FileStream compressedFileStream = new FileStream(GetZipPath(archivePath), FileMode.Open, FileAccess.Read))
                {
                    using (ZipArchive archive = new ZipArchive(compressedFileStream, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            // Извлекаем содержимое файла из архива
                            using (Stream entryStream = entry.Open())
                            {
                                using (FileStream extractedFileStream = File.Create(extractedFilePath))
                                {
                                    entryStream.CopyTo(extractedFileStream);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"Файл успешно извлечен: {extractedFilePath}");
                File.Delete(GetZipPath(a));
                File.Delete(GetEncryptionPath(archivePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при извлечении файла из архива: {ex.Message}");
            }
        }

        // Общий метод для шифрования и дешифрования файла
        public void ProcessFile(string filePath, string password, bool encrypt)
        {
            try
            {
                string outputExtension = encrypt ? encryptionExtension : zipExtension;
                string outputPath = Path.ChangeExtension(filePath, outputExtension);

                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(password);
                    aesAlg.IV = new byte[16]; // Используем нулевой вектор инициализации для простоты

                    using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (FileStream outputFileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            ICryptoTransform cryptoTransform;

                            if (encrypt)
                                cryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                            else
                                cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                            using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, cryptoTransform, CryptoStreamMode.Write))
                            {
                                inputFileStream.CopyTo(cryptoStream);
                            }
                        }
                    }
                }

                Console.WriteLine($"{(encrypt ? "Шифрование" : "Дешифрование")} завершено. Результат в файле: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при {(encrypt ? "шифровании" : "дешифровании")} файла: {ex.Message}");
            }
        }
    }


}
