using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{

    public enum Sides
    {
        WHITE,
        BLACK
    }
    public abstract class Player
    {
        protected Board board;
        protected King playerKing;
        protected List<Move> legalMoves;
        private bool inCheck;
        
        public Player(Board board, List<Move> legalMoves, List<Move> opponentMoves)
        {
            this.board = board;
            this.playerKing = establishKing();
            this.legalMoves = legalMoves;
            this.legalMoves.AddRange(calculateKingCastles(legalMoves, opponentMoves));
            this.inCheck = Player.calculateAttacksCells(this.playerKing.getPiecePosition(), opponentMoves).Any();            
        }

        //Cacualte how many attack moves of enemy's moves on this cell
        protected static List<Move> calculateAttacksCells(int position, List<Move> opponentMoves)
        {
            List<Move> attackMoves = new List<Move>();
            foreach (Move move in opponentMoves)
            {
                if (position == move.DesCoordinate)
                    attackMoves.Add(move);
            }
            return attackMoves;
        }
       
        //Determine player's king position on the board
        private King establishKing()
        {
            foreach (Piece piece in getActivePieces())
            {
                if (piece.isKing())
                {
                    return (King)piece;
                }
            }
            return null;
        }

        public King getPlayerKing()
        {
            return this.playerKing;
        }

        public List<Move> getLegalMoves()
        {
            return this.legalMoves;
        }

        
        public abstract List<Piece> getActivePieces();

        //Get the player alliance (Black or white)  ?
        public abstract Sides getAlliance();
        public abstract Player getOpponent();

        public bool isMoveLegal(Move move)
        {
            return legalMoves.Contains(move);
        }

        //Check for player is in check
        public bool isInCheck()
        {
            return this.inCheck;
        }

        //Check for player is in checkmate
        public bool isCheckMate()
        {
            return this.isInCheck() && !hasEscapedMoves();
        }


        //Determine if player still have escape moves for the king or not
        private bool hasEscapedMoves()
        {
            foreach (Move move in this.legalMoves)
            {
                MoveTransition transition = this.makeMove(move);
                if (transition.getMoveStatus().isDone())
                {
                    return true;
                }
            }
            return false;
        }

        public bool isStaleMate()
        {
            return !this.isInCheck() && !hasEscapedMoves();
        }

        public bool isCastle()
        {
            return false;
        }


        //Player execute the move
        public MoveTransition makeMove(Move move)
        {
            if (!this.isMoveLegal(move))
            {
                return new MoveTransition(this.board, this.board, move, MoveStatus.ILLEGAL_MOVE);
            }
            Board transBoard = move.Execute();
            List<Move> kingAttacks = Player.calculateAttacksCells(transBoard.CurrentPlayer.getOpponent().getPlayerKing().getPiecePosition(),
                transBoard.CurrentPlayer.getLegalMoves());
            if (kingAttacks.Any() == true )
            {
                return new MoveTransition(this.board, this.board, move, MoveStatus.LEAVES_PLAYER_IN_CHECK);
            }
            return new MoveTransition(this.board, transBoard, move, MoveStatus.DONE);
        }


        //Determine if player's king can execute castling or not
        public abstract List<Move> calculateKingCastles(List<Move> playerLegalMoves, List<Move> opponentLegalMoves);
    }

    // --------------------------White Player implementation----------------------------------
    public class WhitePlayer : Player
    {
        public WhitePlayer(Board board, List<Move> whiteStandardLegalMoves, List<Move> blackStandardLegalMoves)
            : base(board, whiteStandardLegalMoves, blackStandardLegalMoves)
        {

        }

        public override List<Piece> getActivePieces()
        {
            return this.board.WhitePieces;
        }

        public override Sides getAlliance()
        {
            return Sides.WHITE;
        }

        public override Player getOpponent()
        {
            return this.board.BlackPlayer;
        }

        public override List<Move> calculateKingCastles(List<Move> playerLegalMoves, List<Move> opponentLegalMoves)
        {
            List<Move> kingCastles = new List<Move>();
            
            if (this.playerKing.isFirstMove() && this.isInCheck() == false)
            {
                //Check for white king side castle
                if (!this.board.getCell(61).isCellOccupied() && !this.board.getCell(62).isCellOccupied())
                {
                    Cell rookCell = this.board.getCell(63);
                    if (rookCell.isCellOccupied() && rookCell.getPiece().isFirstMove())
                    {
                        if (Player.calculateAttacksCells(61, opponentLegalMoves).Any() == false &&
                            Player.calculateAttacksCells(62, opponentLegalMoves).Any() == false &&
                            rookCell.getPiece().isRook())
                        {
                            kingCastles.Add(new KingSideCastleMove(this.board,
                                                                   this.playerKing,
                                                                   62,
                                                                   (Rook)rookCell.getPiece(), 
                                                                   rookCell.getCellCoordinate(),
                                                                   61));
                        }                        
                    }
                }

                //Check for white queen side castle
                if (!this.board.getCell(59).isCellOccupied() && 
                    !this.board.getCell(58).isCellOccupied() && 
                    !this.board.getCell(57).isCellOccupied())
                {
                    Cell rookCell = this.board.getCell(56);
                    if (rookCell.isCellOccupied() && rookCell.getPiece().isFirstMove())
                    {
                        if (Player.calculateAttacksCells(59,opponentLegalMoves).Any() == false &&
                            Player.calculateAttacksCells(58,opponentLegalMoves).Any() == false &&                            
                            rookCell.getPiece().isRook())
                        {
                            kingCastles.Add(new QueenSideCastleMove(this.board,
                                                                    this.playerKing,
                                                                    58,
                                                                    (Rook)rookCell.getPiece(),
                                                                    rookCell.getCellCoordinate(),
                                                                    59));
                        }
                    }
                }
                    
            }

            return kingCastles;
        }
    }

    // --------------------------Black Player implementation-----------------------------------
    public class BlackPlayer : Player
    {
        public BlackPlayer(Board board, List<Move> whiteStandardLegalMoves, List<Move> blackStandardLegalMoves)
            : base(board, blackStandardLegalMoves, whiteStandardLegalMoves)
        {

        }

        public override List<Piece> getActivePieces()
        {
            return this.board.BlackPieces;
        }

        public override Sides getAlliance()
        {
            return Sides.BLACK;
        }

        public override Player getOpponent()
        {
            return this.board.WhitePlayer;
        }

        public override List<Move> calculateKingCastles(List<Move> playerLegalMoves, List<Move> opponentLegalMoves)
        {
            List<Move> kingCastles = new List<Move>();

            if (this.playerKing.isFirstMove() && this.isInCheck() == false)
            {
                //Check for black king side castle
                if (!this.board.getCell(5).isCellOccupied() && !this.board.getCell(6).isCellOccupied())
                {
                    Cell rookCell = this.board.getCell(7);
                    if (rookCell.isCellOccupied() && rookCell.getPiece().isFirstMove())
                    {
                        if (Player.calculateAttacksCells(5, opponentLegalMoves).Any() == false &&
                            Player.calculateAttacksCells(6, opponentLegalMoves).Any() == false &&
                            rookCell.getPiece().isRook())
                        {
                            kingCastles.Add(new KingSideCastleMove(this.board,
                                                                   this.playerKing,
                                                                   6,
                                                                   (Rook)rookCell.getPiece(),
                                                                   rookCell.getCellCoordinate(),
                                                                   5));
                        }
                    }
                }

                //Check for white queen side castle
                if (!this.board.getCell(1).isCellOccupied() && 
                    !this.board.getCell(2).isCellOccupied() && 
                    !this.board.getCell(3).isCellOccupied())
                {
                    Cell rookCell = this.board.getCell(0);
                    if (rookCell.isCellOccupied() && rookCell.getPiece().isFirstMove())
                    {
                        if (Player.calculateAttacksCells(3, opponentLegalMoves).Any() == false &&
                            Player.calculateAttacksCells(2, opponentLegalMoves).Any() == false &&                            
                            rookCell.getPiece().isRook())
                        {
                            kingCastles.Add(new QueenSideCastleMove(this.board,
                                                                    this.playerKing,
                                                                    2,
                                                                    (Rook)rookCell.getPiece(),
                                                                    rookCell.getCellCoordinate(),
                                                                    3));
                        }
                    }
                }

            }

            return kingCastles;
        }
    }
}
