using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class AudioEventChannel : ScriptableObject
{
    public UnityEvent onToggleAudio = new UnityEvent();
    public UnityEvent<bool> onAudioStateChanged = new UnityEvent<bool>();


    public void ToggleAudio()
    {
        onToggleAudio?.Invoke();
    }
}
