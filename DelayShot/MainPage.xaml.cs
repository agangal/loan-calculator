/*****************************
 *                           *
 * Copyright - Ashish Gangal *
 *                           *
 *****************************/

namespace DelayShot
{
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

        public MainPage()
        {
            this.InitializeComponent();
            this.mInt = rate / 12;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var processor = App.Processor;
            base.OnNavigatedTo(e);
            int id = App.Processor.AddNewLoan(rate, nMonth, principal);
            
            processor.RecordLoanPayment(895, 0, 0, id);
            processor.RecordLoanPayment(0, 1, 0, id);
            processor.RecordLoanPayment(0, 50, 0, id);
            processor.RecordLoanPayment(345.61, 19.14, 1, id);
            processor.RecordLoanPayment(498.16, 1.84, 1, id);
            processor.RecordLoanPayment(494.62, 5.38, 1, id);
            processor.RecordLoanPayment(500, 0, 2, id);
            processor.RecordLoanPayment(0, 1, 2, id);
            processor.RecordLoanPayment(999, 1, 2, id);
            processor.RecordLoanPayment(999, 1, 2, id);
            processor.RecordLoanPayment(999, 1, 2, id);
            processor.RecordLoanPayment(3000, 1, 3, id);
            processor.RecordLoanPayment(999, 1, 3, id);
            processor.RecordLoanPayment(1999, 1, 3, id);
        }
    }
}
