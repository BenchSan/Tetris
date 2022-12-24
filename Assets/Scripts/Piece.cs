using System;
using UnityEngine;

public class Piece : MonoBehaviour
{
   public Board board { get; private set; }
   public TetrominoData figure { get; private set; }
   public Vector2Int position { get; private set; }
   public Vector2Int[] cells { get; private set; }
   public float stepDelay { get; private set; }
   public float lockDelay { get; private set; }
   private float stepTime { get; set; }
   private float lockTime { get; set; }
   

   public void Initialize(Board board, Vector2Int position, TetrominoData figure)
   {
      this.board = board;
      this.position = position;
      this.figure = figure;
      stepDelay = 1f;
      lockDelay = 0.5f;
      stepTime = Time.time + stepDelay;
      lockTime = 0f;
      cells = new Vector2Int[figure.positions.Length];
      
      for (int i = 0; i < figure.positions.Length; i++)
      {
         cells[i] = figure.positions[i];
      }
   }

   private void Update()
   {
      board.Clear(this);
      lockTime += Time.deltaTime;
      if (Input.GetKeyDown(KeyCode.A))
      {
         Move(Vector2Int.left);
      }

      if (Input.GetKeyDown(KeyCode.D))
      {
         Move(Vector2Int.right);
      }

      if (Input.GetKeyDown(KeyCode.S))
      {
         Move(Vector2Int.down);
      }

      if (Input.GetKeyDown(KeyCode.Space))
      {
         HardDrop();
      }

      if (Input.GetKeyDown(KeyCode.Q))
      {
         Rotate(-1);
      }

      if (Input.GetKeyDown(KeyCode.E))
      {
         Rotate(1);
      }

      if (Time.time >= stepTime)
      {
         Step();
      }

     
      if(board.controller.gameisStarted)
      {board.Set(this);}
   }

   private void Step()
   {
      stepTime = Time.time + stepDelay;
      Move(Vector2Int.down);
      if (lockTime >= lockDelay)
      {
         Lock();
      }
   }

   private void Lock()
   {
      board.Set(this);
      board.ClearLines();
      // ClearLineboard.listener.PlayOneShot(board.moving);
      board.Spawn();
   }
   public bool Move(Vector2Int transfer)
   {
      Vector2Int newPos = position + transfer;
      bool isValid = board.isValidPosition(this, newPos);
      if (isValid)
      {
         position = newPos;
         if (transfer.Equals(Vector2Int.down))
         {
            lockTime = 0f;
         }
      }
      return isValid;
   }

   private void HardDrop()
   {
      while (Move(Vector2Int.down)){}
      Lock();
   }

   private void Rotate(int direction)
   {
      Vector2Int[] copy = new Vector2Int[cells.Length];
      Array.Copy(cells,copy,cells.Length);
      
      for (int i = 0; i < cells.Length; i++)
      {
         Vector2 cell = cells[i];
         int x, y;
         switch (figure.tetromino)  
         {
            case Tetromino.I:
            case Tetromino.O:
               cell.x -= 0.5f;
               cell.y -= 0.5f;
               x = Mathf.CeilToInt(cell.x * Data.RotationMatrix[0] * direction + cell.y * Data.RotationMatrix[1] * direction);
               y = Mathf.CeilToInt(cell.x * Data.RotationMatrix[2] * direction + cell.y * Data.RotationMatrix[3] * direction);
               break;
            default:
               x = Mathf.RoundToInt(cell.x * Data.RotationMatrix[0] * direction + cell.y * Data.RotationMatrix[1] * direction);
               y = Mathf.RoundToInt(cell.x * Data.RotationMatrix[2] * direction + cell.y * Data.RotationMatrix[3] * direction);
               break;
         }

         if (board.tilemap.HasTile((Vector3Int) new Vector2Int(x, y) + (Vector3Int)position) || !board.Bounds.Contains(new Vector2Int(x,y)+ position))
         {
            cells = copy;
            break;
         }
         cells[i] = new Vector2Int(x, y);
      }
      
   }
   
}
 