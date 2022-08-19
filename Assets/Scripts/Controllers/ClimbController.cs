using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using NaughtyAttributes;

namespace Controllers
{
    public class ClimbController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField, BoxGroup("Atom Variable")] private VoidEvent onAttachmentComplete;
        [SerializeField, BoxGroup("Atom Variable")] private FloatVariable speed;
        [SerializeField, BoxGroup("Atom Variable")] private FloatVariable movementDelay;
        [SerializeField, BoxGroup("Joints")] private FixedJoint leftHandJoint;
        [SerializeField, BoxGroup("Joints")] private FixedJoint rightHandJoint;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Ease easeType;
        #endregion

        #region Private Fields
        private readonly Vector3 _leftHandAttachmentRotation = new Vector3(0,90,-90);
        private readonly Vector3 _rightHandAttachmentRotation =  new Vector3(0,90,90);
        private bool _isLeftHandAttached;
        private bool _isFirstMovement;
        private Tweener _movementTween;
        private Tweener _rotationTween;
        
        #endregion

        #region Public Methods

        public void OnAttachmentTargetSelected(GameObject newRockGameObject)
        {
            if (_movementTween is { active: true }) return;
            Debug.Log("Attaching to " + newRockGameObject.name);
            SetNewAttachment(newRockGameObject);
        }
    
        public void DisableMovement()
        {
            _rotationTween?.Kill();
            _movementTween?.Kill();
        
            leftHandJoint.GetComponent<FixedJoint>().connectedBody = null;
            rightHandJoint.GetComponent<FixedJoint>().connectedBody = null;
        }

        #endregion

        #region Private Methods
        
        private void SetNewAttachment(GameObject movementTarget)
        {
            FixedJoint jointToRemove;
            FixedJoint jointToAttach;
            Transform targetHand;
            Vector3 attachmentRotation;
            if (_isLeftHandAttached)
            {
                jointToRemove = leftHandJoint;
                jointToAttach = rightHandJoint;
                targetHand = rightHand;
                attachmentRotation = _rightHandAttachmentRotation;
                
                _isLeftHandAttached = false;
            }else
            {
                jointToRemove = rightHandJoint;
                jointToAttach = leftHandJoint;
                targetHand = leftHand;
                attachmentRotation = _leftHandAttachmentRotation;
                
                _isLeftHandAttached = true;
            }
            
            DOVirtual.DelayedCall(_isFirstMovement ? 0 : movementDelay.Value, () =>
            {
                var jointT = jointToAttach.transform;
                jointT.rotation = targetHand.rotation;
                jointT.position = targetHand.position;
                jointToAttach.connectedBody = targetHand.GetComponent<Rigidbody>();
            
                MoveJointToTarget(jointToAttach, movementTarget, attachmentRotation);
            }).OnStart(() =>
            {
                jointToRemove.GetComponent<FixedJoint>().connectedBody = null;
                if (_isFirstMovement)
                    _isFirstMovement = false;
                
            });
        }
        
        private void MoveJointToTarget(FixedJoint jointToMove, GameObject target, Vector3 attachmentRotation)
        {
            
            _movementTween = jointToMove.transform.DOMove(target.transform.position, speed.Value).SetUpdate(UpdateType.Fixed).SetEase(easeType).SetSpeedBased(true)
                .OnComplete(() => onAttachmentComplete.Raise());

            _rotationTween = jointToMove.transform.DORotate(attachmentRotation, speed.Value)
                .SetUpdate(UpdateType.Fixed)
                .SetEase(easeType).SetSpeedBased(true);
        }
        #endregion
    }
}

