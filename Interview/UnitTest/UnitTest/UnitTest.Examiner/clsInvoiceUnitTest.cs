using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Candidate;

namespace UnitTest.Examiner
{
    [TestFixture]
    public class clsInvoiceUnitTest
    {
        [Test]
        [Ignore("Not run now.")]
        public void TestInvoiceCalculation()
        {
            clsInvoiceCalculation _clsInvoiceCalculation = new clsInvoiceCalculation();
            int intTotalCost = _clsInvoiceCalculation.CalculateCost(10, 20);
            Assert.AreEqual(201, intTotalCost);
        }

        
        //[TestCase(10, 10, 100)]
        //[Test, TestCaseSource("TestCases")]
        [Test, TestCaseSource(typeof(TestData), "TestCases")]                
        public void TestInvoiceCalculation(int InputA, int InputB, int OutputA)
        {
            /// This function show how to make data driven test.
            clsInvoiceCalculation _clsInvoiceCalculation = new clsInvoiceCalculation();
            int intTotalCost = _clsInvoiceCalculation.CalculateCost(InputA, InputB);
            Assert.AreEqual(OutputA, intTotalCost);
        }
    }
}
