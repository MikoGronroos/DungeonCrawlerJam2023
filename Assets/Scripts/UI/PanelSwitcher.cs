using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [Tooltip("Canvas objects that can be disabled when switching to a new canvas")]
    [SerializeField] private List<UIPanel> panels;
     
    public void SwitchToPanel(UIState uiState)
    {
        DisableAllPanels();
        EnablePanel(panels.FirstOrDefault(x => x.panelIsActiveOnUIState == uiState));
    }

    private static void EnablePanel(Component targetPanel)
    {
        if (targetPanel != default && !targetPanel.gameObject.activeSelf)
        {
            targetPanel.gameObject.SetActive(true);
        }
    }

    private void DisableAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
    }
}
