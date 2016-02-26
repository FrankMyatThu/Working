using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { -1, 3, -4, 5, 1, -6, 2, 1 };
            solution(A);
        }

        public int solution(int[] A)
        {   
            if (A.Length <= 0)
            {   
                return -1;
            }

            long LeftSumValue = 0;
            long RightSumValue = 0;

            /// Get right sum total.
            for (int i = 0; i < A.Length; i++)
            {
                RightSumValue += A[i];
            }

            /// Get left value and compare
            for (int i = 0; i < A.Length; i++)
            {                
                long CurrentValue = RightSumValue - A[i];
                if (LeftSumValue == CurrentValue)
                {
                    /// found equilibrium
                    return i;
                }
                else
                {
                    LeftSumValue += A[i];
                    RightSumValue = CurrentValue;
                }
            }
            return -1;
        }
    }
}
