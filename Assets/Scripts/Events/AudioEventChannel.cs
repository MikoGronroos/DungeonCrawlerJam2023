using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class AudioEventChannel : ScriptableObject
{
    public UnityEvent onToggleSFX = new();
    public UnityEvent onToggleMusic = new();
    public UnityEvent<bool> onSFXStateChanged = new();
    public UnityEvent<bool> onMusicStateChanged = new();

    public void ToggleSFX()
    {
        onToggleSFX?.Invoke();
    }

    public void ToggleMusic()
    {
        onToggleMusic?.Invoke();
    }

}