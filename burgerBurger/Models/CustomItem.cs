using burgerBurger.Enums;
using burgerBurger.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class CustomItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public List<ItemInventory> Ingredients { get; set; } = new List<ItemInventory>();

    public void AddInventoryItem(ItemInventory item)
    {
        Ingredients.Add(item);
    }

    public void RemoveInventoryItem(ItemInventory item)
    {
        Ingredients.Remove(item);
    }

    public List<ItemInventory> ListInventoryItems()
    {
        return Ingredients;
    }
}



