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

    class GameObject
    {
        List<Image> gameobject = new List<Image>();

        public GameObject(List<Image> a)
        {
            gameobject = a;
        }
    }
}
