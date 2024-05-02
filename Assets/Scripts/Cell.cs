using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.WSA;


public class Cell
{
    public int XPosition, YPosition;
    private int xArraySize, yArraySize;
    public GameObject Tile;
    public List<Vector3Int> Neighbours;
    public Biom CellBiom = Biom.Water;
    public bool isOccupied = false;

    
    public Cell(int xPosition, int yPosition, int xArraySize, int yArraySize)
    {
        this.XPosition = xPosition;
        this.YPosition = yPosition;
        this.xArraySize = xArraySize;
        this.yArraySize = yArraySize;
        bool evenRow = yPosition % 2 == 1;

        Neighbours = new List<Vector3Int>();
        if(YPosition != yArraySize-1)
        {
            Neighbours.Add(new Vector3Int(XPosition, 0, YPosition+1));
            
            if(evenRow && xPosition != 0)
            {
                Neighbours.Add(new Vector3Int(XPosition-1, 0, YPosition+1));
            }
            else if(!evenRow && XPosition != xArraySize-1)
            {
                Neighbours.Add(new Vector3Int(xPosition+1, 0, YPosition+1));
            }
        }

        if(XPosition != 0)
        {
            Neighbours.Add(new Vector3Int(XPosition-1, 0, YPosition));
        }

        if(XPosition != xArraySize-1)
        {
            Neighbours.Add(new Vector3Int(XPosition+1, 0, YPosition));
        }

        if(YPosition != 0)
        {
            Neighbours.Add(new Vector3Int(XPosition, 0, YPosition-1));

            if(evenRow && XPosition != 0)
            {
                Neighbours.Add(new Vector3Int(XPosition-1, 0, YPosition-1));
            }
            else if(!evenRow && XPosition != xArraySize-1)
            {
                Neighbours.Add(new Vector3Int(XPosition+1, 0, YPosition-1));
            }
        }
    }

    public enum Biom
    {
        Water,
        Default,
        Snow,
        Dessert,
        
    }
}
