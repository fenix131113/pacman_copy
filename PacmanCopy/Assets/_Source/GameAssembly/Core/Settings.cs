using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public const string MASTER_VOLUME_KEY = "MasterVolume";
        public const string MUSIC_VOLUME_KEY = "MusicVolume";
        public const string SOUNDS_VOLUME_KEY = "SoundsVolume";

        public event Action<float> OnMasterVolumeLoaded;
        public event Action<float> OnMusicVolumeLoaded;
        public event Action<float> OnSoundsVolumeLoaded;

        private void Start() => Load();

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat(MASTER_VOLUME_KEY, Mathf.Log10(volume) * 20);
            SaveVolume(MASTER_VOLUME_KEY, volume);
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(volume) * 20);
            SaveVolume(MUSIC_VOLUME_KEY, volume);
        }

        public void SetSoundsVolume(float volume)
        {
            audioMixer.SetFloat(SOUNDS_VOLUME_KEY, Mathf.Log10(volume) * 20);
            SaveVolume(SOUNDS_VOLUME_KEY, volume);
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(MASTER_VOLUME_KEY))
            {
                audioMixer.SetFloat(MASTER_VOLUME_KEY, Mathf.Log10(PlayerPrefs.GetFloat(MASTER_VOLUME_KEY)) * 20);
                OnMasterVolumeLoaded?.Invoke(PlayerPrefs.GetFloat(MASTER_VOLUME_KEY));
            }

            if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
            {
                audioMixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY)) * 20);
                OnMusicVolumeLoaded?.Invoke(PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY));
            }

            if (PlayerPrefs.HasKey(SOUNDS_VOLUME_KEY))
            {
                audioMixer.SetFloat(SOUNDS_VOLUME_KEY, Mathf.Log10(PlayerPrefs.GetFloat(SOUNDS_VOLUME_KEY)) * 20);
                OnSoundsVolumeLoaded?.Invoke(PlayerPrefs.GetFloat(SOUNDS_VOLUME_KEY));
            }
        }

        public void SaveVolume(string key, float volume) => PlayerPrefs.SetFloat(key, volume);
    }
}