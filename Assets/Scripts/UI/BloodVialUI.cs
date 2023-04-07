using TMPro;
using UnityEngine.UI;

public class BloodVialUI : StatsUI
{
    public TextMeshProUGUI hpAmountUGUI;
    public Image bloodImage;

    public override void UpdateUI(int currentHP, int maxHP)
    {
        hpAmountUGUI.text = currentHP.ToString();
        bloodImage.fillAmount = (float)currentHP / maxHP;
    }
    
}
