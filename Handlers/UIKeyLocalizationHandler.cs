using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyLocalizationHandler : MonoBehaviour
{
  public string key;
  private TextMeshProUGUI TMPText;

  private void Start ()
  {
    TMPText = gameObject.GetComponent<TextMeshProUGUI> ();

    if (TMPText != null) key = TMPText.text; TMPText.text = LocalizationManager.instance.GetText (key);

    LocalizationManager.instance.LocaleChanged += OnLocaleChanged;
  }

  private void OnLocaleChanged (object sender, LocaleChangedEventArgs e)
  {
    UpdateText ();
  }

  public void UpdateKey (string _key)
  {
    key = _key;
    UpdateText ();
  }

  private void UpdateText ()
  {
    if (TMPText != null) TMPText.text = LocalizationManager.instance.GetText (key);
  }
}