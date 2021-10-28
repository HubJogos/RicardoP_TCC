using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();
    public MeshFilter caveMesh;

    public void GenerateMesh(int[,] map, float squareSize)
    {
        triangleDictionary.Clear();
        outlines.Clear();
        checkedVertices.Clear();


        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int i = 0; i < squareGrid.squares.GetLength(0); i++)
        {
            for (int j = 0; j < squareGrid.squares.GetLength(1); j++)
            {
                TriangulateSquare(squareGrid.squares[i, j]);//gera as meshes relevantes para cada quadrado no mapa
            }
        }

        Mesh mesh = new Mesh();
        caveMesh.mesh = mesh;

        //o que realmente gera as meshes in-game
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        Generate2DColliders();
    }

    void Generate2DColliders()
    {

        EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>();
        for (int i =0; i< currentColliders.Length; i++)
        {
            Destroy(currentColliders[i]);
        }
        CalculateMeshOutlines();

        foreach(List<int> outline in outlines)
        {
            EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            Vector2[] edgePoints = new Vector2[outline.Count];

            for (int i=0; i<outline.Count; i++)
            {
                edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].z);
            }
            edgeCollider.points = edgePoints;
        }
    }

    void CalculateMeshOutlines()
    {

        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }
    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            case 0://nenhum ponto recebe mesh
                break;
            //meshes com 1 ponto
            case 1:
                MeshFromPoints(square.centerBottom, square.botLeft, square.centerLeft);
                break;
            case 2:
                MeshFromPoints(square.centerRight, square.botRight, square.centerBottom);
                break;
            case 4:
                MeshFromPoints(square.centerTop, square.topRight, square.centerRight);
                break;
            case 8:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerLeft);
                break;
            //meshes com 2 pontos
            case 3:
                MeshFromPoints(square.centerRight, square.botRight, square.botLeft, square.centerLeft);
                break;
            case 6:
                MeshFromPoints(square.centerTop, square.topRight, square.botRight, square.centerBottom);
                break;
            case 9:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerBottom, square.botLeft);
                break;
            case 12:
                MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerLeft);
                break;
            case 5://2 pontos em diagonais opostas
                MeshFromPoints(square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.botLeft, square.centerLeft);
                break;
            case 10:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.botRight, square.centerBottom, square.centerLeft);
                break;
            //meshes com 3 pontos
            case 7:
                MeshFromPoints(square.centerTop, square.topRight, square.botRight, square.botLeft, square.centerLeft);
                break;
            case 11:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.botRight, square.botLeft);
                break;
            case 13:
                MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.botLeft);
                break;
            case 14:
                MeshFromPoints(square.topLeft, square.topRight, square.botRight, square.centerBottom, square.centerLeft);
                break;
            //mesh com 4 pontos
            case 15:
                MeshFromPoints(square.topLeft, square.topRight, square.botRight, square.botLeft);
                break;

        }
    }//individualmente busca por cada configuração possível dos triângulos das meshes (16 ao todo)

    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (triangleDictionary.ContainsKey(vertexIndexKey))//se vértice já foi encontrado
        {
            triangleDictionary[vertexIndexKey].Add(triangle);//adiciona triangulo novo a sua lista de triangulos associados
        }
        else
        {
            List<Triangle> triangleList = new List<Triangle>();//senão, cria uma nova lista de triangulos pertencentes a esse vértice
            triangleList.Add(triangle);//adiciona triângulo encontrado a lista
            triangleDictionary.Add(vertexIndexKey, triangleList);//adiciona vértice e triangulo ao dicionário de triangulos
        }
    }//dicionário de todos os triângulos das meshes

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);//adiciona próximo vértice a aresta do mapa
        checkedVertices.Add(vertexIndex);//marca que vértice foi checado
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);//busca próximo vértice na aresta do mapa

        if (nextVertexIndex != -1)//se existir
        {
            FollowOutline(nextVertexIndex, outlineIndex);//repete para o próximo
        }
    }//define as linhas das arestas do mapa

    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];//lista de triangulo que contém o vértice

        for (int i = 0; i < trianglesContainingVertex.Count; i++)//para cada triangulo que possui esse vértice
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];//vértice sendo checado
                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))//para cada vértice que não é o selecionado em cada triangulo, e não foi verificado ainda
                {
                    if (IsOutlineEdge(vertexIndex, vertexB))//se é aresta do mapa
                    {
                        return vertexB;//retorna vértice sendo checado para traçar aresta do mapa
                    }
                }
            }
        }

        return -1;//senão, vértice checado não pertence aos vértices da aresta do mapa
    }//busca próximo vértice na aresta do mapa

    bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {
            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)//se 2 vértices estão entre 2 triângulos, não é aresta do mapa
                {
                    break;
                }
            }
        }
        return sharedTriangleCount == 1;
    }//detecta se outline é aresta do mapa

    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
        if (points.Length >= 3)
        {
            CreateTriangle(points[0], points[1], points[2]);
        }
        if (points.Length >= 4)
        {
            CreateTriangle(points[0], points[2], points[3]);
        }
        if (points.Length >= 5)
        {
            CreateTriangle(points[0], points[3], points[4]);
        }
        if (points.Length >= 6)
        {
            CreateTriangle(points[0], points[4], points[5]);
        }
    }//marca vértices dos triângulos de cada mesh gerada
    void AssignVertices(Node[] points)
    {
        for(int i=0;i<points.Length; i++)
        {
            if(points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }//marca posição global dos vértices de cada mesh gerada

    void CreateTriangle(Node a, Node b, Node c)//dados 3 nós, cria a definição de um triangulo em 3 posições sequenciais na lista
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA, triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }

    //para cada bloco de mapa temos 4 "control nodes", um em cada canto, e 4 "nodes"
    //cada um é o ponto intermediário da aresta entre control nodes, usamos eles para
    //gerar as meshes de mapa
    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for(int i = 0; i < nodeCountX; i++)
            {
                for(int j = 0; j < nodeCountY; j++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + i * squareSize + squareSize / 2, 0, -mapHeight / 2 + j * squareSize + squareSize / 2);
                    controlNodes[i, j] = new ControlNode(pos, map[i, j] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int i = 0; i < nodeCountX-1; i++)
            {
                for (int j = 0; j < nodeCountY-1; j++)
                {
                    squares[i, j] = new Square(controlNodes[i, j + 1], controlNodes[i + 1, j + 1], controlNodes[i + 1, j], controlNodes[i, j]);
                }
            }
        }
    }//define grid de quadrados
    public class Square
    {
        public ControlNode topLeft, topRight, botRight, botLeft;
        public Node centerTop, centerRight, centerBottom, centerLeft;
        public int configuration;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _botRight, ControlNode _botLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            botRight = _botRight;
            botLeft = _botLeft;

            centerTop = topLeft.right;
            centerRight = botRight.above;
            centerBottom = botLeft.right;
            centerLeft = botLeft.above;

            if (topLeft.active) configuration += 8;
            if (topRight.active) configuration += 4;
            if (botRight.active) configuration += 2;
            if (botLeft.active) configuration += 1;
        }
    }//define quadrados com nós de controle nos vértices, e nós no meio das arestas
    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }//pontos usado para gerar os triângulos das meshes
    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos){
            active = _active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
            }
    }//pontos usado para gerar os triângulos das meshes
    struct Triangle
    {
        public int vertexIndexA;
        public int vertexIndexB;
        public int vertexIndexC;
        int[] vertices;

        public Triangle(int a, int b, int c)
        {
            vertexIndexA = a;
            vertexIndexB = b;
            vertexIndexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }

        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }


        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }//declaração auxiliar para determinação das meshes
}
