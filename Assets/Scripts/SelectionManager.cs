using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SelectionManager : MonoBehaviour
    {
        //Variables
        [SerializeField] private string selectableTag = "Selectable";
        [SerializeField] private float selectableRadius = 20.0f;

        public GameObject GetObjectAtScreenCenter()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform.gameObject;
                if (hit.distance < selectableRadius && selection.CompareTag(selectableTag))
                {
                    return selection;
                }
            }
            return null;
        }
    }
}