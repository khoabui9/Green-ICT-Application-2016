using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Media;
using System.IO;
using System.Drawing.Imaging;


namespace test1
{
    public  class Game 
    {
        /// <summary>
        /// Game logic
        /// </summary>
        private GameObject gameObject;
        private Player player1;
        private Player player2;
        private Player[] players = new Player[1];
        private DatabaseFunction data;
        private List<Image> images = new List<Image>();
        private List<string> PlayerNameList = new List<string>();
        private List<Image> player1Collection = new List<Image>();
        private List<Image> player2Collection = new List<Image>();
        private int score1 = 0;
        private int score2 = 0;
        bool player1Playing = true;
        bool player2Playing = false;
        bool twoPlaying = false;
        //bool player1Scored = false;
        //bool player2Scored = false;
        bool player1win = false;
        bool player2Win = false;
        bool draw = false;

        public Game()
        {
            data = new DatabaseFunction();
        }
        
        public void Gaming()
        {
            string a = player1.GetPlayer();
            data.NewGame(data.GetPlayerID(a));
        }

        public bool UserLogin(string username, string password)
        {
           player1 = new Player(username, password);
           bool check = data.UserAuthentiacation(username, password);
            if (check != false)
                return true;
            else
                return false;
        }

        public bool UserRegister(string name, string username, string password)
        {
          
                bool check = data.CheckValidUserName(username);
                if (check != false)
                {
                    data.AddPlayer(name, username, password);
                    return true;
                }
                return false;
        }

        public bool AddUserName(string UserName)
        {
            player2 = new Player(UserName);
            bool check = data.CheckValidUserName(UserName);
            if (check != false)
            {
                data.AddUserName(UserName);
                return true;
            }
            return false;
            
        }

        public void Upload()
        {
            string rePlayer = player1.GetPlayer();
            data.Upload(data.GetPlayerID(rePlayer));
        }

        public bool AssignObject()
        {
            string rePlayer = player1.GetPlayer();
            bool check = data.GetObject(data.GetPlayerID(rePlayer));
            if (check != false)
            {
                return true;
            }
            else
                return false;
        }

        public List<Image> ReList()
        { 
            images = data.RetListOfObject();
            gameObject = new GameObject(images);
            return images;
        }

        public bool withoutLoginPlay(string getplayer)
        {
            bool check = data.GetObject(data.GetPlayerID(getplayer));
            if (check != false)
            {
                return true;
            }
            else
                return false;
        }

        public void WithoutloginSetObject(string name)
        { 
            data.Upload(data.GetPlayerID(name));
        }

        public List<string> rePlayerName()
        {
            data.GetListOfPlayer();
            PlayerNameList = data.ReturnPlayerName();
            return PlayerNameList;
        }

        public void checkTwoPlaying(bool a)
        {
            twoPlaying = a;
        }

        public void checkWinner()
        {
            // If the loop didn’t return, it didn't find
            // any unmatched
            // That means the user won. Show a message and close the form
            if (twoPlaying == false)
                MessageBox.Show("You matched all the icons!", "Congratulations");
            else
            {
                if (player1win == true)
                    MessageBox.Show("Player 1 win\n" + score1 + 1 + " score\n" + score1 + 1 + " Objects", "Congratulations");
                else if (player2Win == true)
                    MessageBox.Show("Player 2 win\n" + score2 + 1 + " score\n" + score2 + 1 + " Objects", "Congratulations");
                else if (draw == true)
                    MessageBox.Show("Test");
            }
        }

        public void Play(PictureBox firstClicked, PictureBox secondClicked, Label player1Score, Label player2Score)
        {
            if (twoPlaying != false)
            {
                //Stop the timer
                if (player1Playing)
                {
                    if (firstClicked.BackgroundImage == secondClicked.BackgroundImage)
                    {
                        //player1Scored = true;
                        AddToPlayer1Collection(firstClicked.BackgroundImage);
                        secondClicked.BackgroundImage = null;
                        firstClicked.BackgroundImage = null;
                        firstClicked = null;
                        secondClicked = null;
                        score1 += 1;
                        player1Score.Text = score1.ToString();
                        if (score1 > score2)
                        {
                            player1win = true;
                            player2Win = false;
                        }
                        else if (score1 == score2)
                            draw = true;
                        return;
                    }
                    else
                    { // Hide both picImage
                        firstClicked.Image = Properties.Resources._default;
                        secondClicked.Image = Properties.Resources._default;
                        player1Playing = false;
                        player2Playing = true;
                        MessageBox.Show("Player1 missed!\nPlayer2's turn!");
                    }
                }
                else
                {
                    if (firstClicked.BackgroundImage == secondClicked.BackgroundImage)
                    {
                        player1win = false;
                        player2Win = true;
                        AddToPlayer2Collection(firstClicked.BackgroundImage);
                        secondClicked.BackgroundImage = null;
                        firstClicked.BackgroundImage = null;
                        firstClicked = null;
                        secondClicked = null;
                        score2 += 1;
                        player2Score.Text = score2.ToString();
                        if (score1 < score2)
                        {
                            player1win = false;
                            player2Win = true;
                        }
                        else if (score1 == score2)
                            draw = true;
                        return;
                    }
                    else
                    { // Hide both picImage
                        firstClicked.Image = Properties.Resources._default;
                        secondClicked.Image = Properties.Resources._default;
                        player2Playing = false;
                        player1Playing = true;
                        MessageBox.Show("Player2 missed!\nPlayer1's turn!");
                    }
                }
            }
            else
            {
                if (firstClicked.BackgroundImage == secondClicked.BackgroundImage)
                {
                    //game.AddToPlayer1Collection(firstClicked.BackgroundImage);
                    secondClicked.BackgroundImage = null;
                    firstClicked.BackgroundImage = null;
                    firstClicked = null;
                    secondClicked = null;
                    score1 += 1;
                    player1Score.Text = score1.ToString();
                    return;
                }
                firstClicked.Image = Properties.Resources._default;
                secondClicked.Image = Properties.Resources._default;
            }
        }

        public void AddToPlayer1Collection(Image a)
        {
            player1Collection.Add(a);
        }

        public void AddToPlayer2Collection(Image b)
        {
            player2Collection.Add(b);
        }


        public Player SwitchPlayer(bool check)
        {
            
            players[0] = player1;
            players[1] = player2;
            if (check == true)
            {
                return player1;
            }
            else
            {
                return player2;
            }

        }
    }
}
