using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
   I,O,T,J,L,S,Z
}
[Serializable]
public struct TetrominoData
{
   public Tetromino tetromino;
   public Tile tile;
   public Vector2Int[] positions;
   public Vector2Int[,] wallKicks;

}
