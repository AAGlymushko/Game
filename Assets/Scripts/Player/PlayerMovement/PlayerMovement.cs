using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement
{
    public void update(Transform transform, float speed)
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            Debug.Log("keyboard in PlayerMovement update is null");
            return;
        }
        if (transform == null)
        {
            Debug.Log("transform in PlayerMovement update is null");
            return;
        }

        Vector3 moveDirection = Vector3.zero;

        if (!keyboard.wKey.isPressed || !keyboard.sKey.isPressed)
        {
            if (keyboard.wKey.isPressed)
            {
                moveDirection += transform.forward;
            }
            else if (keyboard.sKey.isPressed)
            {
                moveDirection -= transform.forward;
            }
        }
        if (!keyboard.aKey.isPressed || !keyboard.dKey.isPressed)
        {
            if (keyboard.aKey.isPressed)
            {
                moveDirection -= transform.right;
            } 
            else if (keyboard.dKey.isPressed)
            {
                moveDirection += transform.right;
            }
        }

        if (moveDirection != Vector3.zero) moveDirection.Normalize();
                
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
