using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;

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
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            movementDirection =  worldPosition- transform.position;
            movementDirection.z = 0f;
            movementDirection.Normalize();


        }
        else
        {
            // FixedUpdate 에서 return 지점을 만들어 주기 위한 코드
            movementDirection = Vector3.zero;
        }
        Debug.Log($"movementDirection is : {movementDirection}");

    }

    void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }

        // 중력을 먹여주는 거임. 한번 중력을 먹여주면 FixedUpdate 함수가 발동 안해도 중력의 힘은 obj에 남아있기 때문
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        Debug.Log($"force is : {movementDirection * forceMagnitude * Time.deltaTime}");

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }


}
