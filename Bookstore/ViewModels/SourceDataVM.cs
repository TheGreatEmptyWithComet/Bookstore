using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookstore
{
    public class SourceDataVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;

        private string currentPage;
        public string CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                NotifyPropertyChanged(nameof(CurrentPage));
            }
        }
        #endregion

        
        #region Inner view models
        /****************************************************************************************/
        public UserPageVM UserPageVM { get; private set; }
        public AuthorPageVM AuthorPageVM { get; private set; }
        public PublisherPageVM PublisherPageVM { get; private set; }
        public GenrePageVM GenrePageVM { get; private set; }
        public CampaingPageVM CampaingPageVM { get; private set; }
        public CustomerPageVM CustomerPageVM { get; private set; }
        public ArrivalPageVM ArrivalPageVM { get; private set; }
        public SalePageVM SalePageVM { get; private set; }
        public ReservePageVM ReservePageVM { get; private set; }

        #endregion


        #region Commands
        /****************************************************************************************/
        public ICommand PageNavigationCommand { get; private set; }

        #endregion


        #region Constructor
        /****************************************************************************************/
        public SourceDataVM(Context context)
        {
            this.context = context;

            // Set the start page
            CurrentPage = "UserPageView.xaml";

            // Init inner view models
            UserPageVM = new UserPageVM(context);
            AuthorPageVM = new AuthorPageVM(context);
            PublisherPageVM = new PublisherPageVM(context);
            GenrePageVM = new GenrePageVM(context);
            CampaingPageVM = new CampaingPageVM(context);
            CustomerPageVM = new CustomerPageVM(context);
            ArrivalPageVM = new ArrivalPageVM(context);
            SalePageVM = new SalePageVM(context);
            ReservePageVM = new ReservePageVM(context);

            InitCommands();
        }
        #endregion


        #region Methods
        /****************************************************************************************/
        private void InitCommands()
        {
            PageNavigationCommand = new RelayCommand<string>(p => CurrentPage = p);
        }
        #endregion
    }
}
