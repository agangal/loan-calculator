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
        public ObservableCollection<LoanItem> Loans { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            this.Loans = new ObservableCollection<LoanItem>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            base.OnNavigatedTo(e);
            this.Loans.Clear();
            foreach (var item in App.Processor.Items)
            {
                this.Loans.Add(item);
            }            
        }

        private void AddNewLoan_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.GoToAddNewLoan();            
        }

        private void LoansListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoanItem item = e.ClickedItem as LoanItem;

        }

        private void GoToAddNewLoan()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddLoan));
        }
    }
}
