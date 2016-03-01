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
    public partial class ArrayListLength : Form
    {
        public ArrayListLength()
        {
            InitializeComponent();
        }

        private void ArrayListLength_Load(object sender, EventArgs e)
        {
            int[] A = new int[] { 1, 4, -1, 3, 2 };
            int Result = solution(A);
        }

        // returns the length of the list constructed from A 
        int solution(int[] A)
        {            
            int _Value = A[0];
            List<int> ReturnArrayList = new List<int>();
            ReturnArrayList.Add(_Value);

            int SuccessorNodeValue = 0;
            while (SuccessorNodeValue != -1) {
                SuccessorNodeValue = A[_Value];                
                _Value = SuccessorNodeValue;
                ReturnArrayList.Add(_Value);
            }

            return ReturnArrayList.Count;            
        }
    }
}
