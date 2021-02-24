using Atomus.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atomus.Windows.Controls.Join
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DefaultJoin : UserControl, IAction
    {
        private AtomusControlEventHandler beforeActionEventHandler;
        private AtomusControlEventHandler afterActionEventHandler;


        #region Init
        public DefaultJoin()
        {
            //this.Resources = Application.Current.Windows[0].Resources;

            //this.Style = (System.Windows.Style)this.Resources.MergedDictionaries[0]["UserControlDefaultLogin"];

            InitializeComponent();

            this.DataContext = new ViewModel.DefaultJoinViewModel(this);

            this.UserAgreement.SetValidator("UserAgreement", UpdateSourceTrigger.PropertyChanged);
            this.PersonalInformationCollectionAgreement.SetValidator("PersonalInformationCollectionAgreement", UpdateSourceTrigger.PropertyChanged);

            this.JoinEmail.SetValidator("JoinEmail", UpdateSourceTrigger.PropertyChanged);
            this.ChangeEmail.SetValidator("ChangeEmail", UpdateSourceTrigger.PropertyChanged);

            this.EmailRetry.SetValidator("EmailRetry", UpdateSourceTrigger.PropertyChanged);
            this.AccessNumberOld.SetValidator("AccessNumberOld", UpdateSourceTrigger.PropertyChanged);

            this.AccessNumber.SetValidator("AccessNumber", UpdateSourceTrigger.PropertyChanged);
            this.AccessNumberRetry.SetValidator("AccessNumberRetry", UpdateSourceTrigger.PropertyChanged);

            this.AccessNumber2.SetValidator("AccessNumber", UpdateSourceTrigger.PropertyChanged);
            this.AccessNumberRetry2.SetValidator("AccessNumberRetry", UpdateSourceTrigger.PropertyChanged);

            this.NickName.SetValidator("NickName", UpdateSourceTrigger.PropertyChanged);
        }
        #endregion

        #region Dictionary
        #endregion

        #region Spread
        #endregion

        #region IO
        object IAction.ControlAction(ICore sender, AtomusControlArgs e)
        {
            try
            {
                this.beforeActionEventHandler?.Invoke(this, e);

                switch (e.Action)
                {
                    case "Join.Cancel":
                        return true;

                    case "Form.Size":
                        //if (this.Background == null)//이미지를 늦게 불러오는 경우에 다시 반영
                        //    this.Background = (this.DataContext as ViewModel.DefaultJoinViewModel).BackgroundImage;

                        //this.afterActionEventHandler?.Invoke(this, e);
                        return true;

                    case "Join.ClearPassword":
                        this.AccessNumberOld.Password = "";
                        this.AccessNumber.Password = "";
                        this.AccessNumberRetry.Password = "";
                        return true;

                    default:
                        throw new AtomusException("'{0}'은 처리할 수 없는 Action 입니다.".Translate(e.Action));
                }
            }
            finally
            {
                if (!new string[] { "Join.ClearPassword" }.Contains(e.Action))
                    this.afterActionEventHandler?.Invoke(this, e);
            }
        }
        #endregion

        #region Event
        event AtomusControlEventHandler IAction.BeforeActionEventHandler
        {
            add
            {
                this.beforeActionEventHandler += value;
            }
            remove
            {
                this.beforeActionEventHandler -= value;
            }
        }
        event AtomusControlEventHandler IAction.AfterActionEventHandler
        {
            add
            {
                this.afterActionEventHandler += value;
            }
            remove
            {
                this.afterActionEventHandler -= value;
            }
        }

        private void DefaultJoin_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MouseLeftButtonDownDragMove(object sender, MouseButtonEventArgs e)
        {
            this.afterActionEventHandler?.Invoke(this, new AtomusControlEventArgs("DragMove", null));
        }
        #endregion

        #region ETC
        #endregion

        private void AccessNumberOld_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ViewModel.DefaultJoinViewModel).AccessNumberOld = (sender as PasswordBox).Password;
        }
        private void AccessNumber_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ViewModel.DefaultJoinViewModel).AccessNumber = (sender as PasswordBox).Password;
        }

        private void AccessNumberRetry_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ViewModel.DefaultJoinViewModel).AccessNumberRetry = (sender as PasswordBox).Password;
        }
    }
}