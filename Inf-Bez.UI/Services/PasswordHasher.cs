using System.Security.Cryptography;

namespace InfBez.Ui.Services
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Генерация сольи (salt) для усиления безопасности
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Создание объекта для хеширования пароля
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            // Получение хеша пароля
            byte[] hash = pbkdf2.GetBytes(20);

            // Соединение соль и хеш вместе
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Преобразование хеша в строку
            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Извлечение соли из хешированного пароля
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Повторное хеширование введенного пароля с извлеченной солью
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Сравнение полученного хеша с хешем из базы данных
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false; // Пароли не совпадают
                }
            }

            return true; // Пароли совпадают
        }
    }


}
