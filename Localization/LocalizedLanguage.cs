using System.Collections.Generic;
using UnityEngine;

namespace TNNUtils.Localization
{
    public class LocalizedLanguage
    {
        public Language Language;
        public TextAsset LanguageFile;
        public readonly Dictionary<string, string> Localization = new();
    }
}