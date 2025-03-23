using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [System.Serializable]
    public struct StarSettings
    {
        public int starCount;
        public float minStarSize;
        public float maxStarSize;
        public float scrollSpeed;
    }

    [System.Serializable]
    public struct Star
    {
        public Vector2 position;
        public float size;
        public Color color;
    }

    [Header("Star Settings")]
    public StarSettings starSettings;

    [Header("Colors")]
    public Color backgroundColor = Color.black;
    public Color starColor = Color.white;

    private Star[] stars;
    private Texture2D backgroundTexture;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        InitializeBackground();
    }

    void Update()
    {
        UpdateStars();
    }

    private void InitializeBackground()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // Создаем текстуру размером с экран
        backgroundTexture = new Texture2D(Screen.width, Screen.height);
        backgroundTexture.filterMode = FilterMode.Point; // Пиксельный режим для NES-стиля
        
        // Заполняем текстуру фоновым цветом
        Color[] pixels = new Color[Screen.width * Screen.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = backgroundColor;
        }
        backgroundTexture.SetPixels(pixels);
        
        // Инициализируем массив звёзд
        stars = new Star[starSettings.starCount];
        GenerateStars();
        
        // Применяем текстуру к спрайту
        Sprite sprite = Sprite.Create(backgroundTexture, 
            new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), 
            new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite = sprite;
    }

    private void UpdateStars()
    {
        // Очищаем текстуру
        Color[] pixels = new Color[backgroundTexture.width * backgroundTexture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = backgroundColor;
        }
        backgroundTexture.SetPixels(pixels);

        // Обновляем позиции звёзд
        for (int i = 0; i < stars.Length; i++)
        {
            // Двигаем звезду вниз
            stars[i].position.y -= starSettings.scrollSpeed * Time.deltaTime;

            // Если звезда вышла за пределы экрана снизу, перемещаем её наверх
            if (stars[i].position.y < 0)
            {
                stars[i].position.y = backgroundTexture.height;
                stars[i].position.x = Random.Range(0, backgroundTexture.width);
                stars[i].size = Random.Range(starSettings.minStarSize, starSettings.maxStarSize);
            }

            DrawStar(stars[i]);
        }

        // Применяем изменения текстуры
        backgroundTexture.Apply();
    }

    private void GenerateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i] = new Star
            {
                position = new Vector2(
                    Random.Range(0, backgroundTexture.width),
                    Random.Range(0, backgroundTexture.height)
                ),
                size = Random.Range(starSettings.minStarSize, starSettings.maxStarSize),
                color = starColor
            };
            
            DrawStar(stars[i]);
        }
        backgroundTexture.Apply();
    }

    private void DrawStar(Star star)
    {
        int x = Mathf.RoundToInt(star.position.x);
        int y = Mathf.RoundToInt(star.position.y);
        int size = Mathf.RoundToInt(star.size);

        for (int i = -size; i <= size; i++)
        {
            for (int j = -size; j <= size; j++)
            {
                int pixelX = x + i;
                int pixelY = y + j;
                
                if (pixelX >= 0 && pixelX < backgroundTexture.width &&
                    pixelY >= 0 && pixelY < backgroundTexture.height)
                {
                    backgroundTexture.SetPixel(pixelX, pixelY, star.color);
                }
            }
        }
    }
} 