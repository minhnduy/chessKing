using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Bishop : Piece
    {
        private static int[] legalMovesArguments = { -9, -7, 7, 9 };
        public Bishop(int position, Sides side, bool firstMove)
            : base(position, side, PieceType.BISHOP, firstMove)
        {            
        }

        public override List<Move> getLegalMoves(Board board)
        {
            List<Move> legalMoves = new List<Move>();
            foreach (int argument in Bishop.legalMovesArguments)
            {
                int unCheckedPosition = this.piecePosition;
              
                while (BoardUtils.checkedForLegalPosition(unCheckedPosition))
                {
                    if (firstColumnViolation(unCheckedPosition, argument) ||
                        eightColumnViolation(unCheckedPosition, argument))
                        break;

                    unCheckedPosition += argument;
                    if (BoardUtils.checkedForLegalPosition(unCheckedPosition))
                    {
                        
                        Cell currentCell = board.getCell(unCheckedPosition);
                        if (!currentCell.isCellOccupied())
                        {
                            legalMoves.Add(new NormalMove(board, this, unCheckedPosition));
                        }
                        else
                        {
                            if (this.pieceSide != currentCell.getPiece().getSide())
                            {
                                legalMoves.Add(new AttackMove(board, this, unCheckedPosition, currentCell.getPiece()));
                            }
                            break;
                        }
                    }
                }
                
            }
            return legalMoves;
        }

        public static bool firstColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 0 && ((argument == -9) || (argument == 7)); 
        }
        public static bool eightColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 7 && ((argument == -7) || (argument == 9));
        }

        public override string ToString()
        {
            return this.type.ToString();
        }

        public override Piece movePiece(Move move)
        {
            return new Bishop( move.DesCoordinate, move.MovePiece.getSide(), false);
        }

        public override object Clone()
        {
            return new Bishop(this.piecePosition, this.pieceSide, this.isFirstMove());
        }
    }
}
