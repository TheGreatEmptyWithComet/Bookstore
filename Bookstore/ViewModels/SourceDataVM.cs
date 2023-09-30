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
