using burgerBurger.Enums;
using burgerBurger.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class CustomItem : OrderItem
    {
        public int CustomerId { get; set; }

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
}