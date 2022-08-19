using UnityEngine;

namespace Character
{
    /// <summary>
    /// Limits velocities of rigidbody under root.
    /// </summary>
    public class RigidbodyStabilizer : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private float maxDepenetrationVelocity;
        [SerializeField] private float maxAngularVelocity = 1;
        private Rigidbody[] rbs;
        
        private void Awake()
        {
            rbs = root.GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rbs)
            {
                rb.maxDepenetrationVelocity = maxDepenetrationVelocity;
                rb.maxAngularVelocity = maxAngularVelocity;
            }
        }
    }
}