using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ObjectManipulationBody : MonoBehaviour
    {        
        private GameObject _gameObjectInstant;

        [SerializeField] private GameObject inspectObjectModel;

        private PlayerStateManager _playerStateManager;
        private BoxCollider _boxCollider;
        private Rigidbody _rigidbody;
        private Transform _transform;
        private Transform _originalParent;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
            _boxCollider = GetComponent<BoxCollider>();
            _rigidbody= GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            _originalParent = _transform.parent;
        }

        public void StartDragObject()
        {
            _boxCollider.enabled = false;
            //_rigidbody.useGravity = false;
            _transform.position = _playerStateManager.objectManipulatorStateManager.objectDragState.pickUpDestination.position;
            _transform.parent = _playerStateManager.objectManipulatorStateManager.objectDragState.destinationParent;
        }

        public void UpdateObjectPosition()
        {
            _transform.position = _playerStateManager.objectManipulatorStateManager.objectDragState.pickUpDestination.position;
        }

        public void StopDragObject()
        {
            _boxCollider.enabled = true;
            //_rigidbody.useGravity = true;
            _transform.parent = _originalParent;
        }

        public void StartInspectObject()
        {
            _gameObjectInstant = Instantiate(inspectObjectModel, _playerStateManager.objectManipulatorStateManager.objectInspectState.inspectDestination);
        }
        public void RotateInspectingObject()
        {
            _gameObjectInstant.transform.Rotate(Vector3.up, _playerStateManager.inputManager.mouseScrollDelta * _playerStateManager.objectManipulatorStateManager.objectInspectState.rotateAngle);
        }
        public void StopInspectObject()
        {
            Destroy(_gameObjectInstant, _playerStateManager.objectManipulatorStateManager.objectInspectState.delayTimeUntilDestroyObject);
        }




    }
}
