using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class MoveHistory
    {
        private List<Move> moves;

        public MoveHistory()
        {
            moves = new List<Move>();            
        }

        public void add(Move move)
        {
            this.moves.Add(move);
        }

        public Move removeMove(int index)
        {
            Move move = this.moves[index];
            this.moves.RemoveAt(index);
            return move;
        }

        public void removeMove(Move move)
        {
            this.moves.Remove(move);
        }

        public Move getLatestMove()
        {
            if (this.moves.Count > 0)
            {
                return this.moves[this.moves.Count - 1];
            }
            return null;
        }

        public int getNumberOfMoves()
        {
            return this.moves.Count;                
        }

        public List<Move> getMoveHistory()
        {
            return this.moves;
        }

        public void removeLatestMove()
        {
            if (this.moves.Count > 0)
            {
                this.moves.RemoveAt(this.moves.Count - 1);
            }                
        }
        
    }
}
