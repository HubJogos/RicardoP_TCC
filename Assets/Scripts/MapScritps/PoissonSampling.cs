using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonSampling
{
    //usado para encontrar os possíveis pontos de inserção de objetos no mapa
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int samplesBeforeRejection = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);//determina tamanho da área necessária para objetos, assumindo que são padronizados
        int[,] grid = new int[Mathf.FloorToInt(sampleRegionSize.x / cellSize), Mathf.FloorToInt(sampleRegionSize.y / cellSize)];//determina tamanho da área disponível
        List<Vector2> points = new List<Vector2>();//pontos gerados
        List<Vector2> spawnPoints = new List<Vector2>();//pontos em geração, que podem ser rejeitados

        spawnPoints.Add(sampleRegionSize / 2);//adiciona pontos relativo ao espaço onde serão inseridos
        while (spawnPoints.Count > 0)//enquanto houverem pontos a serem testados
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);//
            Vector2 spawnCentre = spawnPoints[spawnIndex];//determina ponto a ser utilizado
            bool candidateAccepted = false;

            for(int i = 0; i < samplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));//determina direção ao próximo ponto
                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);//gera ponto candidato além do raio do ponto de partida

                if (IsValid(candidate, sampleRegionSize, cellSize, points, grid, radius))//testa validade do ponto candidato
                {
                    points.Add(candidate);//adiciona a lista de pontos gerados
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;//atualiza grid com pontos gerados
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);//caso rejeite ponto, remove da lista de geração
            }
        }
        return points;//retorna pontos aceitos
    }
    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, List<Vector2> points, int[,] grid, float radius)//usado para validar ponto sendo gerado
    {
        if(candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)//se candidato está dentro do mapa
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);//define tamanhõ da região da célula onde o ponto será colocado
            //se cada célula é um quadrado de dimensões 1x1, o ponto pode aparecer em qualquer posição entre 0-1 em ambos os eixos

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);//com raio = 1, o disco projetado está contido na vizinhança 5x5 da célula central

            for (int x = searchStartX; x< searchEndX; x++)
            {
                for (int y = searchStartY; y < searchEndY; y++)//percorrendo a vizinhança 5x5:
                {
                    int pointIndex = grid[x, y] - 1;//-1 pois começa em 0
                    if(pointIndex != -1)//-1 indica um ponto fora do raio
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;//calcula distância do candidato aos pontos gerados na vizinhança
                        if(sqrDst < radius*radius)//se estiver dentro do raio de algum
                        {
                            return false;//rejeita
                        }
                    }
                }
            }
            return true;//após percorrer vizinhança, se não foi rejeitado, aceita
        }
        return false;//rejeita candidatos fora do mapa
    }
}
