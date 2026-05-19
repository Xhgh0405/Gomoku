using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace HW2_Gomoku;

public class Form1 : Form
{
    private const int BoardSize = 15;
    private const int CellSize = 36;
    private const int BoardMargin = 34;
    private const int StoneSize = 30;

    private readonly int[,] board = new int[BoardSize, BoardSize];
    private readonly BoardPanel boardPanel;
    private readonly Label statusLabel;
    private readonly Button restartButton;

    private bool isBlackTurn = true;
    private bool gameOver = false;
    private int stepCount = 0;
    private Point? lastMove = null;

    private Image? blackStoneImage;
    private Image? whiteStoneImage;
    private SoundPlayer? placeSound;
    private SoundPlayer? winSound;

    public Form1()
    {
        Text = "五子棋 Gomoku";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.FromArgb(246, 235, 218);
        ClientSize = new Size(900, 650);

        LoadResources();

        Label titleLabel = new Label
        {
            Text = "五子棋 Gomoku",
            Font = new Font("Microsoft JhengHei UI", 22, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(635, 45),
            Size = new Size(235, 44)
        };

        statusLabel = new Label
        {
            Font = new Font("Microsoft JhengHei UI", 13, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(645, 120),
            Size = new Size(215, 42),
            BackColor = Color.FromArgb(255, 245, 225),
            BorderStyle = BorderStyle.FixedSingle
        };

        restartButton = new Button
        {
            Text = "重新開始",
            Font = new Font("Microsoft JhengHei UI", 12, FontStyle.Bold),
            Location = new Point(675, 185),
            Size = new Size(150, 44)
        };
        restartButton.Click += (_, _) => RestartGame();

        Label ruleLabel = new Label
        {
            Text = "玩法說明：\n\n1. 黑棋先手。\n2. 雙方輪流點擊棋盤交叉點。\n3. 橫、直或斜線先連成五顆者獲勝。\n4. 按下重新開始可重置棋局。",
            Font = new Font("Microsoft JhengHei UI", 10),
            Location = new Point(645, 270),
            Size = new Size(220, 190),
            AutoSize = false
        };

        boardPanel = new BoardPanel
        {
            Location = new Point(30, 34),
            Size = new Size(BoardMargin * 2 + CellSize * (BoardSize - 1),
                            BoardMargin * 2 + CellSize * (BoardSize - 1)),
            BackColor = Color.Transparent
        };
        boardPanel.Paint += BoardPanel_Paint;
        boardPanel.MouseClick += BoardPanel_MouseClick;

        Controls.Add(boardPanel);
        Controls.Add(titleLabel);
        Controls.Add(statusLabel);
        Controls.Add(restartButton);
        Controls.Add(ruleLabel);

        UpdateStatus();
    }

    private void LoadResources()
    {
        string resourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        string blackPath = Path.Combine(resourceDir, "black.png");
        string whitePath = Path.Combine(resourceDir, "white.png");
        string placePath = Path.Combine(resourceDir, "place.wav");
        string winPath = Path.Combine(resourceDir, "win.wav");

        if (File.Exists(blackPath)) blackStoneImage = Image.FromFile(blackPath);
        if (File.Exists(whitePath)) whiteStoneImage = Image.FromFile(whitePath);
        if (File.Exists(placePath)) placeSound = new SoundPlayer(placePath);
        if (File.Exists(winPath)) winSound = new SoundPlayer(winPath);
    }

    private void BoardPanel_MouseClick(object? sender, MouseEventArgs e)
    {
        if (gameOver) return;

        int col = (int)Math.Round((e.X - BoardMargin) / (double)CellSize);
        int row = (int)Math.Round((e.Y - BoardMargin) / (double)CellSize);

        if (!IsInside(row, col)) return;

        int centerX = BoardMargin + col * CellSize;
        int centerY = BoardMargin + row * CellSize;
        if (Math.Abs(e.X - centerX) > CellSize / 2 || Math.Abs(e.Y - centerY) > CellSize / 2) return;

        if (board[row, col] != 0) return;

        int player = isBlackTurn ? 1 : 2;
        board[row, col] = player;
        stepCount++;
        lastMove = new Point(col, row);
        PlaySound(placeSound);

        boardPanel.Invalidate();

        if (CheckWin(row, col, player))
        {
            gameOver = true;
            string winner = player == 1 ? "黑棋" : "白棋";
            statusLabel.Text = $"{winner}獲勝！";
            PlaySound(winSound);
            MessageBox.Show($"{winner}獲勝！", "遊戲結束", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (stepCount == BoardSize * BoardSize)
        {
            gameOver = true;
            statusLabel.Text = "平手！";
            MessageBox.Show("棋盤已滿，平手！", "遊戲結束", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        isBlackTurn = !isBlackTurn;
        UpdateStatus();
    }

    private bool CheckWin(int row, int col, int player)
    {
        int[,] directions =
        {
            { 0, 1 },   // horizontal
            { 1, 0 },   // vertical
            { 1, 1 },   // diagonal down-right
            { 1, -1 }   // diagonal down-left
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int dr = directions[i, 0];
            int dc = directions[i, 1];
            int total = 1 + CountSame(row, col, dr, dc, player) + CountSame(row, col, -dr, -dc, player);
            if (total >= 5) return true;
        }

        return false;
    }

    private int CountSame(int row, int col, int dr, int dc, int player)
    {
        int count = 0;
        int r = row + dr;
        int c = col + dc;

        while (IsInside(r, c) && board[r, c] == player)
        {
            count++;
            r += dr;
            c += dc;
        }

        return count;
    }

    private static bool IsInside(int row, int col)
    {
        return row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
    }

    private void RestartGame()
    {
        Array.Clear(board, 0, board.Length);
        isBlackTurn = true;
        gameOver = false;
        stepCount = 0;
        lastMove = null;
        UpdateStatus();
        boardPanel.Invalidate();
    }

    private void UpdateStatus()
    {
        statusLabel.Text = isBlackTurn ? "目前回合：黑棋" : "目前回合：白棋";
    }

    private static void PlaySound(SoundPlayer? player)
    {
        try
        {
            player?.Play();
        }
        catch
        {
            // If the sound device or file is unavailable, the game can still continue.
        }
    }

    private void BoardPanel_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle boardRect = new Rectangle(0, 0, boardPanel.Width - 1, boardPanel.Height - 1);
        using SolidBrush boardBrush = new SolidBrush(Color.FromArgb(223, 176, 104));
        using Pen borderPen = new Pen(Color.FromArgb(90, 55, 25), 3);
        using Pen gridPen = new Pen(Color.FromArgb(90, 55, 25), 1);
        using SolidBrush starBrush = new SolidBrush(Color.FromArgb(90, 55, 25));

        g.FillRectangle(boardBrush, boardRect);
        g.DrawRectangle(borderPen, boardRect);

        for (int i = 0; i < BoardSize; i++)
        {
            int pos = BoardMargin + i * CellSize;
            g.DrawLine(gridPen, BoardMargin, pos, BoardMargin + CellSize * (BoardSize - 1), pos);
            g.DrawLine(gridPen, pos, BoardMargin, pos, BoardMargin + CellSize * (BoardSize - 1));
        }

        DrawStarPoint(g, 3, 3, starBrush);
        DrawStarPoint(g, 3, 11, starBrush);
        DrawStarPoint(g, 7, 7, starBrush);
        DrawStarPoint(g, 11, 3, starBrush);
        DrawStarPoint(g, 11, 11, starBrush);

        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if (board[row, col] == 0) continue;
                DrawStone(g, row, col, board[row, col]);
            }
        }

        if (lastMove.HasValue)
        {
            int x = BoardMargin + lastMove.Value.X * CellSize;
            int y = BoardMargin + lastMove.Value.Y * CellSize;
            using Pen lastPen = new Pen(Color.Red, 2);
            g.DrawRectangle(lastPen, x - 6, y - 6, 12, 12);
        }
    }

    private void DrawStarPoint(Graphics g, int row, int col, Brush brush)
    {
        int x = BoardMargin + col * CellSize;
        int y = BoardMargin + row * CellSize;
        g.FillEllipse(brush, x - 4, y - 4, 8, 8);
    }

    private void DrawStone(Graphics g, int row, int col, int player)
    {
        int x = BoardMargin + col * CellSize - StoneSize / 2;
        int y = BoardMargin + row * CellSize - StoneSize / 2;
        Image? img = player == 1 ? blackStoneImage : whiteStoneImage;

        if (img != null)
        {
            g.DrawImage(img, x, y, StoneSize, StoneSize);
            return;
        }

        using Brush brush = new SolidBrush(player == 1 ? Color.Black : Color.WhiteSmoke);
        using Pen pen = new Pen(Color.Black, 1);
        g.FillEllipse(brush, x, y, StoneSize, StoneSize);
        g.DrawEllipse(pen, x, y, StoneSize, StoneSize);
    }

    private sealed class BoardPanel : Panel
    {
        public BoardPanel()
        {
            DoubleBuffered = true;
        }
    }
}
