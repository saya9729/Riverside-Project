using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    private Image img;

    private bool isDisplaying = false;

    [SerializeField] private float alphaDecreaseSpeed = 0.4f;
    [SerializeField] private float defaultAlpha = 0.9f;

    void Start()
    {
        img = GetComponent<Image>();

        img.enabled = false;
        var tempColor1 = img.color;
        tempColor1.a = defaultAlpha;
        img.color = tempColor1;
    }

    void Update()
    {
        if (!isDisplaying) return;

        var tempColor2 = img.color;

        if (img.color.a > 0.05f)
        {
            tempColor2.a -= Time.deltaTime * alphaDecreaseSpeed;
        }
        else
        {
            tempColor2.a = defaultAlpha;
            isDisplaying = false;
            img.enabled = false;
        }

        img.color = tempColor2;
    }

    public bool IsDisplaying()
    {
        return isDisplaying;
    }

    public void EnableThis()
    {
        isDisplaying = true;
        img.enabled = true;
    }

    public void DisableThis()
    {
        isDisplaying = false;
        img.enabled = false;
    }
}
