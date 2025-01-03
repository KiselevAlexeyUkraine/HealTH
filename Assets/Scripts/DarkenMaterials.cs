using UnityEngine;

public class DarkenMaterials : MonoBehaviour
{
    public float darkenFactor = 0.5f; // ������ ���������� (0 - ��������� ������, 1 - ��� ���������)

    void Start()
    {
        // �������� ��� ��������� � �����
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // �������� �� ���� ���������� ���������
            foreach (Material material in renderer.materials)
            {
                // ��������� ���� ���������
                Color color = material.color;
                color *= darkenFactor; // ��������� �������
                material.color = color;
            }
        }
    }
}