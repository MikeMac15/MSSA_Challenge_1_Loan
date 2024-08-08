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
        private readonly double _monthlyInterestRate;



        public Loan(double loanAmount, double interestRate, int loanTerm)
        {
            this.originalLoanAmount = loanAmount;
            this.remainingLoanAmount = loanAmount;
            this.interestRate = interestRate;
            this.loanTerm = loanTerm;
            this._monthlyInterestRate = interestRate / 1200;
            this.minMonthlyPayment = CalculateMinMonthlyPayment();
            this.totalInterestPaid = 0;
            this.month = 1;
        }

        private double CalculateMinMonthlyPayment()
        {
            // Calculate the monthly payment
            
            double totalMonthlyPayment = (this.originalLoanAmount * this._monthlyInterestRate) / (1 - Math.Pow((1 + this._monthlyInterestRate), (-this.loanTerm)));
            return totalMonthlyPayment;
        }
        private void UpdateRemainingBalance(double principalPayment)
        {
            // Update the remaining balance
            this.remainingLoanAmount -= principalPayment;
        }
        private void UpdateTotalInterestPaid(double interestPayment)
        {
            // Update the total interest paid
            this.totalInterestPaid += interestPayment;
        }
        private double CalculateInterestPayment()
        {
            // Calculate the interest payment
            return this.remainingLoanAmount * this._monthlyInterestRate;
           
        }
        private double CalculatePrincipalPayment(double totalMonthlyPayment, double interestPayment)
        {
            // Calculate the principal payment

            double principalPayment = totalMonthlyPayment - interestPayment;
            if (this.remainingLoanAmount < principalPayment) {
                principalPayment = this.remainingLoanAmount;
            }
            return principalPayment;
        }


        public void MakePayment(double payment)
        {
            // Calculate the monthly payment
            if (payment <= 0)
            {
                throw new ArgumentException("Payment must be greater than zero.");
            }

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

            while (remainingBalance > 0)
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


        public void SetCurrentLoanState(int month, double totalInterestPaid, double totalPrincipalPaid)
        {
            if (month < 1 || month >= loanTerm)
            {
                throw new ArgumentException("Month invalid.");
            }
            if (totalInterestPaid < 0 || totalPrincipalPaid < 0)
            {
                throw new ArgumentException("Invalid money inputs.");
            }
            this.month = month;
            this.totalInterestPaid = totalInterestPaid;
            this.remainingLoanAmount = this.originalLoanAmount - totalPrincipalPaid;
        
        }

        public int CalculateRemainingMonthsForMinPayment()
        {
            int remainingMonths = 0;
            double remainingBalance = this.remainingLoanAmount;

            while (remainingBalance > 0 && remainingMonths < (loanTerm - this.month))
            {
                double interestPayment = remainingBalance * this._monthlyInterestRate;
                double principalPayment = this.minMonthlyPayment - interestPayment;

                if (principalPayment > remainingBalance)
                {
                    principalPayment = remainingBalance;
                }

                remainingBalance -= principalPayment;
                remainingMonths++;
            }

            return remainingMonths;
        }


    }
}
