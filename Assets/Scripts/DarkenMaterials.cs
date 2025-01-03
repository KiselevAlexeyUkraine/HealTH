using UnityEngine;

public class DarkenMaterials : MonoBehaviour
{
    public float darkenFactor = 0.5f; // Фактор затемнения (0 - полностью черный, 1 - без изменений)

    void Start()
    {
        // Получаем все рендереры в сцене
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Проходим по всем материалам рендерера
            foreach (Material material in renderer.materials)
            {
                // Уменьшаем цвет материала
                Color color = material.color;
                color *= darkenFactor; // Уменьшаем яркость
                material.color = color;
            }
        }
    }
}