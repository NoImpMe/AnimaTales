//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;

//public class KeyManager : MonoBehaviour
//{
//    public static KeyManager Instance;
//    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
//    private string currentBindingKey = null;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        keys["Option"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Option", "Escape"));
//        keys["Attack"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackKey", "Z"));
//        keys["Skill"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SkillKey", "X"));
//        keys["Revert"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RevertKey", "C"));
//    }


//    private void StartRebinding(string keyName)
//    {
//        currentBindingKey = keyName;
//    }

//    private void OnGUI()
//    {
//        if (currentBindingKey != null)
//        {
//            Event e = Event.current;
//            if (e.isKey)
//            {
//                keys[currentBindingKey] = e.keyCode;
//                foreach (var key in keys)
//                {
//                    if (key.Value == keys[currentBindingKey])
//                    {
//                        keys[key.Key] = KeyCode.None;
//                        PlayerPrefs.SetString(key.Key, KeyCode.None.ToString());
//                        break;
//                    }
//                }
//                PlayerPrefs.SetString(currentBindingKey + "Key", e.keyCode.ToString());
//                PlayerPrefs.Save();
//                currentBindingKey = null;
//            }
//        }
//    }
//}
