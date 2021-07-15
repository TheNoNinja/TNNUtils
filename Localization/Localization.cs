using System;
using System.Linq;
using UnityEngine;

namespace  TNNUtils.Localization
{
    [ExecuteAlways]
    public class Localization : MonoBehaviour
    {
        #region Properties
        
        private static Language _currentLanguage = Language.English;
        private static LocalizedLanguage[] _localizations;

        #endregion

        #region Fields

        public static Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                var previousLanguage = _currentLanguage;
                _currentLanguage = value;
                UpdateLocalization();
                OnLanguageChangeTrigger(previousLanguage, _currentLanguage);
            }
        }

        public static LocalizedLanguage CurrentLocalizedLanguage
        {
            get
            {
                if (_localizations == null || _localizations.Length == 0) UpdateLocalization();
                
                return _localizations.Single(l => l.Language == CurrentLanguage);
            }
        }

        public static bool IsLanguageLoaded(Language language)
        {
            if (_localizations == null || _localizations.Length == 0) UpdateLocalization();
            
            return _localizations.Count(l => l.Language == language) > 0;
        }

        #endregion

        #region UnityMethods

        private void Awake()
        {
            UpdateLocalization();
        }

        #endregion

        #region Methods

        public static void UpdateLocalization()
        {
            _localizations = XmlHandler.LoadLanguageFiles();
        }

        public static string GetLocalizedValue(string key)
        {
            if (_localizations == null) UpdateLocalization();

            try
            {
                return _localizations.Single(l => l.Language == CurrentLanguage).Localization
                    .Single(kvp => kvp.Key == key).Value;
            }
            catch
            {
                return key;
            }
        }
        
        #if UNITY_EDITOR

        public static void Add(string key, string value)
        {
            XmlHandler.Add(CurrentLanguage, key, value);
            UpdateLocalization();
        }

        public static void Remove(string key)
        {
            XmlHandler.Remove(key);
            UpdateLocalization();
        }
        
        public static void Edit(string key, string value)
        {
            XmlHandler.Edit(CurrentLanguage, key, value);
            UpdateLocalization();
        }
        
        #endif

        #endregion

        #region LanguageChangeEvent

        public class LanguageChangeEventArgs : EventArgs
        {
            public Language PreviousLanguage { get; }
            public Language NewCurrentLanguage { get; }

            public LanguageChangeEventArgs(Language previousLanguage, Language newCurrentLanguage)
            {
                PreviousLanguage = previousLanguage;
                NewCurrentLanguage = newCurrentLanguage;
            }
        }

        private static void OnLanguageChangeTrigger(Language previousLanguage, Language newCurrentLanguage)
        {
            OnLanguageChange?.Invoke(null, new LanguageChangeEventArgs(previousLanguage, newCurrentLanguage));
        }
        
        public static event EventHandler<LanguageChangeEventArgs> OnLanguageChange;

        #endregion
        
    }
}
