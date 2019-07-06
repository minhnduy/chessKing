using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public abstract class Cell
    {
        protected int cellCoordinate;

        //empty cell that can be reused, you don't have to create
        private static Dictionary<int, EmptyCell> cachedEmptyCells = createCachedEmtyCells();

        public Cell(int coordinate)
        {
            this.cellCoordinate = coordinate;            
        }

        //create empty cell
        public static Dictionary<int, EmptyCell> createCachedEmtyCells()
        {
            Dictionary<int, EmptyCell> temp = new Dictionary<int, EmptyCell>();

            for (int i = 0; i < 64; i++)
            {
                temp[i] = new EmptyCell(i);
            }
            return temp;
        }

        //create a cell, cell can store a piece or not           
        public static Cell createCell(int coordinate, Piece piece){
            if (piece != null)
                return new OccupiedCell(coordinate, piece);
            else
                return cachedEmptyCells[coordinate];
        }

        public abstract bool isCellOccupied();

        public abstract Piece getPiece();

        public int getCellCoordinate()
        {
            return this.cellCoordinate;
        }
    }

    public class EmptyCell : Cell
    {
        public EmptyCell(int coordinate)
            : base(coordinate)
        {

        }       

        public override bool isCellOccupied()
        {
            return false;
        }

        public override Piece getPiece()
        {
            return null;
        }

        public override string ToString()
        {
            return "-";
        }
    }

    public class OccupiedCell : Cell
    {
        private  Piece pieceOnCell;

        public OccupiedCell(int coordinate, Piece pieceOnCell)
            : base(coordinate)
        {
            this.pieceOnCell = pieceOnCell;
        }

        public override bool isCellOccupied()
        {
            return true;
        }

        public override Piece getPiece()
        {
            return this.pieceOnCell;
        }

        public override string ToString()
        {
            return pieceOnCell.ToString();
        }
    }

}
