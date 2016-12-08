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
        GameObject gameObject;
        Player player1;
        Player player2;
        Player[] players = new Player[1];
        DatabaseFunction data;
        List<Image> images = new List<Image>();
        List<string> PlayerNameList = new List<string>();
        List<Image> player1Collection = new List<Image>();
        List<Image> player2Collection = new List<Image>();
        public Game()
        {
            data = new DatabaseFunction();
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

        public void NewGame()
        {

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
