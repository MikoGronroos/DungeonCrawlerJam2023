using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class UIEventChannel : ScriptableObject
{
    public UnityEvent<UIState> onUIStateChanged;
    public void ChangeState(int newState)
    {
        onUIStateChanged?.Invoke((UIState)newState);
    }
}
