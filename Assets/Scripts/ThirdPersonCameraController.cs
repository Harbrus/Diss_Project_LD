using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{

    private float rotY = 0.0f;
    private float rotX = 0.0f;
    private Vector3 followPos;

    [SerializeField] GameObject targetObj, playerObj;
    [SerializeField] Transform obstruction;
    [SerializeField] GameObject cameraObj;
    [SerializeField] float cameraSpeed = 120.0f;
    [SerializeField] float clampAngleMin = -15;
    [SerializeField] float clampAngleMax = 40;
    [SerializeField] float inputSensitivity = 150.0f;
    [SerializeField] float camDistanceXToPlayer;
    [SerializeField] float camDistanceYToPlayer;
    [SerializeField] float camDistanceZToPlayer;
    [SerializeField] float mouseX;
    [SerializeField] float mouseY;
    [SerializeField] float finalInputX;
    [SerializeField] float finalInputZ;
    [SerializeField] float smoothSpeed; // if lerp used



    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        obstruction = targetObj.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CameraControl();
    }

    void LateUpdate()
    {
        CameraUpdater();
    }

    private void CameraControl()
    {
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, clampAngleMin, clampAngleMax);

        playerObj.transform.rotation = Quaternion.Euler(0, rotY, 0);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0); // consider lerp to do it slower.

    }

    private void CameraUpdater()
    {
        // set the target object to follow
        Transform target = targetObj.transform;

        //move towards the game object that is the target
        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
