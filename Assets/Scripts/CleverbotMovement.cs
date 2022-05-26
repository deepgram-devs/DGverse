using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleverbotMovement : MonoBehaviour
{
    float rotationSpeedX = 100.0f;
    float rotationSpeedY = 100.0f;
    float rotationSpeedZ = 0.0f;

    bool increasingX = false;
    bool increasingY = false;
    bool increasingZ = true;

    float rotationSpeedChangeFactor = 10.0f;

    void Start()
    {

    }

    void Update()
    {
        if (rotationSpeedX < -100.0f)
        {
            increasingX = true;
        }
        if (rotationSpeedX > 100.0f)
        {
            increasingX = false;
        }
        if (increasingX)
        {
            rotationSpeedX += Time.deltaTime * rotationSpeedChangeFactor;
        }
        else
        {
            rotationSpeedX -= Time.deltaTime * rotationSpeedChangeFactor;
        }

        if (rotationSpeedY < -100.0f)
        {
            increasingY = true;
        }
        if (rotationSpeedY > 100.0f)
        {
            increasingY = false;
        }
        if (increasingY)
        {
            rotationSpeedY += Time.deltaTime * rotationSpeedChangeFactor;
        }
        else
        {
            rotationSpeedY -= Time.deltaTime * rotationSpeedChangeFactor;
        }

        if (rotationSpeedZ < -100.0f)
        {
            increasingZ = true;
        }
        if (rotationSpeedZ > 100.0f)
        {
            increasingZ = false;
        }
        if (increasingZ)
        {
            rotationSpeedZ += Time.deltaTime * rotationSpeedChangeFactor;
        }
        else
        {
            rotationSpeedZ -= Time.deltaTime * rotationSpeedChangeFactor;
        }

        transform.Rotate(Time.deltaTime * new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ));
    }
}
