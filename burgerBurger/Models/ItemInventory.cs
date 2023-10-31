namespace burgerBurger.Models
{
    public class ItemInventory
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int IngredientId { get; set; }

        public ItemInventory() { }

        public ItemInventory(int item, int ing)
        {
            ItemId = item;
            IngredientId = ing;
        }
    }
}
