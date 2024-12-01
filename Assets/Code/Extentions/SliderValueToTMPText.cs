using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Extentions
{
    public class SliderValueToTMPText : MonoBehaviour
    {
        public Slider slider;
        public TMP_Text text;

        public void SetText()
        {
            text.text = ((int)slider.value).ToString();
        }
    }
}