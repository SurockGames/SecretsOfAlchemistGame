using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Assets.Code
{
    public interface IInventoryService
    {
        public event Action OnInventoryChanged;

        public ItemStack[] GetAllItems();
        public bool TryAddItemToInventory(Item item, int amount);
        public void RemoveItemFromInventory(Item item, int amount);
        public void RemoveItemStackFromInventory(Item item);
    }

    public class PlayerInventoryService : SerializedMonoBehaviour, IInventoryService
    {
        public event Action OnInventoryChanged;
        public event Action<Item, int> OnItemStackAdded;

        public event Action<Item, int> OnItemRemoved;
        public event Action<Item> OnItemStackRemoved;

        [ShowInInspector, ReadOnly]
        private Dictionary<IItem, ItemStack> itemStacks = new();

        //private IItem currentActivePatronStack;

        public ItemStack[] GetAllItems()
        {
            var stacks = new ItemStack[1];

            return stacks;
        }

        public bool TryGetPotionStack(out Item stack, out int amount)
        {
            stack = null;
            amount = 0;

            foreach (var itemStack in itemStacks.Values)
            {
                if (itemStack.Item is Potion)
                {
                    stack = itemStack.Item;
                    amount = itemStack.Amount;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetPatronStack(out Item stack, out int amount)
        {
            stack = null;
            amount = 0;

            foreach (var itemStack in itemStacks.Values)
            {
                if (itemStack.Item is Patron)
                {
                    stack = itemStack.Item;
                    amount = itemStack.Amount;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetItemFromIventory(Item itemToGet, out int amount)
        {
            amount = 0;

            if (itemStacks.ContainsKey(itemToGet))
            {
                amount = itemStacks[itemToGet].Amount;
                RemoveItemStackFromInventory(itemToGet);
                return true;
            }
            return false;
        }

        public ItemStack[] GetItemsOfType(ItemTypes itemType)
        {
            var stacks = new List<ItemStack>();
            foreach (var itemStack in itemStacks.Values)
            {
                if (itemStack.Item.ItemType == itemType) 
                    stacks.Add(itemStack);
            }
            return stacks.ToArray();
        }

        [Button]
        public bool TryAddItemToInventory(Item item, int amount)
        {
            if (amount < 0) return false;

            if (itemStacks.ContainsKey(item))
            {
                itemStacks[item].Amount += amount;
            }
            else
            {
                itemStacks.Add(item, new ItemStack(item, amount));
            }

            OnItemStackAdded?.Invoke(item, amount);
            OnInventoryChanged?.Invoke();

            return true;
        }

        [Button]
        public void RemoveItemFromInventory(Item item, int amount)
        {
            if (amount < 0) return;

            if (itemStacks.ContainsKey(item) && itemStacks[item].Amount > 0)
            {
                if (itemStacks[item].Amount - amount > 0)
                {
                    itemStacks[item].Amount -= amount;
                }
                else
                {
                    RemoveItemStackFromInventory(item);
                    return;
                }
            }

            OnItemRemoved?.Invoke(item, amount);
            OnInventoryChanged?.Invoke();
        }

        [Button]
        public void RemoveItemStackFromInventory(Item item)
        {
            if (itemStacks.ContainsKey(item))
            {
                itemStacks.Remove(item);
            }

            OnItemStackRemoved?.Invoke(item);
            OnInventoryChanged?.Invoke();
        }
    }

}