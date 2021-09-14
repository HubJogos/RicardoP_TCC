using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int mapWidthMultiplier;
    [SerializeField] int repeatNum;
    [SerializeField] GameObject ground;//referencia ao prefab do tile do chão
    Vector2 direction;
    public int width, height;

    public GameObject startingPos;
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
    }

    // Update is called once per frame
    void Generate(Vector2 direction)
    {
        width = Mathf.Abs(Mathf.FloorToInt(direction.x)*mapWidthMultiplier);
        height = Mathf.Abs(Mathf.FloorToInt(direction.y));

        int minHeight = 1;
        int maxHeight = height;

        int repeatValue = 0;    

        for (int i = -width/2; i < width/2; i++)
        {
            if (repeatValue == 0)
            {
                height = Random.Range(minHeight, maxHeight);
                GeneratePlatform(i, height);
                repeatValue = repeatNum;
            }
            else
            {
                GeneratePlatform(i, height);
                repeatValue--;
            }
        }
    }

    void GeneratePlatform(int x, int y)
    {
        for (int j = 0; j < y; j++)
        {
            SpawnObject(ground, x, j);
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
