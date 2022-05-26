using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraControllerVR : MonoBehaviour
{
    public XRNode inputSource;
    Vector2 inputAxis;

    CharacterController character;
    Camera cam;

    void Start()
    {
        character = GetComponent<CharacterController>();
        cam = GetComponentsInChildren<Camera>()[0];
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        Quaternion headYaw = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime);
    }
}
