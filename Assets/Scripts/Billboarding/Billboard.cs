using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform target; // The target the billboard should face (e.g. player character)
    private Camera mainCamera; // The main camera in the scene

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player character by tag
        mainCamera = Camera.main; // Get the main camera
    }

    private void LateUpdate()
    {
        Vector3 targetDirection = target.position - transform.position; // Get the direction to the target
        targetDirection.y = 0; // Make sure the billboard is only rotated on the y-axis

        if (targetDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-targetDirection); // Rotate the billboard to face the opposite direction of the target
        }
    }

}