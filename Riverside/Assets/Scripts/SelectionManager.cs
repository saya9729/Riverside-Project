using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    //Variables
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private float selectableRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            var selection = hit.transform;
            if (Vector3.Distance(selection.position,Camera.main.transform.position) < selectableRadius && selection.CompareTag(selectableTag))
            {
                //do something
                Debug.Log("Selectable");
            }
        }
    }
}
