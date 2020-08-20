using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vip.ComboBox.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Its possible change match method
            //cboNames.MatchingMethod = StringMatchingMethod.NoWildcards;
            //cboNames.MatchingMethod = StringMatchingMethod.UseWildcards;
            //cboNames.MatchingMethod = StringMatchingMethod.UseRegexs;

            cboNames.DataSource = GetNames();
            cboNames.Refresh();
        }

        private static List<string> GetNames()
        {
            return new List<string>
            {
                "Vanesa Villani",
                "Billie Mcnally",
                "Argentina Deane",
                "Cody Hinrichs",
                "Donnette Victorian",
                "Sandie Wales",
                "Paulette Gillespi",
                "Stanford Gaetano",
                "Sanford Esper",
                "Edie Barberio",
                "Luther Gorski",
                "Maximina Rodriquez",
                "Ardell Saulsbury",
                "Shenita Toone",
                "Toni Iriarte",
                "Alessandra Mccoin",
                "Edna Diedrich",
                "Azzie Sentell",
                "Karren Foss",
                "Epifania Athens",
                "Luetta Burell",
                "Margurite Jessen",
                "Lucina Michalowski",
                "Rosemarie Bradt",
                "Renetta Zilnicki",
                "Chae Rosato",
                "Dori Mcgraw",
                "Bok Hirt",
                "Idell Klinger",
                "Edmund Westley",
                "Cherri Trigg",
                "Mariano Wiley",
                "Donny Caughman",
                "Houston Brindle",
                "Trudy Leiter",
                "Teodora Audet",
                "Anglea Prindle",
                "Marcela Ventura",
                "Arden Dowling",
                "Cherilyn Malson",
                "Evia Canez",
                "Jackqueline Sitton",
                "Erinn Houseman",
                "Meredith Wingler",
                "Neva Winfrey",
                "Geraldo Koo",
                "Enriqueta Pereyra",
                "Pa Ketchum",
                "Donetta Dory",
                "Zachary Ferrara",
            };
        }
    }
}