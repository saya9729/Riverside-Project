using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class TimeManager : MonoBehaviour
    {
        public void ContinueTime()
        {
            var objects = FindObjectsOfType<TimeBody>();  //Find Every object with the Timebody Component
            for (var i = 0; i < objects.Length; i++)
            {
                objects[i].GetComponent<TimeBody>().ContinueTime(); //continue time in each of them
            }
        }
        public void StopTime()
        {
            var objects = FindObjectsOfType<TimeBody>();  //Find Every object with the Timebody Component
            for (var i = 0; i < objects.Length; i++)
            {
                objects[i].GetComponent<TimeBody>().StopTime(); //stop time in each of them
            }
        }
    }
}
