using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Pawn : Piece
    {
        private int direction;        
        protected static int[] legalMoveArguments = { 8, 16, 7, 9 };
        public Pawn(int position, Sides side, bool firstMove)
            : base(position, side, PieceType.PAWN, firstMove)
        {
            if (side == Sides.BLACK)
                this.direction = 1;
            else
                this.direction = -1;                        
        }

        private bool isPromotionSquare(int position)
        {
            if (this.pieceSide == Sides.WHITE)
                return position / 8 == 0;
            else
                return position / 8 == 7;
        }

        public override List<Move> getLegalMoves(Board board)
        {
            List<Move> legalMoves = new List<Move>();
            foreach (int argument in legalMoveArguments)
            {
                int unCheckedPosition = this.piecePosition + (this.direction * argument);

                if (!BoardUtils.checkedForLegalPosition(unCheckedPosition))
                    continue;

                if (argument == 8 && !board.getCell(unCheckedPosition).isCellOccupied())
                {                    
                    if (this.isPromotionSquare(unCheckedPosition))
                    {
                        legalMoves.Add(new PawnPromotionMove(new NormalMove(board, this, unCheckedPosition), this.getPromotionPiece(PieceType.ROOK)));
                        legalMoves.Add(new PawnPromotionMove(new NormalMove(board, this, unCheckedPosition), this.getPromotionPiece(PieceType.KNIGHT)));
                        legalMoves.Add(new PawnPromotionMove(new NormalMove(board, this, unCheckedPosition), this.getPromotionPiece(PieceType.BISHOP)));
                        legalMoves.Add(new PawnPromotionMove(new NormalMove(board, this, unCheckedPosition), this.getPromotionPiece()));
                    }
                    else
                    {
                        legalMoves.Add(new PawnMove(board, this, unCheckedPosition));
                    }
                }

                if (argument == 16 && this.isFirstMove() && 
                    (this.piecePosition / 8 == 1 && this.isBlack() || 
                     this.piecePosition / 8 == 6 && this.isWhite()))
                {
                    int behindPosition = this.piecePosition + (this.direction * 8);
                    if (!board.getCell(unCheckedPosition).isCellOccupied() &&
                        !board.getCell(behindPosition).isCellOccupied())
                    {
                        legalMoves.Add(new PawnJump(board, this, unCheckedPosition));
                    }
                }

                if (argument == 9 && 
                    !((this.piecePosition % 8 == 7 && this.isBlack() ||
                    (this.piecePosition % 8 == 0 && this.isWhite()))))
                {
                    if (board.getCell(unCheckedPosition).isCellOccupied())
                    {
                        Piece occupiedPiece = board.getCell(unCheckedPosition).getPiece();
                        if (occupiedPiece.getSide() != this.getSide())
                        {
                            if (this.isPromotionSquare(unCheckedPosition))
                            {
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition,occupiedPiece), this.getPromotionPiece(PieceType.ROOK)));
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece), this.getPromotionPiece(PieceType.KNIGHT)));
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition,occupiedPiece), this.getPromotionPiece(PieceType.BISHOP)));
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece), this.getPromotionPiece()));
                            }
                            else
                            {
                                legalMoves.Add(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece));
                            }
                            
                        }
                    }
                    else if(board.getEnPassantPawn() != null)
                    {
                        if (board.getEnPassantPawn().getPiecePosition() == this.piecePosition - (this.direction * -1))
                        {
                            Piece enPassantPiece = board.getEnPassantPawn();
                            if (this.getSide() != enPassantPiece.getSide())
                                legalMoves.Add(new PawnEnPassantAttackMove(board, this, unCheckedPosition, enPassantPiece));
                        }
                    }
                }

                if (argument == 7 &&
                    !((this.piecePosition % 8 == 0 && this.isBlack() ||
                    (this.piecePosition % 8 == 7 && this.isWhite()))))
                {
                    if (board.getCell(unCheckedPosition).isCellOccupied())
                    {
                        Piece occupiedPiece = board.getCell(unCheckedPosition).getPiece();
                        if (occupiedPiece.getSide() != this.getSide())
                        {
                            if (this.isPromotionSquare(unCheckedPosition))
                            {
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece), this.getPromotionPiece(PieceType.ROOK)));
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece), this.getPromotionPiece(PieceType.KNIGHT)));
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece), this.getPromotionPiece(PieceType.BISHOP)));
                                legalMoves.Add(new PawnPromotionMove(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece), this.getPromotionPiece()));
                            }
                            else
                            {
                                legalMoves.Add(new PawnAttackMove(board, this, unCheckedPosition, occupiedPiece));
                            }
                        }
                    }
                    else if (board.getEnPassantPawn() != null)
                    {
                        if (board.getEnPassantPawn().getPiecePosition() == this.piecePosition + (this.direction * -1))
                        {
                            Piece enPassantPiece = board.getEnPassantPawn();
                            if (this.getSide() != enPassantPiece.getSide())
                                legalMoves.Add(new PawnEnPassantAttackMove(board, this, unCheckedPosition, enPassantPiece));
                        }
                    }
                }                
            }
            return legalMoves;
        }
        public override string ToString()
        {
            return this.type.ToString();
        }

        public override Piece movePiece(Move move)
        {
            return new Pawn(move.DesCoordinate, move.MovePiece.getSide(), false);
        }

        public Piece getPromotionPiece(PieceType type)
        {
            if (type == PieceType.BISHOP)
                return new Bishop(this.piecePosition, this.pieceSide, false);
            else if (type == PieceType.ROOK)
                return new Rook(this.piecePosition, this.pieceSide, false);
            else if (type == PieceType.KNIGHT)
                return new Knight(this.piecePosition, this.pieceSide, false);
            
            return new Queen(this.piecePosition, this.pieceSide, false);
        }

        public Piece getPromotionPiece()
        {
            return new Queen(this.piecePosition, this.pieceSide, false);
        }

        public override object Clone()
        {
            return new Pawn(this.piecePosition, this.pieceSide, this.isFirstMove());
        }
    }
}
