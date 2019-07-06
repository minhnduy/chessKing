using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Knight : Piece
    {
        protected static int[]  legalMoveArguments = { -17, -15, -10, -6, 6, 10, 15, 17 };
        public Knight(int position, Sides side, bool firstMove)
            : base(position, side, PieceType.KNIGHT, firstMove)
        {            
        }

        public override List<Move> getLegalMoves(Board board)
        {
            List<Move> legalMoves = new List<Move>();
            foreach (int argument in Knight. legalMoveArguments)
            {
                int unCheckedPosition = this.piecePosition + argument;
                if (!BoardUtils.checkedForLegalPosition(unCheckedPosition) ||
                    Knight.firstColumnViolation(this.piecePosition, argument) ||
                    Knight.secondColumnViolation(this.piecePosition, argument) ||
                    Knight.seventhColumnViolation(this.piecePosition, argument) ||
                    Knight.eightColumnViolation(this.piecePosition, argument))
                    continue;
                else
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
                    }
                }
            }
            return  legalMoves;
        }

        private static bool firstColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 0 && ((argument == -17) || (argument == -10) || (argument == 6) || (argument == 15));
        }
        private static bool secondColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 1 && ((argument == -10) || (argument == 6));
        }
        private static bool seventhColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 6 && ((argument == -6) || (argument == 10));
        }
        private static bool eightColumnViolation(int piecePosition, int argument)
        {
            return piecePosition % 8 == 7 && ((argument == -15) || (argument == -6) || (argument == 10) || (argument == 17));
        }

        public override string ToString()
        {
            return this.type.ToString();
        }

        public override Piece movePiece(Move move)
        {
            return new Knight(move.DesCoordinate, move.MovePiece.getSide(), false);
        }

        public override object Clone()
        {
            return new Knight(this.piecePosition, this.pieceSide, this.isFirstMove());
        }
    }
}
