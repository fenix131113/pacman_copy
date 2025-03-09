using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundsVolumeSlider;
        [SerializeField] private Settings settings;

        private void Awake()
        {
            settings.OnMasterVolumeLoaded += OnMasterVolumeLoaded;
            settings.OnMusicVolumeLoaded += OnMusicVolumeLoaded;
            settings.OnSoundsVolumeLoaded += OnSoundsVolumeLoaded;
        }

        private void Start()
        {
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            soundsVolumeSlider.onValueChanged.AddListener(OnSoundsVolumeChanged);
        }

        private void OnDestroy()
        {
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            soundsVolumeSlider.onValueChanged.RemoveListener(OnSoundsVolumeChanged);
            settings.OnMasterVolumeLoaded -= OnMasterVolumeLoaded;
            settings.OnMusicVolumeLoaded -= OnMusicVolumeLoaded;
            settings.OnSoundsVolumeLoaded -= OnSoundsVolumeLoaded;
        }

        private void OnMasterVolumeLoaded(float value) => masterVolumeSlider.value = value;
        private void OnMusicVolumeLoaded(float value) => musicVolumeSlider.value = value;
        private void OnSoundsVolumeLoaded(float value) => soundsVolumeSlider.value = value;

        private void OnMasterVolumeChanged(float value) => settings.SetMasterVolume(value);

        private void OnMusicVolumeChanged(float value) => settings.SetMusicVolume(value);

        private void OnSoundsVolumeChanged(float value) => settings.SetSoundsVolume(value);
    }
}