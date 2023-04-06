using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables or enables the players movement, depending on what UI panel is active
/// </summary>
public class InputStateController : MonoBehaviour
{
    [SerializeField] private UIEventChannel uiEventChannel;
    [SerializeField] private PlayerMovement playerMovement;
    
    private void OnEnable()
    {
        uiEventChannel.onUIStateChanged.AddListener(OnUIStateChanged);
    }

    private void OnDisable()
    {
        uiEventChannel.onUIStateChanged.RemoveListener(OnUIStateChanged);
    }

    private void OnUIStateChanged(UIState uiState)
    {
        EnableInput(uiState == UIState.GAME);
    }

    private void EnableInput(bool isEnabled)
    {
        if (playerMovement)
        {
            playerMovement.CanMove = isEnabled;
        }
        else
        {
            Debug.LogWarning("No player movement script assigned");
        }
    }
}
