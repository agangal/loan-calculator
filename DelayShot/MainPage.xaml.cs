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
        private ulong nMonth = 60;
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
            var monthlyPayment = this.GetMonthlyPayment();
            System.Diagnostics.Debug.WriteLine(monthlyPayment);
        }

        #region Math

        

        #endregion
    }

    public class Loan
    {
        public double Rate { get; set; }
        public PayPeriod Period { get; set; }
        private double PeriodRate { get; set; }
        public double Principal { get; set; }
        public double RemainingInterest { get; set; }
        public double RemainingPrincipal { get; set; }
        public double ComputedMonthlyPayment { get; set; }
        public double WantedMonthlyPayment { get; set; }
        public long NumPeriods { get; set; }
        public double WantedNumPeriods { get; set; }
        public List<Payment> Payments { get; set; }

        public Loan(double rate, long numPeriods, long principal, PayPeriod period)
        {
            this.Rate = rate;
            this.NumPeriods = numPeriods;
            this.Principal = principal;
            this.Period = period;
            if (this.Period == PayPeriod.Quarterly)
            {
                this.PeriodRate = this.Rate / 4;
            }
            else
            {
                this.PeriodRate = this.Rate / 12;
            }
            this.ComputedMonthlyPayment = this.GetMonthlyPayment();
        }

        private double GetMonthlyPayment()
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
        public double Principal { get; set; }
        public double Interest { get; set; }
        public double TotalSinglePayment { get { return this.Principal + this.Interest; } }
        public double LoanRemainingInterest { get; set; }
        public double LoanRemainingPrinciple { get; set; }
        public double TotalLoanPrinciplePaid { get; set; }
        public double TotalLoanInterestPaid { get; set; }
    }

    public enum PayPeriod
    {
        Monthly,
        Quarterly
    }
}
