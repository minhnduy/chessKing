using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class King : Piece
    {
        protected static int[] legalMoveArguments = { -9, -8, -7, -1, 1, 7, 8, 9 };
        public King(int position, Sides side, bool firstMove)
            : base(position, side, PieceType.KING, firstMove)
        {            
        }

        public override List<Move> getLegalMoves(Board board)
        {
            List<Move> legalMove = new List<Move>();
            foreach (int argument in King.legalMoveArguments)
            {
                int unCheckedPosition = this.piecePosition + argument;
                if (!BoardUtils.checkedForLegalPosition(unCheckedPosition) ||
                    King.firstColumnViolation(this.piecePosition, argument) ||
                    King.eightColumnViolation(this.piecePosition, argument))
                    continue;

                else
                {
                    Cell currentCell = board.getCell(unCheckedPosition);
                    if (!currentCell.isCellOccupied())
                    {
                        legalMove.Add(new NormalMove(board, this, unCheckedPosition));
                    }
                    else
                    {
                        if (this.pieceSide != currentCell.getPiece().getSide())
                        {
                            legalMove.Add(new AttackMove(board, this, unCheckedPosition, currentCell.getPiece()));
                        }
                    }
                }
            }
            if (board.CurrentPlayer != null)
            {
                List<Move> currentPlayerMove = board.CurrentPlayer.getLegalMoves();
                List<Move> opponentPlayerMove = board.CurrentPlayer.getOpponent().getLegalMoves();
                legalMove.AddRange(board.CurrentPlayer.calculateKingCastles(currentPlayerMove, opponentPlayerMove));
            }
            return legalMove;
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
            return new King(move.DesCoordinate, move.MovePiece.getSide(), false);
        }

        public override object Clone()
        {
            return new King(this.piecePosition, this.pieceSide, this.isFirstMove());
        }
    }
}
