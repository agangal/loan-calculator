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
        private double interestRate = 0.0359;
        private ulong nMonth = 60;
        private double principal = 19976.15;
        private double mInt;

        public MainPage()
        {
            this.InitializeComponent();
            this.mInt = interestRate / 12;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var monthlyPayment = this.GetMonthlyPayment();
            System.Diagnostics.Debug.WriteLine(monthlyPayment);
        }

        #region Math

        private double GetMonthlyPayment()
        {
            return principal * (mInt * Math.Pow(1 + mInt, nMonth)) / (Math.Pow(1 + mInt, nMonth) - 1);
        }

        private double GetRemainingLoanBalance(int cMonth)
        {
            return principal * (Math.Pow(1 + mInt, nMonth) - Math.Pow(1 + mInt, cMonth)) / (Math.Pow(1 + mInt, nMonth) - 1);
        }

        private double GetInterestForMonth(double balance)
        {
            return mInt * balance;
        }

        private double GetTotalRemainingInterest(double balance)
        {

        }

        #endregion
    }
}
