using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [Serializable]
    public class ReceiptItem
    {
        [Range(1, 100)] public int amount;
        public Item Item;
        public int pointsForGettingRight;
        public int pointsForGettingNotCompletely;
        public bool IsMustBeThisItemNotAnotherVariant;
    }

    public class ReceiptCraftTable : MonoBehaviour
    {
        [SerializeField] private List<Receipt> receipts;

        public bool TryCraftItem(List<ItemStack> itemsToCook)
        {
            var itemsCanBeGetFromReceipts = new List<ItemStack>();

            //foreach (var receipt in receipts)
            //{
            //    itemsCanBeGetFromReceipts.Add(receipt.TryGetItem(itemsToCook, out int coincidedItemsFromReceiptAmount));
            //}



            return false;
        }
    }

    [CreateAssetMenu(fileName = "Data", menuName = "Receipts/Receipt", order = 1)]
    public class Receipt : SerializedScriptableObject
    {
        [SerializeField] private List<ReceiptItem> receiptItems;
        [SerializeField] private ItemStack itemOutput;

        [Button]
        private void Test(List<Item> itemsToCook, List<int> amountOfEach)
        {
            var itemsStack = new List<ItemStack>();

            for (int i = 0; i < itemsToCook.Count; i++)
            {
                Item item = itemsToCook[i];
                itemsStack.Add(new ItemStack(item, amountOfEach[i]));
            }

            Debug.Log($"Can cook with this items - {CanBeCookedWithIngredients(itemsStack, out int amount, out int amountCanBeCooked)}, coincided ingredients = {amount}, and amount can be cooked {amountCanBeCooked}");
        }

        public int EvaluateAmount(List<ItemStack> itemsToCook)
        {

            return 0;
        }

        public ItemStack TryGetItem(List<ItemStack> itemsToCook, out int coincidedItemsFromReceiptAmount, out int amountCanBeCooked)
        {
            coincidedItemsFromReceiptAmount = 0;
            amountCanBeCooked = 0;

            if (CanBeCookedWithIngredients(itemsToCook, out coincidedItemsFromReceiptAmount, out amountCanBeCooked))
            {
                var itemCooked = new ItemStack(itemOutput.Item, amountCanBeCooked * itemOutput.Amount);
                return itemCooked;
            }
            return null;
        }



        public bool CanBeCookedWithIngredients(List<ItemStack> itemsToCook, out int coincidedItemsFromReceiptAmount, out int amountCanBeCooked)
        {
            coincidedItemsFromReceiptAmount = 0;
            amountCanBeCooked = 0;

            if (itemsToCook.Count < receiptItems.Count) return false;

            var itemsLeft = new List<ItemStack>();
            foreach (var item in itemsToCook)
            {
                itemsLeft.Add(item);
            }

            //bool correctItemsAmount;

            foreach (var receiptItem in receiptItems)
            {
                foreach (ItemStack itemToCook in itemsLeft)
                {
                    if (receiptItem.IsMustBeThisItemNotAnotherVariant)
                    {
                        if (receiptItem.Item == itemToCook.Item)
                        {
                            coincidedItemsFromReceiptAmount++;
                            var amount = itemToCook.Amount / receiptItem.amount;

                            if (coincidedItemsFromReceiptAmount == 1)
                                amountCanBeCooked = amount;
                            else
                                amountCanBeCooked = Mathf.Min(amountCanBeCooked, amount);

                            itemsLeft.Remove(itemToCook);
                            break;
                        }
                    }
                    else
                    {
                        // Compare original items if it's item variant
                        if (receiptItem.Item.CompareItem == itemToCook.Item.CompareItem)
                        {
                            coincidedItemsFromReceiptAmount++;
                            var amount = itemToCook.Amount / receiptItem.amount;

                            if (coincidedItemsFromReceiptAmount == 1)
                                amountCanBeCooked = amount;
                            else
                                amountCanBeCooked = Mathf.Min(amountCanBeCooked, amount);

                            itemsLeft.Remove(itemToCook);
                            break;
                        }
                    }
                }
            }
            //coincidedItemsFromReceiptAmount = correctItemsAmount;

            if (coincidedItemsFromReceiptAmount >= receiptItems.Count && amountCanBeCooked > 0)
            {
                return true;
            }

            return false;
        }
    }
}