using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private float recoverRecoilSpeed;
    [SerializeField] private float maximumRecoilAmount;
    [SerializeField] private float sensitivity;

    private float currentRecoilAmount;
    private float currentVerticalMouseCompensation;

    private float horizontalRotation;
    private float verticalRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AddRecoil(float amount)
    {
        currentRecoilAmount += amount;

        if (currentRecoilAmount > maximumRecoilAmount) currentRecoilAmount = maximumRecoilAmount;
    }

    private void Update()
    {
        horizontalRotation += Input.GetAxis("Mouse X") * sensitivity;
        verticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);

        if (currentRecoilAmount > 0) UpdateRecoil();

        transform.position = cameraPosition.position;
        transform.localRotation = Quaternion.Euler(verticalRotation - currentRecoilAmount, horizontalRotation, 0);
        playerOrientation.localRotation = Quaternion.Euler(0, horizontalRotation, 0);
    }

    private void UpdateRecoil()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && currentVerticalMouseCompensation > 0.25f)
        {
            verticalRotation -= currentVerticalMouseCompensation - currentRecoilAmount;
            currentVerticalMouseCompensation = 0;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            currentVerticalMouseCompensation -= Input.GetAxis("Mouse Y") * sensitivity;
        }
        else
        {
            currentRecoilAmount = Mathf.Lerp(currentRecoilAmount, 0, Time.deltaTime * recoverRecoilSpeed);

            if (currentRecoilAmount <= 0.01f)
            {
                currentRecoilAmount = 0;
            }
        }
    }
}
