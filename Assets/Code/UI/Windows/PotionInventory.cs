using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Code
{
    public class PotionInventory : SerializedMonoBehaviour, IInventoryService
    {
        [SerializeField] private Game game;

        public event Action OnInventoryChanged;
        public event Action<Item, int> OnItemStackAdded;
        public event Action OnItemRemoved;
        public event Action<Item> OnItemStackRemoved;

        [ShowInInspector, ReadOnly]
        private ItemStack potionStack = new ItemStack(null, 0);

        public Potion PotionInSlot
        {
            get
            {
                if (potionStack.Item != null)
                {
                    return potionStack.Item as Potion;
                }

                if (TryGetPotionFromPlayerInventoryIfHasNothing())
                {
                    return potionStack.Item as Potion;
                }
                return null;
            }
        }

        public int AmountOfPotionInSlot => potionStack.Amount;

        private void Awake()
        {
            OnInventoryChanged += UpdateGunSlot;
        }

        private void UpdateGunSlot()
        {

        }

        public bool TryGetPotionFromPlayerInventoryIfHasNothing()
        {
            if (potionStack.Item != null && potionStack.Amount > 0) return false;

            if (game.PlayerInventoryService.TryGetPotionStack(out Item potion, out int amount))
            {
                if (TryAddItemToInventory(potion, amount))
                {
                    game.PlayerInventoryService.RemoveItemStackFromInventory(potion);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool TryAddItemToInventory(Item item, int amount)
        {
            if (amount < 0) return false;
            if (item is Potion == false) return false;

            if (potionStack.Amount == 0 || potionStack.Item == null)
            {
                potionStack.Item = item;
                potionStack.Amount = amount;
            }
            else if (potionStack.Item && potionStack.Item == item)
            {
                potionStack.Amount += amount;
            }
            else
            {
                return false;
            }

            OnItemStackAdded?.Invoke(item, amount);
            OnInventoryChanged?.Invoke();

            return true;
        }

        public void RemovePotionStack()
        {
            if (potionStack.Item == null) return;

            potionStack.Item = null;
            potionStack.Amount = 0;

            OnItemRemoved?.Invoke();
            OnInventoryChanged?.Invoke();

            return;
        }

        public void RemoveItemFromInventory(Item item, int amount)
        {
            if (amount < 0) return;

            if (potionStack.Item == item)
            {
                if (potionStack.Amount - amount <= 0)
                {
                    RemovePotionStack();
                }
                else
                {
                    potionStack.Amount -= amount;
                    OnInventoryChanged?.Invoke();
                }
            }
        }

        public void RemoveItemStackFromInventory(Item item)
        {
            if (potionStack.Item == null) return;

            RemovePotionStack();
        }

        public ItemStack[] GetAllItems()
        {
            ItemStack[] stacks = new ItemStack[1];

            if (potionStack.Item == null)
                return stacks;

            stacks[0] = potionStack;
            return stacks;
        }
    }

}