using Assets.Code.Items;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class WindowsService : MonoBehaviour, IService
    {
        [Title("References")]
        [SerializeField] private Game game;
        [SerializeField] private PlayerInventoryWindow playerInventoryWindow;

        [Title("UI panels")]
        [SerializeField] private List<GameObject> uiWindowPanels;

        [Title("Tabs")]
        [SerializeField] private RectTransform tabsGroup;
        [SerializeField] private HighlightableText uiTabName;
        [SerializeField] private Button leftTabButton;
        [SerializeField] private Button rightTabButton;

        [Title("Item selection")]
        [SerializeField] private GameObject sliderGroup;
        [SerializeField] private Button sliderOkButton;
        [SerializeField] private Button sliderCancelButton;
        [SerializeField] private TMP_Text minimumNumberText;
        [SerializeField] private TMP_Text maximumNumberText;
        [SerializeField] private TMP_Text selectedNumberText;
        [SerializeField] private Slider selectionSlider;

        public KeyCode MoveWholeStackKey => KeyCode.LeftShift;
        public event Action OnWindowOpen;
        public event Action OnWindowClose;
        public event Action<int> OnItemAmountSelectConfirm;
        public event Action OnItemAmountSelectCancel;

        private HighlightableText[] uiTabNames = new HighlightableText[0];
        private List<Window> activeWindows = new List<Window>();
        private Window activeLeftWindow;
        private Window activeRightWindow;
        private WindowGroup activeWindowGroup;

        private bool isUIWindowActive;
        private int isTabButtonsActive;

        private bool isItemSelectionAmountActive;

        private Slot selectedSlot;
        private Slot hoveredSlot;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            //ServiceLocator.Instance.RegisterService(this);
            leftTabButton.gameObject.SetActive(false);
            rightTabButton.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            if (!isUIWindowActive) return;

            if (isTabButtonsActive > 1)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    OnLeftTabButtonClicked();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OnRightTabButtonClicked();
                }
            }

            if (isItemSelectionAmountActive)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ConfirmItemAmountSelection();
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    CancelItemAmountSelection();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                game.CloseUIWindow();
                //CloseAllWindows();
            }

            if (isTabButtonsActive == 1)
                isTabButtonsActive++;
        }

        public void OpenPlayerInventory()
        {
            ActivatePlayerInventoryAsLeftWindow(true, true, true);
        }

        public void OpenUIWindow()
        {
            if (isUIWindowActive) return;

            game.OpenUIWindow();
            isUIWindowActive = true;
            OnWindowOpen?.Invoke();

            foreach (GameObject go in uiWindowPanels)
            {
                go.SetActive(true);
            }
        }


        public bool CanTransferItemToOtherWindow(Item item, Window from)
        {
            if (item == null) return false;

            if (activeLeftWindow == from)
            {
                // Check if can transfer item to right window
                if (activeRightWindow.CheckForTransferItemOpportunity(item))
                    return true;
            }
            else if (activeRightWindow == from)
            {
                // Check if can transfer item to left window
                if (activeLeftWindow.CheckForTransferItemOpportunity(item))
                    return true;
            }
            else if (activeWindows.Contains(from))
            {
                foreach (var window in activeWindows)
                {
                    if (window == from) continue;

                    if (window.CheckForTransferItemOpportunity(item))
                        return true;
                }
            }

            return false;
        }

        public bool TransferItemToOtherWindow(Item item, int amount, Window from)
        {
            if (item == null) return false;

            if (activeLeftWindow == from)
            {
                // Try transfer item to right window
                if (activeRightWindow.TryTransferItemToThisInventory(item, amount))
                    return true;
            }
            else if (activeRightWindow == from)
            {
                // Try transfer item to left window
                if (activeLeftWindow.TryTransferItemToThisInventory(item, amount))
                    return true;
            }
            else if (activeWindows.Contains(from))
            {
                foreach (var window in activeWindows)
                {
                    if (window == from) continue;
                    
                    if (window.TryTransferItemToThisInventory(item, amount))
                        return true;
                }
            }

            return false;
        }

        public void SetItemTransferHint(bool state)
        {
            // Set button hint 
        }

        public void ShowItemSelectAmountSlider(int maxAmount, Action cancelSelection, Action<int> confirmSelection)
        {
            isItemSelectionAmountActive = true;
            sliderGroup.SetActive(true);
            selectionSlider.maxValue = maxAmount;
            selectionSlider.minValue = 1;
            selectionSlider.value = 1;
            selectedNumberText.text = ((int)selectionSlider.value).ToString();
            minimumNumberText.text = "1";
            maximumNumberText.text = maxAmount.ToString();

            sliderOkButton.onClick.AddListener(ConfirmItemAmountSelection);
            sliderCancelButton.onClick.AddListener(CancelItemAmountSelection);

            OnItemAmountSelectCancel += cancelSelection;
            OnItemAmountSelectConfirm += confirmSelection;
        }

        public void HideItemSelectAmountSlider()
        {
            isItemSelectionAmountActive = false;
            sliderOkButton.onClick.RemoveListener(ConfirmItemAmountSelection);
            sliderCancelButton.onClick.RemoveListener(CancelItemAmountSelection);

            OnItemAmountSelectCancel = null;
            OnItemAmountSelectConfirm = null;

            sliderGroup.SetActive(false);
        }

        public void ConfirmItemAmountSelection()
        {
            OnItemAmountSelectConfirm?.Invoke((int)selectionSlider.value);
            HideItemSelectAmountSlider();
        }

        public void CancelItemAmountSelection()
        {
            OnItemAmountSelectCancel?.Invoke();
            HideItemSelectAmountSlider();
        }

        #region Tabs
        public void ActivateTabsGroup(WindowGroup windowGroup)
        {
            activeWindowGroup = windowGroup;
            var tabs = windowGroup.GetAllWindowTabs();

            SetTabs(tabs);
            SetActiveTabName();

            if (tabs.Length > 1)
            {
                ActivateTabButtons();
            }
        }

        public void ActivateTabButtons()
        {
            leftTabButton.gameObject.SetActive(true);
            rightTabButton.gameObject.SetActive(true);

            leftTabButton.onClick.AddListener(OnLeftTabButtonClicked);
            rightTabButton.onClick.AddListener(OnRightTabButtonClicked);

            isTabButtonsActive = 1;
        }

        public void DeactivateTabButtons()
        {
            leftTabButton.gameObject.SetActive(false);
            rightTabButton.gameObject.SetActive(false);

            leftTabButton.onClick.RemoveListener(OnLeftTabButtonClicked);
            rightTabButton.onClick.RemoveListener(OnRightTabButtonClicked);

            isTabButtonsActive = 0;
        }

        public void OnLeftTabButtonClicked()
        {
            CancelItemAmountSelection();
            activeWindowGroup.ActivatePreviousTab();

            SetActiveTabName();
        }

        public void OnRightTabButtonClicked()
        {
            CancelItemAmountSelection();
            activeWindowGroup.ActivateNextTab();

            SetActiveTabName();
        }

        private void SetActiveTabName()
        {
            foreach(var tab in uiTabNames)
            {
                tab.SetActive(false);
            }

            uiTabNames[activeWindowGroup.ActiveTabIndex].SetActive(true);
        }

        private void SetTabs(string[] tabs)
        {
            ClearTabGroup();

            uiTabNames = new HighlightableText[tabs.Length];

            for (int i = 0; i < tabs.Length; i++)
            {
                uiTabNames[i] = Instantiate(uiTabName, tabsGroup);
                uiTabNames[i].SetText(tabs[i]);
            }

            //tabsGroup.GetComponent<LayoutGroup>().CalculateLayoutInputHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(tabsGroup);
        }

        private void ClearTabGroup()
        {
            if (uiTabNames.Length > 0)
            {
                foreach (var tabName in uiTabNames)
                {
                    if (tabName != null)
                        Destroy(tabName.gameObject);
                }
            }

            if (isTabButtonsActive > 0)
            {
                DeactivateTabButtons();
            }
        }
        #endregion

        public void ActivatePlayerInventoryAsLeftWindow(bool ingredientsTab, bool potionsTab, bool ammunitionTab)
        {
            playerInventoryWindow.SetWindow(true);
            playerInventoryWindow.ActivateInventoryTabs(ingredientsTab, potionsTab, ammunitionTab);

            activeLeftWindow = playerInventoryWindow;

            OpenUIWindow();
        }

        public void ActivateLeftWindow(Window window)
        {
            OpenUIWindow();

            activeLeftWindow = window;
            activeLeftWindow.SetWindow(true);
        }

        public void ActivateRightWindow(Window window)
        {
            if (activeRightWindow != null)
            {
                activeRightWindow.SetWindow(false);
            }

            activeRightWindow = window;

            if (activeRightWindow != null)
                activeRightWindow.SetWindow(true);

            OpenUIWindow();
        }

        public void ActivateWindow(Window window)
        {
            if (window == null) return;
            if (activeWindows.Contains(window)) return;

            activeWindows.Add(window);
            window.SetWindow(true);

            OpenUIWindow();
        }

        [Button]
        public void CloseAllWindows()
        {
            if (activeLeftWindow)
                activeLeftWindow.SetWindow(false);
            if (activeRightWindow)
                activeRightWindow.SetWindow(false);

            if (activeWindows.Count > 0)
            {
                foreach (var window in activeWindows)
                {
                    window.SetWindow(false);
                }
            }

            activeLeftWindow = null;
            activeRightWindow = null;
            activeWindowGroup = null;
            activeWindows.Clear();

            ClearTabGroup();

            foreach (GameObject go in uiWindowPanels)
            {
                go.SetActive(false);
            }

            CancelItemAmountSelection();

            isUIWindowActive = false;
            OnWindowClose?.Invoke();
        }


        public void OnDestroy()
        {
            //ServiceLocator.Instance.UnregisterService(this);
        }
    }
}
