using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Rook : Piece
    {
        protected static int[] legalMoveArguments = { -8, -1, 1, 8 };
        public Rook(int position, Sides side, bool firstMove)
            : base(position, side, PieceType.ROOK, firstMove)
        {            
        }
        public override List<Move> getLegalMoves(Board board)
        {
            List<Move> legalMoves = new List<Move>();
            foreach (int argument in Rook.legalMoveArguments)
            {
                int unCheckedPosition = this.piecePosition;

                while (BoardUtils.checkedForLegalPosition(unCheckedPosition))
                {
                    if (Rook.firstColumnViolation(unCheckedPosition, argument) ||
                        Rook.eightColumnViolation(unCheckedPosition, argument))
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
            return piecePosition % 8 == 0 && argument == -1;
        }
        private static bool eightColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 7 && argument == 1;
        }
        public override string ToString()
        {
            return this.type.ToString();
        }

        public override Piece movePiece(Move move)
        {
            return new Rook(move.DesCoordinate, move.MovePiece.getSide(), false);
        }

        public override object Clone()
        {
            return new Rook(this.piecePosition, this.pieceSide, this.isFirstMove());
        }
    }
}
