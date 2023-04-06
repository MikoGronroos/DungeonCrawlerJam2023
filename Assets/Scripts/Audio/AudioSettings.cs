using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioEventChannel audioEventChannel;
    [SerializeField] private AudioMixerGroup master;
    [SerializeField] private float maxVolume;

    private bool _isAudioEnabled;
    private void OnEnable()
    {
        audioEventChannel.onToggleAudio.AddListener(ToggleSound);
    }

    private void OnDisable()
    {
        audioEventChannel.onToggleAudio.RemoveListener(ToggleSound);
    }

    private void Start()
    {
        ToggleSound();
    }

    public void ToggleSound()
    {
        _isAudioEnabled = !_isAudioEnabled;
        audioEventChannel.onAudioStateChanged?.Invoke(_isAudioEnabled);

        master.audioMixer.SetFloat("Volume", _isAudioEnabled ? maxVolume : -80f);

    }

}
