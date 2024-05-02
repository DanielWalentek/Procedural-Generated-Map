using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
    [Header ("Default Tiles")]
    public GameObject[] DefaultTiles;

    [Header ("Snow Tiles")]
    public GameObject[] SnowTiles;

    [Header ("Desert Tiles")]
    public GameObject[] DesertTiles;
    [Header("Objects")]
    public GameObject[] Object;

    public GameObject Sea;
    public GameObject Ocean;
    public GameObject[][] BiomTilesList;

    public void CreateBiomList()
    {
        BiomTilesList = new GameObject[4][];
        BiomTilesList[1] = DefaultTiles;
        BiomTilesList[2] = SnowTiles;
        BiomTilesList[3] = DesertTiles;
    }

}
