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
    public partial class _3_Question : Form
    {
        public _3_Question()
        {
            InitializeComponent();
        }

        private void _3_Question_Load(object sender, EventArgs e)
        {
            string returnValue = solution("ABBCC");   
        }

        public string solution(string S)
        {   
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
              {"AB", "AA"},
              {"AA", "A"},
              {"BA", "AA"},
              {"BC", "CC"},
              {"CB", "CC"},
              {"CC", "C"}
            };

            for (long i = 0; i <= S.Length; i++) {
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    string key = kvp.Key;
                    string val = kvp.Value;
                    S = S.Replace(key, val);
                }
            }

            return S;
        }
    }
}
