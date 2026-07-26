using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation
{
    float rotation = 0;

    float limit = 80;
    public void update(Transform transform, float speedX, float speedY)
    {
        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            Debug.Log("mouse in CameraRotation update is null");
            return;
        }
        if (transform == null)
        {
            Debug.Log("transform in CameraRotation update is null");
            return;
        }

        Vector2 mouseDelta = mouse.delta.ReadValue();

        Transform parent = transform.parent;

        float resultX = mouseDelta.x * speedX * Time.deltaTime;

        float resultY = Mathf.Clamp(-mouseDelta.y * speedY * Time.deltaTime, -limit, limit);

        if (parent)
        {
            parent.Rotate(0, resultX, 0, Space.World);
        }

        rotation = Mathf.Clamp(rotation + resultY, -80f, 80f);

        transform.localRotation = Quaternion.Euler(rotation, 0, 0);
    }
}
