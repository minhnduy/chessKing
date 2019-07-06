using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    //The builder to resemble the piece into board
    public class Builder
    {
        private Dictionary<int, Piece> boardConfig;
        private Sides nextMoveMaker;
        private Pawn enPassantPawn;

        public Pawn EnPassantPawn
        {
            get
            {
                return this.enPassantPawn;
            }
        }
        public Builder()
        {
            boardConfig = new Dictionary<int, Piece>();
        }


        //Set the pieces to their acording cells
        public Builder setPiece(Piece piece)
        {
            this.boardConfig[piece.getPiecePosition()] = piece;
            return this;
        }


        //Determine who will execute next move
        public Builder setMoveMaker(Sides nextMoveMaker)
        {
            this.nextMoveMaker = nextMoveMaker;
            return this;
        }

        public Piece getPiece(int position)
        {
            Piece piece = null;
            return this.boardConfig.TryGetValue(position, out piece) ? piece : null;
        }

        public Board build()
        {
            return new Board(this);
        }

        public Player choosePlayer(Player whitePlayer, Player blackPlayer)
        {
            if (nextMoveMaker == Sides.WHITE)
                return whitePlayer;
            else
                return blackPlayer;
        }

        public void setEnPassantPawn(Pawn pawn)
        {
            this.enPassantPawn = pawn;
        }
    }
}
