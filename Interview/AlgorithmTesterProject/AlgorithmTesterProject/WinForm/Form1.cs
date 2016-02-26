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

        /// <summary>
        /// equilibrium index
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public int[] solution(int[] A)
        {
            List<int> List_ReturnInt = new List<int>();
            if (A.Length <= 0) {
                List_ReturnInt.Add(-1);
                return List_ReturnInt.ToArray(); 
            }

            int LeftSumValue = 0;
            int RightSumValue = 0;
            
            /// Get right sum total.
            for (int i = 0; i < A.Length; i++)
            {
                RightSumValue += A[i];
            }

            /// Get left value and compare
            for (int i = 0; i < A.Length; i++) 
            {
                int CurrentValue = RightSumValue - A[i];
                if (LeftSumValue == CurrentValue)
                {
                    /// found equilibrium
                    List_ReturnInt.Add(i);
                }
                else 
                {
                    LeftSumValue += A[i];
                    RightSumValue = CurrentValue;
                }
            }
            return List_ReturnInt.ToArray();
        }
    }
}
