using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private Slider Master;
    [SerializeField] private Slider Music;
    [SerializeField] private Slider SFX;

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
            LoadVolume();
        else
            SaveDefaultValues();
    }

    public void SetMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", Master.value);
        UpdateCombinedMusicVolume();
        UpdateCombinedSFXVolume();
    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", Music.value);
        UpdateCombinedMusicVolume();
    }

    public void SetSFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFX.value);
        UpdateCombinedSFXVolume();
    }

    private void UpdateCombinedMusicVolume()
    {
        float v = Mathf.Clamp(Music.value * Master.value, 0.0001f, 1f);
        AudioMixer.SetFloat("Music", Mathf.Log10(v) * 20);
    }

    private void UpdateCombinedSFXVolume()
    {
        float v = Mathf.Clamp(SFX.value * Master.value, 0.0001f, 1f);
        AudioMixer.SetFloat("SFX", Mathf.Log10(v) * 20);
    }


    private void LoadVolume()
    {
        Master.value = PlayerPrefs.GetFloat("masterVolume");
        Music.value = PlayerPrefs.GetFloat("musicVolume");
        SFX.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    private void SaveDefaultValues()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }
}
