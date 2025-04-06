using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private const string MasterVolumeParam = "MasterVolume";
    private const string MusicVolumeParam = "MusicVolume";
    private const string SFXVolumeParam = "SFXVolume";
    private Resolution[] resolutions;

    private void Start()
    {
        InitializeResolutionDropdown();

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.onValueChanged.AddListener(SetQualityLevel);

        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vsyncToggle.onValueChanged.AddListener(SetVSync);

        antiAliasingDropdown.value = QualitySettings.antiAliasing / 2;
        antiAliasingDropdown.onValueChanged.AddListener(SetAntiAliasing);

        masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeParam, 0.75f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeParam, 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFXVolumeParam, 0.75f);

        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void InitializeResolutionDropdown()
    {
        resolutions = Screen.resolutions
            .Where(resolution => resolution.refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
            .Distinct()
            .ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }

    public void SetVSync(bool enable)
    {
        QualitySettings.vSyncCount = enable ? 1 : 0;
    }

    public void SetResolutionScale(float scale)
    {
        ScalableBufferManager.ResizeBuffers(scale, scale);
    }

    public void SetAntiAliasing(int level)
    {
        QualitySettings.antiAliasing = level * 2;
    }

    public void SetMasterVolume(float volume)
    {
        float dB = volume > 0.001f ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat(MasterVolumeParam, dB);
        PlayerPrefs.SetFloat(MasterVolumeParam, volume);
    }

    public void SetMusicVolume(float volume)
    {
        float dB = volume > 0.001f ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat(MusicVolumeParam, dB);
        PlayerPrefs.SetFloat(MusicVolumeParam, volume);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = volume > 0.001f ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat(SFXVolumeParam, dB);
        PlayerPrefs.SetFloat(SFXVolumeParam, volume);
    }
}