using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioEventChannel audioEventChannel;
    public UIEventChannel UIEventChannel;
    public UIState initialUIState;
   
    private void Start()
    {
        BootGame();
    }

    private void BootGame()
    {
        UIEventChannel.onUIStateChanged?.Invoke(initialUIState);
        audioEventChannel.ToggleSFX();
        audioEventChannel.ToggleMusic();
    }
}
