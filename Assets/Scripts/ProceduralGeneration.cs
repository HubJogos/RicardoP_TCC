using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [Header("TerrainGen")]
    [SerializeField] int repeatNum;
    [SerializeField] GameObject ground;//referencia ao prefab do tile do chão
    [SerializeField] float smoothness, seed;
    Vector2 direction;
    public int width, height;
    public GameObject startingPos;

    [Header("CaveGen")]
    [Range(0,1)]
    [SerializeField] float modifier;
    void Start()
    {
        //Generate();
        Cursor.lockState = CursorLockMode.Locked;//usado para centralizar o mouse na tela no início
        Cursor.lockState = CursorLockMode.None;

    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))//quando pressionado o botão esquerdo
        {
            DestroyPreviousMap();

            Vector2 screenPos = new Vector2(mousePos.x, mousePos.y);
            direction = new Vector2(screenPos.x - startingPos.transform.position.x, screenPos.y - startingPos.transform.position.y);
            Generate(direction);

        }
        if (Input.GetMouseButtonDown(1))//quando pressionado o botão direito
        {
            smoothness = Random.RandomRange(1, 100);
            seed = Random.RandomRange(1, 100);
        }
    }

    
    void Generate(Vector2 direction)
    {
        width = Mathf.Abs(Mathf.FloorToInt(direction.x));
        height = Mathf.Abs(Mathf.FloorToInt(direction.y));


        int perlinHeight;
        int repeatValue = 0;    

        for (int i = -width/2; i < width/2; i++)
        {
            int minHeight = height - 2;
            int maxHeight = height + 2;
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(i / smoothness, seed) * height/2);
            perlinHeight += height / 2;
            if (repeatValue == 0)
            {
                height = Random.Range(minHeight, maxHeight);
                GeneratePlatform(i, perlinHeight);
                repeatValue = repeatNum;
            }
            else
            {
                GeneratePlatform(i, perlinHeight);
                repeatValue--;
            }
        }
    }

    void GeneratePlatform(int x, int y)
    {
        for (int j = 0; j < y; j++)
        {
            int caveValue = Mathf.RoundToInt(Mathf.PerlinNoise((x * modifier) + seed, (j * modifier) + seed));
            if (caveValue < 1)
            {
                SpawnObject(ground, x, j);
            }
            
        }
    }

    void SpawnObject(GameObject obj, int width, int height)//o que for criado estará como folha do objeto que contém o script
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }

    void DestroyPreviousMap()
    {
        foreach(Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
