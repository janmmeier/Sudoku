using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{   
    public class SudokuRuut : System.Windows.Forms.Button
    {
        public int Value { get; set; }
        public bool OnLukus { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Clear()
        {
            this.Text = string.Empty;
            this.OnLukus = false;
        }
    }
}
