using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Queen : Piece
    {
        protected static int[] legalMoveArguments = { -9, -8, -7, -1, 1, 7, 8, 9 };
        public Queen(int position, Sides side, bool firstMove)
            : base(position, side, PieceType.QUEEN, firstMove)
        {            
        }

        public override List<Move> getLegalMoves(Board board)
        {
            List<Move> legalMoves = new List<Move>();
            foreach (int argument in Queen.legalMoveArguments)
            {
                int unCheckedPosition = this.piecePosition;

                while (BoardUtils.checkedForLegalPosition(unCheckedPosition))
                {
                    if (Queen.firstColumnViolation(unCheckedPosition, argument) ||
                        Queen.eightColumnViolation(unCheckedPosition, argument))
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

        private static bool firstColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 0 && ((argument == -1 || argument == -9 || argument == 7));
        }
            
        private static bool eightColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 7 && ((argument == 1) || (argument == -7) || (argument == 9));
        }

        public override string ToString()
        {
            return this.type.ToString();
        }
        public override Piece movePiece(Move move)
        {
            return new Queen(move.DesCoordinate, move.MovePiece.getSide(), false);
        }

        public override object Clone()
        {
            return new Queen(this.piecePosition, this.pieceSide, this.isFirstMove());
        }
    }
}
