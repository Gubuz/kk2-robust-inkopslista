// första fel jag hitta var i shoppinglist.cs i rad 28.

// vad är fel: loopen började på index 1 (int i = 1), så första varan räknades aldrig med i summan.

// hur jag Lagade : ändrade värdet i loppen från 0 till 1 :
    for (int i = 0; i < items.Count; i++)