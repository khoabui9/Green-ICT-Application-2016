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
    class DatabaseFunction
    {
        private string connstr;
        private OleDbConnection myConnection;
        private OleDbDataAdapter da;
        private DataTable dt = new DataTable();
        private DateTime thisDay = DateTime.Today;
        private List<Image> images = new List<Image>();
        private List<string> playerName = new List<string>();
        private List<int> playerID = new List<int>();
       

        private Image[] ImageArray;
        private bool OpenConnection()
        {
            try {
                connstr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\buigia\Documents\GitHub\Green-ICT-Application-2016\Green-ICT-Application\Green-ICT-Application--master\test1\database\DatabaseGreenICT.mdb";
                //OleDbConnection requires namespace System.Data.OleDb
                myConnection = new OleDbConnection(connstr);
                myConnection.Open();
                return true;
            }
            catch
            {
                MessageBox.Show("false connection");
                return false;
            }
        }

        public List<Image> RetListOfObject()
        {
            return images;
        }

        public bool CheckValidUserName(string username)
        {
            OpenConnection();
            if (OpenConnection() == true)
            {
                //Building command object
                //OleDbCommand myCommand = new OleDbCommand();
                //myCommand.Connection = myConnection;
                //myCommand.CommandText = "INSERT into LOGIN(USERNAME, PASSWORD) " + "VALUES(@username, @password)";
                OleDbCommand cmd = new OleDbCommand();

                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM player WHERE Username = username";
                try {
                    OleDbDataReader reader = cmd.ExecuteReader();

                    //This method allows to control the reading of database response rows
                    bool notEoF;
                    //read first row from database
                    notEoF = reader.Read();
                    bool CheckUserName = false;
                    //bool CheckPassword = false;
                    //read row by row until the last row
                    while (notEoF)
                    {
                        if (username == reader["Username"].ToString())
                        {
                            CheckUserName = true;
                        }
                        notEoF = reader.Read();
                    }
                    reader.Close();
                    if (CheckUserName)
                    {
                        MessageBox.Show("Someone took that username or you just added \nchoose other username");
                    }
                    else
                    {
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }

        public void AddPlayer(string name, string username, string password)
        {
            try {
                OpenConnection();
                Regex rgx = new Regex(@"^[a-zA-Z0-9]+$");
                OleDbCommand cmd = new OleDbCommand();
                string hashedPassword = PasswordHash.PasswordHash.CreateHash(password);
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO  player(Name , Username, [Password]) values(@name ,@username, @password);";

                if (rgx.IsMatch(username.Trim()) && rgx.IsMatch(name.Trim()))
                {
                    cmd.Parameters.AddWithValue("@name", name.Trim());
                    cmd.Parameters.AddWithValue("@username", username.Trim());
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Registered successful \nnow, log in");

                        myConnection.Close();

                    }
                    catch (OleDbException ex)
                    {
                        MessageBox.Show("Something wrong \n" + ex);
                        myConnection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("special characters are not allowed\nnow, try again");
                }
            }
            catch
            {

            }
        }

        public bool UserAuthentiacation(string username, string password)
        {
            try {
                OpenConnection();
                if (OpenConnection() == true)
                {

                    OleDbCommand cmd = new OleDbCommand();
                    //string hashedPassword = PasswordHash.PasswordHash.CreateHash(password);
                    cmd.Connection = myConnection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM player WHERE Username = username";

                    OleDbDataReader reader = cmd.ExecuteReader();

                    //This method allows to control the reading of database response rows
                    bool notEoF;
                    //read first row from database
                    notEoF = reader.Read();
                    bool CheckUserName = false;
                    bool CheckPassword = false;

                    while (notEoF)//read row by row until the last row
                    {
                        if (username == reader["Username"].ToString() || username.Trim() == reader["Username"].ToString())
                        {
                            CheckUserName = true;

                            if (reader["Password"].ToString() != "")
                            {
                                if (PasswordHash.PasswordHash.ValidatePassword(password, reader["Password"].ToString()))
                                {
                                    CheckPassword = true;
                                }
                            }
                            else
                                MessageBox.Show("This Username doesn't have password \n please choose or register another account");
                        }
                        notEoF = reader.Read();
                    }
                    reader.Close();
                    if (CheckUserName && CheckPassword)
                    {
                        MessageBox.Show("Login successful");
                        return true;
                    }
                    else if (username == "" || password == "")
                    {
                        MessageBox.Show("Invalid input");
                        return false;
                    }
                    else if (username.Trim().Contains(" "))
                    {
                        MessageBox.Show("Wrong username");
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("Login failed \nWrong username or wrong password");
                        return false;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        public int GetPlayerID(string username)
        {
            try {
                string i;
                int a;
                OpenConnection();
                if (OpenConnection() == true)
                {
                    OleDbCommand cmd = new OleDbCommand();

                    cmd.Connection = myConnection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * from player where Username = username";
                    OleDbDataReader reader = cmd.ExecuteReader();

                    //This method allows to control the reading of database response rows
                    bool notEoF;
                    //read first row from database
                    notEoF = reader.Read();
                    while (notEoF)//read row by row until the last row
                    {
                        if (username == reader["Username"].ToString())
                        {
                            i = reader["Player_id"].ToString();
                            a = Int32.Parse(i);
                            return a;
                        }

                        notEoF = reader.Read();
                    }
                    reader.Close();
                    return 0;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public int GetObjectID(string name, int id)
        {
            try {
                string i;
                int a;
                OpenConnection();
                if (OpenConnection() == true)
                {

                    OleDbCommand cmd = new OleDbCommand();

                    cmd.Connection = myConnection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * from GameObject where ObjectName = '" + name + "' AND ref_player_id = " + @id;
                    OleDbDataReader reader = cmd.ExecuteReader();

                    //This method allows to control the reading of database response rows
                    bool notEoF;
                    //read first row from database
                    notEoF = reader.Read();
                    while (notEoF)//read row by row until the last row
                    {
                        if (name == reader["ObjectName"].ToString())
                        {
                            i = reader["Object_id"].ToString();
                            a = Int32.Parse(i);
                            return a;
                        }

                        notEoF = reader.Read();
                    }
                    reader.Close();
                    return 0;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public bool CheckObjectExist(string name, int id)
        {
            try {
                OpenConnection();
                if (OpenConnection() == true)
                {
                    //Building command object
                    //OleDbCommand myCommand = new OleDbCommand();
                    //myCommand.Connection = myConnection;
                    //myCommand.CommandText = "INSERT into LOGIN(USERNAME, PASSWORD) " + "VALUES(@username, @password)";
                    OleDbCommand cmd = new OleDbCommand();

                    cmd.Connection = myConnection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM GameObject WHERE ObjectName = '" + name + "' AND ref_player_id = " + @id;
                    OleDbDataReader reader = cmd.ExecuteReader();

                    //This method allows to control the reading of database response rows
                    bool notEoF;
                    //read first row from database
                    notEoF = reader.Read();
                    bool CheckObject = false;
                    //bool CheckPassword = false;
                    //read row by row until the last row
                    while (notEoF)
                    {
                        if (name == reader["ObjectName"].ToString())
                        {
                            CheckObject = true;
                        }
                        notEoF = reader.Read();
                    }
                    reader.Close();
                    if (CheckObject)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
           
        }

        public void Upload(int playerid)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Title = "select images";
            openFile.Filter = "JPG Files|*.jpg|JPEG Files|*.jpeg|GIF|*.gif|PNG|*.png";
            DialogResult result = openFile.ShowDialog();
            if (openFile.FileNames.Length < 10)
            {
                MessageBox.Show("The minimum number of files allowed is 10");
                return;
            }
            else
            {
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (string img in openFile.FileNames)
                    {
                        byte[] FileBytes = null;
                        string fname;
                        string now = thisDay.ToString("d");
                        fname = Path.GetFileNameWithoutExtension(img);
                        //First read bytes from image to insert into table
                        string path = img; //change your path here
                        string fileType = CheckFile(path);
                        
                        FileStream FS = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        BinaryReader BR = new BinaryReader(FS);
                        long allbytes = new FileInfo(path).Length;
                        FileBytes = BR.ReadBytes((Int32)allbytes);
                        // close all instances
                        FS.Close();
                        FS.Dispose();
                        BR.Close();
                        //DateTime dateTaken = GetDateTakenFromImage(path);
                        //Insert into access database
                      
                        bool checkObject = CheckObjectExist(fname, playerid);
                        if (checkObject != false)
                        {
                            try {
                                OpenConnection();

                                OleDbCommand cmd1 = new OleDbCommand();

                                cmd1.Connection = myConnection;
                                cmd1.CommandType = CommandType.Text;
                                cmd1.CommandText = "INSERT INTO GameObject(filetype ,ObjectName, path,[binarydata], DayAdded, ref_player_id) VALUES (@fileType,@fname, @path, @FileBytes, @now, @playerid)";

                                cmd1.Parameters.AddWithValue("@fileType", fileType);
                                cmd1.Parameters.AddWithValue("@fname", fname);
                                cmd1.Parameters.AddWithValue("@path", path); //alter as per your requirement
                                cmd1.Parameters.AddWithValue("@FileBytes", FileBytes);
                                cmd1.Parameters.AddWithValue("@now", now);
                                cmd1.Parameters.AddWithValue("@playerid", playerid);
                                cmd1.ExecuteNonQuery();
                                myConnection.Close();

                                //OpenConnection();
                                //OleDbCommand cmd = new OleDbCommand();
                                //cmd.Connection = myConnection;
                                //cmd.CommandType = CommandType.Text;
                                //cmd.CommandText = "INSERT INTO Categories(Cate_name, ref_object_id) VALUES (@fileType, @ObjectID)";
                                //cmd.Parameters.AddWithValue("@fileName", fileType);
                                //cmd.Parameters.AddWithValue("@ObjectID", ObjectID);
                                //cmd.ExecuteNonQuery();
                                //myConnection.Close();

                                OpenConnection();
                                int ObjectID = GetObjectID(fname, playerid);
                                OleDbCommand cmd2 = new OleDbCommand();
                                cmd2.Connection = myConnection;
                                cmd2.CommandType = CommandType.Text;
                                cmd2.CommandText = "INSERT INTO Metadata(Metadata_detail, ref_gameobject) VALUES (@fileName, @ObjectID)";
                                cmd2.Parameters.AddWithValue("@fileName", fname); //add name because have not done the add detail part
                                cmd2.Parameters.AddWithValue("@ObjectID", ObjectID);

                                //cmd2.Parameters.AddWithValue("@ObjectID", dateTaken); 

                                //cmd2.Parameters.AddWithValue("@ObjectID", dateTaken); //add datetaken (not all objet have)

                                cmd2.ExecuteNonQuery();
                                myConnection.Close();
                            }
                            catch
                            {
                                MessageBox.Show("There are some problems when adding to database");
                            }
                        }
                    }
                }
            }
        }
        
        public void AddUserName(string userName)
        {
            try {
                OpenConnection();
                string ps = "";
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO player(Username, [Password]) VALUES (@name, @ps)";
                cmd.Parameters.AddWithValue("@name", userName);
                cmd.Parameters.AddWithValue("@ps", ps);
                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch
            {

            }
        }

        public static DateTime GetDateTakenFromImage(string path)
        {
                Image myImage = Image.FromFile(path);
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = new Regex(":").Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
        }

        public bool GetObject(int id)
        {
            try {
                int i = 0;
                int count = 0;

                OpenConnection();

                Random random = new Random();
                OleDbCommand cmd;
                cmd = new OleDbCommand("SELECT [binarydata] FROM GameObject where ref_player_id = " + @id, myConnection);
                da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                ImageArray = new Image[dt.Rows.Count];
                i = ImageArray.Length;

                if (i != 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (dt.Rows[0]["binarydata"] != DBNull.Value)
                        {
                            ImageArray[count] = ByteArrayToImage((Byte[])row["binarydata"]);
                            count++;
                        }
                    }

                    Random number = new Random();
                    int num = 0;
                    while (num < 10)
                    {
                        int randNum = random.Next(ImageArray.Length);
                        bool check = images.Contains(ImageArray[randNum]);
                        if (check == false)
                        {
                            images.Add(ImageArray[randNum]);
                            images.Add(ImageArray[randNum]);
                            num++;
                        }
                    }
                    myConnection.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
       
        Bitmap ByteArrayToImage(byte[] b)
        {
           
                MemoryStream ms = new MemoryStream();
                byte[] pData = b;
                ms.Write(pData, 0, Convert.ToInt32(pData.Length));
                Bitmap bm = new Bitmap(ms, false);
                ms.Dispose();
                return bm;
           
        }
        public void GetListOfPlayer()
        {
            try {
                OpenConnection();
                OleDbCommand cmd = new OleDbCommand();

                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Username FROM player";
                OleDbDataReader reader = cmd.ExecuteReader();

                //This method allows to control the reading of database response rows
                bool notEoF;
                //read first row from database
                notEoF = reader.Read();
                while (notEoF)//read row by row until the last row
                {
                    string a = reader["Username"].ToString();
                    playerName.Add(reader["Username"].ToString());
                    notEoF = reader.Read();
                }
                reader.Close();
            }
            catch
            {

            }
        }

        public List<string> ReturnPlayerName()
        {
            return playerName;
        }

        public string CheckFile(string path)
        {
            string s = Path.GetExtension(path);

            if (s == ".JPG" || s == ".jpg" || s == ".PNG" || s == ".GIF" || s == ".JPEG")
            {
                return "photo";
            }
            else if (s == ".mp3" || s == ".wav")
            {
                return "sound";
            }
            else if (s == ".mp4")
            {
                return "video";
            }
            else
                return "undefined";
        }
    }
}
