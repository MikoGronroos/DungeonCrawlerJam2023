using UnityEngine;

[RequireComponent(typeof(Player))]
public class AltarInteraction : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var raycastHit, 1))
        {
            var col = raycastHit.collider;

            if (col.CompareTag("Altar") && Input.GetKeyDown(KeyCode.E))
            {
                col.GetComponent<Altar>()?.SacrificeBlood(_player);
            }
        }
    }

}
