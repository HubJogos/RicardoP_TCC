using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTileMapColor : MonoBehaviour
{

    public Tilemap tilemap;
    public Color[] colors;
    private Color targetColor;
    private Color currentColor;
    private float timeElapsed;

    [SerializeField]
    public float colorChangeInterval = 0f; //se o campo for maior do que 0, o mapa atualiza a cor sozinho

    public GameObject floorStageBoss;
    public GameObject floorStageHomemEncapuzado;

    private SpriteRenderer floorStageBossSpriteRenderer;
    private SpriteRenderer floorStageHomemEncapuzadoSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }

        if (floorStageBoss!= null)
        {
            floorStageBossSpriteRenderer = floorStageBoss.GetComponent<SpriteRenderer>();
        }

        if (floorStageHomemEncapuzado != null)
        {
            floorStageHomemEncapuzadoSpriteRenderer = floorStageHomemEncapuzado.GetComponent<SpriteRenderer>();
        }


        colors = new Color[]
        {
            new Color(0f, 0.5f, 1f),   // Azul claro
            new Color(0.2f, 0.6f, 1f), // Azul
            new Color(0f, 0.7f, 1f),   // Azul vivo
            new Color(0f, 0.8f, 1f),   // Azul brilhante
            new Color(0f, 0.9f, 1f),   // Azul celeste
            new Color(0.1f, 1f, 1f),   // Azul ciano
            new Color(0.2f, 1f, 0.9f), // Turquesa
            new Color(0.3f, 1f, 0.8f), // Turquesa claro
            new Color(0.4f, 1f, 0.7f), // Turquesa claro
            new Color(0.5f, 1f, 0.6f), // Turquesa claro
            new Color(0.6f, 1f, 0.5f), // Turquesa claro
            new Color(0.7f, 1f, 0.4f), // Verde-azulado claro
            new Color(0.8f, 1f, 0.3f), // Verde-azulado claro
            new Color(0.9f, 1f, 0.2f), // Verde-azulado claro
            new Color(1f, 1f, 0.1f),   // Amarelo claro
            new Color(1f, 1f, 0f),     // Amarelo
            new Color(1f, 0.9f, 0f),   // Amarelo vivo
            new Color(1f, 0.8f, 0f),   // Amarelo brilhante
            new Color(1f, 0.7f, 0f),   // Amarelo ouro
            new Color(1f, 0.6f, 0f),   // Laranja claro
            new Color(1f, 0.5f, 0f),   // Laranja
            new Color(1f, 0.4f, 0f),   // Laranja escuro
            new Color(1f, 0.2f, 0f),   // Vermelho alaranjado
            new Color(1f, 0.1f, 0f),   // Vermelho alaranjado
            new Color(1f, 0f, 0f),     // Vermelho
            new Color(0.9f, 0f, 0f),   // Vermelho vivo
            new Color(0.8f, 0f, 0f),   // Vermelho brilhante
            new Color(0.7f, 0f, 0f),   // Vermelho escuro
            new Color(0.6f, 0f, 0f),   // Vermelho escuro
            new Color(0.5f, 0f, 0f),   // Vermelho escuro
            new Color(0.33f, 0.17f, 0.5f),     // Preto
            new Color(0.4f, 0.4f, 0.4f), // Cinza
            new Color(0.5f, 0.5f, 0.5f), // Cinza
            new Color(0.6f, 0.6f, 0.6f), // Cinza claro
            new Color(0.7f, 0.7f, 0.7f), // Cinza claro
            new Color(0.8f, 0.8f, 0.8f), // Cinza claro
            new Color(0.9f, 0.9f, 0.9f), // Cinza claro
            new Color(1f, 1f, 1f)       // Branco
        };

        ApplyRandomColor();
    }


    void ApplyRandomColor()
    {
        if (tilemap != null && colors != null && colors.Length > 0)
        {
            int randomIndex = Random.Range(0, colors.Length);
            Color randomColor = colors[randomIndex];

            currentColor = randomColor;
            tilemap.color = randomColor;
            ApplyColorInFloors(randomColor);
        }
    }

    void ApplyColor(Color color)
    {
        if (tilemap != null)
        {
            tilemap.color = color;
            ApplyColorInFloors(color);
        }
    }


    void ApplyColorInFloors(Color color)
    {
        if (floorStageBossSpriteRenderer != null)
        {
            Color floorColor = color;
            floorColor.a = 0.5f;
            floorStageBossSpriteRenderer.color = floorColor;
        }

        if (floorStageHomemEncapuzadoSpriteRenderer != null)
        {
            Color floorColor = color;
            floorColor.a = 0.5f;
            floorStageHomemEncapuzadoSpriteRenderer.color = floorColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (colorChangeInterval > 0)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= colorChangeInterval)
            {
                // Escolhe uma nova cor aleatória
                targetColor = colors[Random.Range(0, colors.Length)];

                // Reinicia o contador
                timeElapsed = 0f;
            }

            // Interpola gradualmente entre a cor atual e a cor alvo
            currentColor = Color.Lerp(currentColor, targetColor, timeElapsed / colorChangeInterval);

            // Aplica a cor atual ao tilemap
            ApplyColor(currentColor);
        }
        
    }
}
