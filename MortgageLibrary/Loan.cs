using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageLibrary
{
    public class Loan
    {
        public double originalLoanAmount { get; }
        public double remainingLoanAmount { get; private set; }
        public double interestRate { get; }
        public int    loanTerm { get; }
        public double minMonthlyPayment { get; }
        public double totalInterestPaid { get; private set; }
        public int month { get; private set; }
        public double totalPrincipalPaid => originalLoanAmount - remainingLoanAmount;



        public Loan(double loanAmount, double interestRate, int loanTerm)
        {
            this.originalLoanAmount = loanAmount;
            this.remainingLoanAmount = loanAmount;
            this.interestRate = interestRate;
            this.loanTerm = loanTerm;
            this.minMonthlyPayment = CalculateMinMonthlyPayment();
            this.totalInterestPaid = 0;
            this.month = 1;
        }

        private double CalculateMinMonthlyPayment()
        {
            // Calculate the monthly payment
            double IR = this.interestRate / 1200;
            double totalMonthlyPayment = (this.originalLoanAmount * IR) / (1 - Math.Pow((1 + IR), (-this.loanTerm)));
            return totalMonthlyPayment;
        }
        private void UpdateRemainingBalance(double principalPayment)
        {
            // Calculate the remaining balance
            this.remainingLoanAmount -= principalPayment;
        }
        private void UpdateTotalInterestPaid(double interestPayment)
        {
            // Calculate the total interest paid
            this.totalInterestPaid += interestPayment;
        }
        private double CalculateInterestPayment()
        {
            // Calculate the interest payment
            double interestPayment = this.remainingLoanAmount * (this.interestRate / 1200);
            return interestPayment;
        }
        private double CalculatePrincipalPayment(double totalMonthlyPayment, double interestPayment)
        {
            // Calculate the principal payment
            double principalPayment = totalMonthlyPayment - interestPayment;
            return principalPayment;
        }


        public void MakePayment(double payment)
        {
            // Calculate the monthly payment
            if (payment < Math.Round(this.minMonthlyPayment,2))
            {
                throw new ArgumentException("Monthly payment is less than the minimum monthly payment.");
            }

            double interestPayment = CalculateInterestPayment();
            double principalPayment = CalculatePrincipalPayment(payment, interestPayment);

            UpdateTotalInterestPaid(interestPayment);
            UpdateRemainingBalance(principalPayment);
            this.month++;
            //return $"Payment of ${payment} was successful. Thank You. Remaining Balance: ${this.remainingLoanAmount}";
        }  

        public void MakeMultiplePayments(double[] payments)
        {
            foreach (double payment in payments)
            {
                MakePayment(payment);
            }
        }
        
        public List<LoanMonthDetails> SimulateLoan()
        {
            List<LoanMonthDetails> loanDetailsList = new List<LoanMonthDetails>();
            // Simulate the loan
            double remainingBalance = this.remainingLoanAmount;
            double totalMonthlyPayment = this.minMonthlyPayment;
            double totalInterestPaid = this.totalInterestPaid;
            double totalPrincipalPaid = this.totalPrincipalPaid;
            int month = this.month;

            while (remainingBalance > 0.001)
            {
                double interestPayment = remainingBalance * (this.interestRate / 1200);
                if (totalMonthlyPayment > remainingBalance + interestPayment)
                {
                    totalMonthlyPayment = remainingBalance + interestPayment;
                }
                double principalPayment = totalMonthlyPayment - interestPayment;

               

                remainingBalance -= principalPayment;
                totalInterestPaid += interestPayment;
                totalPrincipalPaid += principalPayment;

                LoanMonthDetails details = new LoanMonthDetails()
                {
                    Month = month,
                    Payment = Math.Round(totalMonthlyPayment,2),
                    InterestPayment = Math.Round(interestPayment,2),
                    PrincipalPayment = Math.Round(principalPayment,2),
                    RemainingBalance = Math.Round(remainingBalance,2)
                };

                loanDetailsList.Add(details);
                month++;
            }

            return loanDetailsList;

            //Console.WriteLine($"Total Interest Paid: {Math.Round(totalInterestPaid, 2)} - Total Principal Paid: {Math.Round(totalPrincipalPaid, 2)}");
        }


    }
}
