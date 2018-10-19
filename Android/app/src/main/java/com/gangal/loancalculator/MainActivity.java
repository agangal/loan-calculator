package com.gangal.loancalculator;

import android.provider.ContactsContract;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        DataProcessor proc = new DataProcessor();
        setContentView(R.layout.activity_main);
    }
}
