using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public UIState initialUIState;
    public UIEventChannel UIEventChannel;
    private void Start()
    {
        BootGame();
    }

    private void BootGame()
    {
        UIEventChannel.onUIStateChanged?.Invoke(initialUIState);
    }
}
