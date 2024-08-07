using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageLibrary
{
    public class LoanMonthDetails
    {
        public int Month { get; set; }
        public double Payment { get; set; }
        public double InterestPayment { get; set; }
        public double PrincipalPayment { get; set; }
        public double RemainingBalance { get; set; }
    }
}
