using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    public class Player
    {
        
        private string name;
        private string userName;
        private string passWord;

        public Player( string getUsername, string getPassword) 
        {
            //name = getName;
            userName = getUsername;
            passWord = getPassword;
            
        } 
        public void AddName(string getName)
        {
            name = getName;
        }
        public string GetPlayer()
        {
            return userName;
        }
        public string GetName()
        {
            return name;
        }
    }
}
