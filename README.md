// fel 1: shoppinglist.cs rad 28, Total()
// vad är fel: loopen började på index 1, så första varan räknades inte med i summan.
// hur jag lagade: ändrade startvärdet från 1 till 0.
    for (int i = 0; i < items.Count; i++)


// fel 2: program.cs rad 16, 27 och 37 (menyvalet, priset och numret)
// vad är fel: int.Parse kraschar om man skriver något som inte är ett tal, t.ex. "blåbär".
// hur jag lagade: bytte int.Parse mot int.TryParse som visar ett meddelande i stället för att krascha.
    
    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Skriv en siffra mellan 1 och 5.");
        continue;
    }

// fel 3: shoppinglist.cs RemoveAt()

// vad är fel: programmet kraschade om man tog bort ett nummer som inte finns, t.ex. 0 eller 99.

// hur jag lagade: RemoveAt kollar nu numret och returnerar false om det inte finns. program.cs visar då ett meddelande.
    
    if (number < 1 || number > items.Count)
    {
        return false;
    }

// fel 4: shoppinglist.cs Load()
// vad är fel: om items.txt inte finns kraschar File.ReadAllText.
// hur jag lagade: kollar med File.Exists om filen finns först. Finns den inte startar programmet med en tom lista.
    if (!File.Exists(path))
    {
        Console.WriteLine("Ingen sparad lista hittades, startar med en tom lista.");
        return;
    }


