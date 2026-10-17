using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp
{
    enum Category
    {
        Electronics,
        Food,
        Clothing
    }

    class Product
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Category Cat { get; set; }

        public bool IsInStock()
        {
            return Quantity > 0;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price}, Кол-во: {Quantity}, Категория: {Cat}, На складе: {IsInStock()}";
        }
    }

    // Запись о продаже для стека
    class SaleRecord
    {
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }

    class Program
    {
        static List<Product> products = new List<Product>();
        static Stack<SaleRecord> salesHistory = new Stack<SaleRecord>(); // Стек для отмены продаж
        static int nextCode = 1;

        static void Main(string[] args)
        {
            // Загружаем тестовые данные
            LoadTestProducts();

            bool work = true;
            while (work)
            {
                Console.Clear();
                Console.WriteLine("=== УЧЁТ ТОВАРОВ В МАГАЗИНЕ ===");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Показать все товары");
                Console.WriteLine("7. Отменить последнюю продажу");
                Console.WriteLine("8. Отчёт о продажах");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        RemoveProduct();
                        break;
                    case "3":
                        OrderSupply();
                        break;
                    case "4":
                        SellProduct();
                        break;
                    case "5":
                        SearchProducts();
                        break;
                    case "6":
                        ShowAllProducts();
                        break;
                    case "7":
                        UndoLastSale();
                        break;
                    case "8":
                        ShowSalesReport();
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

        static void AddProduct()
        {
            Console.Write("Введите название: ");
            string name = Console.ReadLine();
            if (name == "")
            {
                Console.WriteLine("Название не может быть пустым!");
                return;
            }

            Console.Write("Введите цену: ");
            decimal price;
            if (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
            {
                Console.WriteLine("Неверная цена!");
                return;
            }

            Console.Write("Введите количество: ");
            int qty;
            if (!int.TryParse(Console.ReadLine(), out qty) || qty < 0)
            {
                Console.WriteLine("Неверное количество!");
                return;
            }

            Console.WriteLine("Выберите категорию:");
            Console.WriteLine("1. Electronics");
            Console.WriteLine("2. Food");
            Console.WriteLine("3. Clothing");
            Console.Write("Ваш выбор: ");
            int catChoice;
            if (!int.TryParse(Console.ReadLine(), out catChoice) || catChoice < 1 || catChoice > 3)
            {
                Console.WriteLine("Неверная категория!");
                return;
            }

            Category cat = (Category)(catChoice - 1);

            Product p = new Product();
            p.Code = nextCode;
            p.Name = name;
            p.Price = price;
            p.Quantity = qty;
            p.Cat = cat;

            products.Add(p);
            nextCode++;

            Console.WriteLine($"Товар добавлен с кодом {p.Code}");
        }

        static void RemoveProduct()
        {
            Console.Write("Введите код товара для удаления: ");
            int code;
            if (!int.TryParse(Console.ReadLine(), out code))
            {
                Console.WriteLine("Неверный код!");
                return;
            }

            // Ищем товар по коду
            Product found = null;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Code == code)
                {
                    found = products[i];
                    break;
                }
            }

            if (found == null)
            {
                Console.WriteLine("Товар не найден!");
                return;
            }

            products.Remove(found);
            Console.WriteLine("Товар удалён");
        }

        static void OrderSupply()
        {
            Console.Write("Введите код товара: ");
            int code;
            if (!int.TryParse(Console.ReadLine(), out code))
            {
                Console.WriteLine("Неверный код!");
                return;
            }

            Console.Write("Введите количество для поставки: ");
            int amount;
            if (!int.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Неверное количество!");
                return;
            }

            Product found = null;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Code == code)
                {
                    found = products[i];
                    break;
                }
            }

            if (found == null)
            {
                Console.WriteLine("Товар не найден!");
                return;
            }

            found.Quantity += amount;
            Console.WriteLine($"Поставка принята. Новое количество: {found.Quantity}");
        }

        static void SellProduct()
        {
            Console.Write("Введите код товара: ");
            int code;
            if (!int.TryParse(Console.ReadLine(), out code))
            {
                Console.WriteLine("Неверный код!");
                return;
            }

            Console.Write("Введите количество для продажи: ");
            int amount;
            if (!int.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Неверное количество!");
                return;
            }

            Product found = null;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Code == code)
                {
                    found = products[i];
                    break;
                }
            }

            if (found == null)
            {
                Console.WriteLine("Товар не найден!");
                return;
            }

            // Проверяем есть ли товар на складе
            if (!found.IsInStock())
            {
                Console.WriteLine("Товара нет на складе!");
                return;
            }

            if (found.Quantity < amount)
            {
                Console.WriteLine($"Недостаточно товара. На складе: {found.Quantity}");
                return;
            }

            // Продаём
            found.Quantity -= amount;

            // Записываем в стек для возможной отмены
            SaleRecord record = new SaleRecord();
            record.ProductCode = found.Code;
            record.ProductName = found.Name;
            record.Price = found.Price;
            record.Qty = amount;
            salesHistory.Push(record);

            Console.WriteLine($"Продано {amount} шт. на сумму {amount * found.Price} руб.");
        }

        static void SearchProducts()
        {
            Console.WriteLine("Поиск:");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Введите код: ");
                int code;
                if (!int.TryParse(Console.ReadLine(), out code))
                {
                    Console.WriteLine("Неверный код!");
                    return;
                }

                Product found = null;
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].Code == code)
                    {
                        found = products[i];
                        break;
                    }
                }

                if (found != null)
                    Console.WriteLine(found.ToString());
                else
                    Console.WriteLine("Не найдено");
            }
            else if (choice == "2")
            {
                Console.Write("Введите часть названия: ");
                string search = Console.ReadLine();

                Console.WriteLine("Результаты поиска:");
                bool foundAny = false;
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].Name.ToLower().Contains(search.ToLower()))
                    {
                        Console.WriteLine(products[i].ToString());
                        foundAny = true;
                    }
                }

                if (!foundAny)
                    Console.WriteLine("Ничего не найдено");
            }
            else if (choice == "3")
            {
                Console.WriteLine("Выберите категорию:");
                Console.WriteLine("1. Electronics");
                Console.WriteLine("2. Food");
                Console.WriteLine("3. Clothing");
                Console.Write("Ваш выбор: ");
                int catChoice;
                if (!int.TryParse(Console.ReadLine(), out catChoice) || catChoice < 1 || catChoice > 3)
                {
                    Console.WriteLine("Неверная категория!");
                    return;
                }

                Category cat = (Category)(catChoice - 1);

                Console.WriteLine("Результаты поиска:");
                bool foundAny = false;
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].Cat == cat)
                    {
                        Console.WriteLine(products[i].ToString());
                        foundAny = true;
                    }
                }

                if (!foundAny)
                    Console.WriteLine("Ничего не найдено");
            }
        }

        static void ShowAllProducts()
        {
            Console.WriteLine("=== ВСЕ ТОВАРЫ ===");
            if (products.Count == 0)
            {
                Console.WriteLine("Список пуст");
                return;
            }

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine(products[i].ToString());
            }
            Console.WriteLine($"Всего товаров: {products.Count}");
        }

        static void UndoLastSale()
        {
            if (salesHistory.Count == 0)
            {
                Console.WriteLine("История продаж пуста!");
                return;
            }

            // Достаём последнюю продажу из стека
            SaleRecord lastSale = salesHistory.Pop();

            // Ищем товар и возвращаем количество
            Product found = null;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Code == lastSale.ProductCode)
                {
                    found = products[i];
                    break;
                }
            }

            if (found != null)
            {
                found.Quantity += lastSale.Qty;
                Console.WriteLine($"Отменена продажа: {lastSale.ProductName}, возвращено {lastSale.Qty} шт.");
            }
            else
            {
                Console.WriteLine("Товар был удалён, но продажа отменена");
            }
        }

        static void ShowSalesReport()
        {
            Console.WriteLine("=== ОТЧЁТ О ПРОДАЖАХ ===");
            if (salesHistory.Count == 0)
            {
                Console.WriteLine("Продаж ещё не было");
                return;
            }

            // Группируем продажи по названию товара
            var grouped = salesHistory.GroupBy(s => s.ProductName)
                                      .Select(g => new
                                      {
                                          Name = g.Key,
                                          TotalQty = g.Sum(x => x.Qty),
                                          TotalSum = g.Sum(x => x.Price * x.Qty)
                                      });

            foreach (var item in grouped)
            {
                Console.WriteLine($"{item.Name}: продано {item.TotalQty} шт., сумма {item.TotalSum} руб.");
            }

            decimal grandTotal = 0;
            foreach (SaleRecord s in salesHistory)
            {
                grandTotal += s.Price * s.Qty;
            }
            Console.WriteLine($"Общая сумма продаж: {grandTotal} руб.");
        }

        static void LoadTestProducts()
        {
            Product p1 = new Product();
            p1.Code = nextCode++;
            p1.Name = "Смартфон Samsung";
            p1.Price = 25000;
            p1.Quantity = 10;
            p1.Cat = Category.Electronics;
            products.Add(p1);

            Product p2 = new Product();
            p2.Code = nextCode++;
            p2.Name = "Хлеб Бородинский";
            p2.Price = 50;
            p2.Quantity = 50;
            p2.Cat = Category.Food;
            products.Add(p2);

            Product p3 = new Product();
            p3.Code = nextCode++;
            p3.Name = "Куртка зимняя";
            p3.Price = 8000;
            p3.Quantity = 5;
            p3.Cat = Category.Clothing;
            products.Add(p3);

            Product p4 = new Product();
            p4.Code = nextCode++;
            p4.Name = "Молоко 1л";
            p4.Price = 80;
            p4.Quantity = 30;
            p4.Cat = Category.Food;
            products.Add(p4);

            Product p5 = new Product();
            p5.Code = nextCode++;
            p5.Name = "Наушники";
            p5.Price = 2000;
            p5.Quantity = 15;
            p5.Cat = Category.Electronics;
            products.Add(p5);
        }
    }
}