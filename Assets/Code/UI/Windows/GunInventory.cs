using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Code
{

    public class GunInventory : SerializedMonoBehaviour, IInventoryService
    {
        [SerializeField] private Game game;

        public event Action OnInventoryChanged;
        public event Action<Item, int> OnItemStackAdded;
        public event Action OnItemRemoved;
        public event Action<Item> OnItemStackRemoved;

        [ShowInInspector, ReadOnly]
        private ItemStack patronStack = new ItemStack(null, 0);

        public Patron PatronInSlot
        {
            get
            {
                if (patronStack.Item != null)
                {
                    return patronStack.Item as Patron;
                }

                if (TryGetPatronFromPlayerInventoryIfHasNothing())
                {
                    return patronStack.Item as Patron;
                }
                return null;
            }
        }

        public int AmountOfPatronsInSlot => patronStack.Amount;

        private void Awake()
        {
            OnInventoryChanged += UpdateGunSlot;
            game.PlayerInventoryService.OnInventoryChanged += TryGetPatronOfEquipedTypeFromPlayerInventory;
        }

        private void UpdateGunSlot()
        {

        }

        private void OnDestroy()
        {
            OnInventoryChanged -= UpdateGunSlot;
            game.PlayerInventoryService.OnInventoryChanged -= TryGetPatronOfEquipedTypeFromPlayerInventory;
        }

        public bool TryGetPatronFromPlayerInventoryIfHasNothing()
        {
            if (patronStack.Item != null && patronStack.Amount > 0) return false;

            if (game.PlayerInventoryService.TryGetPatronStack(out Item patron, out int amount))
            {
                if (TryAddItemToInventory(patron, amount))
                {
                    game.PlayerInventoryService.RemoveItemStackFromInventory(patron);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void TryGetPatronOfEquipedTypeFromPlayerInventory()
        {
            if (patronStack.Item == null) return;

            if (game.PlayerInventoryService.TryGetItemFromIventory(patronStack.Item, out int amount))
            {
                patronStack.Amount += amount;
                OnInventoryChanged?.Invoke();
            }
        }

        public bool TryAddItemToInventory(Item item, int amount)
        {
            if (amount < 0) return false;
            if (item is Patron == false) return false;

            if (patronStack.Amount == 0 || patronStack.Item == null)
            {
                patronStack.Item = item;
                patronStack.Amount = amount;
            }
            else if (patronStack.Item && patronStack.Item == item)
            {
                patronStack.Amount += amount;
            }
            else
            {
                return false;
            }

            OnItemStackAdded?.Invoke(item, amount);
            OnInventoryChanged?.Invoke();

            return true;
        }

        public void RemovePatronStack()
        {
            if (patronStack.Item == null) return;

            patronStack.Item = null;
            patronStack.Amount = 0;

            OnItemRemoved?.Invoke();
            OnInventoryChanged?.Invoke();

            return;
        }

        public void RemoveItemFromInventory(Item item, int amount)
        {
            if (amount < 0) return;

            if (patronStack.Item == item)
            {
                if (patronStack.Amount - amount <= 0)
                {
                    RemovePatronStack();
                }
                else
                {
                    patronStack.Amount -= amount;
                    OnInventoryChanged?.Invoke();
                }
            }
        }

        public void RemoveItemStackFromInventory(Item item)
        {
            if (patronStack.Item == null) return;

            RemovePatronStack();
        }

        public ItemStack[] GetAllItems()
        {
            ItemStack[] stacks = new ItemStack[1];

            if (patronStack.Item == null)
                return stacks;

            
            stacks[0] = patronStack;
            return stacks;
        }
    }

}