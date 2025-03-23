using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    void Start()
    {
        // Создаем текстуру 32x32 пикселя
        Texture2D texture = new Texture2D(32, 32);
        texture.filterMode = FilterMode.Point; // Пиксельный режим

        // Заполняем текстуру цветом в форме корабля
        Color[] pixels = new Color[32 * 32];
        Color shipColor = new Color(0.2f, 0.8f, 1f); // Голубой цвет для корабля
        Color engineColor = new Color(1f, 0.5f, 0f); // Оранжевый цвет для двигателя

        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                // Основной корпус корабля (треугольник)
                if (y >= 16)
                {
                    if (x >= 16 - (31-y)/2 && x <= 15 + (31-y)/2)
                    {
                        pixels[y * 32 + x] = shipColor;
                    }
                }
                // Нижняя часть корабля
                else if (y < 16 && y >= 8)
                {
                    if (x >= 8 && x <= 23)
                    {
                        pixels[y * 32 + x] = shipColor;
                    }
                }
                // Двигатели
                else if (y < 8)
                {
                    // Левый двигатель
                    if (x >= 8 && x <= 12)
                    {
                        pixels[y * 32 + x] = engineColor;
                    }
                    // Правый двигатель
                    else if (x >= 19 && x <= 23)
                    {
                        pixels[y * 32 + x] = engineColor;
                    }
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();

        // Создаем спрайт из текстуры
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        
        // Применяем спрайт к SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = sprite;
        
        // Устанавливаем сортировку, чтобы корабль был поверх фона
        spriteRenderer.sortingOrder = 1;
    }
} 