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
        Player player;
        DatabaseFunction data;
        List<Image> images = new List<Image>();
        List<string> PlayerNameList = new List<string>();

        public Game()
        {
            data = new DatabaseFunction();
        }

        public bool UserLogin(string username, string password)
        {
           player = new Player(username, password);
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
            bool check = data.CheckValidUserName(UserName);
            if (check != false)
            {
                data.AddUserName(UserName);
                return true;
            }
            return false;
            
        }
        public void SetObject()
        {
            string rePlayer = player.GetPlayer();
            data.Upload(data.GetPlayerID(rePlayer));
        }
        public bool AssignObject()
        {
            string rePlayer = player.GetPlayer();
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

        public void SwitchPlayer()
        {

        }

    }
}
