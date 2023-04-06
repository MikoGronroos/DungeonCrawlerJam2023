using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIStateSwitcher : MonoBehaviour
{
    [SerializeField] private UIEventChannel uiEventChannel;
    
    public Button startGameButton;

    [Tooltip("DEBUG")]
    public UIState currentUIState;
    
    private void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClick);
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
        currentUIState = newState;
        uiEventChannel.onUIStateChanged?.Invoke(newState);
    }
    
}
