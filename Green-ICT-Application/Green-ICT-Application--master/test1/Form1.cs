﻿using System;
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
    //combox1, textbox6
    public partial class Form1 : Form
    {
        Game game;
        bool Addchecking = false;
        List<Image> images = new List<Image>();
        List<string> playerList = new List<string>();
        PictureBox firstClicked = null;
        PictureBox secondClicked = null;
        bool WithOutLog;
        Regex rgx = new Regex(@"^[a-zA-Z0-9]+$");
        public Form1()
        {
            InitializeComponent();
            game = new Game();
            textBox7.Enabled = false;
            textBox8.Enabled = false;
        }

        public void ListPlayer()
        {
            comboBox1.Items.Add("Choose Name");
            comboBox2.Items.Add("Choose Name");
            comboBox3.Items.Add("Choose Name");
            comboBox4.Items.Add("Choose Name");
            playerList = game.rePlayerName();
            for (int i = 0; i < playerList.Count; i++)
            {
                comboBox1.Items.Add(playerList[i].ToString());
                comboBox2.Items.Add(playerList[i].ToString());
                comboBox3.Items.Add(playerList[i].ToString());
                comboBox4.Items.Add(playerList[i].ToString());
            }
            comboBox1.SelectedItem = "Choose Name";
            comboBox2.SelectedItem = "Choose Name";
            comboBox3.SelectedItem = "Choose Name";
            comboBox4.SelectedItem = "Choose Name";

        }
        
        private void AssignPhoto()
        {
            
            Random random = new Random();
            foreach (Control control in tableLayoutPanel9.Controls)
            {

                PictureBox pa = control as PictureBox;
                if (pa != null)
                {
                    int randomNumber = random.Next(images.Count);
                    pa.BackgroundImage = images[randomNumber];
                    pa.SizeMode = PictureBoxSizeMode.StretchImage;
                    images.RemoveAt(randomNumber);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) //login
        {
           
            WithOutLog = false;
            panel14.Visible = false;
            bool check = game.UserLogin(textBox1.Text, textBox2.Text);
            if (check != false)
            {
                panel6.Visible = true;
                label22.Text = textBox1.Text.ToUpper();
                label25.Text = textBox1.Text.ToUpper();
            }
        }

        private void button2_Click(object sender, EventArgs e) //go to register page
        {
            panel4.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e) //register
        {
           
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" )
            {
                bool check = game.UserRegister(textBox3.Text, textBox4.Text, textBox5.Text);
                if (check != false && rgx.IsMatch(textBox3.Text.Trim()) && rgx.IsMatch(textBox4.Text.Trim()))
                {
                    panel4.Visible = false;
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
               
            }
            else
                MessageBox.Show("invalid input");
        }

        private void panel5_click(object sender, MouseEventArgs e)
        {
            panel4.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e) //play without login
        {
            panel6.Visible = true;
            panel14.Visible = true;
            WithOutLog = true;
            tableLayoutPanel11.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e) //1 player
        {
            if (WithOutLog)
            {
                panel15.Visible = true;
                panel14.Visible = false;
                tableLayoutPanel11.Visible = false;
                if (comboBox1.Items.Count == 0 && comboBox2.Items.Count == 0 && comboBox3.Items.Count == 0 && comboBox4.Items.Count == 0)
                    ListPlayer();
            }
            else
            {
                panel9.Visible = true;
                panel14.Visible = false;
                tableLayoutPanel11.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e) //2 players
        {
            panel14.Visible = false;
            tableLayoutPanel11.Visible = true;
            if (comboBox1.Items.Count == 0 && comboBox2.Items.Count == 0 && comboBox3.Items.Count == 0 && comboBox4.Items.Count == 0)
                ListPlayer();
            if (WithOutLog)
            {
                panel11.Visible = true;
            }
            else
            {
                string a = textBox1.Text;
                comboBox1.Items.Remove(a);
                panel7.Visible = true;
                panel14.Visible = false;
            }

        }

        private void button7_Click(object sender, EventArgs e) //2 players upload
        {
            game.SetObject();
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e) // 1 player uploads
        {
            game.SetObject();
        }

        private void panel10_click(object sender, EventArgs e)
        {
            panel9.Visible = false;
            if (WithOutLog)
            panel14.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e) //1 player play
        {
            bool check = game.AssignObject();
            if (check != false)
            {
                images = game.ReList();
                AssignPhoto();
                panel2.Visible = true;
                panel7.Visible = false;
                panel9.Visible = false;
                panel6.Visible = false;
                panel12.Visible = true;
            }
            else
                MessageBox.Show("upload di dmm");
        }

        private void button12_Click(object sender, EventArgs e) //upload
        {
            string a = comboBox2.GetItemText(comboBox2.SelectedItem);
            string b = comboBox3.GetItemText(comboBox3.SelectedItem);
            if (a != "Choose Name")
            {
                game.WithoutloginSetObject(a);
            }
            else
            {
                MessageBox.Show("Choose name");
            }
            //else
            //{
            //    if (rgx.IsMatch(textBox7.Text.Trim()) && textBox7.Text != "")
            //    {
            //        bool checkUsername = game.AddUserName(textBox7.Text.Trim());
            //        bool checkUsername1 = game.AddUserName(textBox8.Text.Trim());
            //        if (checkUsername != false && checkUsername1 != false) 
            //            game.WithoutloginSetObject(textBox7.Text);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Enter name or choose name");
            //    }
            //}
        }

        private void button8_Click(object sender, EventArgs e) //2 players play
        {
           
            int i = comboBox1.SelectedIndex;
            string a = comboBox1.GetItemText(comboBox1.SelectedItem);
            if (a != "Choose Name")
            {
                label29.Text = a.ToUpper();
                bool check = game.AssignObject();
                if (check != false)
                {
                    images = game.ReList();
                    AssignPhoto();
                    panel2.Visible = true;
                    panel7.Visible = false;
                    panel9.Visible = false;
                    panel6.Visible = false;
                    panel12.Visible = true;
                }
                else
                    MessageBox.Show("upload di dmm");
            }
            else
            {
                if (rgx.IsMatch(textBox6.Text.Trim()) && textBox6.Text != "")
                {
                    bool checkUsername = game.AddUserName(textBox6.Text.Trim());
                    if (checkUsername != false)
                    {
                        label29.Text = textBox6.Text.ToUpper();
                        bool check = game.AssignObject();
                        if (check != false)
                        {
                            images = game.ReList();
                            AssignPhoto();
                            panel2.Visible = true;
                            panel7.Visible = false;
                            panel9.Visible = false;
                            panel6.Visible = false;
                            panel12.Visible = true;
                        }
                        else
                            MessageBox.Show("upload di dmm");
                    }
                }
                else
                {
                    MessageBox.Show("Enter name or choose name");  
                }
            }
        
        }

        private void button11_Click(object sender, EventArgs e) //without login 2 players play
        {

            string a = comboBox2.GetItemText(comboBox2.SelectedItem);
            string b = comboBox3.GetItemText(comboBox3.SelectedItem);
            if (a != b)
            {
                if (a != "Choose Name" && b != "Choose Name")
                {
                    bool check = game.withoutLoginPlay(a);
                    if (check != false)
                    {
                        label25.Text = a;
                        label29.Text = b;
                        images = game.ReList();
                        AssignPhoto();
                        panel2.Visible = true;
                        panel7.Visible = false;
                        panel9.Visible = false;
                        panel6.Visible = false;
                        panel12.Visible = true;
                    }
                    else
                        MessageBox.Show("upload di dmm");
                }
                else
                {
                    MessageBox.Show("Choose name for both players");
                }
            }
            else
            {
                MessageBox.Show("Choose different names");
            }
            //if (a == "Choose Name" && textBox7.Text == "" && b == "Choose Name" && textBox8.Text == "")
            //{
            //    MessageBox.Show("please enter name for player ");
            //}
            //else
            //{
            //    if (a != "Choose Name" && b == "Choose Name" && textBox8.Text == "")
            //    {
            //        MessageBox.Show("Choose Name for player 2");
            //    }
            //    else if (b != "Choose Name"  && a == "Choose Name" && textBox7.Text == "")
            //    {
            //        MessageBox.Show("Choose Name for player 1");
            //    } 
            //    else if (a != "Choose Name" && textBox7.Text != "" && textBox8.Text != "")
            //    {
            //        bool check = game.withoutLoginPlay(textBox7.Text.Trim());
            //        if (check != false)
            //        {
            //            images = game.ReList();
            //            AssignPhoto();
            //            panel2.Visible = true;
            //            panel7.Visible = false;
            //            panel9.Visible = false;
            //            panel6.Visible = false;
            //            panel12.Visible = true;
            //        }
            //        else
            //            MessageBox.Show("upload di dmm");
            //    }

            //    else if (b != "Choose Name" && textBox7.Text != "" && textBox8.Text != "")
            //    {
            //        bool check = game.withoutLoginPlay(textBox7.Text.Trim());
            //        if (check != false)
            //        {
            //            images = game.ReList();
            //            AssignPhoto();
            //            panel2.Visible = true;
            //            panel7.Visible = false;
            //            panel9.Visible = false;
            //            panel6.Visible = false;
            //            panel12.Visible = true;
            //        }
            //        else
            //            MessageBox.Show("upload di dmm");
            //    }

            //    else if (textBox7.Text != "" && textBox8.Text != "")
            //    {
            //            bool check = game.withoutLoginPlay(textBox7.Text.Trim());
            //            if (check != false)
            //            {
            //                images = game.ReList();
            //                AssignPhoto();
            //                panel2.Visible = true;
            //                panel7.Visible = false;
            //                panel9.Visible = false;
            //                panel6.Visible = false;
            //                panel12.Visible = true;
            //            }
            //            else
            //                MessageBox.Show("upload di dmm");
            //    }
            //    else if (a != "Choose Name" && b!= "Choose Name")
            //    {
            //        bool check = game.withoutLoginPlay(a);
            //        if (check != false)
            //        {
            //            images = game.ReList();
            //            AssignPhoto();
            //            panel2.Visible = true;
            //            panel7.Visible = false;
            //            panel9.Visible = false;
            //            panel6.Visible = false;
            //            panel12.Visible = true;
            //        }
            //        else
            //            MessageBox.Show("upload di dmm");
            //    }
            //    else if (a != "Choose Name" && textBox8.Text != "")
            //    {
            //            bool check = game.withoutLoginPlay(a);
            //            if (check != false)
            //            {

            //                images = game.ReList();
            //                AssignPhoto();
            //                panel2.Visible = true;
            //                panel7.Visible = false;
            //                panel9.Visible = false;
            //                panel6.Visible = false;
            //                panel12.Visible = true;
            //            }
            //            else
            //                MessageBox.Show("upload di dmm");

            //    }
            //    else if (b != "Choose Name" && textBox7.Text != "")
            //    {
            //            bool check = game.withoutLoginPlay(b);
            //            if (check != false)
            //            {

            //                images = game.ReList();
            //                AssignPhoto();
            //                panel2.Visible = true;
            //                panel7.Visible = false;
            //                panel9.Visible = false;
            //                panel6.Visible = false;
            //                panel12.Visible = true;
            //            }
            //            else
            //                MessageBox.Show("upload di dmm");
            //    }
            //}
        }

        private void Picturebox_click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic.BackgroundImage != null)
            {
                // If the clicked pictureBox image is null, the player clicked
                // an pic that's already been revealed --
                // ignore the click
                if (pic.Image == null)
                {
                    return;
                }

                // If firstClicked is null, this is the first pic 
                // in the pair that the player clicked,
                // so set firstClicked to the picturebox that the player 
                // clicked, change its Image to null and return
                if (firstClicked == null)
                {
                    firstClicked = pic;
                    firstClicked.Image = null;

                    return;
                }
                // If the player gets this far, the timer isn't
                // running and firstClicked isn't null,
                // so this must be the second icon the player clicked
                // Set its Image to null
                if (secondClicked == null)
                {
                    secondClicked = pic;
                    secondClicked.Image = null;
                }

                // If the player clicked two matching icons, keep them 
                // Image null and reset firstClicked and secondClicked  to null
                // so the player can click another icon
               
                
                // If the player gets this far, the player 
                // clicked two different picturebox, so start the 
                // timer (which will wait a second, and then hide)             
                timer1.Start();
                
            }
            
        }
       
        private void timer1_click(object sender, EventArgs e)
        {
            timer1.Stop();
            //Stop the timer
            if (firstClicked.BackgroundImage == secondClicked.BackgroundImage)
            {
                secondClicked.BackgroundImage = null;
                firstClicked.BackgroundImage = null;

                firstClicked = null;
                secondClicked = null;


                return;
            }
            // Hide both picImage
            firstClicked.Image = Properties.Resources.logo;
            secondClicked.Image = Properties.Resources.logo;

            // Reset firstClicked and secondClicked 
            // so the next time a pictureBox is
            // clicked, the program knows it's the first click       
            firstClicked = null;
            secondClicked = null;
           
            
        }

        private void panel13_click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (WithOutLog)
                panel14.Visible = true;
        }

        private void panel14_click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button13_Click(object sender, EventArgs e) //start game
        {

        }

        private void panel16_click(object sender, EventArgs e)
        {
            panel15.Visible = false;
            panel14.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e) //withoutlogin play
        {
            int i = comboBox1.SelectedIndex;
            string a = comboBox4.GetItemText(comboBox4.SelectedItem);
            if (a != "Choose Name")
            {
                bool check = game.withoutLoginPlay(a);
                if (check != false)
                {
                    images = game.ReList();
                    AssignPhoto();
                    panel2.Visible = true;
                    panel7.Visible = false;
                    panel9.Visible = false;
                    panel6.Visible = false;
                    panel12.Visible = true;
                }
                else
                    MessageBox.Show("upload di dmm");
            }
            else
            {
                if (rgx.IsMatch(textBox9.Text.Trim()) && textBox9.Text != "")
                {
                    if (Addchecking == true)
                    {
                        label29.Text = textBox6.Text.ToUpper();
                        bool check = game.withoutLoginPlay(textBox9.Text.Trim());
                        if (check != false)
                        {
                            images = game.ReList();
                            AssignPhoto();
                            panel2.Visible = true;
                            panel7.Visible = false;
                            panel9.Visible = false;
                            panel6.Visible = false;
                            panel12.Visible = true;
                        }
                        else
                            MessageBox.Show("upload di dmm");
                    }
                    else
                        MessageBox.Show("Test");
                }
                
                else
                {
                    MessageBox.Show("Enter name or choose name");
                }
            }
           
        }

        private void button15_Click(object sender, EventArgs e) //withoutlogin upload
        {
            string a = comboBox4.GetItemText(comboBox4.SelectedItem);
            if (a != "Choose Name")
            {
                label29.Text = a.ToUpper();
                game.WithoutloginSetObject(a);
            }
            else
            {
                if (Addchecking == true)
                {
                    if (rgx.IsMatch(textBox9.Text.Trim()) && textBox9.Text != "")
                    {
                        game.WithoutloginSetObject(textBox9.Text);
                    }
                    else
                    {
                        MessageBox.Show("Enter name or choose name");
                    }
                }
                else
                    MessageBox.Show("Choose another username");
            }
            
        }

        private void button16_Click(object sender, EventArgs e) //add player
        {
            if (textBox9.Text != "" && rgx.IsMatch(textBox9.Text.Trim()))
            {
                bool checkUsername = game.AddUserName(textBox9.Text.Trim());
                if (checkUsername != false)
                {
                    Addchecking = true;
                    MessageBox.Show("Successfully added");
                }
            }
            else
            {
                MessageBox.Show("Enter Name");
            }
        }
    }
}