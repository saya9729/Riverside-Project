using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Recoil_Transform")]
    public Transform recoilPositionTranform;
    public Transform recoilRotationTranform;
    [Space(10)]
    [Header("Recoil_Settings")]
    public float positionDampTime;
    public float rotationDampTime;
    [Space(10)]
    public float recoil1;
    public float recoil2;
    public float recoil3;
    public float recoil4;
    [Space(10)]
    public Vector3 recoilRotation;
    public Vector3 recoilKickBack;

    public Vector3 recoilRotationAim;
    public Vector3 recoilKickBackAim;
    [Space(10)]
    public Vector3 currentRecoil1;
    public Vector3 currentRecoil2;
    public Vector3 currentRecoil3;
    public Vector3 currentRecoil4;
    [Space(10)]
    public Vector3 rotationOutput;

    public bool aim;

void FixedUpdate()
    {
        currentRecoil1 = Vector3.Lerp(currentRecoil1, Vector3.zero, recoil1 * Time.deltaTime);
        currentRecoil2 = Vector3.Lerp(currentRecoil2, currentRecoil1, recoil2 * Time.deltaTime);
        currentRecoil3 = Vector3.Lerp(currentRecoil3, Vector3.zero, recoil3 * Time.deltaTime);
        currentRecoil4 = Vector3.Lerp(currentRecoil4, currentRecoil3, recoil4 * Time.deltaTime);

        recoilPositionTranform.localPosition = Vector3.Slerp(recoilPositionTranform.localPosition, currentRecoil3, positionDampTime * Time.fixedDeltaTime);
        rotationOutput = Vector3.Slerp(rotationOutput, currentRecoil1, rotationDampTime * Time.fixedDeltaTime);
        recoilRotationTranform.localRotation = Quaternion.Euler(rotationOutput);
    }
    public void Fire()
    {
        if (aim == true)
        {
            currentRecoil1 += new Vector3(recoilRotationAim.x, Random.Range(-recoilRotationAim.y, recoilRotationAim.y), Random.Range(-recoilRotationAim.z, recoilRotationAim.z));
            currentRecoil3 += new Vector3(Random.Range(-recoilKickBackAim.x, recoilKickBackAim.x), Random.Range(-recoilKickBackAim.y, recoilKickBackAim.y), recoilKickBackAim.z);
        }
        if (aim == false)
        {
            currentRecoil1 += new Vector3(recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
            currentRecoil3 += new Vector3(Random.Range(-recoilKickBack.x, recoilKickBack.x), Random.Range(-recoilKickBack.y, recoilKickBack.y), recoilKickBack.z);
        }
    }
}