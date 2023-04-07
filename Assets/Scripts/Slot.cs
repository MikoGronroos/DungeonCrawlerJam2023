using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    [SerializeField] private Image slotImage;

    public void Setup(Sprite icon)
    {
        slotImage.sprite = icon;

        slotImage.color = icon == null ? Color.clear : Color.white;
    }

}