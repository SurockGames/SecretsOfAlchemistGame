using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Assets.Code.Items
{
    public class PromptButtonHint : MonoBehaviour
    {
        public TMP_Text promptText;
        public TMP_Text promptKey;

        [Button]
        public void SetPromptHint(string key, string text)
        {
            promptText.text = text;
            promptKey.text = key;
        }
    }
}