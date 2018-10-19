/*****************************
 *                           *
 * Copyright - Ashish Gangal *
 *                           *
 *****************************/

namespace DelayShot
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private double rate = 0.0359;
        private int nMonth = 60;
        private double principal = 19976.15;
        private double mInt;
        public ObservableCollection<LoanItem> Loans { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            this.Loans = new ObservableCollection<LoanItem>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            base.OnNavigatedTo(e);
            //var processor = App.Processor;
            //LoanItem ln = App.Processor.AddNewLoan(rate, nMonth, principal);
            //int id = ln.Id;
            //processor.RecordLoanPayment(895, 0, 0, id);
            //processor.RecordLoanPayment(0, 1, 0, id);
            //processor.RecordLoanPayment(0, 50, 0, id);
            //processor.RecordLoanPayment(345.61, 19.14, 1, id);
            //processor.RecordLoanPayment(498.16, 1.84, 1, id);
            //processor.RecordLoanPayment(494.62, 5.38, 1, id);
            //processor.RecordLoanPayment(500, 0, 2, id);
            //processor.RecordLoanPayment(0, 1, 2, id);
            //processor.RecordLoanPayment(999, 1, 2, id);
            //processor.RecordLoanPayment(999, 1, 2, id);
            //processor.RecordLoanPayment(999, 1, 2, id);
            //processor.RecordLoanPayment(3000, 1, 3, id);
            //processor.RecordLoanPayment(999, 1, 3, id);
            //processor.RecordLoanPayment(1999, 1, 3, id);
        }

        private void AddNewLoan_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddLoan));
            //LoanItem ln = App.Processor.AddNewLoan(rate, nMonth, principal);
            //this.Loans.Add(ln);
        }

        private void LoansListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
