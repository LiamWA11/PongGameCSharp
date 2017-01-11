using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PongGameCSharp
{
    public partial class GameArea : Form
    {
        PictureBox picBoxPlayer;
        PictureBox picBoxAI, picBoxBall;
        Timer gameTime;
        Label lblScoreAI;
        Label lblScorePlayer;

        const int SCREEN_WIDTH = 800;
        const int SCREEN_HEIGHT = 600;

        private Size sizePlayer = new Size(25, 100);
        private Size sizeAI = new Size(25, 100);
        Size sizeBall = new Size(20, 20);
        Size scoresSize = new Size(200, 200);

        private int ballSpeedX;
        private int ballSpeedY;
        const int ballSpeedConst = 6;
        private int gameTimeInterval = 1;

        private int playerScore;
        private int aiScore;

        int diffVar = 0; //Sets the difficult of the AI - close to 0 is hard, far from 0 is easy

        private float sizeFont = 30;

        Random rnd = new Random();

        public GameArea()
        {
            InitializeComponent();

            this.Text = "Pong - Liam Angus 2016";

            picBoxPlayer = new PictureBox();
            picBoxAI = new PictureBox();
            picBoxBall = new PictureBox();

            lblScoreAI = new Label();
            lblScorePlayer = new Label();

            gameTime = new Timer();

            gameTime.Enabled = true;
            gameTime.Interval = gameTimeInterval;

            gameTime.Tick += new EventHandler(gameTime_Tick);

            this.Width = SCREEN_WIDTH;
            this.Height = SCREEN_HEIGHT;
            StartPosition = FormStartPosition.CenterScreen;

            picBoxPlayer.Size = sizePlayer;
            picBoxPlayer.Location = new Point(picBoxPlayer.Width/2, ClientSize.Height/2 - picBoxPlayer.Height/2);
            picBoxPlayer.BackColor = Color.Blue;
            this.Controls.Add(picBoxPlayer);

            picBoxAI.Size = sizeAI;
            picBoxAI.Location = new Point(picBoxAI.Width / 2, ClientSize.Height / 2 - picBoxAI.Height / 2);
            picBoxAI.BackColor = Color.Red;
            this.Controls.Add(picBoxAI);

            picBoxBall.Size = sizeBall;
            picBoxBall.Location = new Point(picBoxBall.Width / 2, ClientSize.Height / 2 - picBoxAI.Height / 2);
            picBoxBall.BackColor = Color.Green;
            this.Controls.Add(picBoxBall);

            lblScoreAI.Size = new Size(80, 50);
            lblScoreAI.Location = new Point(ClientSize.Width / 2 - lblScoreAI.Width, ClientSize.Height / lblScoreAI.Height);
            lblScoreAI.BackColor = Color.Gray;
            lblScoreAI.TextAlign = ContentAlignment.MiddleCenter;
            lblScoreAI.Font = new Font(lblScoreAI.Font.FontFamily, sizeFont);
            this.Controls.Add(lblScoreAI);

            lblScorePlayer.Size = new Size(80, 50);
            lblScorePlayer.Location = new Point(ClientSize.Width / 2 + lblScorePlayer.Width, ClientSize.Height / lblScorePlayer.Height);
            lblScorePlayer.BackColor = Color.Gray;
            lblScorePlayer.TextAlign = ContentAlignment.MiddleCenter;
            lblScorePlayer.Font = new Font(lblScorePlayer.Font.FontFamily, sizeFont);
            this.Controls.Add(lblScorePlayer);

            resetGame();
        }

        void gameTime_Tick(object sender, EventArgs e)
        {
            picBoxBall.Location = new Point(picBoxBall.Location.X + ballSpeedX, picBoxBall.Location.Y + ballSpeedY);
            gameAreaCollision();
            paddleCollision();
            playerMovement();
            aiMovement();
        }

        private void aiMovement() //Moves the AI paddle to the y location of the ball.
        {
            if (ballSpeedX > 0) {picBoxAI.Location = new Point(ClientSize.Width - (picBoxAI.Width + picBoxAI.Width / 2), picBoxBall.Location.Y - picBoxAI.Height / 2 + diffVar); }
        }

        private void playerMovement() //Moves the players paddle according to the mouse.
        {
            if (this.PointToClient(MousePosition).Y >= picBoxPlayer.Height/2 && this.PointToClient(MousePosition).Y <= ClientSize.Height - picBoxPlayer.Height / 2)
            {
               int playerX = picBoxPlayer.Width/2;
               int playerY = this.PointToClient(MousePosition).Y - picBoxPlayer.Height/2;

               picBoxPlayer.Location = new Point(playerX, playerY);
            }
        }

        private void paddleCollision() //Detects if the ball collides with any paddle (AI or Player)
        {
            if (picBoxBall.Bounds.IntersectsWith(picBoxAI.Bounds)) //Does the ball touch the AI Paddle?
            {
                //ballSpeedX = rnd.Next(0, 5);
                ballSpeedX = -ballSpeedX;
                aiDifficultyChange();
            }

            if (picBoxBall.Bounds.IntersectsWith(picBoxPlayer.Bounds)) //Does teh ball touch the Player Paddle?
            {
                //ballSpeedX = rnd.Next(0, 5);
                ballSpeedX = -ballSpeedX;
                aiDifficultyChange();
            }
        }

        private void gameAreaCollision()//Detects if the ball collides with the edge of the game area
        {
            if (picBoxBall.Location.Y > ClientSize.Height - picBoxBall.Height || picBoxBall.Location.Y < 0)
            {
                //If ball hits wall
                ballSpeedY = -ballSpeedY;
                aiDifficultyChange();
            }
            else if (picBoxBall.Location.X > ClientSize.Width)
            {
                keepScore(0);
            }
            else if (picBoxBall.Location.X < 0)
            {
                keepScore(1);
            }
        }

        private void keepScore(int side)
        {
            if (side == 0)
            {
                aiScore++;
                aiDifficultyChange();
            }
            else if (side == 1)
            {
                playerScore++;
                aiDifficultyChange();
            }

            if (playerScore == 10)
            {
                ballSpeedX = 0;
                ballSpeedY = 0;
                updateScore();
                MessageBox.Show("You Won!", "Game Over");
                resetGame();
            }
            else if (aiScore == 10)
            {
                ballSpeedX = 0;
                ballSpeedY = 0;
                updateScore();
                MessageBox.Show("Your Lost!", "Game Over");
                resetGame();
            }
            resetBall();
            updateScore();
        }

        private void updateScore()
        {
            lblScorePlayer.Text = playerScore.ToString();
            lblScoreAI.Text = aiScore.ToString();
        }
        private void aiDifficultyChange()
        {
            int cs1 = rnd.Next(0, 10);
            //DEBUG: Console.WriteLine("Selection: " + cs1);
            switch (cs1)
            {
                case 0:
                    diffVar = 0;
                    break;
                case 1:
                    diffVar = 50;
                    break;
                case 2:
                    diffVar = 100;
                    break;
                case 3:
                    diffVar = 30;
                    break;
                case 4:
                    diffVar = 30;
                    break;
                case 5:
                    diffVar = 50;
                    break;
                case 6:
                    diffVar = 50;
                    break;
                case 7:
                    diffVar = 70;
                    break;
                case 8:
                    diffVar = 70;
                    break;
                case 9:
                    diffVar = 80;
                    break;
                case 10:
                    diffVar = 90;
                    break;
                default:
                    diffVar = 0;
                    break;

            }
            //DEBUG: Console.WriteLine("diffVar: " + diffVar);
        }
        private void resetBall() //Resets the ball
        {
            picBoxBall.Location = new Point(ClientSize.Width / 2 - picBoxBall.Width / 2, ClientSize.Height / 2 - picBoxBall.Height / 2);
        }

        private void resetGame()
        {
            MessageBox.Show("Use your mouse to move the blue paddle up and down to intercept the green ball.", "How To Play");
            playerScore = 0;
            aiScore = 0;
            diffVar = 0;
            ballSpeedX = ballSpeedConst;
            ballSpeedY = ballSpeedConst;
            updateScore();
            resetBall();
        }
    }
}
