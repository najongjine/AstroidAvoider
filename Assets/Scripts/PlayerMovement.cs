using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;

    private Camera mainCamera;

    private Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();

        RotateToFaceVelocity();

    }

    void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }

        // 중력을 먹여주는 거임. 한번 중력을 먹여주면 FixedUpdate 함수가 발동 안해도 중력의 힘은 obj에 남아있기 때문
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        //Debug.Log($"force is : {movementDirection * forceMagnitude * Time.deltaTime}");

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            movementDirection = worldPosition- transform.position;
            movementDirection.z = 0f;
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = Vector3.zero;
        }

    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        transform.position = newPosition;

    }

    private void RotateToFaceVelocity()
    {
        if (rb.velocity == Vector3.zero) { return; }
        //Debug.Log($"rb.velocity : {rb.velocity}");

        /* LookRotation 앞 방향과 회전 기준을 줘야한다.
         * updirection은 position 의 y값(초록색 화살표)를 의미하는데,
         * scene 에서 현재 카메라에 비추는 기준으로는 player object 가 화면으로 가까워 지는게 back, 화면에서 멀어지는게 foward  */
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);

        transform.rotation = Quaternion.Lerp(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}
