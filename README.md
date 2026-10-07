Robust inköpslista – fel jag hittade och lagade

Jag hittade 6 fel i startkoden. För varje fel står det var felet finns, vad som gick fel och vad jag ändrade.


Fel 1 – Totalsumman blev fel

Var: "ShoppingList.cs", metoden "Total()"

Vad var fel: Loopen började på index 1 i stället för 0. Därför räknades den första varan aldrig med i totalsumman.

Vad jag ändrade: Jag ändrade startvärdet från 1 till 0.

Före:

    for (int i = 1; i < items.Count; i++)

Efter:

    for (int i = 0; i < items.Count; i++)


Fel 2 – Programmet kraschade om man skrev bokstäver i stället för en siffra

Var: "Program.cs", menyvalet, priset och numret när man tar bort en vara

Vad var fel: "int.Parse" kraschar om man skriver något som inte är ett tal, till exempel "blåbär".

Vad jag ändrade: Jag bytte "int.Parse" mot "int.TryParse". Om det man skriver inte är ett tal visas ett meddelande och menyn kommer tillbaka, i stället för att programmet kraschar.

Före:

    int choice = int.Parse(Console.ReadLine());

Efter:

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Skriv en siffra mellan 1 och 5.");
        continue;
    }


Fel 3 – Programmet kraschade om man tog bort en vara som inte finns

Var: "ShoppingList.cs", metoden "RemoveAt()"

Vad var fel: Om man skrev ett nummer som inte finns i listan, till exempel 0 eller 99, kraschade programmet.

Vad jag ändrade: "RemoveAt" kontrollerar nu först att numret finns. Om det inte finns returnerar metoden "false", och då skriver "Program.cs" ut "Det finns ingen vara med det numret."

Efter:

    public bool RemoveAt(int number)
    {
        if (number < 1 || number > items.Count)
        {
            return false;
        }

        items.RemoveAt(number - 1);
        return true;
    }


Fel 4 – Programmet kraschade om filen items.txt inte fanns

Var: "ShoppingList.cs", metoden "Load()"

Vad var fel: Om "items.txt" inte finns kraschar "File.ReadAllText" direkt när programmet startar.

Vad jag ändrade: Jag kontrollerar först med "File.Exists" att filen finns. Om den inte finns startar programmet med en tom lista.

Efter:

    if (!File.Exists(path))
    {
        Console.WriteLine("Ingen sparad lista hittades, startar med en tom lista.");
        return;
    }


Fel 5 – Programmet sa "Listan är sparad." fast den inte sparades

Var: "ShoppingList.cs", metoden "Save()"

Vad var fel: "catch"-blocket var tomt, och "Listan är sparad." skrevs ut efter "try/catch". Om sparningen misslyckades fick man alltså inget felmeddelande, och programmet sa ändå att listan var sparad.

Vad jag ändrade: Jag flyttade "Listan är sparad." in i "try", så att meddelandet bara visas när sparningen lyckas. I "catch" visas nu ett felmeddelande.

Före:

    try
    {
        File.WriteAllText(path, ...);
    }
    catch
    {
    }

    Console.WriteLine("Listan är sparad.");

Efter:

    try
    {
        File.WriteAllText(path, ...);
        Console.WriteLine("Listan är sparad.");
    }
    catch (IOException)
    {
        Console.WriteLine("Kunde inte spara listan.");
    }
    catch (UnauthorizedAccessException)
    {
        Console.WriteLine("Programmet har inte behörighet att skriva till filen.");
    }


Fel 6 – Programmet kraschade på tomma eller felaktiga rader i filen

Var: "ShoppingList.cs", metoden "Load()"

Vad var fel: Filen slutar med en radbrytning, så den sista raden är tom. På en tom rad finns inget "parts[1]", och då kraschar programmet. Om en rad hade fel format, till exempel ett pris som inte är ett tal, kraschade "int.Parse".

Vad jag ändrade: Programmet hoppar nu över tomma rader. Jag använder "int.TryParse" i stället för "int.Parse", och en felaktig rad hoppas över med ett meddelande i stället för att programmet kraschar.

Efter:

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





-- DEL 2

Item skyddar sig själv

Var: "Item.cs", konstruktorn

Vad jag ändrade: Konstruktorn kontrollerar nu värdena innan de sparas. Om namnet är tomt kastas "ArgumentException". Om priset är negativt kastas "ArgumentOutOfRangeException". Då skapas aldrig ett trasigt Item.

Jag ändrade också "set" till "private set" på Name och Price, så att ingen kan ändra dem utanför klassen och kringgå kontrollen.

    if (string.IsNullOrWhiteSpace(name))
    {
        throw new ArgumentException("Namnet får inte vara tomt.");
    }

    if (price < 0)
    {
        throw new ArgumentOutOfRangeException(nameof(price), "Priset får inte vara negativt.");
    }


    
    
    Listan har ett budgettak

Var: "ShoppingList.cs", fälten och konstruktorn

Vad jag ändrade: Jag lade till ett nytt fält "budget" som håller reda på hur dyr listan sammanlagt får bli. Konstruktorn tar nu emot taket som en andra parameter, så man måste bestämma ett tak när man skapar listan. Fältet är "private", så taket kan inte ändras utifrån.

Före:

    private string path;

    public ShoppingList(string path)
    {
        this.path = path;
    }

Efter:

    private string path;
    private int budget;

    public ShoppingList(string path, int budget)
    {
        this.path = path;
        this.budget = budget;
    }