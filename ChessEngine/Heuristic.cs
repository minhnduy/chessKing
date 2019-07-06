using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Heuristic
    {
        private const int LIGHT = 0;
        private const int DARK = 1;
        private const int CHECK_BONUS = 50;
        private const int CHECK_MATE_BONUS = 10000;
        private const int DEPTH_BONUS = 100;
        private int[,,] pieceSquare = new int[,,] {  
            // King end game
            {{-50, -30, -30, -30, -30, -30, -30, -50},
            {-30, -30,  0,  0,  0,  0, -30, -30},
            {-30, -10, 20, 30, 30, 20, -10, -30},
            {-30, -10, 30, 40, 40, 30, -10, -30},
            {-30, -10, 30, 40, 40, 30, -10, -30},
            {-30, -10, 20, 30, 30, 20, -10, -30},
            {-30, -20, -10,  0,  0, -10, -20, -30},
            {-50, -40, -30, -20, -20, -30, -40, -50}},
            // King early game
            {{20, 30, 10,  0,  0, 10, 30, 20},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {-10, -20, -20,     -20, -20, -20, -20, -10},
            {-20, -30, -30, -40, -40, -30, -30, -20},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30}},
            // Queen
            {{-20, -10, -10, -5, -5, -10, -10, -20},
            {-10,  0,  5,  0,  0,  0,  0, -10},
            {-10,  5,  5,  5,  5,  5,  0, -10},
            {0,  0,  5,  5,  5,  5,  0, -5},
            {-5,  0,  5,  5,  5,  5,  0, -5},
            {-10,  0,  5,  5,  5,  5,  0, -10},
            {-10,  0,  0,  0,  0,  0,  0, -10},
            {-20, -10, -10, -5, -5, -10, -10, -20}},
            // Rook
            {{0,  0,  0,  5,  5,  0,  0,  0},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {5, 10, 10, 10, 10, 10, 10,  5},
            {0,  0,  0,  0,  0,  0,  0,  0}},
            // Knight
            {{-50, -40, -30, -30, -30, -30, -40, -50},
            {-40, -20,  0,  5,  5,  0, -20, -40},
            {-30,  5, 10, 15, 15, 10,  5, -30},
            {-30,  0, 15, 20, 20, 15,  0, -30},
            {-30,  5, 15, 20, 20, 15,  5, -30},
            {-30,  0, 10, 15, 15, 10,  0, -30},
            {-40, -20,  0,  0,  0,  0, -20, -40},
            {-50, -40, -30, -30, -30, -30, -40, -50}},
            // Bishop
            {{-20, -10, -10, -10, -10, -10, -10, -20},
            {-10,  5,  0,  0,  0,  0,  5, -10},
            {-10, 10, 10, 10, 10, 10, 10, -10},
            {-10,  0, 10, 10, 10, 10,  0, -10},
            {-10,  5,  5, 10, 10,  5,  5, -10},
            {-10,  0,  5, 10, 10,  5,  0, -10},
            {-10,  0,  0,  0,  0,  0,  0, -10},
            {-20, -10, -10, -10, -10, -10, -10, -20}},
            // Pawn
            {{0,  0,  0,  0,  0,  0,  0,  0},
            {5, 10, 10, -20, -20, 10, 10,  5},
            {5, -5, -10,  0,  0, -10, -5,  5},
            {0,  0,  0, 20, 20,  0,  0,  0},
            {5,  5, 10, 25, 25, 10,  5,  5},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {0,  0,  0,  0,  0,  0,  0,  0}}};
        private const int INITIAL_PIECE_MATERIAL = 3450;
        private int[,] dist_bonus = new int[64, 64];
        private int[] bonus_dia_distance = new int[] { 5, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private int[] diag_nw = {   0, 1, 2, 3, 4, 5, 6, 7,
                                    1, 2, 3, 4, 5, 6, 7, 8,
                                    2, 3, 4, 5, 6, 7, 8, 9,
                                    3, 4, 5, 6, 7, 8, 9,10,
                                    4, 5, 6, 7, 8, 9,10,11,
                                    5, 6, 7, 8, 9,10,11,12,
                                    6, 7, 8, 9,10,11,12,13,
                                    7, 8, 9,10,11,12,13,14  };
        private int[] diag_ne = {   7, 6, 5, 4, 3, 2, 1, 0,
                                    8, 7, 6, 5, 4, 3, 2, 1,
                                    9, 8, 7, 6, 5, 4, 3, 2,
                                    10, 9, 8, 7, 6, 5, 4, 3,
                                    11,10, 9, 8, 7, 6, 5, 4,
                                    12,11,10, 9, 8, 7, 6, 5,
                                    13,12,11,10, 9, 8, 7, 6,
                                    14,13,12,11,10, 9, 8, 7 };
        private int[,] pawn_rank = new int[2, 10];
        private int[] piece_mat = new int[2];
        private int[] pawn_mat = new int[2];
        private int DOUBLED_PAWN_PENALTY = 10;
        private int ISOLATED_PAWN_PENALTY = 20;
        private int BACKWARDS_PAWN_PENALTY = 8;
        private int PASSED_PAWN_BONUS = 20;
        private int ROW(int k)
        {
            return k / 8;
        }
        private int COL(int k)
        {
            return k % 8;
        }

        public Heuristic()
        {
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    dist_bonus[i, j] = 14 - (Math.Abs(COL(i) - COL(j)) + Math.Abs(ROW(i) - ROW(j)));
                }
            }
        }

        public int calculate(Board board, int depth)
        {
            for (int i = 0; i < 10; i++)
            {
                pawn_rank[LIGHT, i] = 0;
                pawn_rank[DARK, i] = 7;
            }
            piece_mat[LIGHT] = piece_mat[DARK] = pawn_mat[LIGHT] = pawn_mat[DARK] = 0;

            Piece WhiteKing = null, BlackKing = null;
            foreach(Piece piece in board.getAllActivePieces())
            {
                int color = Convert.ToInt32(piece.getSide());
                int sq = piece.getPiecePosition();
                PieceType type = piece.getPieceType();
                if (type == PieceType.KING)
                {
                    if (color == 0)
                        WhiteKing = piece;
                    else
                        BlackKing = piece;
                }
                else if (type == PieceType.PAWN)
                {
                    pawn_mat[color] += piece.getPieceValue();
                    int f = COL(sq) + 1;
                    if (color == LIGHT)
                    {
                        if (pawn_rank[LIGHT, f] < ROW(sq))
                            pawn_rank[LIGHT, f] = ROW(sq);
                    }
                    else
                    {
                        if (pawn_rank[DARK, f] > ROW(sq))
                            pawn_rank[DARK, f] = ROW(sq);
                    }
                }
                else
                    piece_mat[color] += piece.getPieceValue();
            }

            return pieceValueAndPosition(board) + mobility(board)
                + check(board) + checkmate(board, depth) + kingSafety(board, WhiteKing, BlackKing);
        }

        private int index(Piece piece, int numPieces)
        {
            PieceType type = piece.getPieceType();
            if (type == PieceType.KING)
            {
                if (numPieces < 8)
                    return 0;
                else
                    return 1;
            }
            else if (type == PieceType.QUEEN)
                return 2;
            else if (type == PieceType.ROOK)
                return 3;
            else if (type == PieceType.KNIGHT)
                return 4;
            else if (type == PieceType.BISHOP)
                return 5;
            else
                return 6;
        }

        int eval_light_pawn_structure(int sq)
        {
            int r = 0;  /* the value to return */
            int f = COL(sq) + 1;  /* the pawn's file */

            /* if there's a pawn behind this one, it's doubled */
            if (pawn_rank[LIGHT, f] > ROW(sq))
                r -= DOUBLED_PAWN_PENALTY;

            /* if there aren't any friendly pawns on either side of
               this one, it's isolated */
            if ((pawn_rank[LIGHT, f - 1] == 0) &&
                    (pawn_rank[LIGHT, f + 1] == 0))
                r -= ISOLATED_PAWN_PENALTY;

            /* if it's not isolated, it might be backwards */
            else if ((pawn_rank[LIGHT, f - 1] < ROW(sq)) &&
                    (pawn_rank[LIGHT, f + 1] < ROW(sq)))
                r -= BACKWARDS_PAWN_PENALTY;

            /* add a bonus if the pawn is passed */
            if ((pawn_rank[DARK, f - 1] >= ROW(sq)) &&
                    (pawn_rank[DARK, f] >= ROW(sq)) &&
                    (pawn_rank[DARK, f + 1] >= ROW(sq)))
                r += (7 - ROW(sq)) * PASSED_PAWN_BONUS;

            return r;
        }

        int eval_dark_pawn_structure(int sq)
        {
            int r = 0;  /* the value to return */
            int f = COL(sq) + 1;  /* the pawn's file */

            /* if there's a pawn behind this one, it's doubled */
            if (pawn_rank[DARK, f] < ROW(sq))
                r -= DOUBLED_PAWN_PENALTY;

            /* if there aren't any friendly pawns on either side of
               this one, it's isolated */
            if ((pawn_rank[DARK, f - 1] == 7) &&
                    (pawn_rank[DARK, f + 1] == 7))
                r -= ISOLATED_PAWN_PENALTY;

            /* if it's not isolated, it might be backwards */
            else if ((pawn_rank[DARK, f - 1] > ROW(sq)) &&
                    (pawn_rank[DARK, f + 1] > ROW(sq)))
                r -= BACKWARDS_PAWN_PENALTY;

            /* add a bonus if the pawn is passed */
            if ((pawn_rank[LIGHT, f - 1] <= ROW(sq)) &&
                    (pawn_rank[LIGHT, f] <= ROW(sq)) &&
                    (pawn_rank[LIGHT, f + 1] <= ROW(sq)))
                r += ROW(sq) * PASSED_PAWN_BONUS;

            return r;
        }

        private int pieceValueAndPosition(Board board)
        {
            int value = 0;
            List<Piece> blackPieces = board.BlackPieces;
            List<Piece> whitePieces = board.WhitePieces;
            int numPieces = blackPieces.Count() + whitePieces.Count();
            foreach (Piece piece in blackPieces)
            {
                int pos = piece.getPiecePosition();
                value -= piece.getPieceValue();
                value -= pieceSquare[index(piece, numPieces), pos / 8, pos % 8];
                if(piece.getPieceType() == PieceType.PAWN)
                {
                    value -= eval_dark_pawn_structure(piece.getPiecePosition()); 
                }
            }
            foreach (Piece piece in whitePieces)
            {
                int pos = piece.getPiecePosition();
                value += piece.getPieceValue();
                value += pieceSquare[index(piece, numPieces), 7 - pos / 8, pos % 8];
                if(piece.getPieceType() == PieceType.PAWN)
                {
                    value += eval_light_pawn_structure(piece.getPiecePosition());
                }
            }
            return value;
        }

        private int mobility(Board board)
        {
            return board.WhitePlayer.getLegalMoves().Count()
                - board.BlackPlayer.getLegalMoves().Count();
        }

        private int check(Board board)
        {
            return (board.BlackPlayer.isInCheck() ? CHECK_BONUS : 0)
                - (board.WhitePlayer.isInCheck() ? CHECK_BONUS : 0);
        }

        private int depthBonus(int depth)
        {
            return (depth == 0) ? 1 : DEPTH_BONUS * depth;
        }
        private int checkmate(Board board, int depth)
        {
            return (board.BlackPlayer.isCheckMate() ? CHECK_MATE_BONUS * depthBonus(depth) : 0)
                - (board.WhitePlayer.isCheckMate() ? CHECK_MATE_BONUS * depthBonus(depth) : 0);
        }

        private int eval_lkp(int f)
        {
            int r = 0;

            if (pawn_rank[LIGHT, f] == 6) { }   /* pawn hasn't moved */
            else if (pawn_rank[LIGHT, f] == 5)
                r -= 10;  /* pawn moved one square */
            else if (pawn_rank[LIGHT, f] != 0)
                r -= 20;  /* pawn moved more than one square */
            else
                r -= 25;  /* no pawn on this file */

            if (pawn_rank[DARK, f] == 7)
                r -= 15;  /* no enemy pawn */
            else if (pawn_rank[DARK, f] == 5)
                r -= 10;  /* enemy pawn on the 3rd rank */
            else if (pawn_rank[DARK, f] == 4)
                r -= 5;   /* enemy pawn on the 4th rank */

            return r;
        }
        private int eval_light_king_shield(int sq)
        {
            int r = 0;
            /* if the king is castled, use a special function to evaluate the
	   pawns on the appropriate side */
            if (COL(sq) < 3)
            {
                r += eval_lkp(1);
                r += eval_lkp(2);
                r += eval_lkp(3) / 2;  /* problems with pawns on the c & f files
								  are not as severe */
            }
            else if (COL(sq) > 4)
            {
                r += eval_lkp(8);
                r += eval_lkp(7);
                r += eval_lkp(6) / 2;
            }

            /* otherwise, just assess a penalty if there are open files near
               the king */
            else {
                for (int i = COL(sq); i <= COL(sq) + 2; ++i)
                    if ((pawn_rank[LIGHT, i] == 0) &&
                            (pawn_rank[DARK, i] == 7))
                        r -= 10;
            }

            return r;
        }

        private int eval_dkp(int f)
        {
            int r = 0;

            if (pawn_rank[DARK, f] == 1) { }
            else if (pawn_rank[DARK, f] == 2)
                r -= 10;
            else if (pawn_rank[DARK, f] != 7)
                r -= 20;
            else
                r -= 25;

            if (pawn_rank[LIGHT, f] == 0)
                r -= 15;
            else if (pawn_rank[LIGHT, f] == 2)
                r -= 10;
            else if (pawn_rank[LIGHT, f] == 3)
                r -= 5;

            return r;
        }
        private int eval_dark_king_shield(int sq)
        {
            int r = 0;

            if (COL(sq) < 3)
            {
                r += eval_dkp(1);
                r += eval_dkp(2);
                r += eval_dkp(3) / 2;
            }
            else if (COL(sq) > 4)
            {
                r += eval_dkp(8);
                r += eval_dkp(7);
                r += eval_dkp(6) / 2;
            }
            else {
                for (int i = COL(sq); i <= COL(sq) + 2; ++i)
                    if ((pawn_rank[LIGHT, i] == 0) &&
                            (pawn_rank[DARK, i] == 7))
                        r -= 10;
            }

            return r;
        }
        
        private int tropism_to_white_king(Board board, int j)
        {
            int r = 0;
            foreach(Piece piece in board.BlackPieces)
            {
                PieceType type = piece.getPieceType();
                int i = piece.getPiecePosition();
                if(type == PieceType.QUEEN)
                    r += (dist_bonus[i, j] * 5) / 2;
                if (type == PieceType.ROOK)
                    r += dist_bonus[i, j] / 2;
                if (type == PieceType.KNIGHT)
                    r += dist_bonus[i, j];
                if(type == PieceType.BISHOP)
                {
                    r += bonus_dia_distance[Math.Abs(diag_ne[i] - diag_ne[j])];
                    r += bonus_dia_distance[Math.Abs(diag_nw[i] - diag_nw[i])];
                }
            }
            return r;
        }

        private int tropism_to_black_king(Board board, int j)
        {
            int r = 0;
            foreach(Piece piece in board.WhitePieces)
            {
                PieceType type = piece.getPieceType();
                int i = piece.getPiecePosition();
                if(type == PieceType.QUEEN)
                    r += (dist_bonus[i, j] * 5) / 2;
                if (type == PieceType.ROOK)
                    r += dist_bonus[i, j] / 2;
                if (type == PieceType.KNIGHT)
                    r += dist_bonus[i, j];
                if(type == PieceType.BISHOP)
                {
                    r += bonus_dia_distance[Math.Abs(diag_ne[i] - diag_ne[j])];
                    r += bonus_dia_distance[Math.Abs(diag_nw[i] - diag_nw[i])];
                }
            }
            return r;
        }

        private int kingSafety(Board board, Piece WhiteKing, Piece BlackKing)
        {
            int wk_pos = WhiteKing.getPiecePosition();
            int bk_pos = BlackKing.getPiecePosition();
            int whiteKingSafety = 
                (eval_light_king_shield(wk_pos) - tropism_to_white_king(board, wk_pos)) * piece_mat[DARK] / INITIAL_PIECE_MATERIAL;
            int blackKingSafety =
                (eval_dark_king_shield(bk_pos) - tropism_to_black_king(board, bk_pos)) * piece_mat[LIGHT] / INITIAL_PIECE_MATERIAL;
            return whiteKingSafety - blackKingSafety;
        }
    }
}

