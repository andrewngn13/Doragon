using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Doragon.Logging;
using Cysharp.Text;

namespace Doragon.UI
{
    public class TerminalReader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoDisplay;
        [SerializeField] private Transform scrollContent;
        private Dictionary<string, string> dbDict;
        private const string ingameDB = "terminal";
        private const int fontSize = 24;
        private void Start()
        {
            TextAsset ingameDBText = Resources.Load<TextAsset>("terminal");
            dbDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(ingameDBText.text);
            DLogger.Log("Initialized ingame db dictionary");
            DLogger.Log("Populating scrollview content..");
            foreach (var article in dbDict)
            {
                Button button = new GameObject().AddComponent<Button>();
                button.gameObject.AddComponent<Image>();
                TextMeshProUGUI childText = new GameObject().AddComponent<TextMeshProUGUI>();
                childText.color = Color.black;
                childText.fontSize = fontSize;
                childText.alignment = TextAlignmentOptions.Left;
                childText.SetText(article.Key);
                childText.transform.SetParent(button.transform, false);
                button.onClick.AddListener(() => LoadValueFromKeyToUI(article.Key));
                button.transform.SetParent(scrollContent, false);
            }
            DLogger.Log("Content populated");
        }
        // TODO: populate the scrollview directly with buttons and listeners?
        /// <summary>
        /// Loads a queried string of text from Dictionary dbDict with 'key' to a unity text display 'infoDisplay' 
        /// </summary>
        /// <param name="key"></param>
        public void LoadValueFromKeyToUI(string key)
        {
            string value;
            if (dbDict.TryGetValue(key, out value))
            {
                DLogger.Log(ZString.Format("Key {0} retrieved", key));
                infoDisplay.SetText(value);
            }
            else
            {
                DLogger.LogError(ZString.Format("Key '{0}' not found", key));
            }
        }
    }
}