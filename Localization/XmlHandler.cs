using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace TNNUtils.Localization
{
    public static class XmlHandler
    {
        private static TextAsset[] _languageFiles;

        public static LocalizedLanguage[] LoadLanguageFiles()
        {
            _languageFiles = Resources.LoadAll<TextAsset>("Localization");
            var localizations = new List<LocalizedLanguage>();
            
            foreach (var languageFile in _languageFiles)
            {
                var xml = XDocument.Parse(languageFile.text);
            
                if (xml.Element("Language") == null)
                {
                    Debug.LogError($"[TNNUtils.Localization] No language element in language file '{languageFile.name}'");
                    throw new InvalidLanguageFile();
                }

                if (!Enum.TryParse<Language>(xml.Element("Language").Attribute("Name")?.Value, out var language))
                {
                    Debug.LogError($"[TNNUtils.Localization] Can find language enum of value '{xml.Element("Language").Attribute("Name")?.Value}' in '{language}'");
                    throw new InvalidLanguageFile();;
                }

                var localization = new LocalizedLanguage {Language = language, LanguageFile = languageFile};

                foreach (var element in xml.Element("Language").Elements())
                {
                    var key = element.Attribute("Key")?.Value;

                    if (string.IsNullOrEmpty(key))
                    {
                        Debug.LogError($"[TNNUtils.Localization] Missing or invalid key for element '{element.Value}' in '{language}'");
                        throw new InvalidLanguageFile();;
                    }
                
                    localization.Localization.Add(key, element.Value);
                }

                localizations.Add(localization);
            }

            return localizations.ToArray();
        }
        
        #if UNITY_EDITOR

        public static void Add(Language currentLanguage, string key, string value)
        {
            foreach (var languageFile in _languageFiles)
            {
                var xml = XDocument.Parse(languageFile.text);

                if (xml.Element("Language") == null)
                {
                    Debug.LogError($"[TNNUtils.Localization] No language element in language file '{languageFile.name}'");
                    throw new InvalidLanguageFile();
                }
                
                if (!Enum.TryParse<Language>(xml.Element("Language").Attribute("Name")?.Value, out var language))
                {
                    Debug.LogError($"[TNNUtils.Localization] Can find language enum of value '{xml.Element("Language").Attribute("Name")?.Value}' in '{language}'");
                    throw new InvalidLanguageFile();
                }
                
                var element = new XElement("text");
                element.SetAttributeValue("Key", key);
                element.Value = key;
                
                if (language == currentLanguage)
                {
                    element.Value = value;
                }
                
                xml.Element("Language").Add(element);
                
                File.WriteAllText($"Assets/Resources/Localization/{languageFile.name}.xml", xml.ToString());
            }
            AssetDatabase.Refresh();
        }
        
        public static void Remove(string key)
        {
            foreach (var languageFile in _languageFiles)
            {
                var xml = XDocument.Parse(languageFile.text);

                if (xml.Element("Language") == null)
                {
                    Debug.LogError(
                        $"[TNNUtils.Localization] No language element in language file '{languageFile.name}'");
                    throw new InvalidLanguageFile();
                }

                if (!Enum.TryParse<Language>(xml.Element("Language").Attribute("Name")?.Value, out var language))
                {
                    Debug.LogError(
                        $"[TNNUtils.Localization] Can find language enum of value '{xml.Element("Language").Attribute("Name")?.Value}' in '{language}'");
                    throw new InvalidLanguageFile();
                }
                
                xml.Element("Language").Elements().Single(e => e.Attribute("Key").Value == key).Remove();
                
                File.WriteAllText($"Assets/Resources/Localization/{languageFile.name}.xml", xml.ToString());
            }
            AssetDatabase.Refresh();
        }
        
        public static void Edit(Language currentLanguage, string key, string value)
        {
            foreach (var languageFile in _languageFiles)
            {
                var xml = XDocument.Parse(languageFile.text);

                if (xml.Element("Language") == null)
                {
                    Debug.LogError(
                        $"[TNNUtils.Localization] No language element in language file '{languageFile.name}'");
                    throw new InvalidLanguageFile();
                }

                if (!Enum.TryParse<Language>(xml.Element("Language").Attribute("Name")?.Value, out var language))
                {
                    Debug.LogError(
                        $"[TNNUtils.Localization] Can find language enum of value '{xml.Element("Language").Attribute("Name")?.Value}' in '{language}'");
                    throw new InvalidLanguageFile();
                    
                }

                if (currentLanguage != language) continue;

                try
                {
                    xml.Element("Language").Elements().Single(e => e.Attribute("Key").Value == key).SetValue(value);
                }
                catch
                {
                    Debug.LogError($"[TNNUtils.Localization] Can find element with key of value '{key}' in '{language}'");
                    throw new InvalidLanguageFile();
                }
                File.WriteAllText($"Assets/Resources/Localization/{languageFile.name}.xml", xml.ToString());
            }
            AssetDatabase.Refresh();
        }
        
        #endif
    }
}