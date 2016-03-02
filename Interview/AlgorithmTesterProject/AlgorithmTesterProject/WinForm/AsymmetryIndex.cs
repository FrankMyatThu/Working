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
    public partial class AsymmetryIndex : Form
    {
        public AsymmetryIndex()
        {
            InitializeComponent();
        }

        private void AsymmetryIndex_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { 5, 5, 1, 7, 2, 3, 5 };
            int Result = solution(5, A);
        }

        // To split array A into two parts, 
        // such that the number of elements equal to X in the first part is 
        // the same as the number of elements different from X in the other part.
        // Looking for an index K.
        int solution(int X, int[] A)
        {
            int ReturnValue = 0;
            // Top - Down
            int SymmetricCount = 0;
            for (int i = 0; i < A.Length; i++)
            {
                if (X == A[i]) {
                    SymmetricCount++;
                }
            }

            // Bottom - Up
            int ToBottonUpCount = A.Length;
            int AsymmetricCount = 0;
            while(ToBottonUpCount >= 0){

                if (X == A[ToBottonUpCount - 1])
                {
                    SymmetricCount--;
                }
                else {                    
                    AsymmetricCount++;
                    if (SymmetricCount == AsymmetricCount)
                    {
                        ReturnValue = ToBottonUpCount - 1;
                        break;
                    }
                }

                ToBottonUpCount--;
            }

            return ReturnValue;
        }
    }
}
