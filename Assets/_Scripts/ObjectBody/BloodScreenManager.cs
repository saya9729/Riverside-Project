using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScreenManager : MonoBehaviour
{
    //public static BloodScreenManager instance;
    private bool isPlaying = false;

    public ImageController[] imageController;

    public void Play()
    {
        if (isPlaying) 
        {
            return;
        }    
        else 
        {
            StartCoroutine(ToggleChecker());
        }

        if (imageController.Length == 0) 
        {
            return;
        }
        var index = Random.Range(0, imageController.Length);
        imageController[index].EnableThis();
    }

    IEnumerator ToggleChecker()
    {
        isPlaying = true;

        yield return new WaitForSeconds(0.5f);
        //no need to be changed in the future

        isPlaying = false;
    }
}
