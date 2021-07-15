using System;

namespace TNNUtils.Localization
{
    [Serializable]
    public class LocalizedString
    {
        public string key;

        public LocalizedString(string key)
        {
            this.key = key;
        }

        public string Value => Localization.GetLocalizedValue(key);

        public override string ToString() => Value;

        public static implicit operator LocalizedString(string key) => new (key);
    }
}