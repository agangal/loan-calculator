using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DelayShot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private double rate = 0.0359;
        private int nMonth = 60;
        private double principal = 19976.15;
        private double mInt;

        public MainPage()
        {
            this.InitializeComponent();
            this.mInt = rate / 12;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoanItem item = new LoanItem();
            item.Simulated = new Loan(rate, nMonth, principal);   
            item.Real = new Loan(rate, nMonth, principal);
            item.Simulated.GeneratePeriodicPayments();
            item.Real.AddNewPayment(895, 0, 0);
            item.Real.AddNewPayment(0, 1, 0);
            item.Real.AddNewPayment(0, 50, 0);
            item.Real.AddNewPayment(345.61, 19.14, 1);
            item.Real.AddNewPayment(498.16, 1.84, 1);
            item.Real.AddNewPayment(494.62, 5.38, 1);
            item.Real.AddNewPayment(500, 0, 2);
            item.Real.AddNewPayment(0, 1, 2);
            item.Real.AddNewPayment(999, 1, 2);
            item.Real.AddNewPayment(999, 1, 2);
            item.Real.AddNewPayment(999, 1, 2);
            item.Real.AddNewPayment(3000, 1, 3);
            item.Real.AddNewPayment(999, 1, 3);
            item.Real.AddNewPayment(1999, 1, 3);
        }
    }

    public class LoanItem
    {
        public Loan Simulated { get; set; }
        public Loan Real { get; set; }
    }

    public class Loan
    {
        public double Rate { get; set; }
        public double PeriodRate { get; set; }
        public double Principal { get; set; }
        public double TotalExpectedInterest { get; set; }
        public double TotalInterestPaid { get; set; }
        public double Balance { get; set; }
        public double ComputedPeriodicPayment { get; set; }
        public int NumPeriods { get; set; }       
        public List<Payment> Payments { get; set; }
        public Loan(double rate, int numPeriods, double principal)
        {
            this.Rate = rate;
            this.NumPeriods = numPeriods;
            this.Principal = principal;
            this.TotalExpectedInterest = 0;
            this.TotalInterestPaid = 0;
            this.Balance = 0;
            this.ComputedPeriodicPayment = 0;
            this.PeriodRate = 0;
            this.Payments = new List<Payment>();
            this.ComputeBasic();
        }

        public void AddNewPayment(double principal, double interest, int currPeriodNum)
        {
            Payment payment = new Payment(principal, interest);
            this.TotalInterestPaid += interest;
            this.Balance = this.Balance - principal;
            payment.InterestRemainingAfterPayment = this.GetTotalRemainingInterest(this.Balance, this.NumPeriods - currPeriodNum);
            payment.TotalInterestPaid = this.TotalInterestPaid;
            this.Payments.Add(payment);
        }

        public void GeneratePeriodicPayments()
        {
            for (int i = 0; i < this.NumPeriods; i++)
            {
                double interest = this.GetInterestForMonth(this.Balance);
                this.TotalInterestPaid += interest;
                double principalPaid = Math.Round(this.ComputedPeriodicPayment - interest, 3, MidpointRounding.ToEven);
                Payment payment = new Payment(principalPaid, interest);                
                this.Balance = Math.Round(this.Balance - principalPaid, 3, MidpointRounding.ToEven);               
                payment.TotalInterestPaid = this.TotalInterestPaid;
                payment.InterestRemainingAfterPayment = this.GetTotalRemainingInterest(this.Balance, this.NumPeriods - i - 1);
                this.Payments.Add(payment);
                if (this.Balance < 1 )
                {
                    break;
                }                              
            }
        }

        private void ComputeBasic()
        {
            this.Balance = this.Principal;                              // In the beginning, the balance is the same as the principal
            this.PeriodRate = this.Rate / 12;            
            this.ComputedPeriodicPayment = this.GetPeriodicPayment(this.Principal, this.NumPeriods);           
        }        

        /// <summary>
        /// Compute monthly payment
        /// </summary>
        /// <returns></returns>
        private double GetPeriodicPayment(double principal, double numPeriods)
        {
            return Math.Round(principal
                * (this.PeriodRate * Math.Pow(1 + this.PeriodRate, numPeriods)) 
                / (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - 1), 3, MidpointRounding.ToEven);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cMonth">Completed Months</param>
        /// <returns></returns>
        private double GetRemainingLoanBalance(int cMonth)
        {
            return Math.Round(this.Principal * (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - Math.Pow(1 + this.PeriodRate, cMonth)) / (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - 1), 3, MidpointRounding.ToEven);
        }

        private double GetInterestForMonth(double balance)
        {
            return Math.Round(this.PeriodRate * balance, 3, MidpointRounding.ToEven);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="rMonth">Remaining months</param>
        /// <returns></returns>
        private double GetTotalRemainingInterest(double balance, int rMonth)
        {
            double sum = 0;
            double interest = 0;
            double principal = 0;
            for (int i = 0; i < rMonth; i++)
            {
                interest = this.GetInterestForMonth(balance);
                principal = this.ComputedPeriodicPayment - interest;
                sum += interest;
                if (balance > principal)
                    balance = balance - principal;

                if (balance < 1)
                {
                    break;
                }
            }

            return Math.Round(sum, 3, MidpointRounding.ToEven);
        }
    }

    public class Payment
    {
        public double PrincipalPaid { get; set; }
        public double InterestPaid { get; set; }
        public double TotalSinglePayment { get { return this.PrincipalPaid + this.InterestPaid; } }
        public double InterestRemainingAfterPayment { get; set; }
        public double TotalInterestPaid { get; set; }
        public Payment(double principalPaid, double interestPaid)
        {
            this.PrincipalPaid = principalPaid;
            this.InterestPaid = interestPaid;
        }
    }
}
