using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    public int x, z;

    [SerializeField] private UnityEvent onSteppedEvent;
    [SerializeField] private UnityEvent onSteppedFireOnceEvent;

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
            onSteppedFireOnceEvent.Invoke();
        }
        hasBeenStepped = true;
        onSteppedEvent.Invoke();
    }

}
