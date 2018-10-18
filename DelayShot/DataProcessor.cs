/*****************************
 *                           *
 * Copyright - Ashish Gangal *
 *                           *
 *****************************/

namespace DelayShot
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.Storage;
    public class DataProcessor
    {
        private const string loandb = "loandb.json";
        public List<LoanItem> Items;
        int loanItemId = 0;
        public DataProcessor()
        {
            this.Initialize();
        }

        private async void Initialize()
        {
            this.Items = await this.LoadFromDB();
        }

        #region Public method to add/update loans

        public int AddNewLoan(double rate, int numPeriods, double principal)
        {
            LoanItem loan = new LoanItem();            
            loan.Simulated = new Loan(rate, numPeriods, principal);
            loan.Simulated.GeneratePeriodicPayments();
            loan.Real = new Loan(rate, numPeriods, principal);
            loan.Id = loanItemId;
            loanItemId++;
            Items.Add(loan);
            this.WriteToDB();
            return loan.Id;
        }

        public void RecordLoanPayment(double principal, double interest, int currPeriodNum, int loanId)
        {
            if (this.Items.Count == 0)
                return;

            LoanItem item = this.Items[loanId];
            item.Real.AddNewPayment(principal, interest, currPeriodNum);
            item.Real.SavingsOnInterest = item.Real.OriginalExpectedInterest - item.Real.TotalExpectedInterest;
            this.WriteToDB();
        }

        #endregion

        #region Public methods to read loans

        public LoanItem GetLoanData(int loanId)
        {
            if (this.Items.Count < loanId)
                return null;

            return this.Items[loanId];
        }

        #endregion

        #region Methods to interact with the DB

        private async Task<List<LoanItem>> LoadFromDB()
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localFolder = applicationData.LocalFolder;
            try
            {
                StorageFile file = await localFolder.GetFileAsync(loandb);
                string data = await FileIO.ReadTextAsync(file);
                if (String.IsNullOrEmpty(data))
                    return new List<LoanItem>();

                return JsonConvert.DeserializeObject<List<LoanItem>>(data);
            }
            catch (UnauthorizedAccessException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return new List<LoanItem>();
        }

        private async void WriteToDB()
        {
            string json = JsonConvert.SerializeObject(this.Items);
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localFolder = applicationData.LocalFolder;
            try
            {
                StorageFile sampleFile = await localFolder.CreateFileAsync(loandb, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, json);
            }
            catch (System.UnauthorizedAccessException e)
            {   

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }

    public class LoanItem
    {
        public int Id { get; set; }
        public Loan Simulated { get; set; }
        public Loan Real { get; set; }
    }

    public class Loan
    {
        public double Rate { get; set; }
        public double PeriodRate { get; set; }
        public double Principal { get; set; }
        public double TotalRemainingInterest { get; set; }
        public double TotalExpectedInterest { get { return this.TotalRemainingInterest + this.TotalInterestPaid; } }
        public double OriginalExpectedInterest { get; set; }
        public double TotalInterestPaid { get; set; }
        public double SavingsOnInterest { get; set; }
        public double Balance { get; set; }
        public double ComputedPeriodicPayment { get; set; }
        public int NumPeriods { get; set; }
        public List<Payment> Payments { get; set; }
        public Loan(double rate, int numPeriods, double principal)
        {
            this.Rate = rate;
            this.NumPeriods = numPeriods;
            this.Principal = principal;
            this.TotalRemainingInterest = 0;
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
            this.TotalRemainingInterest = payment.InterestRemainingAfterPayment;
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
                if (this.Balance < 1)
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
            this.TotalRemainingInterest = this.GetTotalRemainingInterest(this.Principal, this.NumPeriods);
            this.OriginalExpectedInterest = this.TotalRemainingInterest;
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cMonth">Completed Period Num (starting from 0)</param>
        ///// <returns></returns>
        //private double GetRemainingLoanBalance(int cPeriod)
        //{
        //    return Math.Round(this.Principal * (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - Math.Pow(1 + this.PeriodRate, cPeriod)) / (Math.Pow(1 + this.PeriodRate, this.NumPeriods) - 1), 3, MidpointRounding.ToEven);
        //}

        private double GetInterestForMonth(double balance)
        {
            return Math.Round(this.PeriodRate * balance, 3, MidpointRounding.ToEven);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="balance">Remaining balance</param>
        /// <param name="rPeriod">Remaining periods</param>
        /// <returns></returns>
        private double GetTotalRemainingInterest(double balance, int rPeriod)
        {
            double sum = 0;
            double interest = 0;
            double principal = 0;
            for (int i = 0; i < rPeriod; i++)
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
