using Microsoft.VisualStudio.TestTools.UnitTesting;
using MortgageLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageLibrary.Tests
{
    [TestClass()]
    public class LoanTests
    {
        [TestMethod()]
        public void LoanTest()
        {
            double loanAmount = 25000;
            double interestRate = 5;
            int loanTerm = 60;
            Loan loan1 = new Loan(loanAmount,interestRate,loanTerm);
           

            Assert.AreEqual<double>(471.78, Math.Round(loan1.minMonthlyPayment, 2));
        }

        [TestMethod()]
        public void LoanTest2()
        {
            double loanAmount = 100000;
            double interestRate = 3.75;
            int loanTerm = 30*12;
            Loan loan1 = new Loan(loanAmount, interestRate, loanTerm);


            Assert.AreEqual<double>(463.12, Math.Round(loan1.minMonthlyPayment, 2));
        }

        [TestMethod()]
        public void MakePaymentTest()
        {
            double loanAmount = 25000;
            double interestRate = 5;
            int loanTerm = 60;
            Loan loan1 = new Loan(loanAmount, interestRate, loanTerm);
            

            Assert.AreEqual<double>(471.78, Math.Round(loan1.minMonthlyPayment,2));
            loan1.MakePayment(471.78);
            Assert.AreEqual<double>(24632.39, Math.Round(loan1.remainingLoanAmount,2));
            Assert.AreEqual<double>(104.17, Math.Round(loan1.totalInterestPaid,2));
            loan1.MakePayment(471.78);
            Assert.AreEqual<double>(24263.24, Math.Round(loan1.remainingLoanAmount,2));
            Assert.AreEqual<double>(206.80, Math.Round(loan1.totalInterestPaid,2));
            Assert.AreEqual<double>(367.61+369.15, Math.Round(loan1.totalPrincipalPaid,2));
            //loan1.MakePayment(2400);
            //loan1.SimulateLoan();

        }
    }
}