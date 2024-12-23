using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    // ���������� ��� �������� ���������� ��������� ���������
    private int itemCount = 0;

    // �����, ������� ����� ���������� ��� ������������ � ���������
    private void OnTriggerEnter(Collider other)
    {
        // ���������, �������� �� ������, � ������� �� �����������, ���������
        if (other.CompareTag("Key"))
        {
            // ����������� ���������� ��������� ���������
            itemCount++;

            // ������� ���������� ��������� ��������� � �������
            Debug.Log("������� ���������: " + itemCount);

            // ������� ������� �� �����
            Destroy(other.gameObject);
        }
    }
}
