
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HSVPicker
{
    [System.Serializable]
    public class ColorPickerSetup
    {
        public enum ColorHeaderShowing
        {
            Hide,
            ShowColor,
            ShowColorCode,
            ShowAll,
        }

        [System.Serializable]
        public class UiElements
        {
            public RectTransform[] Elements;


            public void Toggle(bool active)
            {
                for (int cnt = 0; cnt < Elements.Length; cnt++)
                {
                    Elements[cnt].gameObject.SetActive(active);
                }
            }

        }

        public bool ShowRgb = true;
        public bool ShowHsv;

        public ColorHeaderShowing ShowHeader = ColorHeaderShowing.ShowAll;

        public UiElements RgbSliders;

        public UiElements ColorHeader;
        public UiElements ColorPreview;

        public string PresetColorsId = "default";
        public Color[] DefaultPresetColors;
    }
}
