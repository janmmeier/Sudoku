using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LooRuudud();
            AlustaUusMang();
        }

        SudokuRuut[,] ruudud = new SudokuRuut[9, 9];

        private void LooRuudud()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //loob 81 uut ruutu
                    ruudud[i, j] = new SudokuRuut();
                    ruudud[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 20);
                    ruudud[i, j].Size = new Size(40, 40);
                    ruudud[i, j].ForeColor = SystemColors.ControlDarkDark;
                    ruudud[i, j].Location = new Point(i * 40, j * 40);
                    ruudud[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                    ruudud[i, j].FlatStyle = FlatStyle.Flat;
                    ruudud[i, j].X = i;
                    ruudud[i, j].Y = j;

                    //Nupuvajutus
                    ruudud[i, j].KeyPress += cell_keyPressed;

                    panel1.Controls.Add(ruudud[i, j]);
                }
            }
        }

        private void cell_keyPressed(object sender, KeyPressEventArgs e)
        {
            var ruut = sender as SudokuRuut;

            //ära tee midagi, kui ruut on lukus
            if (ruut.OnLukus)
                return;

            int value;

            //Lisa vajutatud klahvi element ruutu ainult siis, kui see on number
            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                if (value == 0)
                    ruut.Clear();
                else
                    ruut.Text = value.ToString();

                ruut.ForeColor = SystemColors.ControlDarkDark;
            }
        }

        private void AlustaUusMang()
        {
            GenereeriNumbrid();

            var vihjeid = 0;

            if (radioButton1.Checked)
                vihjeid = 80;
            else if (radioButton4.Checked)
                vihjeid = 60;
            else if (radioButton2.Checked)
                vihjeid = 45;
            else if (radioButton3.Checked)
                vihjeid = 30;
            else if (radioButton5.Checked)
                vihjeid = 20;
            
            VihjeteArv(vihjeid);
        }

        private void GenereeriNumbrid()
        {
            foreach(var ruut in ruudud)
            {
                ruut.Value = 0;
                ruut.Clear();
            }

            JargmiseRuuduArv(0, -1);
        }

        Random random = new Random();

        private bool JargmiseRuuduArv(int i, int j)
        {
            if (++j > 8)
            {
                j = 0;

                if (++i > 8)
                    return true;
            }

            var vaartus = 0;
            var numsJargi = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            do
            {
                if (numsJargi.Count < 1)
                {
                    ruudud[i, j].Value = 0;
                    return false;
                }

                vaartus = numsJargi[random.Next(0, numsJargi.Count)];
                ruudud[i, j].Value = vaartus;

                numsJargi.Remove(vaartus);
            }
            while (!SobilikNumber(vaartus, i, j) || !JargmiseRuuduArv(i, j));
            //Alumine koodijupp näitab genereeritud numbreid
            //ruudud[i, j].Text = vaartus.ToString();

            return true;
        }

        private bool SobilikNumber(int vaartus, int x, int y)
        {
            for (int i = 0; i < 9; i++)
            {
                //kontrollib ruute vertikaalselt
                if (i != y && ruudud[x, i].Value == vaartus)
                    return false;

                //kontrollib ruute horisontaalselt
                if (i != x && ruudud[i, y].Value == vaartus)
                    return false;
            }

            for (int i = x - (x % 3); i < x - (x % 3) + 3; i++)
            {
                for (int j = y - (y % 3); j < y - (y % 3) + 3; j++)
                {
                    if (i != x && j != y && ruudud[i, j].Value == vaartus)
                        return false;
                }
            }
            return true;
        }
        private void VihjeteArv(int vihjeid)
        {
            //näitab vihjeid suvalistes ruutudes
            //vihjete arv sõltub mängu raskusastmest
            for (int i = 0; i < vihjeid; i++)
            {
                var rx = random.Next(9);
                var ry = random.Next(9);

                //vihjete ruudud on teistsugused ja lukustab need ruudud, et muuta ei saaks
                ruudud[rx, ry].Text = ruudud[rx, ry].Value.ToString();
                ruudud[rx, ry].ForeColor = Color.Black;
                ruudud[rx, ry].OnLukus = true;
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            var ValeRuut = new List<SudokuRuut>();

            //leiab kõik valed arvud
            foreach (var ruut in ruudud)
            {
                if (!string.Equals(ruut.Value.ToString(), ruut.Text))
                {
                    ValeRuut.Add(ruut);
                }
            }
            
            //kontrolli kas on valesi arve või kas mängija võidab
            if (ValeRuut.Any())
            {
                ValeRuut.ForEach(x => x.ForeColor = Color.Red);
                MessageBox.Show("Valed arvud, proovi uuesti!");
            }
            else
            {
                MessageBox.Show("Võitsite!");
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            foreach (var ruut in ruudud)
            {
                //kustutab ruudu sisu ainult siis, kui see ei ole lukus
                if (ruut.OnLukus == false)
                    ruut.Clear();
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            AlustaUusMang();
        }
    }
}


