using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public sealed class LanguageSelector : DirectionalSelector<Locale>
{
	protected override void SetDisplayText()
	{
		selectedText.text = _selected.LocaleName;
	}

	public void ChangeLanguage(int index)
	{
		if (gameObject.activeInHierarchy)
		{
			StartCoroutine(SetLanguage(index));
		}
		else
		{
			previousButton.interactable = true;
			nextButton.interactable = true;
		}
	}

	private IEnumerator SetLanguage(int index)
	{
		previousButton.interactable = false;
		nextButton.interactable = false;

		yield return LocalizationSettings.InitializationOperation.WaitForCompletion();
		LocalizationSettings.SelectedLocale = _selected;
		UserSettings.LocaleIndex = index;

		yield return new WaitForSecondsRealtime(.2f);
		
		previousButton.interactable = true;
		nextButton.interactable = true;
	}
}