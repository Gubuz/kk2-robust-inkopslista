// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;
    private int budget;

    public ShoppingList(string path, int budget)
    {
        this.path = path;
        this.budget = budget;
    }

    // Adds the item if it fits within the budget. Returns false if it doesn't.
    public bool Add(Item item)
    {
        if (Total() + item.Price > budget)
        {
            return false;
        }

        items.Add(item);
        return true;
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public bool RemoveAt(int number)
    {
        if (number < 1 || number > items.Count)
        {
            return false;
        }

        items.RemoveAt(number - 1);
        return true;
    }

    // Adds up the price of every item on the list.
    public int Total()
    {
        int sum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (item.Name == name)
            {
                return item;
            }
        }

        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }

    // Writes one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}");
        }

        try
        {
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
            Console.WriteLine("Listan är sparad.");
        }
        catch (IOException)
        {
            Console.WriteLine($"Kunde inte spara listan.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Programmet har inte behörighet att skriva till filen.");
        }
    }

    // Reads the file back into the list.
    public void Load()
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("Ingen sparad lista hittades, startar med en tom lista.");
            return;
        }

        string[] lines;
        try
        {
            lines = File.ReadAllLines(path);
        }
        catch (IOException)
        {
            Console.WriteLine($"Kunde inte läsa listan.");
            return;
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Programmet har inte behörighet att läsa filen.");
            return;
        }

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split(';', 2);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int price))
            {
                Console.WriteLine($"Hoppar över felaktig rad: {line}");
                continue;
            }

            Item item;
            try
            {
                item = new Item(parts[1], price);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Hoppar över felaktig rad: {line}");
                continue;
            }

            if (!Add(item))
            {
                Console.WriteLine($"Hoppar över {item.Name}, den får inte plats i budgeten.");
            }
        }
    }
}
