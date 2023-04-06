using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class UIEventChannel : ScriptableObject
{
    public UnityEvent<UIState> onUIStateChanged;
}
