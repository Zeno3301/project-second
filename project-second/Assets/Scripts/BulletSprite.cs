using UnityEngine;

public class BulletSprite : MonoBehaviour
{
    void Start()
    {
        // Создаем текстуру 8x8 пикселей
        Texture2D texture = new Texture2D(8, 8);
        texture.filterMode = FilterMode.Point; // Пиксельный режим

        // Заполняем текстуру белым цветом в форме пули
        Color[] pixels = new Color[8 * 8];
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                // Создаем круглую пулю
                float distanceFromCenter = Mathf.Sqrt(
                    Mathf.Pow(x - 3.5f, 2) + 
                    Mathf.Pow(y - 3.5f, 2)
                );
                
                if (distanceFromCenter <= 2)
                {
                    pixels[y * 8 + x] = Color.white;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();

        // Создаем спрайт из текстуры
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 8);
        
        // Применяем спрайт к SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = Color.white;
    }
} 