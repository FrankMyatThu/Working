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
    public partial class BitcountInProduct : Form
    {
        public BitcountInProduct()
        {
            InitializeComponent();
        }

        private void BitcountInProduct_Load(object sender, EventArgs e)
        {
            int Result = solution(7, 2);
        }

        int solution(int A, int B)
        {
            int ReturnOneCount = 0;
            int AB_Decimal = (A * B);
            string AB_Binary = Convert.ToString(AB_Decimal, 2);

            for(int i=0; i < AB_Binary.Length; i++ ){
                if (AB_Binary.Substring(i, 1).Equals("1")) ReturnOneCount++;
            }

            return ReturnOneCount;
        }
    }
}
