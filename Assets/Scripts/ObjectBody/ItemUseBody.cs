using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ItemUseBody : MonoBehaviour
    {
        void UseThisItem()
        {
            if(gameObject.tag == "drink")
            {
                Destroy(gameObject);
            }
            //do sommething based on the tag of the item. e.g. consume item with tag "drink"
        }
    }
}
