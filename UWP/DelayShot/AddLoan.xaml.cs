﻿using System;
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

namespace DelayShot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddLoan : Page
    {
        public AddLoan()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(PrinicipalTextBox.Text, out double principal))
                return;

            if (!int.TryParse(TermInMonthsTextBox.Text, out int months))
                return;

            if (!double.TryParse(RateTextBox.Text, out double rate))
                return;

            App.Processor.AddNewLoan(rate, months, principal);
            this.Frame.GoBack();
        }
    }
}
