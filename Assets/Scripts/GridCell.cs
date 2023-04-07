using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    

    public int x, z;

    [SerializeField] private AudioSource pickupAudioSource;
    [SerializeField] private UnityEvent onSteppedEvent;
    [SerializeField] private UnityEvent onSteppedFireOnceEvent;
    [SerializeField] private UnityEvent onSteppedPickupEvent;
    [SerializeField] private Item gridCellItem;
    [SerializeField] private GameObject gridCellItemGameObject;
    private GameObject cloneItem;

    private bool hasBeenStepped = false;

    private void Awake()
    {
        x = (int)transform.position.x;
        z = (int)transform.position.z;
        if (gridCellItemGameObject)
        {
            cloneItem = (GameObject)Instantiate(gridCellItemGameObject, transform.position + Vector3.up / 2, Quaternion.identity);
        }
    }


    public void OnStepped()
    {
        if (!hasBeenStepped)
        {
            onSteppedFireOnceEvent?.Invoke();
        }

        if (gridCellItem || gridCellItemGameObject)
        {
            if (Inventory.Instance.TryToAddItem(gridCellItem, this))
            {
                pickupAudioSource.Play();
                ClearCell();
                onSteppedPickupEvent?.Invoke();
            }
        }

        hasBeenStepped = true;
        onSteppedEvent?.Invoke();
    }

    public void ClearCell()
    {
        Destroy(cloneItem);
        gridCellItem = null;
        gridCellItemGameObject = null;
        
    }

}
