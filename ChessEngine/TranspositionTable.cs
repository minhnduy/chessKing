using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class Node
    {
        private int value;
        public int Value
        {
            get
            {
                return this.value;
            }
        }
        private int depth;

        public int Depth
        {
            get
            {
                return this.depth;
            }
        }

        private Move bestMove;

        public Move BestMove
        {
            get
            {
                return this.bestMove;
            }
        }

        private Board board;
        public Board Board
        {
            get
            {
                return this.board;
            }
        }

        public Node(Move move, int value, int depth, Board board)
        {
            this.bestMove = move;
            this.value = value;
            this.depth = depth;
            this.board = board;
        }
    }
    class TranspositionTable
    {
        private int[,] hashTableValue = new int[64, 12];
        private Node[] hashTable; 
        private bool duplicateValue(int i, int j)
        {
            for (int m = 0;  m < i; m ++)
                for (int n = 0; n < j; n++ )
                    if (hashTableValue[m,n] == hashTableValue[i, j])
                    {
                        return true;
                    }
            return false;
        }

        public TranspositionTable()
        {
            Random rand = new Random();
            for(int i = 0; i < 64; i++)
            {
                for(int j = 0; j < 12; j++)
                {
                    hashTableValue[i, j] = rand.Next() % 500000;
                    while (duplicateValue(i,j))
                    {
                        hashTableValue[i, j] = rand.Next() % 500000;
                    }
                }
            }

            this.hashTable = new Node[10000003];
        }

        public int columnIndex(Piece piece)
        {
            PieceType type = piece.getPieceType();
            Sides color = piece.getSide();
            int index = 0;
            if (color == Sides.BLACK)
                index += 6;
            if (type == PieceType.KING)
                return 0 + index;
            else if (type == PieceType.QUEEN)
                return 1 + index;
            else if (type == PieceType.ROOK)
                return 2 + index;
            else if (type == PieceType.BISHOP)
                return 3 + index;
            else if (type == PieceType.KNIGHT)
                return 4 + index;
            return 5 + index;
        }

        public int hash(Board board)
        {
            int value = 0;
            foreach(Piece piece in board.getAllActivePieces())
            {
                int i = piece.getPiecePosition();
                int j = columnIndex(piece);
                value ^= hashTableValue[i, j];
            }
            return value % 10000003;
        }

        public void add(int key, Node node)
        {
            this.hashTable[key] = node;
        }

        public Node getNode(int key, int depth, Board board)
        {
            Node node = this.hashTable[key];
            if (node != null && node.Depth >= depth && board.isTheSame(node.Board))
                return node;
            else
                return null;
        }


    }
}
