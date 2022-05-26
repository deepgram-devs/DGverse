using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3D : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    public float speedX;
    public float speedZ;

    float rotationX;
    float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        float motionX = Input.GetAxis("Horizontal") * Time.deltaTime * speedX;
        float motionZ = Input.GetAxis("Vertical") * Time.deltaTime * speedZ;

        // This translation method lets you fly freely
        //transform.Translate(new Vector3(motionX, 0.0f, motionZ));

        // This translation strictly works for motion on a 2D plane
        // It ought to be upgraded to something more proper, attaching the camera to a Rigidbody or something
        Vector3 translation = transform.forward;
        translation.y = 0.0f;
        translation.Normalize();
        Vector3 orthoTranslation = Vector3.Cross(Vector3.up, translation).normalized;
        translation = translation * motionZ + orthoTranslation * motionX;
        transform.Translate(translation, Space.World);
    }
}
