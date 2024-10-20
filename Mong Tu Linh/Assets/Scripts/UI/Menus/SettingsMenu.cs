using UnityEngine;
using UnityEngine.Audio;

public sealed class SettingsMenu : MonoBehaviour
{
	[Header("Menu References"), Space]
	[SerializeField] private TweenableUIMaster settingsMenu;
	[SerializeField] private TweenableUIMaster mainMenu;

	[Header("Audio Mixer"), Space]
	[SerializeField] private AudioMixer mixer;

	[Header("Slider Groups"), Space]
	[SerializeField] private SliderGroup masterSlider;
	[SerializeField] private SliderGroup musicSlider;
	[SerializeField] private SliderGroup soundSlider;
	[SerializeField] private SliderGroup ambienceSlider;
	[SerializeField] private SliderGroup dialogueSpeedSlider;

	[Header("Directional Selectors"), Space]
	[SerializeField] private LanguageSelector languageSelector;

	private void OnEnable()
	{
		ReloadUI();
	}

	#region Callback Methods for UI.
	public async void OpenMainMenu()
	{
		await settingsMenu.SetActive(false);
		await mainMenu.SetActive(true);
	}

	public void SetMasterVolume(float amount)
	{
		mixer.SetFloat("masterVol", masterSlider.ValueAsMixerDecibel);

		masterSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.MasterVolume = amount;
	}
	
	public void SetMusicVolume(float amount)
	{
		mixer.SetFloat("musicVol", musicSlider.ValueAsMixerDecibel);

		musicSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.MusicVolume = amount;
	}

	public void SetSoundVolume(float amount)
	{
		mixer.SetFloat("soundVol", soundSlider.ValueAsMixerDecibel);

		soundSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.SoundVolume = amount;
	}

	public void SetAmbienceVolume(float amount)
	{
		mixer.SetFloat("ambienceVol", ambienceSlider.ValueAsMixerDecibel);

		ambienceSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.AmbienceVolume = amount;
	}

	public void SetQualityLevel(int index)
	{
		QualitySettings.SetQualityLevel(index);
		UserSettings.QualityLevel = index;
	}
	
	public void SetDialogueSpeed(float amount)
	{
		dialogueSpeedSlider.DisplayText = amount.ToString();
		UserSettings.DialogueSpeed = (int)amount;
	}

	public void ResetToDefault()
	{
		UserSettings.ResetToDefault(UserSettings.SettingSection.All);
		ReloadUI();
	}
	#endregion

	#region Utility Functions.
	private string ConvertDecibelToText(float amount)
	{
		return (amount * 100f).ToString("0");
	}
	#endregion

	private void ReloadUI()
	{
		float masterVol = UserSettings.MasterVolume;
		float musicVol = UserSettings.MusicVolume;
		float soundVol = UserSettings.SoundVolume;
		float ambienceVol = UserSettings.AmbienceVolume;
		int dialogueSpeed = UserSettings.DialogueSpeed;

		masterSlider.Value = masterVol;
		musicSlider.Value = musicVol;
		soundSlider.Value = soundVol;
		//ambienceSlider.Value = ambienceVol;
		dialogueSpeedSlider.Value = dialogueSpeed;

		masterSlider.DisplayText = ConvertDecibelToText(masterVol);
		musicSlider.DisplayText = ConvertDecibelToText(musicVol);
		soundSlider.DisplayText = ConvertDecibelToText(soundVol);
		//ambienceSlider.DisplayText = ConvertDecibelToText(ambienceVol);
		dialogueSpeedSlider.DisplayText = dialogueSpeed.ToString();

		languageSelector.Index = UserSettings.LocaleIndex;
	}
}
