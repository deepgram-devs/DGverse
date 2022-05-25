using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASRTriggerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void HandleASR(string message)
	{
		Debug.Log("HandleASR: " + message);
		if (message.Contains("cube"))
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Rigidbody cubeRigidbody = cube.AddComponent<Rigidbody>();
			cube.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
		}
	}
}
