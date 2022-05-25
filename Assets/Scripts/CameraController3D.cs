using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3D : MonoBehaviour
{
	public float sensitivityX;
	public float sensitivityY;
	
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
    }
}
