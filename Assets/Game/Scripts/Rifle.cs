using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private SoundBundle weaponSounds;
    
    [Header("Shooting")]
    [SerializeField] private CameraShake shaker;
    [SerializeField] private float cameraShake;
    [SerializeField] private string soundBundleProfileName;
    [SerializeField] private float bulletPerMinute;
    [SerializeField] private Transform positionRecoilTransform;
    [SerializeField] private Transform rotationRecoilTransform;
    [SerializeField] private Vector2 horizontalRotation;
    [SerializeField] private Vector2 verticalRotation;
    [SerializeField] private float firstShotRecoilMultiplier = 1;
    [SerializeField] private float backwardForce;
    [SerializeField] private float gunStrength;
    [SerializeField] private Transform muzzleSpot;
    [SerializeField] private Transform muzzleFlash;
    [SerializeField] private float muzzleFlashDuration;

    [Header("Running")]
    [SerializeField] private Transform runningTransform;
    [SerializeField] private Vector3 runningPoint;
    [SerializeField] private float runningAngle;
    [SerializeField] private float runningAngleSpeed;
    [SerializeField] private float runningAngleOutSpeed;
    [SerializeField] private Transform runningSineTransform;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Vector3 leftArmStartRotation;
    [SerializeField] private Vector3 leftArmRotation;
    [SerializeField] private float leftArmRotationSpeed;
    [SerializeField] private float leftArmRotationOutSpeed;
    [SerializeField] private float runningSineRotation;
    [SerializeField] private float runningSineHorizontal;
    [SerializeField] private float runningSineVertical;
    [SerializeField] private float runningSineSpeed;

    [Header("Aiming")]
    [SerializeField] private Transform aimingTransform;
    [Space(10)]
    [SerializeField] private Vector3 aimingPoint;
    [SerializeField] private Vector3 aimingEuler;
    [SerializeField] private AnimationCurve aimingInCurve;
    [SerializeField] private AnimationCurve aimingOutCurve;
    [SerializeField] private float aimingSpeed;

    private Vector3 aimingStoredPosition;
    private Vector3 aimingStoredRotation;

    private Vector3 aimingCurveStartPosition;
    private Vector3 aimingCurveStartRotation;
    private bool isAiming;

    private float sineTime;
    private float nextTimeToFire;
    private int shotBullets;

    private void Start()
    {
        aimingStoredPosition = aimingTransform.localPosition;
        aimingStoredRotation = aimingTransform.localEulerAngles;
    }

    private void Aim(bool value)
    {
        isAiming = value;
        aimingCurveStartPosition = aimingTransform.localPosition;
        aimingCurveStartRotation = aimingTransform.localEulerAngles;
        StartCoroutine(AimingInterpolation());
    }

    private IEnumerator AimingInterpolation()
    {
        float elapsedTime = 0.0f;
        Vector3 start = (isAiming) ? aimingStoredPosition : aimingPoint;
        Vector3 end = (isAiming) ? aimingPoint : aimingStoredPosition;
        Vector3 endRotation = (isAiming) ? aimingEuler : aimingStoredRotation;

        while (elapsedTime <= aimingSpeed)
        {
            float curveValue;
            float progressPercentage = start.magnitude / aimingCurveStartPosition.magnitude;

            if (aimingCurveStartPosition == Vector3.zero || progressPercentage == 0)
            {
                progressPercentage = 1;
            }

            if (isAiming)
            {
                curveValue = aimingInCurve.Evaluate(elapsedTime / aimingSpeed / progressPercentage);
            }
            else
            {
                curveValue = aimingOutCurve.Evaluate(elapsedTime / aimingSpeed / progressPercentage);
            }

            Vector3 value = Vector3.Lerp(aimingCurveStartPosition, end, curveValue);
            Vector3 valueRotation = Vector3.Lerp(aimingCurveStartRotation, endRotation, curveValue);

            elapsedTime += Time.deltaTime;
            aimingTransform.localPosition = value;
            aimingTransform.localEulerAngles = valueRotation;

            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Aim(true);
        else if (Input.GetKeyUp(KeyCode.Space)) Aim(false);

        if (Input.GetKey(KeyCode.A))
        {
            leftArm.localRotation = Quaternion.Slerp(leftArm.localRotation, Quaternion.Euler(leftArmRotation), Time.deltaTime * leftArmRotationSpeed);

            runningTransform.localRotation = Quaternion.Slerp(runningTransform.localRotation, Quaternion.Euler(runningAngle,0,0), Time.deltaTime * runningAngleSpeed);
            runningTransform.localPosition = Vector3.Lerp(runningTransform.localPosition, runningPoint, Time.deltaTime * runningAngleSpeed);

            sineTime += Time.deltaTime * runningSineSpeed;
            runningSineTransform.localPosition = new Vector3(runningSineHorizontal * Mathf.Cos(sineTime), 0, -Mathf.Abs(Mathf.Sin(sineTime) * runningSineVertical));
            runningSineTransform.localEulerAngles = new Vector3(0, Mathf.Sin(sineTime) * runningSineRotation, 0);
        }
        else
        {
            runningSineTransform.localPosition = Vector3.Lerp(runningSineTransform.localPosition, Vector3.zero, Time.deltaTime * 10);
            runningSineTransform.localRotation = Quaternion.Lerp(runningSineTransform.localRotation, Quaternion.identity, Time.deltaTime * 10);
            runningTransform.localRotation = Quaternion.Slerp(runningTransform.localRotation, Quaternion.Euler(0,0,0), Time.deltaTime * runningAngleOutSpeed);
            runningTransform.localPosition = Vector3.Lerp(runningTransform.localPosition, Vector3.zero, Time.deltaTime * runningAngleOutSpeed);
            leftArm.localRotation = Quaternion.Slerp(leftArm.localRotation, Quaternion.Euler(leftArmStartRotation), Time.deltaTime * leftArmRotationOutSpeed);
        }

        positionRecoilTransform.localPosition = Vector3.Lerp(positionRecoilTransform.localPosition, Vector3.zero, Time.deltaTime * gunStrength);
        rotationRecoilTransform.localRotation = Quaternion.Slerp(rotationRecoilTransform.localRotation, Quaternion.identity, Time.deltaTime * gunStrength);

        if (Input.GetKey(KeyCode.Mouse0) && Time.unscaledTime > nextTimeToFire)
        {
            nextTimeToFire = Time.unscaledTime + 60f / bulletPerMinute;
            positionRecoilTransform.localPosition = -positionRecoilTransform.forward * backwardForce * (shotBullets == 0 ? firstShotRecoilMultiplier : 1);

            float verticalRandomRot = Random.Range(verticalRotation.x, verticalRotation.y);
            float horizontalRandomRot = Random.Range(horizontalRotation.x, horizontalRotation.y);

            if (Random.value >= 0.5f)
            {
                horizontalRandomRot = -horizontalRandomRot;
            }

            rotationRecoilTransform.localRotation = Quaternion.Euler(-verticalRandomRot, horizontalRandomRot, 0);

            if (shotBullets == 0)
                Helper.PlaySoundBundle(weaponSounds, soundBundleProfileName, SoundBundle.SoundProfile.SoundPack.PlayMode.FirstFire);
            else
                Helper.PlaySoundBundle(weaponSounds, soundBundleProfileName);

            Instantiate(muzzleFlash, muzzleSpot.transform.position, muzzleSpot.transform.rotation);

            shaker.PlayShake(cameraShake);
            shotBullets++;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) shotBullets = 0;
    }
}
