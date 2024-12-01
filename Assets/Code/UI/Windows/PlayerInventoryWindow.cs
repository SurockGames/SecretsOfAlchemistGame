using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class PlayerInventoryWindow : Window, ILeftWindow
    {
        [SerializeField] private GameObject playerInventoryGroup;

        [SerializeField] private PlayerInventoryService inventory;

        [SerializeField] private List<Image> IngredientsSlots;
        [SerializeField] private List<Image> PotionsSlots;
        [SerializeField] private List<Image> AmmunitionSlots;

        [SerializeField] private Image SelectSlotImage;

        [SerializeField] private Button leftTabButton;
        [SerializeField] private Button rightTabButton;

        [SerializeField] private GameObject ingredientTab;
        [SerializeField] private GameObject potionsTab;
        [SerializeField] private GameObject ammunitionTab;

        [SerializeField] private RectTransform tabsGroup;

        [SerializeField] private int tabOffset;

        [SerializeField] private TMP_Text tabsName;

        [ShowInInspector, ReadOnly]
        private int activeTabId;
        private List<GameObject> activeTabs;
        private int slotsAmount;

        private bool ingredientsTabActive;
        private bool potionTabActive;
        private bool ammunitionTabActive;

        //private int activeToPassSlotId;
        //private List<int> highlightedSlotsIds;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            ActivateInventoryTabs(true, true, true);

            leftTabButton.onClick.AddListener(MoveToLeftTab);
            rightTabButton.onClick.AddListener(MoveToRightTab);
        }

        private void Start()
        {
            inventory.OnInventoryChanged += UpdateInventory;
        }

        private void OnDisable()
        {
            inventory.OnInventoryChanged -= UpdateInventory;

            leftTabButton.onClick.RemoveListener(MoveToLeftTab);
            rightTabButton.onClick.RemoveListener(MoveToRightTab);
        }

        public void Initialize()
        {
            slotsAmount = 0;

            foreach (var slot in IngredientsSlots)
            {
                var slotVisuals = slot.gameObject.GetOrAddComponent<SlotVisual>();
                slotVisuals.OnSlotHovered += OnSlotHover;
                slotVisuals.OnSlotUnhovered += OnSlotUnhover;
                slotVisuals.OnSlotClicked += OnSlotClicked;

                slotVisuals.Initialize(slotsAmount);
                slotsAmount++;
            }

            foreach (var slot in PotionsSlots)
            {
                var slotVisuals = slot.gameObject.GetOrAddComponent<SlotVisual>();
                slotVisuals.OnSlotHovered += OnSlotHover;
                slotVisuals.OnSlotUnhovered += OnSlotUnhover;
                slotVisuals.OnSlotClicked += OnSlotClicked;

                slotVisuals.Initialize(slotsAmount);
                slotsAmount++;
            }

            foreach (var slot in AmmunitionSlots)
            {
                var slotVisuals = slot.gameObject.GetOrAddComponent<SlotVisual>();
                slotVisuals.OnSlotHovered += OnSlotHover;
                slotVisuals.OnSlotUnhovered += OnSlotUnhover;
                slotVisuals.OnSlotClicked += OnSlotClicked;

                slotVisuals.Initialize(slotsAmount);
                slotsAmount++;
            }
        }

        public override void SetWindow(bool isActive)
        {
            playerInventoryGroup.SetActive(isActive);
        }

        #region Tab Buttons
        [Button]
        public void ActivateInventoryTabs(bool ingredients, bool potions, bool ammunition)
        {
            activeTabId = 0;
            activeTabs = new List<GameObject>();

            if (ingredients)
            {
                ingredientsTabActive = true;
                activeTabs.Add(ingredientTab);
                ingredientTab.SetActive(true);
            }
            else
            {
                ingredientsTabActive = false;
                ingredientTab.SetActive(false);
            }

            if (potions)
            {
                potionTabActive = true;
                activeTabs.Add(potionsTab);
                potionsTab.SetActive(true);
            }
            else
            {
                potionTabActive = false;
                potionsTab.SetActive(false);
            }


            if (ammunition)
            {
                ammunitionTabActive = true;
                activeTabs.Add(ammunitionTab);
                ammunitionTab.SetActive(true);
            }
            else
            {
                ammunitionTabActive = false;
                ammunitionTab.SetActive(false);
            }

            if (activeTabs.Count > 1)
                ActivateTabButtons();
            else
                DeactivateTabButtons();

            UpdateInventory();
            UpdateTabsGroup();
        }

        public void ActivateTabButtons()
        {
            leftTabButton.gameObject.SetActive(true);
            rightTabButton.gameObject.SetActive(true);
        }

        public void DeactivateTabButtons()
        {
            leftTabButton.gameObject.SetActive(false);
            rightTabButton.gameObject.SetActive(false);
        }

        public void UpdateTabsGroup()
        {
            tabsGroup.SetRight(tabOffset * activeTabId);
            SetTabName();
        }

        public void MoveToLeftTab()
        {
            if (activeTabId == 0) return;

            activeTabId--;
            SetTabName();
            tabsGroup.SetRight(tabOffset * activeTabId);
        }

        public void MoveToRightTab()
        {
            if (activeTabId == activeTabs.Count - 1) return;

            activeTabId++;
            SetTabName();
            tabsGroup.SetRight(tabOffset * activeTabId);
        }

        private void SetTabName()
        {
            if (activeTabs[activeTabId] == ingredientTab)
            {
                tabsName.text = "Ingredients";
            }
            else if (activeTabs[activeTabId] == potionsTab)
            {
                tabsName.text = "Potions";
            }
            else if (activeTabs[activeTabId] == ammunitionTab)
            {
                tabsName.text = "Patrons";
            }

        }

        #endregion

        #region Update Inventory tabs
        public void UpdateInventory()
        {
            if (ingredientsTabActive)
                UpdateIngredients();

            if (potionTabActive)
                UpdatePotions();

            if (ammunitionTabActive)
                UpdateAmmunition();
        }

        private void UpdateIngredients()
        {
            var items = inventory.GetItemsOfType(ItemTypes.Ingredient);

            for (int i = 0; i < items.Length; i++)
            {
                IngredientsSlots[i].sprite = items[i].Item.Icon;
                IngredientsSlots[i].color = Color.white;
                var slot = IngredientsSlots[i].GetComponent<SlotVisual>();
                slot.SetAmount(items[i].Amount);
                slot.SetItem(items[i].Item);
            }

            var emptySlotsAmount = IngredientsSlots.Count - items.Length;

            if (emptySlotsAmount <= 0) return;

            for (int i = items.Length; i < IngredientsSlots.Count; i++)
            {
                IngredientsSlots[i].GetComponent<SlotVisual>().ClearToDefault();
            }
        }

        private void UpdatePotions()
        {
            var items = inventory.GetItemsOfType(ItemTypes.Potion);

            for (int i = 0; i < items.Length; i++)
            {
                PotionsSlots[i].sprite = items[i].Item.Icon;
                PotionsSlots[i].color = Color.white;
                var slot = PotionsSlots[i].GetComponent<SlotVisual>();
                slot.SetAmount(items[i].Amount);
                slot.SetItem(items[i].Item);
            }

            var emptySlotsAmount = PotionsSlots.Count - items.Length;

            if (emptySlotsAmount <= 0) return;

            for (int i = items.Length; i < PotionsSlots.Count; i++)
            {
                PotionsSlots[i].GetComponent<SlotVisual>().ClearToDefault();
            }
        }

        private void UpdateAmmunition()
        {
            var items = inventory.GetItemsOfType(ItemTypes.Ammunition);

            for (int i = 0; i < items.Length; i++)
            {
                AmmunitionSlots[i].sprite = items[i].Item.Icon;
                AmmunitionSlots[i].color = Color.white;
                var slot = AmmunitionSlots[i].GetComponent<SlotVisual>();
                slot.SetAmount(items[i].Amount);
                slot.SetItem(items[i].Item);
            }

            var emptySlotsAmount = AmmunitionSlots.Count - items.Length;

            if (emptySlotsAmount <= 0) return;

            for (int i = items.Length; i < AmmunitionSlots.Count; i++)
            {
                AmmunitionSlots[i].GetComponent<SlotVisual>().ClearToDefault();
            }
        }
        #endregion

        #region unused
        public void SelectSlot()
        {

        }
        public void SortIngredients()
        {

        }
        public void ShowIngrediens()
        {

        }
        public void ShowAmmunition()
        {

        }
        public void ShowPotions()
        {

        }
        public void ShowSwitchTabsButtons()
        {

        }
        #endregion

        protected override void TransferItem(SlotVisual from, int amount)
        {
            base.TransferItem(from, amount);

            UpdateInventory();
        }

        protected override void ClearSlot(SlotVisual slot)
        {
            inventory.RemoveItemStackFromInventory(slot.Slot.EquipedItem);

            base.ClearSlot(slot);
        }

        protected override void RemoveItemAmountFromSlot(SlotVisual slot, int amount)
        {
            base.RemoveItemAmountFromSlot(slot, amount);

            inventory.RemoveItemFromInventory(slot.Slot.EquipedItem, amount);
        }

        public override bool TryTransferItemToThisInventory(Item item, int amount)
        {
            if (inventory.TryAddItemToInventory(item, amount))
                return true;
            else
                return false;
        }

        public override bool CheckForTransferItemOpportunity(Item item)
        {
            return true;
        }
    }
}