using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Bookstore
{
    // task to complite
    // 1. add user to arrival, sale and reserve documents +
    // 2. refresh book page when opens
    // 3. add sale, reserve, arrival command to books page
    // 4. customise books page to different users
    // 5. add satistic
    // 6. add amount, cost column to books table
    
    
    public class MainVM : NotifyPropertyChangeHandler
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
        public LoginVM LoginVM { get; private set; }
        public SourceDataVM SourceDataVM { get; private set; }
        public BookPageVM BookPageVM { get; private set; }


        #endregion


        #region Commands
        /****************************************************************************************/
        public ICommand PageNavigationCommand { get; private set; }

        #endregion


        #region Constructor
        /****************************************************************************************/
        public MainVM()
        {
            context = new Context();

            // set the start page
            CurrentPage = "BooksPageView.xaml";

            // init inner view models
            LoginVM = new LoginVM(context);
            // On logout set the current page to the one that is available to unregistered users
            LoginVM.OnNotifyLogout += () => { CurrentPage = "BooksPageView.xaml"; };
            
            SourceDataVM = new SourceDataVM(context);
            BookPageVM = new BookPageVM(context);

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
