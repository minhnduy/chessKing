using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Board
    {
                
        private List<Cell> gameBoard;

        private List<Piece> whitePieces;
        public List<Piece> WhitePieces
        {
            get
            {
                return this.whitePieces;
            }
        }

        private List<Piece> blackPieces;
        public List<Piece> BlackPieces
        {
            get
            {
                return this.blackPieces;
            }
        }

        private WhitePlayer whitePlayer;
        public Player WhitePlayer
        {
            get
            {
                return this.whitePlayer;
            }
        }

        private BlackPlayer blackPlayer;       
        public Player BlackPlayer
        {
            get
            {
                return this.blackPlayer;
            }
        }

        private Player currentPlayer;
        public Player CurrentPlayer
        {
            get
            {
                return this.currentPlayer;
            }
        }

        private List<Piece> activePieces;       
        private Pawn enPassantPawn;        
        public Board(Builder builder)
        {            
            this.gameBoard = this.createGameBoard(builder);
            this.whitePieces = this.countActivePieces(this.gameBoard,Sides.WHITE);
            this.blackPieces = this.countActivePieces(this.gameBoard, Sides.BLACK);

            this.activePieces = new List<Piece>();
            this.activePieces.AddRange(this.whitePieces);
            this.activePieces.AddRange(this.blackPieces);
            this.enPassantPawn = builder.EnPassantPawn;
            List<Move> whiteStandardLegalMoves = this.createLegalMoves(this.whitePieces);
            List<Move> blackStandardLegalMoves = this.createLegalMoves(this.blackPieces);


            //Set up player and their legal moves
            this.whitePlayer = new WhitePlayer(this, whiteStandardLegalMoves, blackStandardLegalMoves);
            this.blackPlayer = new BlackPlayer(this, whiteStandardLegalMoves, blackStandardLegalMoves);
            this.currentPlayer = builder.choosePlayer(whitePlayer, blackPlayer);
        }

        public Pawn getEnPassantPawn()
        {
            return this.enPassantPawn;
        }

        public Cell getCell(int cellPostion)
        {
            return this.gameBoard[cellPostion];
        }

        public static Board createStandardBoard()
        {
            Builder builder = new Builder();

            //Black Side
            builder.setPiece(new Rook(0, Sides.BLACK, true));
            builder.setPiece(new Knight(1,Sides.BLACK, true));
            builder.setPiece(new Bishop(2, Sides.BLACK, true));
            builder.setPiece(new Queen(3, Sides.BLACK, true));
            builder.setPiece(new King(4, Sides.BLACK, true));
            builder.setPiece(new Bishop(5, Sides.BLACK, true));
            builder.setPiece(new Knight(6, Sides.BLACK, true));
            builder.setPiece(new Rook(7, Sides.BLACK, true));
            builder.setPiece(new Pawn(8, Sides.BLACK, true));
            builder.setPiece(new Pawn(9, Sides.BLACK, true));
            builder.setPiece(new Pawn(10, Sides.BLACK, true));
            builder.setPiece(new Pawn(11, Sides.BLACK, true));
            builder.setPiece(new Pawn(12, Sides.BLACK, true));
            builder.setPiece(new Pawn(13, Sides.BLACK, true));
            builder.setPiece(new Pawn(14, Sides.BLACK, true));
            builder.setPiece(new Pawn(15, Sides.BLACK, true));
            //White Side

            builder.setPiece(new Pawn(48, Sides.WHITE, true));
            builder.setPiece(new Pawn(49, Sides.WHITE, true));
            builder.setPiece(new Pawn(50, Sides.WHITE, true));
            builder.setPiece(new Pawn(51, Sides.WHITE, true));
            builder.setPiece(new Pawn(52, Sides.WHITE, true));
            builder.setPiece(new Pawn(53, Sides.WHITE, true));
            builder.setPiece(new Pawn(54, Sides.WHITE, true));
            builder.setPiece(new Pawn(55, Sides.WHITE, true));
            builder.setPiece(new Rook(56, Sides.WHITE, true));
            builder.setPiece(new Knight(57, Sides.WHITE, true));
            builder.setPiece(new Bishop(58, Sides.WHITE, true));
            builder.setPiece(new Queen(59, Sides.WHITE, true));
            builder.setPiece(new King(60, Sides.WHITE, true));
            builder.setPiece(new Bishop(61, Sides.WHITE, true));
            builder.setPiece(new Knight(62, Sides.WHITE, true));
            builder.setPiece(new Rook(63, Sides.WHITE, true));
            builder.setMoveMaker(Sides.WHITE);
            return builder.build();
        }        
        public List<Cell> createGameBoard(Builder builder)
        {
            Cell[] cells = new Cell[BoardUtils.NUM_CELLS];
            for (int i = 0; i < BoardUtils.NUM_CELLS; i++)
                cells[i] = Cell.createCell(i, builder.getPiece(i));
            return cells.ToList<Cell>();
        }

        //get active pieces for chosen side
        public List<Piece> countActivePieces(List<Cell> board, Sides side)
        {
            List<Piece> activePieces = new List<Piece>();
            foreach (Cell cell in board)
            {
                if (cell.isCellOccupied() && cell.getPiece().getSide() == side)
                    activePieces.Add(cell.getPiece());
            }
            return activePieces;
        }

        //create legal move for active pieces on board
        public List<Move> createLegalMoves(List<Piece> pieces)
        {
            List<Move> moves = new List<Move>();
            foreach (Piece x in pieces)
            {
                moves.AddRange(x.getLegalMoves(this));
            }
            return moves;
        }

        //get all legal moves on the board
        public List<Move> getAllLegalMoves()
        {
            List<Move> moves = new List<Move>(whitePlayer.getLegalMoves());
            moves.AddRange(new List<Move>(blackPlayer.getLegalMoves()));
            return moves;
        }       
                                    
        public void setCurrentPlayer(Player player)
        {
            this.currentPlayer = player;
        }

        public List<Piece> getAllActivePieces()
        {
            return this.activePieces;
        }
    
        public bool isTheSame(Board board)
        {
            for (int i = 0; i < 64; i++)
            {
                if (!this.gameBoard[i].isCellOccupied() && board.gameBoard[i].isCellOccupied())
                    return false;
                if (this.gameBoard[i].isCellOccupied() && !board.gameBoard[i].isCellOccupied())
                    return false;
                if (!this.gameBoard[i].isCellOccupied() && !board.gameBoard[i].isCellOccupied())
                    continue;
                if (!this.gameBoard[i].getPiece().Equals(board.gameBoard[i].getPiece()))
                    return false;
            }
            if (this.CurrentPlayer.getAlliance() != board.CurrentPlayer.getAlliance())
                return false;
            return true;
        }
    }
}
