package com.gangal.loancalculator;

public class Payment {
    public double PrincipalPaid;// { get; set; }
    public double InterestPaid;// { get; set; }
    //public double TotalSinglePayment; { get { return this.PrincipalPaid + this.InterestPaid; } }
    public double InterestRemainingAfterPayment;// { get; set; }
    public double TotalInterestPaid;// { get; set; }
    public Payment(double principalPaid, double interestPaid)
    {
        this.PrincipalPaid = principalPaid;
        this.InterestPaid = interestPaid;
    }
}
