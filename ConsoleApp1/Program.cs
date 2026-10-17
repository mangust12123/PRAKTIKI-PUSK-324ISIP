using System;
using System.Collections.Generic;

namespace TextAnalyzer
{
    // Класс для хранения статистики
    class TextStats
    {
        public string Text { get; set; }
        public int WordCount { get; set; }
        public int SentenceCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }
        public DateTime Date { get; set; }

        public TextStats()
        {
            LetterFrequency = new Dictionary<char, int>();
            Date = DateTime.Now;
        }
    }

    class Program
    {
        static List<TextStats> history = new List<TextStats>(); // Список для хранения истории

        static void Main(string[] args)
        {
            bool work = true;
            while (work)
            {
                Console.Clear();
                Console.WriteLine("=== АНАЛИЗАТОР ТЕКСТА ===");
                Console.WriteLine("1. Анализировать новый текст");
                Console.WriteLine("2. Показать историю анализов");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    AnalyzeText();
                }
                else if (choice == "2")
                {
                    ShowHistory();
                }
                else if (choice == "0")
                {
                    work = false;
                }
                else
                {
                    Console.WriteLine("Неверный ввод!");
                }

                if (work)
                {
                    Console.WriteLine("\nНажмите Enter для продолжения...");
                    Console.ReadLine();
                }
            }
        }

        static void AnalyzeText()
        {
            Console.WriteLine("Введите текст (минимум 100 символов):");
            string text = Console.ReadLine();

            // Проверяем длину
            if (text == null || text.Length < 100)
            {
                Console.WriteLine("Текст слишком короткий! Нужно минимум 100 символов.");
                return;
            }

            // Создаём объект статистики
            TextStats stats = new TextStats();
            stats.Text = text;

            // Подсчитываем слова
            stats.WordCount = CountWords(text);

            // Подсчитываем предложения
            stats.SentenceCount = CountSentences(text);

            // Ищем самое короткое и длинное слово
            FindShortestAndLongest(text, out stats.ShortestWord, out stats.LongestWord);

            // Подсчитываем гласные и согласные
            CountVowelsAndConsonants(text, out stats.VowelCount, out stats.ConsonantCount);

            // Статистика частоты букв
            stats.LetterFrequency = GetLetterFrequency(text);

            // Выводим результаты
            Console.WriteLine("\n=== РЕЗУЛЬТАТЫ АНАЛИЗА ===");
            Console.WriteLine($"Длина текста: {text.Length} символов");
            Console.WriteLine($"Количество слов: {stats.WordCount}");
            Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
            Console.WriteLine($"Самое короткое слово: \"{stats.ShortestWord}\" (длина: {stats.ShortestWord.Length})");
            Console.WriteLine($"Самое длинное слово: \"{stats.LongestWord}\" (длина: {stats.LongestWord.Length})");
            Console.WriteLine($"Гласных букв: {stats.VowelCount}");
            Console.WriteLine($"Согласных букв: {stats.ConsonantCount}");

            Console.WriteLine("\nЧастота встречаемости букв:");
            foreach (var pair in stats.LetterFrequency)
            {
                Console.WriteLine($"  '{pair.Key}': {pair.Value} раз");
            }

            // Сохраняем в историю
            history.Add(stats);
            Console.WriteLine("\nСтатистика сохранена в историю.");
        }

        static int CountWords(string text)
        {
            int count = 0;
            bool inWord = false;

            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsLetterOrDigit(text[i]))
                {
                    if (!inWord)
                    {
                        count++;
                        inWord = true;
                    }
                }
                else
                {
                    inWord = false;
                }
            }

            return count;
        }

        static int CountSentences(string text)
        {
            int count = 0;
            bool inSentence = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (!char.IsWhiteSpace(c))
                {
                    if (!inSentence)
                    {
                        count++;
                        inSentence = true;
                    }
                }

                if (c == '.' || c == '!' || c == '?')
                {
                    inSentence = false;
                }
            }

            return count;
        }

        static void FindShortestAndLongest(string text, out string shortest, out string longest)
        {
            shortest = "";
            longest = "";
            string currentWord = "";

            for (int i = 0; i <= text.Length; i++)
            {
                if (i < text.Length && char.IsLetterOrDigit(text[i]))
                {
                    currentWord += text[i];
                }
                else
                {
                    if (currentWord.Length > 0)
                    {
                        if (shortest == "" || currentWord.Length < shortest.Length)
                        {
                            shortest = currentWord;
                        }
                        if (currentWord.Length > longest.Length)
                        {
                            longest = currentWord;
                        }
                        currentWord = "";
                    }
                }
            }
        }

        static void CountVowelsAndConsonants(string text, out int vowels, out int consonants)
        {
            vowels = 0;
            consonants = 0;

            string vowelChars = "аеёиоуыэюяaeiouyАЕЁИОУЫЭЮЯAEIOUY";

            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsLetter(text[i]))
                {
                    bool isVowel = false;
                    for (int j = 0; j < vowelChars.Length; j++)
                    {
                        if (text[i] == vowelChars[j])
                        {
                            isVowel = true;
                            break;
                        }
                    }

                    if (isVowel)
                    {
                        vowels++;
                    }
                    else
                    {
                        consonants++;
                    }
                }
            }
        }

        static Dictionary<char, int> GetLetterFrequency(string text)
        {
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsLetter(text[i]))
                {
                    char letter = char.ToLower(text[i]);

                    if (frequency.ContainsKey(letter))
                    {
                        frequency[letter]++;
                    }
                    else
                    {
                        frequency[letter] = 1;
                    }
                }
            }

            return frequency;
        }

        static void ShowHistory()
        {
            Console.WriteLine("=== ИСТОРИЯ АНАЛИЗОВ ===");
            if (history.Count == 0)
            {
                Console.WriteLine("История пуста");
                return;
            }

            for (int i = 0; i < history.Count; i++)
            {
                Console.WriteLine($"\n--- Анализ #{i + 1} ({history[i].Date:dd.MM.yyyy HH:mm:ss}) ---");
                Console.WriteLine($"Длина текста: {history[i].Text.Length} символов");
                Console.WriteLine($"Слов: {history[i].WordCount}");
                Console.WriteLine($"Предложений: {history[i].SentenceCount}");
                Console.WriteLine($"Короткое слово: \"{history[i].ShortestWord}\"");
                Console.WriteLine($"Длинное слово: \"{history[i].LongestWord}\"");
                Console.WriteLine($"Гласных: {history[i].VowelCount}, Согласных: {history[i].ConsonantCount}");
            }
        }
    }
}