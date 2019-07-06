using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public abstract class Move
    {
        protected Board board;
        public Board CurrentBoard
        {
            get
            {
                return this.board;
            }
        }

        protected Piece piece;
        public Piece MovePiece
        {
            get
            {
                return this.piece;
            }
        }
        public int CurCoordinate
        {
            get
            {
                return this.MovePiece.getPiecePosition();
            }
        }

        protected int desCoordinate;
        public int DesCoordinate
        {
            get
            {
                return this.desCoordinate;
            }
        }

        public static NullMove NULL_MOVE = new NullMove();

        public Move(Board board, Piece piece, int desCoordinate)
        {
            this.board = board;
            this.piece = piece;
            this.desCoordinate = desCoordinate;
        }

        public override int GetHashCode()
        {
            int result = 1 * 31 + this.desCoordinate;
            result = 31 * result + this.piece.GetHashCode();
            result = 31 * result + this.piece.getPiecePosition();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(obj is Move))
                return false;
            Move move = obj as Move;

            return this.getCurrentCoordinate() == move.getCurrentCoordinate() &&
                   this.DesCoordinate == move.DesCoordinate &&
                   this.MovePiece.Equals(move.MovePiece);

        }      
              

        public virtual Board Execute()
        {
            Builder builder = new Builder();                        
            //Set piece that not move piece to board
            foreach (Piece activePiece in this.board.CurrentPlayer.getActivePieces())
            {
                if (!this.piece.Equals(activePiece))
                {
                    builder.setPiece((Piece)activePiece.Clone());
                }
            }

            foreach (Piece activePiece in this.board.CurrentPlayer.getOpponent().getActivePieces())
            {
                builder.setPiece((Piece)activePiece.Clone());
            }

            //move the moved piece;
            builder.setPiece(this.piece.movePiece(this));
            builder.setMoveMaker(this.board.CurrentPlayer.getOpponent().getAlliance());
            return builder.build();
        }

        public virtual int getCurrentCoordinate()
        {
            return this.piece.getPiecePosition();
        }

        public virtual bool isAttack()
        {
            return false;
        }

        public virtual bool isCastlingMove()
        {
            return false;
        }

        public virtual Piece getAttackedPiece()
        {
            return null;
        }

        public virtual bool isPromote()
        {
            return false;
        }
       

        public virtual Board undo()
        {
            Builder builder = new Builder();
            foreach (Piece activePiece in this.board.getAllActivePieces())
            {
                builder.setPiece((Piece)activePiece.Clone());
            }
           
            builder.setMoveMaker(this.board.CurrentPlayer.getAlliance());
            return builder.build();
        }
             
    }


    //-------------------------------------Normal move---------------------------------------
    public class NormalMove : Move
    {
        public NormalMove(Board board, Piece piece, int desCoordinate) : 
            base(board, piece, desCoordinate) 
        {
        }

        public override bool Equals(object obj)
        {
            return this==obj || obj is NormalMove && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.piece.getPieceType().getPieceName() + BoardUtils.getPositionAtCoordinate(this.getCurrentCoordinate()) + " "
                                                            + BoardUtils.getPositionAtCoordinate(this.DesCoordinate);
        }
    }


    //-------------------------------------Attack move---------------------------------------
    public class AttackMove : Move
    {
        private Piece attackedPiece;
        public AttackMove(Board board, Piece piece, int desCoordinate, Piece attackedPiece) : 
            base(board, piece, desCoordinate)
        {
            this.attackedPiece = attackedPiece;
        }

        public override int GetHashCode()
        {
            return this.attackedPiece.GetHashCode() + base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(obj is AttackMove))
                return false;
            AttackMove attackMove = obj as AttackMove;
            return base.Equals(attackMove) &&
                   this.getAttackedPiece().Equals(attackMove.getAttackedPiece());
        }      

        public override bool isAttack()
        {
            return true;
        }

        public override Piece getAttackedPiece()
        {
            return this.attackedPiece;
        }

        public override string ToString()
        {
            return this.MovePiece.getPieceType().getPieceName() + BoardUtils.getPositionAtCoordinate(this.getCurrentCoordinate())
                                                                     + " " + this.getAttackedPiece().getPieceType().getPieceName()
                                                                     + BoardUtils.getPositionAtCoordinate(this.DesCoordinate);
                                                                     
        }
    }

    //-------------------------------------Pawn move-----------------------------------------
    public class PawnMove : Move
    {
        public PawnMove(Board board, Piece piece, int desCoordinate) : 
            base(board, piece, desCoordinate)
        {            
        }

        public override bool Equals(object obj)
        {
            return this == obj || obj is PawnMove && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return BoardUtils.getPositionAtCoordinate(this.DesCoordinate);
        }

    }

    ////-----------------------------------Pawn attack move----------------------------------
    public class PawnAttackMove : AttackMove
    {
        public PawnAttackMove(Board board, Piece piece, int desCoordinate, Piece attackedPiece)
            : base(board, piece, desCoordinate, attackedPiece)
        {

        }

        public override bool Equals(object obj)
        {
            return this == obj || obj is PawnAttackMove && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return BoardUtils.getPositionAtCoordinate(getCurrentCoordinate()) + " " + this.getAttackedPiece().getPieceType().getPieceName() 
                                                     + BoardUtils.getPositionAtCoordinate(this.DesCoordinate);
        }
    }

    ////-----------------------------------Pawn Enpassant attack move------------------------
    public class PawnEnPassantAttackMove : PawnAttackMove
    {
        public PawnEnPassantAttackMove(Board board, Piece piece, int desCoordinate, Piece attackedPiece)
            : base(board, piece, desCoordinate, attackedPiece)
        {

        }

        public override bool Equals(object obj)
        {
            return this == obj || obj is PawnEnPassantAttackMove && base.Equals(obj);
        }

        public override int GetHashCode()
        {
 	        return base.GetHashCode();
        }

        public override Board Execute()
        {
            Builder builder = new Builder();
            foreach (Piece activePiece in this.board.CurrentPlayer.getActivePieces())
            {
                if (!this.piece.Equals(activePiece))
                    builder.setPiece((Piece)activePiece.Clone());
            }

            foreach (Piece activePiece in this.board.CurrentPlayer.getOpponent().getActivePieces())
            {   
                if (!this.getAttackedPiece().Equals(activePiece))
                    builder.setPiece((Piece)activePiece.Clone());
            }

            builder.setPiece(this.piece.movePiece(this));
            builder.setMoveMaker(this.board.CurrentPlayer.getOpponent().getAlliance());
            return builder.build();
        }

        public override Board undo()
        {
            Builder builder = new Builder();
            foreach (Piece activePiece in this.board.getAllActivePieces())
            {
                builder.setPiece((Piece)activePiece.Clone());
            }
            builder.setEnPassantPawn(this.board.getEnPassantPawn());
            builder.setMoveMaker(this.board.CurrentPlayer.getAlliance());
            return builder.build();
        }
        public override string ToString()
        {
            return BoardUtils.getPositionAtCoordinate(this.getCurrentCoordinate()) + " " + this.getAttackedPiece().getPieceType().getPieceName()
                                                                                   + BoardUtils.getPositionAtCoordinate(this.getAttackedPiece().getPiecePosition());
        }
    }
        
    ////-----------------------------------Pawn jump-----------------------------------------
    public class PawnJump : Move
    {
        public PawnJump(Board board, Piece piece, int desCoordinate)
            : base(board, piece, desCoordinate)
        {

        }

        public override bool Equals(object obj)
        {
            return this == obj || obj is PawnJump && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Board Execute()
        {
            Builder builder = new Builder();
            foreach(Piece activePiece in this.board.CurrentPlayer.getActivePieces())
            {
                if (!this.piece.Equals(activePiece))
                {
                    builder.setPiece((Piece)activePiece.Clone());
                }
            }

            foreach (Piece piece in this.board.CurrentPlayer.getOpponent().getActivePieces())
                builder.setPiece(piece);

            Pawn movePawn = (Pawn)this.piece.movePiece(this);
            builder.setPiece(movePawn);
            builder.setEnPassantPawn((Pawn)movePawn.Clone());
            builder.setMoveMaker(this.board.CurrentPlayer.getOpponent().getAlliance());
            return builder.build();
        }

        public override string ToString()
        {
            return BoardUtils.getPositionAtCoordinate(this.DesCoordinate);
        }
    }

    ////-----------------------------------Pawn promotion-----------------------------------------
    public class PawnPromotionMove : Move
    {
        private Move move;
        private Pawn promotedPawn;
        private Piece promotedPiece;
        public PawnPromotionMove(Move move, Piece promotedPiece = null)
            : base(move.CurrentBoard, move.MovePiece, move.DesCoordinate)
        {
            this.move = move;
            this.promotedPawn = (Pawn)move.MovePiece;
            if (promotedPiece == null)
                this.promotedPiece = this.promotedPawn.getPromotionPiece();
            else
                this.promotedPiece = promotedPiece;
        }

        public override bool Equals(object obj)
        {
            return this == obj || obj is PawnPromotionMove && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.move.GetHashCode() + (31 * this.promotedPawn.GetHashCode()) ;
        }

        public override string ToString()
        {
            return move.ToString() + " -> " + this.promotedPiece.getPieceType().getPieceName();
        }

        public override bool isPromote()
        {
            return true;
        }

        public override Board Execute()
        {
            Board pawnBoard = this.move.Execute();
            Builder builder = new Builder();
            foreach (Piece activePiece in pawnBoard.CurrentPlayer.getActivePieces())
            {
                if (!this.promotedPawn.Equals(activePiece))
                {
                    builder.setPiece((Piece)activePiece.Clone());
                }
            }

            foreach (Piece activePiece in pawnBoard.CurrentPlayer.getOpponent().getActivePieces())
            {                            
                    builder.setPiece((Piece)activePiece.Clone());                
            }

            builder.setPiece(this.promotedPiece.movePiece(this));
            builder.setMoveMaker(pawnBoard.CurrentPlayer.getAlliance());
            return builder.build();
        }

        public void setPromotedPiece(PieceType type)
        {
            this.promotedPiece = promotedPawn.getPromotionPiece(type);
        }
    }

    ////-----------------------------------Castle move---------------------------------------
    public class CastleMove : Move
    {

        private Rook castleRook;
        private int castleRookStart;
        private int castleRookDes;
        public CastleMove(Board board, Piece piece, int desCoordinate, Rook castleRook, int start, int des) :
            base(board, piece, desCoordinate)
        {
            this.castleRook = castleRook;
            this.castleRookStart = start;
            this.castleRookDes = des;
        }


        public Rook getCastleRook()
        {
            return this.castleRook;
        }

        public override bool isCastlingMove()
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(this is CastleMove))
                return false;
            CastleMove move = (CastleMove)obj;

            return base.Equals(obj) && this.castleRook.Equals(move.getCastleRook());
        }

        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = 31 * result + this.castleRook.GetHashCode();
            result = 31 * result + this.castleRookDes;
            return result;
        }

        public override Board Execute()
        {
            Builder builder = new Builder();
            //Set piece that not move piece to board
            foreach (Piece activePiece in this.board.CurrentPlayer.getActivePieces())
            {
                if (!this.piece.Equals(activePiece) && !this.castleRook.Equals(activePiece))
                {
                    builder.setPiece((Piece)activePiece.Clone());
                }
            }

            foreach (Piece activePiece in this.board.CurrentPlayer.getOpponent().getActivePieces())
            {
                builder.setPiece((Piece)activePiece.Clone());
            }

            builder.setPiece(this.piece.movePiece(this));            
            builder.setPiece(new Rook(this.castleRookDes,this.castleRook.getSide(), false));
            builder.setMoveMaker(this.board.CurrentPlayer.getOpponent().getAlliance());
            return builder.build();
        }
    }

    ////-----------------------------------Kingside castle move------------------------------
    public class KingSideCastleMove : CastleMove
    {
         public KingSideCastleMove(Board board, Piece piece, int desCoordinate, Rook castleRook, int start, int des) :
            base(board, piece, desCoordinate, castleRook, start, des)
        {

        }

         public override bool Equals(object obj)
         {
             return  this == obj || obj is KingSideCastleMove && base.Equals(obj);
         }

         public override int GetHashCode()
         {
             return base.GetHashCode();
         }

        public override string ToString()
        {
            return "O-O";
        }
    }

    ////-----------------------------------Queenside castle move-----------------------------
    public class QueenSideCastleMove : CastleMove
    {
        public QueenSideCastleMove(Board board, Piece piece, int desCoordinate, Rook castleRook, int start, int des) :
            base(board, piece, desCoordinate, castleRook, start, des)
        {
        }

        public override bool Equals(object obj)
        {
            return this == obj || obj is QueenSideCastleMove && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "O-O-O";
        }
    }

    ////-----------------------------------Null move------------------------------
    public class NullMove : Move
    {
        public NullMove() :
            base(null, null, - 1)
        {
        }

        public override Board Execute()
        {
            throw new Exception();
        }

        public override int getCurrentCoordinate()
        {
            return -1;
        }
        
    }

    public class MoveFactory
    {
        private MoveFactory()
        {
            throw new Exception();
        }

        public static Move createMove(Board board, int currentCoordinate, int desCoordinate)
        {           
            foreach (Move move in board.getAllLegalMoves())
            {
                if (move.DesCoordinate == desCoordinate &&
                    move.getCurrentCoordinate() == currentCoordinate)
                    return move;
            }
            return Move.NULL_MOVE;
        }
    }

}
