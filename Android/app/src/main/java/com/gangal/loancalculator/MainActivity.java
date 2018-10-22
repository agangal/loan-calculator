package com.gangal.loancalculator;

import android.provider.ContactsContract;
import android.view.View;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class MainActivity extends AppCompatActivity {

    private DataProcessor processor;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        processor = new DataProcessor();
        setContentView(R.layout.activity_main);
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (processor == null)
        {
            processor = new DataProcessor();
        }
    }

    @Override
    protected void onPause() {
        super.onPause();
    }

    public void AddNewLoan(View v)
    {

    }
}
