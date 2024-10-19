using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using DG.Tweening;

public sealed class MainMenu : MonoBehaviour
{
	[Header("Menu References"), Space]
	[SerializeField] private TweenableUIMaster mainMenu;
	[SerializeField] private TweenableUIMaster settingsMenu;

	[Header("Audio Mixer"), Space]
	[SerializeField] private AudioMixer mixer;

	// Private fields.
	private static bool _userSettingsLoaded;

	private void Start()
	{
		#if UNITY_EDITOR
			_userSettingsLoaded = false;
		#endif

		LoadUserSettings();
	}

	#region Callback Methods for UI.
	public async void OpenSettingsMenu()
	{
		await mainMenu.SetActive(false);
		await settingsMenu.SetActive(true);
	}

	public void StartGame()
	{
		DOTween.Clear();
		SceneLoader.Instance.LoadSceneAsync("Scenes/Game");
	}

	public void QuitGame()
	{
		Debug.Log("Quiting player...");
		Application.Quit();
	}
	#endregion

	private void LoadUserSettings()
	{
		if (!_userSettingsLoaded)
		{
			Debug.Log("Loading user settings...");

			mixer.SetFloat("masterVol", UserSettings.ToMixerDecibel(UserSettings.MasterVolume));
			mixer.SetFloat("musicVol", UserSettings.ToMixerDecibel(UserSettings.MusicVolume));
			mixer.SetFloat("soundVol", UserSettings.ToMixerDecibel(UserSettings.SoundVolume));
			mixer.SetFloat("ambienceVol", UserSettings.ToMixerDecibel(UserSettings.AmbienceVolume));

			QualitySettings.SetQualityLevel(UserSettings.QualityLevel);
			
			LocalizationSettings.InitializationOperation.WaitForCompletion();
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[UserSettings.LocaleIndex];

			_userSettingsLoaded = true;
		}
	}
}