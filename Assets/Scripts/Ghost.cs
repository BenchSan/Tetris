using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

public class Ghost : MonoBehaviour
{
    public Tile tile;

    public Board board;

    public Piece trackingPiece;

    public Tilemap tilemap;
    private Vector2Int position;

    private Vector2Int[] cells;
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector2Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int tilePos = cells[i] + position;
            tilemap.SetTile((Vector3Int) tilePos, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = trackingPiece.cells[i];
        }

        position = trackingPiece.position;
    }

    private void Drop()
    {
        board.Clear(trackingPiece);
        while (isValidPosition(position))
        {
            position.y--;
        }
        board.Set(trackingPiece);
        position.y++;
       
    }

    private void Set()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int tilePos = cells[i] + position;
            tilemap.SetTile((Vector3Int) tilePos, tile);
        }
    }
    public bool isValidPosition(Vector2Int position)
    {
        for (int i = 0; i < cells.Length; i++)
        {
           
            Vector2Int tilePos = cells[i] + position;
            if (!board.Bounds.Contains(tilePos))
            {
                return false;
            }

            if (trackingPiece.board.tilemap.HasTile((Vector3Int) tilePos))
            {
                return false;
            }
        }
        
        return true;
    }
}
