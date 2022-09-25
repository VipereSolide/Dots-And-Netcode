using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] private Transform flash;
    [SerializeField] private bool randomRotation;
    [SerializeField] private float rotationMaxAngle;
    [SerializeField] private float lifetime;
    [SerializeField] private float flashLifetime;

    private void Start()
    {
        if (randomRotation) transform.Rotate(0, 0, Random.Range(-rotationMaxAngle,rotationMaxAngle));
        Destroy(gameObject, lifetime);
        Destroy(flash.gameObject, flashLifetime);
    }
}