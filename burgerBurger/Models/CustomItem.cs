using burgerBurger.Enums;
using burgerBurger.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class CustomItem : OrderItem
{
    public void AddInventoryItem(InventoryOutline item)
    {
        Ingredients.Add(item);
    }

    public void RemoveInventoryItem(InventoryOutline item)
    {
        Ingredients.Remove(item);
    }

    public List<InventoryOutline> ListInventoryItems()
    {
        return Ingredients;
    }
}



