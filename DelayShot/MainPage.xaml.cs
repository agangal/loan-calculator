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
            Loan loan = new Loan(rate, nMonth, principal);
            System.Diagnostics.Debug.WriteLine(loan.ExpectedPayments.Count);
        }
    }

    public class Loan
    {
        public double Rate { get; set; }
        public double PeriodRate { get; set; }
        public double Principal { get; set; }
        public double RemainingInterest { get; set; }
        public double Balance { get; set; }
        public double ComputedPeriodicPayment { get; set; }
        public double WantedMonthlyPayment { get; set; }
        public int NumPeriods { get; set; }
        public double WantedNumPeriods { get; set; }
        public List<Payment> ExpectedPayments { get; set; }
        public List<Payment> ActualPayments { get; set; }

        public Loan(double rate, int numPeriods, double principal)
        {
            this.Rate = rate;
            this.NumPeriods = numPeriods;
            this.Principal = principal;
            this.ExpectedPayments = new List<Payment>();
            this.ActualPayments = new List<Payment>();
            this.ComputeBasic();
        }

        public void ComputeBasic()
        {
            this.Balance = this.Principal;                              // In the beginning, the balance is the same as the principal
            this.PeriodRate = this.Rate / 12;            
            this.ComputedPeriodicPayment = this.GetPeriodicPayment();
            this.GeneratePeriodicPayments();
        }

        public void GeneratePeriodicPayments()
        {
            for(int i = 0; i < this.NumPeriods; i++)
            {
                double interest = this.GetInterestForMonth(this.Balance);
                double principalPaid = this.ComputedPeriodicPayment - interest;
                Payment payment = new Payment(principalPaid, interest);
                this.Balance = this.Balance - principalPaid;
                this.ExpectedPayments.Add(payment);
                if (this.Balance == 0)
                    break;
            }
        }

        /// <summary>
        /// Compute monthly payment
        /// </summary>
        /// <returns></returns>
        private double GetPeriodicPayment()
        {
            return this.Principal 
                * (this.PeriodRate * Math.Pow(1 + this.PeriodRate, this.NumPeriods)) 
                / (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cMonth">Completed Months</param>
        /// <returns></returns>
        private double GetRemainingLoanBalance(int cMonth)
        {
            return this.Principal * (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - Math.Pow(1 + this.PeriodRate, cMonth)) / (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - 1);
        }

        private double GetInterestForMonth(double balance)
        {
            return this.PeriodRate * balance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="rMonth">Remaining months</param>
        /// <returns></returns>
        private double GetTotalRemainingInterest(double balance, int rMonth)
        {
            return this.Rate * balance / (1 - (1 / Math.Pow(1 + this.Rate, rMonth)));
        }
    }

    public class Payment
    {
        public double PrincipalPaid { get; set; }
        public double InterestPaid { get; set; }
        public double TotalSinglePayment { get { return this.PrincipalPaid + this.InterestPaid; } }        

        public Payment(double principalPaid, double interestPaid)
        {
            this.PrincipalPaid = principalPaid;
            this.InterestPaid = interestPaid;
        }
    }
}
