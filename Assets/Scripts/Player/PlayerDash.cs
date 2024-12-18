using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public KeyCode dashKey = KeyCode.Z;
    public float dashDistance = 4f; // ��������� �����
    public float dashDuration = 0.5f; // ����� �����
    public float dashCooldown = 1f; // ����� ����������� �����
    private bool canDash = true; // ����, �����������, ����� �� �������� ������� �����

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) && canDash) // �������� KeyCode.Space �� ������ ��� ������
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false; // ��������� ��������� �����
        Vector3 startPosition = rb.position;
        Vector3 dashDirection = transform.forward * dashDistance; // ����������� �����
        Vector3 targetPosition = startPosition + dashDirection;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, (elapsedTime / dashDuration)));
            elapsedTime += Time.deltaTime;
            yield return null; // ����� ���������� �����
        }

        rb.MovePosition(targetPosition); // ���������, ��� �������� ������ �������� �������
        yield return new WaitForSeconds(dashCooldown); // ����� ����� �����������
        canDash = true; // ��������� ��������� �����
    }
}
   