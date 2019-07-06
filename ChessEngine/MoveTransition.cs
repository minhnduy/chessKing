using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class MoveTransition
    {
        private Board fromBoard;
        public Board FromBoard
        {
            get
            {
                return this.fromBoard;
            }
            set
            {
                this.fromBoard = value;
            }
        }

        private Board toBoard;
        public Board ToBoard
        {
            get
            {
                return this.toBoard;
            }
            set
            {
                this.toBoard = value;
            }
        }

        private Move move;
        private MoveStatus moveStatus;


        public MoveTransition(Board fromBoard, Board toBoard, Move move, MoveStatus moveStatus)
        {
            this.fromBoard = fromBoard;
            this.toBoard = toBoard;
            this.move = move;
            this.moveStatus = moveStatus;
        }

        public MoveStatus getMoveStatus()
        {
            return this.moveStatus;
        }
    }

    public class MoveStatus
    {
        private bool done;

        public static MoveStatus DONE                       = new MoveStatus(true);
        public static MoveStatus ILLEGAL_MOVE               = new MoveStatus(false);
        public static MoveStatus LEAVES_PLAYER_IN_CHECK     = new MoveStatus(false);

        public MoveStatus(bool done)
        {
            this.done = done;
        }

        public bool isDone()
        {
            return done;
        }

        
    }
}
