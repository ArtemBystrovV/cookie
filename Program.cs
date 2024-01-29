namespace cookie;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Cookie
{
    public string Form { get; set; }
    public string Size { get; set; }
    public string Flavor { get; set; }
    public string Glaze { get; set; }
    public string Decor { get; set; }
    public decimal Price { get; set; }
}

class Program
{
    static void Main()
    {
        List<Cookie> orderList = new List<Cookie>();
        decimal totalOrderPrice = 0;
        int selectedMenuItem = 0;

        Console.WriteLine("Выберите форму печенья");
        Dictionary<string, decimal> formPrices = new Dictionary<string, decimal>
        {
            { "Круг", 100M },
            { "Квадрат", 120M },
            { "Звезда", 150M }
        };

        Dictionary<string, decimal> sizePrices = new Dictionary<string, decimal>
        {
            { "Маленький", 50M },
            { "Средний", 100M },
            { "Большой", 150M }
        };

        Dictionary<string, decimal> flavorPrices = new Dictionary<string, decimal>
        {
            { "Имбирное", 80M },
            { "Сдобное", 50M },
            { "Овсяное", 40M },
        };

        Dictionary<string, decimal> glazePrices = new Dictionary<string, decimal>
        {
            { "Шоколад", 10M },
            { "Ваниль", 12M },
            { "Карамель", 16M }
        };

        Dictionary<string, decimal> decorPrices = new Dictionary<string, decimal>
        {
            { "Ничего", 0M },
            { "Рисунок", 40M },
            { "Посыпка на сплошной глазури", 30M }
        };

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню заказа печенья:");

            string[] menuItems = { "Собрать печеньку", "Посмотреть заказ", "Оформить заказ", "Выйти (ESC)" };

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedMenuItem)
                {
                    Console.WriteLine("-> " + menuItems[i]);
                }
                else
                {
                    Console.WriteLine("   " + menuItems[i]);
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                break;
            }

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedMenuItem > 0)
                    {
                        selectedMenuItem--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (selectedMenuItem < menuItems.Length - 1)
                    {
                        selectedMenuItem++;
                    }
                    break;

                case ConsoleKey.Enter:
                    switch (selectedMenuItem)
                    {
                        case 0:
                            orderList.Add(CreateCookie(formPrices, sizePrices, flavorPrices, glazePrices, decorPrices));
                            break;

                        case 1:
                            DisplayOrder(orderList);
                            break;

                        case 2:
                            totalOrderPrice += CalculateTotalPrice(orderList);
                            SaveOrderToFile(orderList, totalOrderPrice);
                            orderList.Clear();
                            break;

                        case 3:
                            Environment.Exit(0);
                            break;
                    }
                    break;
            }
        }
    }

    static Cookie CreateCookie(Dictionary<string, decimal> formPrices,
    Dictionary<string, decimal> sizePrices,
    Dictionary<string, decimal> flavorPrices,
    Dictionary<string, decimal> glazePrices,
    Dictionary<string, decimal> decorPrices)
    {
        Cookie cookie = new Cookie();
        cookie.Form = SelectOptionFromMenu(formPrices, "форму");
        cookie.Size = SelectOptionFromMenu(sizePrices, "размер");
        cookie.Flavor = SelectOptionFromMenu(flavorPrices, "вкус");
        cookie.Glaze = SelectOptionFromMenu(glazePrices, "глазурь");
        cookie.Decor = SelectOptionFromMenu(decorPrices, "декор");
        cookie.Price = CalculateTotalPriceForCookie(cookie, formPrices, sizePrices, flavorPrices, glazePrices, decorPrices);
        return cookie;
    }


    static string SelectOptionFromMenu(Dictionary<string, decimal> optionsWithPrices, string category)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();

            Console.WriteLine($"Выберите {category}:");

            for (int i = 0; i < optionsWithPrices.Count; i++)
            {
                var option = optionsWithPrices.ElementAt(i);
                if (i == selectedIndex)
                {
                    Console.WriteLine("-> " + (i + 1) + ". " + option.Key + " - цена: " + option.Value + " руб.");
                }
                else
                {
                    Console.WriteLine("   " + (i + 1) + ". " + option.Key + " - цена: " + option.Value + " руб.");
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedIndex > 0)
                    {
                        selectedIndex--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (selectedIndex < optionsWithPrices.Count - 1)
                    {
                        selectedIndex++;
                    }
                    break;

                case ConsoleKey.Enter:
                    return optionsWithPrices.Keys.ElementAt(selectedIndex);
            }
        }
    }


    static decimal CalculateTotalPriceForCookie(
        Cookie cookie,
        Dictionary<string, decimal> formPrices,
        Dictionary<string, decimal> sizePrices,
        Dictionary<string, decimal> flavorPrices,
        Dictionary<string, decimal> glazePrices,
        Dictionary<string, decimal> decorPrices)
    {
        decimal totalPrice = 0;

        totalPrice += formPrices[cookie.Form];
        totalPrice += sizePrices[cookie.Size];
        totalPrice += flavorPrices[cookie.Flavor];
        totalPrice += glazePrices[cookie.Glaze];
        totalPrice += decorPrices[cookie.Decor];

        return totalPrice;
    }

    static void DisplayOrder(List<Cookie> orderList)
    {
        Console.Clear();
        Console.WriteLine("Ваша печенька:");
        foreach (var cookie in orderList)
        {
            Console.WriteLine($"Форма-{cookie.Form}\nРазмер-{cookie.Size}\nВкус-{cookie.Flavor}\nГлазурь-{cookie.Glaze}\nДекор-{cookie.Decor}");
        }
        Console.WriteLine($"Суммарная цена заказа: {CalculateTotalPrice(orderList)} руб.");
        Console.WriteLine("\nНажмите Enter для продолжения...");
        Console.ReadKey();
    }

    static decimal CalculateTotalPrice(List<Cookie> orderList)
    {
        decimal total = 0;
        foreach (var cookie in orderList)
        {
            total += cookie.Price;
        }
        return total;
    }

    static void SaveOrderToFile(List<Cookie> orderList, decimal totalOrderPrice)
    {
        Console.Write("---Введите путь к файлу для сохранения заказа: \n");
        string Cookiefile = Console.ReadLine();

        using (StreamWriter writer = new StreamWriter(Cookiefile))
        {
            writer.WriteLine("Ваш заказ и состав торта:");
            foreach (var cookie in orderList)
            {
                writer.WriteLine($"Форма-{cookie.Form}\nРазмер-{cookie.Size}\nВкус-{cookie.Flavor}\nГлазурь-{cookie.Glaze}\nДекор-{cookie.Decor}");
            }
            writer.WriteLine($"Суммарная цена заказа: {totalOrderPrice} руб.");
        }
        Console.WriteLine($"Заказ сохранен в файл: {Cookiefile}");
        Console.WriteLine("\nНажмите Enter для продолжения...");
        Console.ReadKey();
    }
}