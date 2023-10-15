using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    public MazeCell mazeCellPrefab;
    public Coin coinPrefab;
    public GameObject cactusPrefab; // Updated to cactus prefab
    public int mazeWidth;
    public int mazeDepth;
    public MazeCell[,] mazeGrid;

    private List<Vector3> spawnedCoinPositions = new List<Vector3>();
    private List<GameObject> spawnedCacti = new List<GameObject>(); // Updated to track cacti
    private int maxCoinsToSpawn = 5;
    private int maxCactiToSpawn = 10; // Updated to 10 cacti

    void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);

        for (int i = 0; i < maxCoinsToSpawn; i++)
        {
            SpawnCoin();
        }

        for (int i = 0; i < maxCactiToSpawn; i++) // Spawn cacti
        {
            SpawnCactus();
        }
    }

    void Update()
    {
        foreach (var cactus in spawnedCacti)
        {
            float deltaX = Random.Range(-5f, 5f);
            float deltaZ = Random.Range(-5f, 5f);
            cactus.transform.Translate(new Vector3(deltaX, 0, deltaZ) * Time.deltaTime);
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }

        } while (nextCell != null);
    }

    private void SpawnCoin()
    {
        int x = Random.Range(0, mazeWidth);
        int z = Random.Range(0, mazeDepth);

        Vector3 spawnPosition = new Vector3(x, 0.5f, z);

        if (!spawnedCoinPositions.Contains(spawnPosition))
        {
            Coin coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            spawnedCoinPositions.Add(spawnPosition);
        }
        else
        {
            SpawnCoin();
        }
    }

    private void SpawnCactus()
    {
        int x = Random.Range(0, mazeWidth);
        int z = Random.Range(0, mazeDepth);

        Vector3 spawnPosition = new Vector3(x, 0.5f, z);

        GameObject cactus = Instantiate(cactusPrefab, spawnPosition, Quaternion.identity);
        spawnedCacti.Add(cactus);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            if (cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            if (cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];
            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];
            if (cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    

}
