using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioEventChannel audioEventChannel;
    public UIEventChannel UIEventChannel;
    public UIState initialUIState;

    [SerializeField] private List<Altar> altars;
    private int altarsCompletedCount = 0;
    
    private void Start()
    {
        BootGame();
    }

    private void OnEnable()
    {
        foreach (var altar in altars)
        {
            altar.OnBloodSacrificed.AddListener(OnBloodSacrificed);
        }
    }

    private void OnDisable()
    {
        foreach (var altar in altars)
        {
            altar.OnBloodSacrificed.RemoveAllListeners();
        }
    }

    private void BootGame()
    {
        altarsCompletedCount = 0;
        UIEventChannel.onUIStateChanged?.Invoke(initialUIState);
        audioEventChannel.ToggleSFX();
        audioEventChannel.ToggleMusic();
    }
    
    private void OnBloodSacrificed()
    {
        altarsCompletedCount++;

        if (altarsCompletedCount >= altars.Count)
        {
            // WIN
            Debug.Log("Win");
            UIEventChannel.ChangeState((int)UIState.VICTORY);
        }
    }
}
