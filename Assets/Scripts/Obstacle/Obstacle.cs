using DG.Tweening;
using NaughtyAttributes;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Obstacle
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private LayerMask includeLayers;
        [SerializeField] private Collider obstacleCollider;
        [SerializeField] private VoidEvent playerHitEvent;
        [SerializeField] private bool enableCooldown = true;
        [SerializeField, ShowIf("enableCooldown")] private float cooldown = 1f;

        private void OnCollisionEnter(Collision other)
        {
            if(((1<<other.gameObject.layer) & includeLayers) != 0)
            {
                playerHitEvent.Raise();
                if (!enableCooldown) return;
                obstacleCollider.enabled = false;
                DOVirtual.DelayedCall(cooldown, () => obstacleCollider.enabled = true);
            }
        }
    }
}
