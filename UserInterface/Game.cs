using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ChessEngine;

namespace UserInterface
{
    #region init
    public partial class Game : Form
    {

        private BoardPanel board;
        private HistoryTable history;
        private RemovedPieces removePieces;
        public int Hard { get; set; }
        public int Mode { get; set; }
        public Menu Menu { get; set; }

        public AI AI;

        public Game(int hard, int mode, Menu menu)
        {
            InitializeComponent();
            this.board = new BoardPanel(this);
            this.history = new HistoryTable();
            this.removePieces = new RemovedPieces(board);            
            this.Controls.Add(board);
            this.Controls.Add(history);
            this.Controls.Add(removePieces);
            this.ControlBox = false;
            this.Menu = menu;
            this.Hard = hard;
            this.Mode = mode;
            if (this.Mode == 2)
            {
                this.AI = new AI(this.Hard);
                label7.Visible = true;
            }

        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        private void Game_Load(object sender, EventArgs e)
        {
        }

        public BoardPanel BoardPanel
        {
            get
            {
                return this.board;
            }
        }

        public HistoryTable getHistoryTable()
        {
            return this.history;
        }

        public RemovedPieces getRemovedPiecesPanel()
        {
            return this.removePieces;
        }

        public PictureBox getWhiteWinStatus()
        {
            return pictureBox1;
        }
        public PictureBox getBlackWinStatus()
        {
            return pictureBox2;
        }

        public Label getPlayer()
        {
            return this.label2;
        }

        public void disable()
        {
            this.board.Enabled = true;            
        }
        
        private void undo()
        {
            Move move = history.getMoveHistory().getLatestMove();
            if (move != null)
            {
                if (pictureBox1.Visible == true)
                {
                    pictureBox1.Visible = false;
                }
                if (pictureBox2.Visible == true)
                {
                    pictureBox2.Visible = false;
                }
                this.board.LogicBoard = move.undo();
                this.board.SourceCell = null;
                this.board.DesCell = null;
                this.board.draw();
                this.history.undoLatestMove();
                this.removePieces.draw(this.history.getMoveHistory());
                this.getPlayer().Text = this.board.LogicBoard.CurrentPlayer.getAlliance() == Sides.WHITE ? "WHITE PLAYER'S TURN" : "BLACK PLAYER'S TURN";
                if (this.Mode == 2 && this.board.LogicBoard.CurrentPlayer.getOpponent().getAlliance() == Sides.WHITE)
                    this.undo();
            }
            this.board.Enabled = true;          
        }

        public AI getAI()
        {
            return this.AI;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Menu.Show();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.undo();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.board.LogicBoard = Board.createStandardBoard();
            this.board.Enabled = true;
            this.board.draw();
            this.history.Rows.Clear();
            this.history.Rows.Add();
            this.history.getMoveHistory().getMoveHistory().Clear();
            this.removePieces.draw(this.history.getMoveHistory());
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Fuchsia;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Lime;
        }
       
        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Fuchsia;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Lime;
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Fuchsia;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Lime;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.board.suggestMove();
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Fuchsia;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Lime;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
    #endregion

    #region BoardPanel Setting
    //----------------------------------------BoardPanel Setting--------------------------------------------
    public class BoardPanel : TableLayoutPanel
    {
        private List<CellPanel> listOfCells;

        private Cell sourceCell;
        public Cell SourceCell
        {
            get
            {
                return this.sourceCell;
            }
            set
            {
                this.sourceCell = value;
            }
        }

        private Cell desCell;
        public Cell DesCell
        {
            get
            {
                return this.desCell;
            }
            set
            {
                this.desCell = value;
            }
        }

        private Piece movePiece;
        public Piece MovePiece
        {
            get
            {
                return this.movePiece;
            }
            set
            {
                this.movePiece = value;
            }
        }

        private Board logicBoard;
        public Board LogicBoard
        {
            get
            {
                return this.logicBoard;
            }
            set
            {
                if (value != null)
                    this.logicBoard = value;
            }
        }

        public Game GameForm { get; set; }

        public BoardPanel(Game game)
            : base()
        {
            this.ColumnCount = 8;
            this.GameForm = game;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            this.RowCount = 8;
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            this.Size = new Size(485, 485);
            this.TabIndex = 0;
            this.Location = new Point(10 + 150, 59);
            this.Name = "board";
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.logicBoard = Board.createStandardBoard();

            listOfCells = new List<CellPanel>();

            for (int i = 0; i < 64; i++)
            {
                CellPanel cell = new CellPanel(this, i);
                this.Controls.Add(cell, i % 8, i / 8);
                listOfCells.Add(cell);
            }

            this.DoubleBuffered = true;
        }


        public void draw()
        {
            this.Controls.Clear();
            for (int i = 0; i < 64; i++)
            {
                this.listOfCells[i].draw();
                this.Controls.Add(this.listOfCells[i]);
            }
            this.Refresh();
        }

        public CellPanel getCellPanel(int cellPanelID)
        {
            return this.listOfCells[cellPanelID];
        }

        public void refreshCell()
        {
            this.SourceCell = null;
            this.DesCell = null;
            this.MovePiece = null;
        }

        public void suggestMove()
        {
            this.resetClick();
            Move move = this.GameForm.getAI().getMove(this.LogicBoard);
            MoveTransition trans = this.LogicBoard.CurrentPlayer.makeMove(move);
            if (trans.getMoveStatus().isDone())
            {
                this.listOfCells[move.getCurrentCoordinate()].BackColor = Color.Cyan;              
                this.listOfCells[move.DesCoordinate].BackColor = Color.Cyan;
            }
        }


        public void setNextPlayer()
        {
            this.GameForm.getPlayer().Text = this.LogicBoard.CurrentPlayer.getAlliance() == Sides.WHITE ? "WHITE PLAYER'S TURN" : "BLACK PLAYER'S TURN";
        }

        public void executeMove(MoveTransition transition, Move move)
        {
            this.LogicBoard = transition.ToBoard;
            this.GameForm.getHistoryTable().updateMoveHistory(this.LogicBoard.CurrentPlayer.getOpponent().getAlliance(), move);
            this.GameForm.getRemovedPiecesPanel().draw(this.GameForm.getHistoryTable().getMoveHistory());
            this.draw();
            this.refreshCell();
            this.setNextPlayer();

            if ((this.LogicBoard.CurrentPlayer.isCheckMate() || this.LogicBoard.CurrentPlayer.isStaleMate()) &&
              this.LogicBoard.CurrentPlayer.getAlliance() == Sides.WHITE)
            {
                this.GameForm.getBlackWinStatus().Visible = true;
                this.GameForm.disable();
            }

            else if ((this.LogicBoard.CurrentPlayer.isCheckMate() || this.LogicBoard.CurrentPlayer.isStaleMate()) &&
                this.LogicBoard.CurrentPlayer.getAlliance() == Sides.BLACK)
            {
                this.GameForm.getWhiteWinStatus().Visible = true;
                this.GameForm.disable();
            }
            else if (this.LogicBoard.CurrentPlayer.isInCheck())
            {
                MessageBox.Show("Check", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
       
        public void resetClick()
        {
            this.draw();
            this.refreshCell();
            this.setNextPlayer();        
        }

    }
    #endregion

    #region CellPanel Setting

    //---------------------------------------------CellPanel Setting-----------------------------------------
    public class CellPanel : Panel
    {
        private int cellID;
        private BoardPanel board;
        private static Color lightColor = Color.Linen;
        private static Color darkColor = Color.Goldenrod;
        private Color prevColor;


        private Game parentForm { get; set; }

        public static List<CellPanel> listOfCellCanMove = new List<CellPanel>();
        public CellPanel(BoardPanel board, int cellID)
        {
            this.board = board;
            this.cellID = cellID;
            this.Margin = new Padding(0);
            this.Size = new Size(60, 60);
            this.Name = "panel" + cellID;
            this.setColor();
            this.setPieceIcon(board.LogicBoard);
            this.parentForm = this.board.GameForm;
            this.MouseClick += new MouseEventHandler(CellPanel_Click);
        }

        public void choosePiece(PawnPromotionMove move)
        {
            PawnPromotion f = new PawnPromotion();
            var res = f.ShowDialog();
            if (res == DialogResult.OK)
            {
                move.setPromotedPiece(f.type);
            }
        }
        
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(obj is CellPanel))
                return false;
            return this.cellID == ((CellPanel)obj).cellID;
        }

        public override int GetHashCode()
        {
            return this.cellID;
        }

        private void setColor()
        {
            if (cellID / 8 == 0 ||
                cellID / 8 == 2 ||
                cellID / 8 == 4 ||
                cellID / 8 == 6)
            {
                this.BackColor = cellID % 2 == 0 ? lightColor : darkColor;
                this.prevColor = cellID % 2 == 0 ? lightColor : darkColor;
            }
            if (cellID / 8 == 1 ||
                cellID / 8 == 3 ||
                cellID / 8 == 5 ||
                cellID / 8 == 7)
            {
                this.BackColor = cellID % 2 == 0 ? darkColor : lightColor;
                this.prevColor = cellID % 2 == 0 ? darkColor : lightColor;
            }
        }

        private void setPieceIcon(Board board)
        {
            this.BackgroundImage = null;
            if (board.getCell(this.cellID).isCellOccupied())
            {
                Piece piece = board.getCell(this.cellID).getPiece();
                string alliance = piece.getSide() == Sides.WHITE ? "W" : "B";
                string type = piece.getPieceType().getPieceName();
                string imagePath = Application.StartupPath + "\\images\\figures\\" + alliance + type + ".gif";
                string temp = imagePath.ToString();
                this.BackgroundImage = Image.FromFile(@imagePath);
                this.BackgroundImageLayout = ImageLayout.Center;
            }
        }


        public void draw()
        {
            this.setColor();
            this.setPieceIcon(this.board.LogicBoard);
        }

        public void hightlightLegalMoves()
        {
            foreach (Move move in this.pieceLegalMoves())
            {
                this.board.getCellPanel(move.DesCoordinate).hightlight();
            }
        }

        private List<Move> pieceLegalMoves()
        {
            if (this.board.MovePiece != null && this.board.MovePiece.getSide() == this.board.LogicBoard.CurrentPlayer.getAlliance())
            {
                List<Move> moves = this.board.MovePiece.getLegalMoves(this.board.LogicBoard);
                return moves;
            }
            return new List<Move>();
        }

        public void hightlight()
        {
            this.BackColor = Color.Green;
            this.Refresh();
        }
        public void unHightlight()
        {
            this.BackColor = this.prevColor;
            this.Refresh();
        }

        void CellPanel_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (this.board.SourceCell == null)
                {
                    this.board.SourceCell = this.board.LogicBoard.getCell(this.cellID);
                    this.board.MovePiece = this.board.SourceCell.getPiece();
                    //Check if cell contains piece
                    if (this.board.MovePiece == null)
                    {
                        this.board.SourceCell = null;
                    }
                    //Check if player click their alliance pieces
                    else if (this.board.LogicBoard.CurrentPlayer.getAlliance() != this.board.MovePiece.getSide())
                    {
                        this.board.SourceCell = null;
                    }
                    else
                    {
                        this.hightlight();
                        this.hightlightLegalMoves();
                        
                    }
                }
                else
                {
                    this.board.DesCell = this.board.LogicBoard.getCell(this.cellID);
                    Move move = MoveFactory.createMove(this.board.LogicBoard, this.board.SourceCell.getCellCoordinate(), this.board.DesCell.getCellCoordinate());
                    if (move.isPromote())
                    {
                        this.choosePiece((PawnPromotionMove)move);
                    }
                    if (move != ChessEngine.Move.NULL_MOVE)
                    {
                        MoveTransition transition = this.board.LogicBoard.CurrentPlayer.makeMove(move);
                        if (transition.getMoveStatus().isDone())
                        {
                            this.board.executeMove(transition, move);                            
                            if (this.board.GameForm.Mode == 2)
                            {
                                move = this.board.GameForm.getAI().getMove(this.board.LogicBoard);
                                if (move != ChessEngine.Move.NULL_MOVE)
                                {
                                    transition = this.board.LogicBoard.CurrentPlayer.makeMove(move);
                                    if (transition.getMoveStatus().isDone())
                                    {
                                        this.board.executeMove(transition, move);
                                    }
                                }                                    
                            }
                        }
                        else
                        {
                            //Clear source cell and legal cells that has been click;
                            this.board.resetClick();                 
                        }
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                this.board.resetClick();
            }
        }
    }
    #endregion

    #region  HistoryPanel Setting

    //---------------------------------------------HistoryPanel Setting-----------------------------------------
    public class HistoryTable : DataGridView
    {

        private MoveHistory moveHistory;
        
        public HistoryTable()
        {
            this.Location = new Point(650, 61);
            this.Size = new Size(300, 480);
            this.BackgroundColor = Color.White;

            this.Columns.Add(new DataGridViewTextBoxColumn());
            this.Columns.Add(new DataGridViewTextBoxColumn());

            this.Columns[0].HeaderText = "White player";
            this.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Columns[1].HeaderText = "Black player";
            this.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.DefaultCellStyle.SelectionBackColor = Color.White;
            this.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.AllowUserToAddRows = false;

            this.moveHistory = new MoveHistory();
            this.Rows.Add();
        }

        public void updateMoveHistory(Sides side, Move move)
        {
            this.moveHistory.add(move);
            if (side == Sides.WHITE)
            {
                this.Rows[this.Rows.Count - 1].Cells[0].Value = move.ToString();
            }
            else
            {
                this.Rows[this.Rows.Count - 1].Cells[1].Value = move.ToString();
                this.Rows.Add();
            }
            this.Refresh();
        }

        public MoveHistory getMoveHistory()
        {
            return this.moveHistory;
        }

        public void undoLatestMove()
        {
            if (this.moveHistory.getNumberOfMoves() > 0)
            {
                this.Rows[(this.moveHistory.getNumberOfMoves()-1) / 2].Cells[(this.moveHistory.getNumberOfMoves()-1) % 2].Value = "";
                if (this.moveHistory.getLatestMove().MovePiece.getSide() == Sides.WHITE)
                    if (this.Rows.Count > 1)
                    {
                        this.Rows.RemoveAt((this.moveHistory.getNumberOfMoves() - 1) / 2);                        
                    }
                this.getMoveHistory().removeLatestMove();
            }
        }
    }
    #endregion

    #region remove pieces
    //---------------------------------------------RemovedPiecesPanel Setting-----------------------------------------
    public class RemovedPieces : TableLayoutPanel
    {
        private BoardPanel board;

        private WhiteRemovedPieces whiteRemovedPieces;
        public WhiteRemovedPieces WhiteRemovedPieces
        {
            get
            {
                return this.whiteRemovedPieces;
            }
        }

        private BlackRemovedPieces blackRemovedPieces;
        public BlackRemovedPieces BlackRemovedPieces
        {
            get
            {
                return this.blackRemovedPieces;
            }
        }
        public RemovedPieces(BoardPanel board)
            : base()
        {
            this.whiteRemovedPieces = new WhiteRemovedPieces();
            this.blackRemovedPieces = new BlackRemovedPieces();

            this.RowCount = 2;
            this.ColumnCount = 1;
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.BackColor = Color.Orange;
            this.Location = new Point(36, 61);
            this.Size = new Size(120, 480);
            this.Controls.Add(this.whiteRemovedPieces);
            this.Controls.Add(this.BlackRemovedPieces);            
            this.board = board;

        }

        public void draw(MoveHistory moveHistory)
        {
            this.whiteRemovedPieces.Controls.Clear();
            this.blackRemovedPieces.Controls.Clear();
            List<Piece> whiteRemovedPiece = new List<Piece>();
            List<Piece> blackRemovedPiece = new List<Piece>();

            foreach (Move move in moveHistory.getMoveHistory())
            {
                if (move.isAttack())
                {
                    Piece attackedPiece = move.getAttackedPiece();
                    if (attackedPiece.getSide() == Sides.WHITE)
                    {
                        whiteRemovedPiece.Add(attackedPiece);
                    }
                    else
                    {
                        blackRemovedPiece.Add(attackedPiece);
                    }
                }
            }


            foreach (Piece piece in whiteRemovedPiece)
            {
                Panel panel = new Panel();
                panel.Size = new Size(60, 30);
                panel.Margin = new Padding(0);
                string alliance = piece.getSide() == Sides.WHITE ? "W" : "B";
                string type = piece.getPieceType().getPieceName();
                panel.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\figures\\" + alliance + type + ".gif");
                panel.BackgroundImageLayout = ImageLayout.Zoom;
                whiteRemovedPieces.Controls.Add(panel);
            }

            foreach (Piece piece in blackRemovedPiece)
            {
                Panel panel = new Panel();
                panel.Size = new Size(60, 30);
                panel.Margin = new Padding(0);
                string alliance = piece.getSide() == Sides.WHITE ? "W" : "B";
                string type = piece.getPieceType().getPieceName();
                panel.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\figures\\" + alliance + type + ".gif");
                panel.BackgroundImageLayout = ImageLayout.Zoom;
                blackRemovedPieces.Controls.Add(panel);
            }

            this.Refresh();
        }
    }

    public class WhiteRemovedPieces : TableLayoutPanel
    {

        public WhiteRemovedPieces()
        {
            this.RowCount = 8;
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.Margin = new Padding(0);
            this.ColumnCount = 2;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.BackColor = Color.Orange;

            this.Size = new Size(120, 240);
            this.DoubleBuffered = true;
        }
    }

    public class BlackRemovedPieces : TableLayoutPanel
    {
        public BlackRemovedPieces()
        {
            this.RowCount = 8;
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.Margin = new Padding(0);
            this.ColumnCount = 2;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.BackColor = Color.Orange;

            this.Size = new Size(120, 240);
            this.DoubleBuffered = true;
        }
    }
    #endregion
}