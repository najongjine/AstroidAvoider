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
            // FixedUpdate ���� return ������ ����� �ֱ� ���� �ڵ�
            movementDirection = Vector3.zero;
        }
        Debug.Log($"movementDirection is : {movementDirection}");

    }

    void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }

        // �߷��� �Կ��ִ� ����. �ѹ� �߷��� �Կ��ָ� FixedUpdate �Լ��� �ߵ� ���ص� �߷��� ���� obj�� �����ֱ� ����
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        Debug.Log($"force is : {movementDirection * forceMagnitude * Time.deltaTime}");

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }


}
