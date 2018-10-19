package com.gangal.loancalculator;
import java.util.ArrayList;
import java.util.List;
public class Loan {

    private int precision = 2;
    public double Rate; // { get; set; }
    public double PeriodRate; // { get; set; }
    public double Principal; // { get; set; }
    public double TotalRemainingInterest; // { get; set; }
    public double TotalExpectedInterest;// { get { return Math.Round(this.TotalRemainingInterest + this.TotalInterestPaid, precision, MidpointRounding.ToEven); } }
    public double OriginalExpectedInterest;// { get; set; }
    public double TotalInterestPaid;// { get; set; }
    public double SavingsOnInterest ;//{ get; set; }
    public double Balance ;//{ get; set; }
    public double ComputedPeriodicPayment;// { get; set; }
    public int NumPeriods;// { get; set; }
    public List<Payment> Payments;// { get; set; }
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
        this.Payments = new ArrayList<Payment>();
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
        Payments.add(payment);
    }

    public void GeneratePeriodicPayments()
    {
        for (int i = 0; i < this.NumPeriods; i++)
        {
            double interest = this.GetInterestForMonth(this.Balance);
            this.TotalInterestPaid += interest;
            double principalPaid = Math.round(this.ComputedPeriodicPayment - interest);
            Payment payment = new Payment(principalPaid, interest);
            this.Balance = Math.round(this.Balance - principalPaid);
            payment.TotalInterestPaid = this.TotalInterestPaid;
            payment.InterestRemainingAfterPayment = this.GetTotalRemainingInterest(this.Balance, this.NumPeriods - i - 1);
            this.Payments.add(payment);
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
        return Math.round(principal
                * (this.PeriodRate * Math.pow(1 + this.PeriodRate, numPeriods))
                / (Math.pow(1 + this.PeriodRate, this.NumPeriods) - 1));
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
        return Math.round(this.PeriodRate * balance);
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

        return Math.round(sum);
    }
}
