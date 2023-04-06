using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    public int x, z;

    [SerializeField] private UnityEvent onSteppedEvent;

    private void Awake()
    {
        x = (int)transform.position.x;
        z = (int)transform.position.z;
    }

    public void OnStepped()
    {
        Debug.Log("xdd");
        onSteppedEvent.Invoke();
    }

}
