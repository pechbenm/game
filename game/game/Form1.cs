using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    public partial class Form1 : Form
    {
        private int seconds = 0;
        private Player player = new Player();
        private List<Player> foods = new List<Player>();

        int maxWidth;
        int maxHeight;

        int score;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;

        public Form1()
        {
            InitializeComponent();
            new Settings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void key_is_down(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) goLeft = true;
            if (e.KeyCode == Keys.Right) goRight = true;
            if (e.KeyCode == Keys.Up) goUp = true;
            if (e.KeyCode == Keys.Down) goDown = true;
        }

        private void key_is_up(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) goLeft = false;
            if (e.KeyCode == Keys.Right) goRight = false;
            if (e.KeyCode == Keys.Up) goUp = false;
            if (e.KeyCode == Keys.Down) goDown = false;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            restart();
        }

        private void restart()
        {
            maxWidth = field.Width / Settings.width - 1;
            maxHeight = field.Height / Settings.height - 1;

            start_button.Enabled = false;
            score = 0;
            seconds = 0;
            points.Text = "0";
            time_show.Text = "0";

            player.x = 10;
            player.y = 5;

            foods.Clear();
            foods.Add(new Player { x = rand.Next(0, maxWidth + 1), y = rand.Next(0, maxHeight + 1) });

            timer.Start();
            game_time.Start();
        }

        private void timer_event(object sender, EventArgs e)
        {
            if (goLeft) Settings.directions = "left";
            if (goRight) Settings.directions = "right";
            if (goUp) Settings.directions = "up";
            if (goDown) Settings.directions = "down";

            switch (Settings.directions)
            {
                case "left":
                    player.x--;
                    break;
                case "right":
                    player.x++;
                    break;
                case "up":
                    player.y--;
                    break;
                case "down":
                    player.y++;
                    break;
            }

            if (player.x < 0) player.x = maxWidth;
            if (player.x > maxWidth) player.x = 0;
            if (player.y < 0) player.y = maxHeight;
            if (player.y > maxHeight) player.y = 0;

            Rectangle playerRect = new Rectangle(player.x * Settings.width, player.y * Settings.height, Settings.width, Settings.height);

            for (int i = foods.Count - 1; i >= 0; i--)
            {
                Rectangle foodRect = new Rectangle(foods[i].x * Settings.food_width, foods[i].y * Settings.food_height, Settings.food_width, Settings.food_height);
                if (playerRect.IntersectsWith(foodRect))
                {
                    eat_food();
                    foods.RemoveAt(i);
                    score++;
                    points.Text = score.ToString();
                }
            }

            field.Invalidate();
        }

        private void update_gamefield_graphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            canvas.FillEllipse(Brushes.Red, new Rectangle(player.x * Settings.width, player.y * Settings.height, Settings.width, Settings.height));

            foreach (var food in foods)
            {
                canvas.FillEllipse(Brushes.Blue, new Rectangle(food.x * Settings.food_width, food.y * Settings.food_height, Settings.food_width, Settings.food_height));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds++;
            time_show.Text = seconds.ToString();

            if (seconds == 60)
            {
                game_over();
            }
            else if (seconds == 40)
            {
                for (int i = 0; i < 3; i++)
                {
                    add_food();
                }
            }
        }

        private void eat_food()
        {
            score++;
            points.Text = score.ToString();
            add_food();
        }

        private void game_over()
        {
            timer.Stop();
            game_time.Stop();
            seconds = 0;
            start_button.Enabled = true;
        }

        private void add_food()
        {
            foods.Add(new Player { x = rand.Next(0, maxWidth + 1), y = rand.Next(0, maxHeight + 1) });
        }

        private void field_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
        }
    }
}