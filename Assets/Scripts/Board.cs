using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;
    private Piece activePiece;
    public Vector2Int spawnPoint;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public UIController controller;
    private int score = 0;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
    }

 

    public void Spawn()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData piece = tetrominoes[random];
        activePiece.Initialize(this, spawnPoint, piece);
        if (isValidPosition(activePiece, spawnPoint))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
        
    }

    private void GameOver()
    {
        score = 0;
        controller.GameOver();
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePos = piece.cells[i] + piece.position;
            tilemap.SetTile((Vector3Int) tilePos, piece.figure.tile);
        }
    }
    
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePos = piece.cells[i] + piece.position;
            tilemap.SetTile((Vector3Int) tilePos, null);
        }
    }

    public bool isValidPosition(Piece piece, Vector2Int position)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
           
            Vector2Int tilePos = piece.cells[i] + position;
            if (!Bounds.Contains(tilePos))
            {
                return false;
            }

            if (tilemap.HasTile((Vector3Int) tilePos))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        int row = Bounds.yMin;

        while (row < Bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        for (int column = Bounds.xMin; column < Bounds.xMax; column++)
        {
            if (!tilemap.HasTile((Vector3Int) new Vector2Int(column, row)))
            {
                return false;
            }
        }
        
        score += 100;
        FindObjectOfType<Score>().GetComponent<Text>().text = "Score: " + score;
        return true;
    }

    private void LineClear(int row)
    {
        for (int column = Bounds.xMin; column < Bounds.xMax; column++)
        {
            tilemap.SetTile((Vector3Int)new Vector2Int(column,row),null);
        }

        while (row < Bounds.yMax)
        {
            for (int column = Bounds.xMin; column < Bounds.xMax; column++)
            {
                Vector2Int position = new Vector2Int(column, row+1);
                TileBase above = tilemap.GetTile((Vector3Int) position);
                position = new Vector2Int(column, row);
                tilemap.SetTile((Vector3Int) position,above);
            }

            row++;
        }
    }
}