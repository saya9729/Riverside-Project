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
            //_boxCollider.enabled = false;
            _rigidbody.useGravity = false;
            //_rigidbody.freezeRotation = true;
            //need to change how this work
            //_transform.parent = GameObject.Find("PlayerCapsule").GetComponent<Transform>();
        }

        public void UpdateObjectPosition()
        {
            Vector3 nextPosision = Camera.main.transform.position + _playerStateManager.selectionManager.currentPlayerAim.direction * _playerStateManager.playerDragState.distanceFromPlayerToObject;
            Vector3 currPosision = _transform.position;

            _rigidbody.velocity = (nextPosision - currPosision) * _playerStateManager.playerDragState.catchUpVelocity;
        }

        public void StopDragObject()
        {
            //_boxCollider.enabled = true;            
            _rigidbody.useGravity = true;
            //_rigidbody.freezeRotation = false;
            //_transform.parent = _originalParent;
        }

        public void StartInspectObject()
        {
            _gameObjectInstant = Instantiate(inspectObjectModel, Camera.main.transform.position + _playerStateManager.selectionManager.currentPlayerAim.direction * _playerStateManager.playerInspectState.distanceFromPlayerToObject,new Quaternion());
        }
        public void RotateInspectingObject()
        {
            _gameObjectInstant.transform.Rotate(Vector3.up, _playerStateManager.inputManager.mouseScrollDelta * _playerStateManager.playerInspectState.rotateAngle);
        }
        public void StopInspectObject()
        {
            Destroy(_gameObjectInstant, _playerStateManager.playerInspectState.delayTimeUntilDestroyObject);
        }
    }
}
