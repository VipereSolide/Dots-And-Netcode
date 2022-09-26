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

    public void PlayShake(float amount, int instances, float timeBetweenInstances = 0.05f)
    {
        StartCoroutine(ShakeForInstances(amount, instances, timeBetweenInstances));
    }

    private IEnumerator ShakeForInstances(float amount, int instances, float timeBetweenInstances)
    {
        int i = 0;

        while (i < instances)
        {
            PlayShake(amount);

            yield return new WaitForSeconds(timeBetweenInstances);
            i++;
        }
    }

    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * shakeStrength);
    }
}
