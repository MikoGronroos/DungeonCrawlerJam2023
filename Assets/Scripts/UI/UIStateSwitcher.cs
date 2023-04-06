using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIStateSwitcher : MonoBehaviour
{
    public UnityEvent<UIState> onUIStateChanged;

    public UIState initialState;
    
    public Button startGameButton;

    private void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClick);
    }

    private void Start()
    {
        ChangeUIState(initialState);
    }
    
    private void OnDisable()
    {
        startGameButton.onClick.RemoveListener(OnStartGameButtonClick);
    }

    private void OnStartGameButtonClick()
    {
        ChangeUIState(UIState.GAME);
    }

    private void ChangeUIState(UIState newState)
    {
        onUIStateChanged?.Invoke(newState);
    }
    
}
