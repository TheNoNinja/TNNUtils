using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    public int currentLanguageID = 0;
    [SerializeField]
    public List<TextAsset> languageFiles = new List<TextAsset>();
    public List<Language> languages = new List<Language>();

    private void Update(){
      if (Input.GetKey (KeyCode.Space)){
        LocalizationManager.instance.ChangeLocale(LocalizationManager.instance.currentLanguageID == 1 ? 0 : 1);
      }
      if (Input.GetKey(KeyCode.E)){
        LocalizationManager.instance.ChangeLocale(1);
      }
      if (Input.GetKey(KeyCode.D)){
        LocalizationManager.instance.ChangeLocale(0);
      }
    }

    void Awake()
    {
        if(instance == null)instance = this;

        foreach (TextAsset languageFile in languageFiles)
        {
            XDocument languageXMLData = XDocument.Parse(languageFile.text);
            Language language = new Language();
            language.languageID = System.Int32.Parse(languageXMLData.Element("Language").Attribute("ID").Value);
            language.languageString = languageXMLData.Element("Language").Attribute("LANG").Value;
            foreach (XElement textx in languageXMLData.Element("Language").Elements())
            {
                language.textKeyValueList.Add(textx.Attribute("key").Value, textx.Value);
            }
            languages.Add(language);
        }
    }

    public string GetText(string key)
    {
        foreach(Language language in languages)
        {
            if (language.languageID == currentLanguageID)
            {
                foreach(KeyValuePair<string, string> kvp in language.textKeyValueList)
                {
                    if (kvp.Key == key)
                    {
                        return kvp.Value;
                    }
                }
            }
        }
        return "Undefined\nPlease report to operator";
    }

    public void ChangeLocale(string languageString)
    {
      foreach (Language language in languages){
        if (language.languageString == languageString){
          ChangeLocale(language.languageID);
          return;
        }
      }
    }

    public void ChangeLocale(int newLanguageID)
    {
      currentLanguageID = newLanguageID;
      OnLocaleChanged(new LocaleChangedEventArgs(currentLanguageID, newLanguageID));

    }

    private void OnLocaleChanged(LocaleChangedEventArgs e)
        {
            var handler = LocaleChanged;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }

    public event EventHandler<LocaleChangedEventArgs> LocaleChanged;
}

public class LocaleChangedEventArgs : EventArgs
{
    public int PreviousLanguage { get; private set; }

    public int CurrentLanguage { get; private set; }

    public LocaleChangedEventArgs(int previousLanguage, int currentLanguage)
    {
        PreviousLanguage = previousLanguage;
        CurrentLanguage = currentLanguage;
    }
}

[System.Serializable]
public class Language
{
    public string languageString;
    public int languageID;
    public Dictionary<string, string> textKeyValueList = new Dictionary<string, string>();
}
