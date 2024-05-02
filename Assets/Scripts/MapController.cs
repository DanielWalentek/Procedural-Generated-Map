using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private int width, height;
    [SerializeField]
    private TilesController tilesController; 
    [SerializeField]
    public GridCreator gridCreator;
    [SerializeField]
    CameraController cameraController;
    public int[,] cellXYArray;
    private void Start()
    {
        cellXYArray = new int [width, height];
        tilesController.CreateBiomList();
        gridCreator.CreateCellArray(cellXYArray);
        gridCreator.CreateContinent();
        gridCreator.CreateBiom();
        gridCreator.CreateTerrain();
        gridCreator.CreateCities();
        gridCreator.Water();
        gridCreator.CreateTiles();

        cameraController.Camera();
    }
   
}
