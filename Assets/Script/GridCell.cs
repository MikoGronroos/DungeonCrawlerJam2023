using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    public int x, z;

    [SerializeField] private UnityEvent onSteppedEvent;
    [SerializeField] private UnityEvent onSteppedFireOnceEvent;
    [SerializeField] private UnityEvent onSteppedPickupEvent;
    [SerializeField] private Item gridCellItem;

    private bool hasBeenStepped = false;

    private void Awake()
    {
        x = (int)transform.position.x;
        z = (int)transform.position.z;
    }

    public void OnStepped()
    {
        if (!hasBeenStepped)
        {
            onSteppedFireOnceEvent?.Invoke();
            if (Inventory.Instance.TryToAddItem(gridCellItem))
            {
                onSteppedPickupEvent?.Invoke();
            }
        }
        hasBeenStepped = true;
        onSteppedEvent?.Invoke();
    }

}
