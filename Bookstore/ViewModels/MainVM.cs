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
    public class MainVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        private User currentUser;

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
            CurrentPage = "SourceDataPageView.xaml";

            // init inner view models
            LoginVM = new LoginVM(context);
            LoginVM.OnSetCurrentUser += (user) => { currentUser = user; };
            SourceDataVM = new SourceDataVM(context);

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
