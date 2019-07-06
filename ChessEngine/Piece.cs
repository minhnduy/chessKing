using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{   
   

    public class PieceType
    {
        private string pieceName;
        private int pieceValue;
        public static PieceType PAWN        = new PieceType("P", 100);
        public static PieceType ROOK        = new PieceType("R", 525);
        public static PieceType KNIGHT      = new PieceType("N", 350);
        public static PieceType BISHOP      = new PieceType("B", 350);
        public static PieceType QUEEN       = new PieceType("Q", 900);
        public static PieceType KING        = new PieceType("K", 1000);

        public PieceType(string name, int value)
        {
            this.pieceName = name;
            this.pieceValue = value;
        }

        public override string ToString()
        {
            return this.pieceName;
        }

        public bool EqualsTo(PieceType b)
        {
            return string.Compare(this.pieceName, b.pieceName) == 0;
        }

        public string getPieceName()
        {
            return this.pieceName;
        }

        public int getPieceValue()
        {
            return this.pieceValue;
        }
        
        
    }
    public abstract class Piece : ICloneable
    {
        protected int piecePosition;        
        protected Sides pieceSide;
        protected bool firstMove;
        protected PieceType type;        
        public int cachedHashCode;        
       
        public Piece(int position, Sides side, PieceType type, bool firstMove)
        {
            this.piecePosition = position;
            this.pieceSide = side;
            this.firstMove = firstMove;
            this.type = type;
            this.cachedHashCode = computeHashCode();

        }
     
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is Piece))
            {
                return false;
            }

            Piece otherPiece = obj as Piece;
            return this.piecePosition == otherPiece.getPiecePosition() &&
                   this.pieceSide == otherPiece.getSide() &&
                   this.type == otherPiece.getPieceType() &&
                   this.isFirstMove() == otherPiece.isFirstMove(); 
        }

        private int computeHashCode()
        {
            int result = this.type.GetHashCode();
            result = 31 * result + this.pieceSide.GetHashCode();
            result = 31 * result + this.piecePosition;
            result = 31 * result + (this.isFirstMove() ? 1 : 0);
            return result;
        }

        public override int GetHashCode()
        {
            return this.cachedHashCode;
        }

        public bool isWhite()
        {
            return this.pieceSide == Sides.WHITE;
        }

        public bool isBlack()
        {
            return this.pieceSide == Sides.BLACK;
        }

        public bool FirstMove
        {
            get
            {
                return firstMove;
            }
            set
            {
                firstMove = value;
            }
        }

        public bool isFirstMove()
        {
            return firstMove;
        }



        public bool isKing()
        {
            return this.type.EqualsTo(PieceType.KING);
        }

        public bool isPawn()
        {
            return this.type.EqualsTo(PieceType.PAWN);
        }

        public bool isKnight()
        {
            return this.type.EqualsTo(PieceType.KNIGHT);
        }

        public bool isQueen()
        {
            return this.type.EqualsTo(PieceType.QUEEN);
        }

        public bool isRook()
        {
            return this.type.EqualsTo(PieceType.ROOK);
        }

        public bool isBishop()
        {
            return this.type.EqualsTo(PieceType.BISHOP);
        }

        public int getPiecePosition()
        {
            return this.piecePosition;
        }

        public PieceType getPieceType()
        {
            return this.type;
        }

        public Sides getSide()
        {
            return this.pieceSide;
        }

        public int getPieceValue()
        {
            return this.type.getPieceValue();
        }


        //Get all legal moves of this piece
        public abstract List<Move> getLegalMoves(Board board);


        //Get the next piece after move this piece
        public abstract Piece movePiece(Move move);

        public abstract object Clone();
    }
}
