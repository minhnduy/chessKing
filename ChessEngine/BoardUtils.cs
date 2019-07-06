using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class BoardUtils
    {            
        public static int NUM_CELLS = 64;
        public static int NUM_CELLS_PER_ROWS = 8;

        public static string[] Algebreic = createAlgebreicNotation();
        public static Dictionary<string, int> PosToAl = createAlMap();

        public static string[] createAlgebreicNotation()
        {
            return new string[] 
                {"a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
                "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
                "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
                "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
                "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
                "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
                "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
                "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"};
        }

        public static Dictionary<string, int> createAlMap()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();
            for (int i = 0; i < BoardUtils.NUM_CELLS; i++)
                map[Algebreic[i]] = i;
            return map;
        }

        public BoardUtils()
        {

        }

        public static bool checkedForLegalPosition(int position)
        {
            return 0 <= position && position < NUM_CELLS;
        }

        public static string getPositionAtCoordinate(int position)
        {
            return Algebreic[position];
        }

        public static int getCoordinateAtPosition(string position)
        {
            return PosToAl[position];
        }

        public static bool isBoardinStaleMateorCheckMate(Board board)
        {
            return board.CurrentPlayer.isCheckMate() || board.CurrentPlayer.isStaleMate();
        }
    }
}
