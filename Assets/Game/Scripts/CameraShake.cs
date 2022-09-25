using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeStrength;

    public void PlayShake(float amount)
    {
        Vector3 shake = Random.insideUnitSphere * amount;
        transform.localRotation = Quaternion.Euler(shake);
    }

    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * shakeStrength);
    }
}
