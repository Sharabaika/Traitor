namespace ScriptableItems
{
    public interface IItemContainer
    {
        void Combine(ItemSlot itemsToAdd, ItemSlot target);
        bool HasItem(Item item);
        int Count(Item itemsToCount);
    }
}