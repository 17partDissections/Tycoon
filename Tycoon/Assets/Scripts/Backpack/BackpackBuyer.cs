using System.Collections.Generic;

public class BackpackBuyer : Backpack
{
    public int NumberInQueue;
    public List<ItemName> WannaBuy = new List<ItemName>();
    public List<ItemName> RepeatingItems = new List<ItemName>();
    public int LeftItems(ItemName itemName)
    {
        var item = _items.FindAll(x => x.ItemName == itemName);
        return item.Count;
    }
    

}