using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class AI
    {      
        private Heuristic heuristic;        
        private int depth;
        private int nodeCount; 
        private TranspositionTable table;
        public AI( int depth)
        {
            this.depth = depth;
            this.heuristic = new Heuristic();
            table = new TranspositionTable();
        }

        public Move getMove(Board board){
            return this.findMove(board, this.depth);
        }

        private Move findMove(Board board, int depth)
        {
            nodeCount = 0;
            int alpha = Int32.MinValue;
            int beta = Int32.MaxValue;

            //Find state in transposition table
            Move bestMove = null;
            int key = this.table.hash(board);
            if (this.table.getNode(key,depth, board) != null)
            {
                Node node = this.table.getNode(key, depth, board);
                return node.BestMove;
            }

            //If the current state is not in TT then run minimax;
            int currentValue;
            if (board.CurrentPlayer.getAlliance() == Sides.BLACK)
                currentValue = Int32.MaxValue;
            else
                currentValue = Int32.MinValue;

            foreach (Move move in board.CurrentPlayer.getLegalMoves())
            {
                nodeCount++;
                MoveTransition trans = board.CurrentPlayer.makeMove(move);
                if (trans.getMoveStatus().isDone())
                {
                    if (board.CurrentPlayer.getAlliance() == Sides.BLACK)
                    {
                        int temp = maxValue(trans.ToBoard, depth - 1, alpha, beta);
                        if (temp < currentValue)
                        {
                            currentValue = temp;
                            bestMove = move;
                        }
                        beta = Math.Min(beta, currentValue);
                    }
                    else
                    {
                        int temp = minValue(trans.ToBoard, depth - 1, alpha, beta);
                        if(temp > currentValue)
                        {
                            currentValue = temp;
                            bestMove = move;
                        }
                        alpha = Math.Max(alpha, currentValue);
                    }
                }
            }
            table.add(key, new Node(bestMove, currentValue, depth, board));
            return bestMove;
        }


        private int maxValue(Board board, int depth, int alpha, int beta)
        {
            if (depth == 0 || BoardUtils.isBoardinStaleMateorCheckMate(board))
            {
                return this.heuristic.calculate(board, depth);
            }
            
            int value = Int32.MinValue;

            //Find state in transposition table
            int key = this.table.hash(board);
            if (this.table.getNode(key,depth, board) != null)
            {
                return this.table.getNode(key, depth, board).Value;
            }

            //If the current state is not in TT then run minimax;

            Move bestMove = null;

            foreach (Move move in board.CurrentPlayer.getLegalMoves())
            {
                nodeCount++;
                MoveTransition trans = board.CurrentPlayer.makeMove(move);
                if (trans.getMoveStatus().isDone())
                {
                    int temp = minValue(trans.ToBoard, depth - 1, alpha, beta);
                    if (temp > value)
                    {
                        value = temp;
                        bestMove = move;
                    }
                    if (value > beta)
                    {
                        this.table.add(key, new Node(bestMove, value, depth, board));
                        return value;
                    }                                        
                    alpha = Math.Max(alpha, value);
                }
            }
            this.table.add(key, new Node(bestMove, value, depth, board));
            return value;   
        }

        private int minValue(Board board, int depth, int alpha, int beta)
        {
            if (depth == 0 || BoardUtils.isBoardinStaleMateorCheckMate(board))
            {
                return this.heuristic.calculate(board, depth);
            }
            
            //Find state in transposition table
            int key = this.table.hash(board);
            if (this.table.getNode(key, depth, board) != null)
            {
                return this.table.getNode(key, depth, board).Value;
            }

            //If the current state is not in TT then run minimax;
            int value = Int32.MaxValue;
            Move bestMove = null;
            foreach (Move move in board.CurrentPlayer.getLegalMoves())
            {
                nodeCount++;
                MoveTransition trans = board.CurrentPlayer.makeMove(move);
                if (trans.getMoveStatus().isDone())
                {
                    int temp = maxValue(trans.ToBoard, depth - 1, alpha, beta);
                    if (temp < value)
                    {
                        value = temp;
                        bestMove = move;
                    }
                    if (value < alpha)
                    {
                        this.table.add(key, new Node(bestMove, value, depth, board));
                        return value;
                    }
                    beta = Math.Min(beta, value);
                }
            }
            this.table.add(key, new Node(bestMove, value, depth, board));  
            return value;
        }

    }
}
