using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ChasingAI devil;
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
        
        UIEventChannel.onUIStateChanged.AddListener(OnUIStateChanged);
    }

    private void OnUIStateChanged(UIState uiState)
    {
        if (uiState == UIState.GAME)
        {
            devil._canRunAI = true;
        }
    }

    private void OnDisable()
    {
        foreach (var altar in altars)
        {
            altar.OnBloodSacrificed.RemoveAllListeners();
        }
        UIEventChannel.onUIStateChanged.RemoveListener(OnUIStateChanged);
    }

    private void BootGame()
    {
        altarsCompletedCount = 0;
        UIEventChannel.onUIStateChanged?.Invoke(initialUIState);
        audioEventChannel.ToggleSFX();
        audioEventChannel.ToggleMusic();
        devil._canRunAI = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseGame()
    {
        UIEventChannel.ChangeState((int)UIState.LOSE);
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
