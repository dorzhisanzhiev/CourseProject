using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.Vigenere
{
    public class Vigenere
    {
        public static string Encrypt(string message, string key)
        {
            return Crypt(message, key, true);
        }
        public static string Decrypt(string message, string key)
        {
            return Crypt(message, key, false);
        }
        public static string Crypt(string message, string strkey, bool encrypt)
        {
            if (String.IsNullOrEmpty(message))
            {
                Console.WriteLine("Сообщение пусто.");
                return string.Empty;
            }
            if (String.IsNullOrEmpty(strkey))
            {
                Console.WriteLine("Ключ пуст.");
                return string.Empty;
            }
            string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            List<byte> key = new List<byte>(); /* Ключ, переведённый в список целых чисел. Все символы ключа, не являющиеся
                                                   * буквами русского алфавита, игнорируются; в массив переводятся значения букв
                                                   * ключа, а соотв. 0, я соотв. 32. Регистр не учитывается. Используется byte,
                                                   * так как в списке никогда не должны быть значения ниже 0 или выше 32*/
            for (int i = 0; i < strkey.Length; ++i)
            {
                if (!alphabet.Contains(Char.ToLower(strkey[i])))
                    continue;
                key.Add((byte)alphabet.IndexOf(strkey[i])); //Получаем ключ как список шагов отступа
            }
            int keylength = key.Count;
            if (keylength == 0) return message; //если переведённый ключ пустой, то и изменять ничего не надо
            string output = string.Empty;
            int nonAlphabetCharCount = 0; //число неалфавитных символов в сообщении, используется при получении индекса ключа
            for (int i = 0; i < message.Length; i++)
            {
                if (alphabet.Contains(Char.ToLower(message[i])))
                {
                    byte keyByte = key[(i - nonAlphabetCharCount) % keylength]; //шаг от 0 до 32 на текущий символ ключа
                    byte messageByte = (byte)alphabet.IndexOf(Char.ToLower(message[i])); //шаг от 0 до 32 на символ сообщения
                    int outputByte;
                    if (encrypt)
                    {
                        outputByte = messageByte + keyByte;
                        if (outputByte > 32) outputByte -= 33;
                    }
                    else
                    {
                        outputByte = messageByte - keyByte;
                        if (outputByte < 0) outputByte += 33;
                    }
                    if (char.IsUpper(message[i])) output += Char.ToUpper(alphabet[outputByte]);
                    else output += alphabet[outputByte];
                }
                else
                {
                    nonAlphabetCharCount++;
                    output += message[i];
                }
            }
            return output;
        }
    }
}
