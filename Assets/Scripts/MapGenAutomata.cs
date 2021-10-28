using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class MapGenAutomata : MonoBehaviour
{
    public int width, height;
    public int smooth = 5;
    public int minRegionSize = 50;

    public string seed;
    public bool useRandomSeed;


    [Range(0,100)]
    public int randomFillPercent;//porcentagem de terreno/parede
    int[,] map;//matriz do mapa

    private void Start()
    {
        GenerateMap();
    }//gera mapa
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }//somente detecta input do mouse para gerar novamente

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        //ProcessMap();

        for (int x=0; x < smooth; x++)
        {
            SmoothMap();
        }

        

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1);
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());//pseudo random number generator

        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                if(i==0 || i == width-1 || j==0 || j== height-1)
                {
                    map[i, j] = 1;//garante paredes nas bordas do mapa
                }
                else
                {
                    map[i, j] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;//aleatoriza que áreas serão terreno ou parede
                }
                
            }
        }
    }

    void SmoothMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(i, j);
                if (neighbourWallTiles > 4)
                {
                    map[i, j] = 1;
                }else if (neighbourWallTiles < 4)
                {
                    map[i, j] = 0;
                }
                
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) 
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) 
            {
                if(IsInMapRange(neighbourX, neighbourY))//garante que não conta tiles fora do mapa
                {
                    if (neighbourX != gridX || neighbourY != gridY)//garante que não conta tile selecionado
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;//se estamos na aresta do mapa
                }
            }
        }
        return wallCount;
    }
    //região problema inicia aqui////////////////////////////////////////////////////////////////////////
   
    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach(Coord tile in newRegion)
                    {

                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }//forma uma lista de regiões

    void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < minRegionSize)//deleta regiões muito pequenas
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegions.Count < minRegionSize)//deleta regiões muito pequenas
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
        }
    }//usado para detectar regiões no mapa e eliminá-las se forem muito pequenas, posteriormente será usado para conectá-las

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; x <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x, y] == 0 && map[x,y] == tileType) 
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }//forma uma lista dos tiles de cada região

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }//definição de coordenadas de um tile
    
    //região problema encerra aqui//////////////////////////////////////////////////////////////////////
    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }//auxiliar
}
