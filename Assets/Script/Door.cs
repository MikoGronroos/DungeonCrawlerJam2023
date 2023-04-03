using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] private bool isOpen;

    [SerializeField] private bool needsAKey;
    [SerializeField] private ItemKey neededKey;

    public void Interact()
    {
        if (needsAKey)
        {
            if (Inventory.Instance.HasItem(neededKey))
            {
                Inventory.Instance.RemoveItem(neededKey);
                isOpen = !isOpen;
            }
        }
        else
        {
            isOpen = !isOpen;
        }
        gameObject.SetActive(!isOpen);
    }
}
