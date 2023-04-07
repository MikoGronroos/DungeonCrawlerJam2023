using UnityEngine;

public class PlayerAttackInteraction : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayAttackAnimation();
            var enemy = CheckForEnemy();
            if (enemy != null)
            {
                Combat.Instance.StartCombat(enemy);
            }
        }
        
        
    }

    [ContextMenu("AttackAnimation")]
    private void PlayAttackAnimation()
    {
        if (animator.enabled)
        {
            animator.Play("ritual knife hit render");
        }
    }

    private IParticipant CheckForEnemy()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1, enemyLayer))
        {
            if (hit.collider)
            {
                return hit.collider.GetComponent<IParticipant>();
            }
        }

        return null;
    }
    
    
}
