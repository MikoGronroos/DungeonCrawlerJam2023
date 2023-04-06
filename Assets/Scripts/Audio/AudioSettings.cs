using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioEventChannel audioEventChannel;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float maxVolume;
    
    private bool _isMusicEnabled;
    private bool _isSFXEnabled;

    private void OnEnable()
    {
        audioEventChannel.onToggleSFX.AddListener(ToggleSFX);
        audioEventChannel.onToggleMusic.AddListener(ToggleMusic);
    }

    private void OnDisable()
    {
        audioEventChannel.onToggleSFX.RemoveListener(ToggleSFX);
        audioEventChannel.onToggleMusic.RemoveListener(ToggleMusic);
    }

    private void ToggleSFX()
    {
        _isSFXEnabled = !_isSFXEnabled;
        mixer.SetFloat("SFXVolume", _isSFXEnabled ? maxVolume : -80f);
        audioEventChannel.onSFXStateChanged?.Invoke(_isSFXEnabled);
    }
    
    private void ToggleMusic()
    {
        _isMusicEnabled = !_isMusicEnabled;
        mixer.SetFloat("BGVolume", _isMusicEnabled ? maxVolume : -80f);
        audioEventChannel.onMusicStateChanged?.Invoke(_isMusicEnabled);
    }
}