using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Assets.Code.Items
{
    public class ItemInfoPopup : MonoBehaviour
    {
        public TMP_Text Header;
        public TMP_Text Text;

        [Button]
        public void SetInfo(string header, string text)
        {
            Header.text = header;
            Text.text = text;
        }
    }
}