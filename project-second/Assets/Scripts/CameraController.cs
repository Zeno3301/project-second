using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        // Получаем компонент камеры
        Camera cam = GetComponent<Camera>();
        
        // Устанавливаем ортографическую проекцию для 2D
        cam.orthographic = true;
        
        // Устанавливаем размер камеры, чтобы игровое поле точно соответствовало экрану
        float targetAspect = 16.0f / 9.0f; // Стандартное соотношение сторон
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        
        // Если экран шире чем 16:9
        if (scaleHeight < 1.0f)
        {
            cam.orthographicSize = 5.0f / scaleHeight;
        }
        else
        {
            cam.orthographicSize = 5.0f;
        }
        
        // Центрируем камеру
        transform.position = new Vector3(0, 0, -10);
    }
} 