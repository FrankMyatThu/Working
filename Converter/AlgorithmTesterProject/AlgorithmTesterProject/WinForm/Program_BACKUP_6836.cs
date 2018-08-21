using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
<<<<<<< HEAD
            Application.Run(new NumberSorting());
=======
            //Application.Run(new GenerateRandomNumber());

            Application.Run(new OrderingClosestPoints());
>>>>>>> ba85d55845e1a326d6ac8782533d4b401738d386
        }
    }
}
