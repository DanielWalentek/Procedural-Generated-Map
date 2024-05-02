using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridCreator : MonoBehaviour
{
    public Cell[,] cellArray;
    [SerializeField]
    MapController mapController;
    [SerializeField]
    TilesController tilesController;
    private int minBiomSize = 50;
    private int maxBiomSize = 70; 
    private int mapBorder=2;
    private int landPercentage=50;
    private List<Cell> continentTiles = new List<Cell>();
    private List<Cell> biomTiles;
    private List<List<Cell>> biomList;
    [SerializeField]
    private int additionalBioms;
    int landSize;
    int numberOfCities = 10;

    
    public void CreateCellArray(int[,] cellXYArray)
    {
        cellArray = new Cell[cellXYArray.GetLength(0), cellXYArray.GetLength(1)];
        
        for (int x = 0; x < cellXYArray.GetLength(0); x++) 
        {
            for (int y = 0; y < cellXYArray.GetLength(1); y++)
            {
                
                Cell cell = new Cell(x, y, cellXYArray.GetLength(0), cellXYArray.GetLength(1));
                
                cellArray[x,y] = cell;
                cellArray[x,y].Tile = tilesController.Ocean;
            }
        }
    }

    public void CreateContinent ()
    {   
        int yContinentSeed = UnityEngine.Random.Range(cellArray.GetLength(1)/3, cellArray.GetLength(1)*2/3);
        int xContinentSeed = cellArray.GetLength(0) / 2;
        
        landSize = ((cellArray.GetLength(0) * cellArray.GetLength(1)) * landPercentage) / 100;

        cellArray[xContinentSeed, yContinentSeed].Tile = tilesController.DefaultTiles[0];
        cellArray[xContinentSeed, yContinentSeed].CellBiom = Cell.Biom.Default;
        continentTiles.Add(cellArray[xContinentSeed,yContinentSeed]);

        for(float i=0; i<landSize;)
        {
            int randomContinentTile = UnityEngine.Random.Range(0, continentTiles.Count-1);
            Cell mainCell = continentTiles[randomContinentTile];
            Cell neighbour;
            
            int randomNeighbour = UnityEngine.Random.Range(0,mainCell.Neighbours.Count-1);

            neighbour = cellArray[mainCell.Neighbours[randomNeighbour].x, mainCell.Neighbours[randomNeighbour].z];
            
            if(neighbour.Tile != tilesController.BiomTilesList[1][0] && neighbour.XPosition > mapBorder && neighbour.XPosition < cellArray.GetLength(0) - mapBorder && neighbour.YPosition > mapBorder - 1 && neighbour.YPosition < cellArray.GetLength(1) - mapBorder)
            {
                neighbour.Tile = tilesController.BiomTilesList[1][0];
                neighbour.CellBiom = Cell.Biom.Default;
                continentTiles.Add(neighbour);
                i++;
            }
            
        }
    } 

    public void CreateTiles ()
    {

        foreach(Cell cell in cellArray)
        {
            bool oddRow = cell.YPosition % 2 == 1;
            GameObject.Instantiate(cell.Tile, new Vector3(oddRow? cell.XPosition : (cell.XPosition + 0.5f), 0, 1.5f * cell.YPosition * (float) Math.Sqrt(1/3f)), quaternion.identity);
        }
    }
    
    public void CreateBiom ()
    {
        biomList = new List<List<Cell>>();
        foreach (Cell.Biom biomIndex in Enum.GetValues (typeof (Cell.Biom)))
        {
            if ((int) biomIndex != 0 && (int) biomIndex != 1)
            {
                biomTiles = new List<Cell>();
                biomList.Add(biomTiles);

                int xSeedPosition = continentTiles[UnityEngine.Random.Range(0, continentTiles.Count -1)].XPosition;
                int ySeedPosition = continentTiles[UnityEngine.Random.Range(0, continentTiles.Count -1)].YPosition;

                Cell seedCell = cellArray[xSeedPosition, ySeedPosition];
                seedCell.CellBiom = biomIndex;
                biomTiles.Add(seedCell);

                int biomSize = (int)UnityEngine.Random.Range(minBiomSize, maxBiomSize);
                
                for(int i=0; i<biomSize;)
                {
                    int randomBiomTile = UnityEngine.Random.Range(0, biomTiles.Count -1);
                    Cell mainCell = biomTiles[randomBiomTile];
                    int randomNeighbour = UnityEngine.Random.Range(0, mainCell.Neighbours.Count -1);
                    Cell neighbour = cellArray[mainCell.Neighbours[randomNeighbour].x, mainCell.Neighbours[randomNeighbour].z];

                    if(neighbour.CellBiom == Cell.Biom.Default)
                    {
                        neighbour.CellBiom = biomIndex;
                        GameObject[] biomTile = tilesController.BiomTilesList[(int) biomIndex];
                        neighbour.Tile = biomTile[0];
                        biomTiles.Add(neighbour);
                        i++;
                    }  

                    int tries=0;
                    tries++;

                    if(tries>=100)
                    {
                        i++;
                    }
                }

            }
        }

    }

    public void CreateTerrain ()
    {
        foreach(Cell cell in cellArray)
        {
            if(cell.CellBiom != Cell.Biom.Water)
            {
                int biomID = (int) cell.CellBiom;
                cell.Tile = tilesController.BiomTilesList[biomID][UnityEngine.Random.Range(0, tilesController.BiomTilesList[biomID].GetLength(0))];
            }
        }
    }

    public void CreateCities ()
    {
        for (int i = 0; i<numberOfCities;)
        {
            Cell randomCell = cellArray[UnityEngine.Random.Range(mapBorder, cellArray.GetLength(0) - mapBorder), UnityEngine.Random.Range(mapBorder, cellArray.GetLength(1) - mapBorder)];
            
                if(randomCell.CellBiom != Cell.Biom.Water && !randomCell.isOccupied)
                {
                    if(randomCell.Tile == tilesController.BiomTilesList[(int)randomCell.CellBiom][0]) 
                    {
                        bool oddRow = randomCell.YPosition % 2 == 1;
                        GameObject newObject = tilesController.Object[UnityEngine.Random.Range(0, tilesController.Object.GetLength(0) -1 )];
                        Instantiate<GameObject>(newObject, new Vector3(oddRow? randomCell.XPosition : randomCell.XPosition + 0.5f,0,1.5f * randomCell.YPosition * (float) Math.Sqrt(1/3f)),Quaternion.identity);
                        randomCell.isOccupied = true;
                        i++;
                    }
                }
        }
    }

    public void Water()
    {
        foreach(Cell cell in cellArray)
        {
            if(cell.CellBiom == Cell.Biom.Water)
            {
                foreach(Vector3 neighbour in cell.Neighbours)
                {
                    if(cellArray[(int)neighbour.x, (int)neighbour.z].Tile != tilesController.Ocean && cellArray[(int)neighbour.x, (int)neighbour.z].Tile != tilesController.Sea)
                    {
                        cell.Tile = tilesController.Sea;
                    }
                    
                }
            }
            
        }
    }
}
