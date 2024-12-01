using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class WindowGroup : MonoBehaviour
    {
        //public bool AddPlayerInventoryTab;
        public List<WindowContainer> windowActivators;
        public int ActiveTabIndex => activeTabIndex;
        private int activeTabIndex;

        public string[] GetAllWindowTabs()
        {
            string[] tabs;

            //if (!AddPlayerInventoryTab)
                tabs = new string[windowActivators.Count];
            //else 
            //    tabs = new string[windowActivators.Count + 1];

            for (int i = 0; i < windowActivators.Count; i++)
            {
                tabs[i] = windowActivators[i].ToString();
            }

            //if (AddPlayerInventoryTab)
            //{
            //    tabs[tabs.Length - 1] = "Inventory";
            //}

            return tabs;
            // return List where items is strings 
        }

        public bool TryActivateWindowContainer(WindowContainer windowContainer)
        {
            var tabIndex = windowActivators.FindIndex(i => i == windowContainer);

            if (tabIndex >= 0)
            {
                ActivateTab(tabIndex);
                return true;
            }
            
            return false;
        }

        public void ActivateTab(int tabIndex)
        {
            windowActivators[tabIndex].Activate();
            activeTabIndex = tabIndex;
        }

        public void ActivateNextTab()
        {
            if (activeTabIndex == windowActivators.Count - 1)
                activeTabIndex = 0;
            else
                activeTabIndex++;

            ActivateTab(activeTabIndex);
        }

        public void ActivatePreviousTab()
        {
            if (activeTabIndex == 0)
                activeTabIndex = windowActivators.Count - 1;
            else
                activeTabIndex--;

            ActivateTab(activeTabIndex);
        }
    }

}