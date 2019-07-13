using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace Game2._0
{
    public partial class Form1 : Form
    {
        abstract class Ship
        {
            public abstract void makeBullet(Panel panel1, PictureBox x);
        }
        class player1 : Ship
        {
            public int playerSpeed = 6;
            public bool goleft;
            public bool goright;
            public bool isPressed;
            public int frames;
            public PictureBox player;
            public override void makeBullet(Panel panel1,PictureBox x)
            {
                PictureBox bullet = new PictureBox();
                bullet.Image = Properties.Resources.cyan_lazer;
                bullet.Size = new Size(5, 20);
                bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                bullet.Tag = "bullet";
                bullet.Left = x.Left + x.Width / 2;
                bullet.Top = x.Top - 20;
                panel1.Controls.Add(bullet);
                bullet.BringToFront();
                System.IO.Stream str = Game2._0.Properties.Resources.WOO;
                System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                snd.Play();
                //throw new NotImplementedException();
            }
        }
        /*
        class Enemy : Ship
        {
            public int speed = 5;
            public int frames;
            public PictureBox x;
            public override void makeBullet(Panel panel1, PictureBox x)
            {
                PictureBox bullet = new PictureBox();
                bullet.Image = Properties.Resources.spaceMissiles_018;
                bullet.Size = new Size(5, 20);
                bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                bullet.Tag = "EnemyBullet";
                bullet.Left = x.Left + x.Width / 2;
                bullet.Top = x.Top + x.Height - 20;
                panel1.Controls.Add(bullet);
                //throw new NotImplementedException();
            }
        }*/
        class Enemy : PictureBox
        {
            public static int speed = 2;
            public int frames;
            public void makeBullet(Panel panel1, Enemy x)
            {
                PictureBox bullet = new PictureBox();
                bullet.Image = Properties.Resources.red_lazer;
                bullet.Size = new Size(5, 20);
                bullet.SizeMode = PictureBoxSizeMode.StretchImage;
                bullet.Tag = "EnemyBullet";
                bullet.Left = x.Left + x.Width / 2;
                bullet.Top = x.Top + x.Height - 20;
                panel1.Controls.Add(bullet);
                //throw new NotImplementedException();
            }
        }
        List<Bitmap> imgs = new List<Bitmap>();
        Bitmap[] arrayImgs;
        int curEnemies = 0;
        int score = 0;
        int level = 1;
        int frame = 1;
        player1 playerX = new player1();
        List<Enemy> Enemies = new List<Enemy>();
        public Form1()
        {
            InitializeComponent();
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }/*&
        private void makeBullet()
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.spaceMissiles_019;
            bullet.Size = new Size(5, 20);
            bullet.SizeMode = PictureBoxSizeMode.StretchImage;
            bullet.Tag = "bullet";
            bullet.Left = player.Left + player.Width / 2;
            bullet.Top = player.Top - 20;
            panel1.Controls.Add(bullet);
            bullet.BringToFront();
        }
        private void makeEnemyBullet(PictureBox x)
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.spaceMissiles_018;
            bullet.Size = new Size(5, 20);
            bullet.SizeMode = PictureBoxSizeMode.StretchImage;
            bullet.Tag = "EnemyBullet";
            bullet.Left = x.Left + x.Width / 2;
            bullet.Top = x.Top + x.Height -20;
            panel1.Controls.Add(bullet);
           // bullet.BringToFront();
        }*/
        private void gameOver()
        {
            timer1.Stop();
            this.Hide();
            new Form1().Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (frame < 21)
            {
                frame++;
            }
            else
            {
                frame = 1;
            }

            if (curEnemies == 0)
            {
                int top = 0;
                for (int i = 0; i < level; i++)
                {
                    int lef = -30;
                    for (int j = 0; j < 20; j++)
                    {
                        if (lef + 70 < panel1.Width)
                        {
                            Enemy x = new Enemy();
                            x.Image = Game2._0.Properties.Resources.spaceShips_007;
                            x.Top = top;
                            x.Left = lef;
                            x.Width = 70;
                            x.Height = 70;
                            x.Tag = "invader";
                            lef += x.Width + 20;
                            x.BackColor = Color.Transparent;
                            x.SizeMode = PictureBoxSizeMode.StretchImage;
                            x.BringToFront();
                            Enemies.Add(x);
                            curEnemies++;
                        }
                    }
                    top -= 80;
                }
                level++;
                foreach (Enemy en in Enemies)
                {
                    panel1.Controls.Add(en);
                }
            }
            //  label1.Text += "1";
            if ((playerX.goleft) && (playerX.player.Left > 0 - 100))
            {
                if (playerX.player.Left - 20 < -80)
                    playerX.player.Left = this.Width;
                playerX.player.Left -= playerX.playerSpeed * 2;
            }
            else if ((playerX.goright) && (playerX.player.Left < this.Width - 10))
            {
                // MessageBox.Show(player.Left.ToString());
                if (playerX.player.Left+20 > this.Width)
                    playerX.player.Left = -70;
                playerX.player.Left += playerX.playerSpeed * 2;
            }
            foreach (PictureBox x in panel1.Controls)
            {
                if (x.Tag == "invader")
                {
                    if (x.Bounds.IntersectsWith(playerX.player.Bounds))
                    {
                        gameOver();
                    }
                    if (frame == 20 && x.Top > 0 && x.Left<this.Width-10 && x.Left>0)
                    {
                        /*
                        if (new Random().Next(-20, 10) == 8)
                        {
                            x = Enemies[new Random().Next(0, Enemies.Count)];
                            makeEnemyBullet((PictureBox)x);
                        };*/
                       // Enemies[new Random().Next(0, Enemies.Count)].makeBullet(this.panel1, x);
                        Enemy en = Enemies[new Random().Next(0, Enemies.Count)];
                        en.makeBullet(panel1, en);
                        //makeEnemyBullet(Enemies[new Random().Next(0, Enemies.Count)]);
                        frame = 1;
                    }

                    x.Left += Enemy.speed * level;//скорость коробля && x.Left > 0
                    if (x.Left >= panel1.Width)
                    {
                        x.Left = -80;
                        x.Top += 80;
                    }
                }
            }

            foreach (PictureBox y in panel1.Controls)
            {
                if (y.Tag == "bullet")
                {
                    y.Top -= 20;
                    if (y.Top < 15)
                    {
                        panel1.Controls.Remove(y);
                    }
                }
            }
            foreach (PictureBox y in panel1.Controls)
            {
                if (y.Tag == "EnemyBullet")
                {
                    y.Top += 5;//скороксть рокеты
                    if (y.Top > panel1.Height)
                    {
                        panel1.Controls.Remove(y);
                    }
                }
            }
            foreach (PictureBox i in panel1.Controls)
            {
                foreach (PictureBox j in panel1.Controls)
                {
                    if (i.Tag == "invader")
                    {
                        if (j.Tag == "bullet")
                        {
                            if (i.Bounds.IntersectsWith(j.Bounds))
                            {
                                score++;
                                 i.Image = Game2._0.Properties.Resources.spaceEffects_008;
                                 i.Tag = "poof";
                                //panel1.Controls.Remove(i);
                                /*
                                Predicate<Enemy> pic = (Enemy en) =>{
                                return en.x == i;
                                };
                                //Enemies.Find(pic);
                                Enemies.Remove(Enemies.Find(pic));*/
                                //Enemies.Remove((Enemy)i);
                                curEnemies--;
                                panel1.Controls.Remove(j);
                                this.Text = "Счет: " + score.ToString();
                            }
                        }
                    }
                }
            }
            foreach (PictureBox x in panel1.Controls)
            {
                if (x.Tag == "poof")
                {
                    Enemy en = (Enemy)x;
                    if (en.frames < 9)
                    {
                        x.Image = arrayImgs[en.frames];
                        en.frames++;
                    }
                    else {
                        panel1.Controls.Remove(x);
                        Enemies.Remove((Enemy)x);
                    }
                }
            }
            foreach (PictureBox j in panel1.Controls)
            {
                if (j.Tag == "EnemyBullet")
                {
                    if (playerX.player.Bounds.IntersectsWith(j.Bounds))
                    {
                        //score++;
                        // i.Image = Game2._0.Properties.Resources.spaceEffects_016;
                        // i.Tag = "poof";
                       // MessageBox.Show("1");
                        panel1.Controls.Remove(playerX.player);
                        panel1.Controls.Remove(j);
                        gameOver();
                        this.Text = "Вы проиграли,"+" набрано очков = "+score.ToString() ;
                        MessageBox.Show("Вы проиграли," + " набрано очков = " + score.ToString());
                    }
                }
            }
            // label1.Text = "Score : " + score;score > totalEnemies-1
            if ( curEnemies==0 && level==4)
            {
                gameOver();
                this.Text = "УРАААА ВЫ ВЫИГРАЛИ, ВАШ СЧЕТ: "+score.ToString();
                MessageBox.Show("You Won");
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                playerX.goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                playerX.goright = false;
            }
            if (playerX.isPressed)
            {
                playerX.isPressed = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                playerX.goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                playerX.goright = true;
            }
            if (e.KeyCode == Keys.Space && !playerX.isPressed)
            {
                playerX.isPressed = true;
                playerX.makeBullet(this.panel1,playerX.player);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_008);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_009);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_010);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_011);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_012);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_013);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_014);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_015);
            imgs.Add(Game2._0.Properties.Resources.spaceEffects_016);
            arrayImgs = imgs.ToArray();
            // Image spc = Game2._0.Properties.Resources.spaceShips_009;
            // spc = ResizeImage(spc, Convert.ToInt32(spc.Width * 0.5), Convert.ToInt32(spc.Height * 0.5));
            // player.Image = spc;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Remove(panel2);
            playerX.player = new PictureBox();
            playerX.player.Image = Game2._0.Properties.Resources.spaceShips_009;
            playerX.player.Top = panel1.Height - 71;
            playerX.player.Size = new Size(76, 62);
            playerX.player.BackColor = Color.Transparent;
            playerX.player.SizeMode = PictureBoxSizeMode.StretchImage;
            playerX.player.Left = panel1.Width / 2 - playerX.player.Width / 2;
            panel1.Controls.Add(playerX.player);
            playerX.player.BringToFront();
            this.Focus();
            timer1.Start();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (playerX.player != null)
                    playerX.player.Top = panel1.Height - 71;
                if (panel2 != null)
                    panel2.Left = (int)this.Width / 2 - panel2.Width;
            }
            catch(Exception ex)  {
                MessageBox.Show("Упс, произошла ошибка. Больше так не делайте \n" + ex.ToString());
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
