using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap  backWallTilemap, wallSidesTilemap, wallTilemap, floorTilemap;
    [SerializeField]
    private TileBase wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft,
        wallTopFarRight, wallTopFarLeft, wallDiagonalUpperRightCorner, wallDiagonalUpperLeftCorner;
    [SerializeField]
    private TileBase[] floorTile, wallTop, wallBottom, wallSideRight, wallSideLeft;

    public List<Vector2Int> wallPositions = new List<Vector2Int>();
    private bool wallCanHaveTorch = false;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    public void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        Tilemap tilemap = backWallTilemap;
        wallCanHaveTorch = false;
        if(WallByteTypes.wallTop.Contains(typeAsInt))
        {
            tile = wallTop[UnityEngine.Random.Range(0, wallTop.Length)];
            tilemap = wallTilemap;
            wallCanHaveTorch = true;
        }
        else if(WallByteTypes.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight[UnityEngine.Random.Range(0, wallSideRight.Length)];
            tilemap = wallSidesTilemap;
        }
        else if (WallByteTypes.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft[UnityEngine.Random.Range(0, wallSideLeft.Length)];
            tilemap = wallSidesTilemap;
        }
        else if (WallByteTypes.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom[UnityEngine.Random.Range(0, wallBottom.Length)];
        }
        else if (WallByteTypes.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if (tile != null)
        {
            PaintSingleTile(tilemap, tile, position);
            if(wallCanHaveTorch)
                wallPositions.Add(position);
        }
    }

    public void RemoveSingleBasicWall(Vector2Int position, string tilemapName)
    {
        Tilemap tilemap = new Tilemap();
        switch (tilemapName)
        {
            case "side":
                tilemap = wallSidesTilemap;
                break;
            case "back":
                tilemap = backWallTilemap;
                break;
            case "front":
                tilemap = wallTilemap;
                break;
        }
        TileBase tile = null;
        PaintSingleTile(tilemap, tile, position);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase[] tile)
    {
        foreach(var position in positions)
        {
            PaintSingleTile(tilemap, tile[UnityEngine.Random.Range(0, floorTile.Length)], position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        backWallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        Tilemap tilemap = backWallTilemap;

        if(WallByteTypes.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if(WallByteTypes.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallByteTypes.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallByteTypes.wallDiagonalUpperRightCorner.Contains(typeAsInt))
        {
            tile = wallDiagonalUpperRightCorner;
        }
        else if (WallByteTypes.wallDiagonalUpperLeftCorner.Contains(typeAsInt))
        {
            tile = wallDiagonalUpperLeftCorner;
        }
        else if (WallByteTypes.wallTopFarRight.Contains(typeAsInt))
        {
            tile = wallTopFarRight;
            tilemap = wallTilemap;
        }
        else if (WallByteTypes.WallTopFarLeft.Contains(typeAsInt))
        {
            tile = wallTopFarLeft;
            tilemap = wallTilemap;
        }
        else if (WallByteTypes.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallByteTypes.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom[UnityEngine.Random.Range(0, wallBottom.Length)];
        }

        if (tile != null)
        {
            PaintSingleTile(tilemap, tile, position);
        }

    }
}
