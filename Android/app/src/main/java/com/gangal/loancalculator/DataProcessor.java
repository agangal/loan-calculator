package com.gangal.loancalculator;
import com.google.*;
import com.google.gson.Gson;

import java.util.List;
public class DataProcessor {

    private String loandb = "loandb.json";
    private Gson jsonLib;
    public List<LoanItem> Items;
    private int loanItemId = 0;
    public DataProcessor()
    {
        this.Initialize();
    }

    private void Initialize()
    {
        this.jsonLib = new Gson();
        //this.Items = this.LoadFromDB();
    }

    public LoanItem AddNewLoan(double rate, int numPeriods, double principal)
    {
        LoanItem loan = new LoanItem();
        loan.Simulated = new Loan(rate, numPeriods, principal);
        loan.Simulated.GeneratePeriodicPayments();
        loan.Real = new Loan(rate, numPeriods, principal);
        loan.Id = loanItemId;
        loanItemId++;
        Items.add(loan);
        this.WriteToDB();
        return loan;
    }

    public void RecordLoanPayment(double principal, double interest, int currPeriodNum, int loanId)
    {
        if (this.Items.size() == 0)
            return;

        LoanItem item = this.Items.get(loanId);
        item.Real.AddNewPayment(principal, interest, currPeriodNum);
        item.Real.SavingsOnInterest = item.Real.OriginalExpectedInterest - item.Real.TotalExpectedInterest;
        this.WriteToDB();
    }

    public LoanItem GetLoanData(int loanId)
    {
        if (this.Items.size() < loanId)
            return null;

        return this.Items.get(loanId);
    }

    private void LoadFromDB()
    {

    }

    private void WriteToDB()
    {

    }
}
