using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera = null;

    private Vector3 moveInput = Vector3.zero;
    private float speed = 0.2f;


    private void Update()
    {
        var moveVec = new Vector3(moveInput.x * speed, moveInput.y, moveInput.z * speed);
        transform.Translate(moveVec);
    }

    public void CameraMove(InputAction.CallbackContext context)
    {
        var temp = context.ReadValue<Vector2>();
        moveInput = new Vector3(temp.x, camera.velocity.y, temp.y);
    }
}
