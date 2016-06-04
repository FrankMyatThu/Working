using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Candidate
{
    public class clsInvoiceCalculation
    {
        public int CalculateCost(int intPerProductCost, int intNumberOfProducts)
        {
            return intPerProductCost * intNumberOfProducts;
        }
    }
}
