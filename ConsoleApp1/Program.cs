using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp
{
    // Перечисление жанров
    enum Genre
    {
        Fiction,
        Science,
        History,
        Fantasy,
        Detective
    }

    // Класс книги
    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"[ID: {Id}] \"{Title}\" | Автор: {Author} | Жанр: {Genre} | Год: {Year} | Цена: {Price} руб.";
        }
    }

    class Program
    {
        static List<Book> books = new List<Book>();
        static List<Book> cart = new List<Book>(); // Корзина
        static int nextId = 1;

        static void Main(string[] args)
        {
            // Загружаем тестовые данные
            LoadTestBooks();

            bool work = true;
            while (work)
            {
                Console.Clear();
                Console.WriteLine("=== УЧЁТ КНИГ В БИБЛИОТЕКЕ ===");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу");
                Console.WriteLine("3. Найти книги");
                Console.WriteLine("4. Отсортировать книги");
                Console.WriteLine("5. Самая дорогая книга");
                Console.WriteLine("6. Самая дешёвая книга");
                Console.WriteLine("7. Группировка по авторам");
                Console.WriteLine("8. Показать все книги");
                Console.WriteLine("9. Пакетный импорт");
                Console.WriteLine("10. Добавить в корзину");
                Console.WriteLine("11. Показать корзину");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        RemoveBook();
                        break;
                    case "3":
                        SearchBooks();
                        break;
                    case "4":
                        SortBooks();
                        break;
                    case "5":
                        ShowMostExpensive();
                        break;
                    case "6":
                        ShowCheapest();
                        break;
                    case "7":
                        GroupByAuthor();
                        break;
                    case "8":
                        ShowAllBooks();
                        break;
                    case "9":
                        BatchImport();
                        break;
                    case "10":
                        AddToCart();
                        break;
                    case "11":
                        ShowCart();
                        break;
                    case "0":
                        work = false;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод!");
                        break;
                }

                if (work)
                {
                    Console.WriteLine("\nНажмите Enter для продолжения...");
                    Console.ReadLine();
                }
            }
        }

        static void AddBook()
        {
            Console.Write("Введите название: ");
            string title = Console.ReadLine();
            if (title == "")
            {
                Console.WriteLine("Название не может быть пустым!");
                return;
            }

            Console.Write("Введите автора: ");
            string author = Console.ReadLine();
            if (author == "")
            {
                Console.WriteLine("Автор не может быть пустым!");
                return;
            }

            Console.WriteLine("Выберите жанр:");
            Console.WriteLine("1. Fiction  2. Science  3. History  4. Fantasy  5. Detective");
            Console.Write("Ваш выбор: ");
            int genreChoice;
            if (!int.TryParse(Console.ReadLine(), out genreChoice) || genreChoice < 1 || genreChoice > 5)
            {
                Console.WriteLine("Неверный жанр!");
                return;
            }

            Console.Write("Введите год издания: ");
            int year;
            if (!int.TryParse(Console.ReadLine(), out year) || year < 0)
            {
                Console.WriteLine("Неверный год!");
                return;
            }

            Console.Write("Введите цену: ");
            decimal price;
            if (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
            {
                Console.WriteLine("Неверная цена!");
                return;
            }

            Book b = new Book();
            b.Id = nextId;
            b.Title = title;
            b.Author = author;
            b.Genre = (Genre)(genreChoice - 1);
            b.Year = year;
            b.Price = price;

            books.Add(b);
            nextId++;

            Console.WriteLine($"Книга добавлена с ID {b.Id}");
        }

        static void RemoveBook()
        {
            Console.Write("Введите ID книги для удаления: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Неверный ID!");
                return;
            }

            // Используем LINQ для поиска
            Book found = books.FirstOrDefault(b => b.Id == id);
            if (found == null)
            {
                Console.WriteLine("Книга не найдена!");
                return;
            }

            books.Remove(found);
            Console.WriteLine("Книга удалена");
        }

        static void SearchBooks()
        {
            Console.WriteLine("Поиск:");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По автору");
            Console.WriteLine("3. По жанру");
            Console.Write("Выберите: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Введите часть названия: ");
                string search = Console.ReadLine();

                // LINQ Where для фильтрации
                var found = books.Where(b => b.Title.ToLower().Contains(search.ToLower())).ToList();

                if (found.Count == 0)
                {
                    Console.WriteLine("Ничего не найдено");
                    return;
                }

                Console.WriteLine("Результаты:");
                foreach (var b in found)
                {
                    Console.WriteLine(b.ToString());
                }
            }
            else if (choice == "2")
            {
                Console.Write("Введите часть имени автора: ");
                string search = Console.ReadLine();

                var found = books.Where(b => b.Author.ToLower().Contains(search.ToLower())).ToList();

                if (found.Count == 0)
                {
                    Console.WriteLine("Ничего не найдено");
                    return;
                }

                Console.WriteLine("Результаты:");
                foreach (var b in found)
                {
                    Console.WriteLine(b.ToString());
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine("Выберите жанр:");
                Console.WriteLine("1. Fiction  2. Science  3. History  4. Fantasy  5. Detective");
                Console.Write("Ваш выбор: ");
                int genreChoice;
                if (!int.TryParse(Console.ReadLine(), out genreChoice) || genreChoice < 1 || genreChoice > 5)
                {
                    Console.WriteLine("Неверный жанр!");
                    return;
                }

                Genre genre = (Genre)(genreChoice - 1);

                var found = books.Where(b => b.Genre == genre).ToList();

                if (found.Count == 0)
                {
                    Console.WriteLine("Ничего не найдено");
                    return;
                }

                Console.WriteLine("Результаты:");
                foreach (var b in found)
                {
                    Console.WriteLine(b.ToString());
                }
            }
        }

        static void SortBooks()
        {
            Console.WriteLine("Сортировка:");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По году издания");
            Console.Write("Выберите: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                // LINQ OrderBy для сортировки
                var sorted = books.OrderBy(b => b.Title).ToList();
                Console.WriteLine("Книги отсортированы по названию:");
                foreach (var b in sorted)
                {
                    Console.WriteLine(b.ToString());
                }
            }
            else if (choice == "2")
            {
                var sorted = books.OrderBy(b => b.Year).ToList();
                Console.WriteLine("Книги отсортированы по году:");
                foreach (var b in sorted)
                {
                    Console.WriteLine(b.ToString());
                }
            }
        }

        static void ShowMostExpensive()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Список книг пуст!");
                return;
            }

            // LINQ OrderByDescending + First для поиска самой дорогой
            var expensive = books.OrderByDescending(b => b.Price).First();
            Console.WriteLine("Самая дорогая книга:");
            Console.WriteLine(expensive.ToString());
        }

        static void ShowCheapest()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Список книг пуст!");
                return;
            }

            var cheap = books.OrderBy(b => b.Price).First();
            Console.WriteLine("Самая дешёвая книга:");
            Console.WriteLine(cheap.ToString());
        }

        static void GroupByAuthor()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Список книг пуст!");
                return;
            }

            // LINQ GroupBy для группировки
            var grouped = books.GroupBy(b => b.Author)
                               .Select(g => new { Author = g.Key, Count = g.Count() })
                               .OrderByDescending(g => g.Count)
                               .ToList();

            Console.WriteLine("Группировка по авторам:");
            foreach (var g in grouped)
            {
                Console.WriteLine($"{g.Author}: {g.Count} книг(а)");
            }
        }

        static void ShowAllBooks()
        {
            Console.WriteLine("=== ВСЕ КНИГИ ===");
            if (books.Count == 0)
            {
                Console.WriteLine("Список пуст");
                return;
            }

            foreach (var b in books)
            {
                Console.WriteLine(b.ToString());
            }
            Console.WriteLine($"Всего книг: {books.Count}");
        }

        static void BatchImport()
        {
            Console.WriteLine("Пакетный импорт книг");
            Console.WriteLine("Формат: Название;Автор;Жанр(1-5);Год;Цена");
            Console.WriteLine("Введите пустую строку для завершения:");

            while (true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                string[] parts = line.Split(';');
                if (parts.Length != 5)
                {
                    Console.WriteLine("Неверный формат строки!");
                    continue;
                }

                try
                {
                    Book b = new Book();
                    b.Id = nextId;
                    b.Title = parts[0].Trim();
                    b.Author = parts[1].Trim();
                    b.Genre = (Genre)(int.Parse(parts[2].Trim()) - 1);
                    b.Year = int.Parse(parts[3].Trim());
                    b.Price = decimal.Parse(parts[4].Trim());

                    books.Add(b);
                    nextId++;

                    Console.WriteLine($"Книга \"{b.Title}\" добавлена с ID {b.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке строки: {ex.Message}");
                }
            }
        }

        static void AddToCart()
        {
            ShowAllBooks();
            Console.Write("Введите ID книги для добавления в корзину: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Неверный ID!");
                return;
            }

            Book found = books.FirstOrDefault(b => b.Id == id);
            if (found == null)
            {
                Console.WriteLine("Книга не найдена!");
                return;
            }

            cart.Add(found);
            Console.WriteLine($"Книга \"{found.Title}\" добавлена в корзину");
        }

        static void ShowCart()
        {
            Console.WriteLine("=== КОРЗИНА ===");
            if (cart.Count == 0)
            {
                Console.WriteLine("Корзина пуста");
                return;
            }

            foreach (var b in cart)
            {
                Console.WriteLine(b.ToString());
            }

            // LINQ Sum для подсчёта общей стоимости
            decimal total = cart.Sum(b => b.Price);
            Console.WriteLine($"Всего книг: {cart.Count}");
            Console.WriteLine($"Итоговая стоимость: {total} руб.");
        }

        static void LoadTestBooks()
        {
            Book b1 = new Book();
            b1.Id = nextId++;
            b1.Title = "Война и мир";
            b1.Author = "Лев Толстой";
            b1.Genre = Genre.Fiction;
            b1.Year = 1869;
            b1.Price = 850;
            books.Add(b1);

            Book b2 = new Book();
            b2.Id = nextId++;
            b2.Title = "Краткая история времени";
            b2.Author = "Стивен Хокинг";
            b2.Genre = Genre.Science;
            b2.Year = 1988;
            b2.Price = 620;
            books.Add(b2);

            Book b3 = new Book();
            b3.Id = nextId++;
            b3.Title = "Гарри Поттер";
            b3.Author = "Дж.К. Роулинг";
            b3.Genre = Genre.Fantasy;
            b3.Year = 1997;
            b3.Price = 550;
            books.Add(b3);

            Book b4 = new Book();
            b4.Id = nextId++;
            b4.Title = "Преступление и наказание";
            b4.Author = "Фёдор Достоевский";
            b4.Genre = Genre.Detective;
            b4.Year = 1866;
            b4.Price = 480;
            books.Add(b4);

            Book b5 = new Book();
            b5.Id = nextId++;
            b5.Title = "История России";
            b5.Author = "Николай Карамзин";
            b5.Genre = Genre.History;
            b5.Year = 1818;
            b5.Price = 1200;
            books.Add(b5);
        }
    }
}