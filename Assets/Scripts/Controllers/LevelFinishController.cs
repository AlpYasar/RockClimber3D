using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Controllers
{
    public class LevelFinishController : MonoBehaviour
    {
        [SerializeField] private LayerMask includeLayers;
        [SerializeField] private VoidEvent onLevelEndEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            if(((1<<other.gameObject.layer) & includeLayers) != 0)
            {
                onLevelEndEvent.Raise();
            }
        }
    }
}
