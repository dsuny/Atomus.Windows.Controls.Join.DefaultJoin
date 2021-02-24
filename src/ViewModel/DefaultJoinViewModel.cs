using Atomus.ComponentModel.DataAnnotations;
using Atomus.Control;
using Atomus.Diagnostics;
using Atomus.Security;
using Atomus.Service;
using Atomus.Windows.Controls.Join.Controllers;
using Atomus.Windows.Controls.Join.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Atomus.Windows.Controls.Join.ViewModel
{
    public class DefaultJoinViewModel : MVVM.ViewModel
    {
        #region Declare
        private bool _UserAgreement;

        [IsTrue]
        [Display(Name = "이용약관 동의")]
        public bool UserAgreement
        {
            get { return _UserAgreement; }
            set
            {
                if (this._UserAgreement != value)
                {
                    this._UserAgreement = value; this.NotifyPropertyChanged();
                }
            }
        }

        private string _UserAgreement_Detail;

        public string UserAgreement_Detail
        {
            get { return _UserAgreement_Detail; }
            set
            {
                if (this._UserAgreement_Detail != value)
                {
                    this._UserAgreement_Detail = value; this.NotifyPropertyChanged();
                }
            }
        }

        private bool _PersonalInformationCollectionAgreement;

        [IsTrue]
        [Display(Name = "개인정보 수집 및 이용 동의")]
        public bool PersonalInformationCollectionAgreement
        {
            get { return _PersonalInformationCollectionAgreement; }
            set
            {
                if (this._PersonalInformationCollectionAgreement != value)
                {
                    this._PersonalInformationCollectionAgreement = value; this.NotifyPropertyChanged();
                }
            }
        }

        private string _PersonalInformationCollectionAgreement_Detail;

        public string PersonalInformationCollectionAgreement_Detail
        {
            get { return _PersonalInformationCollectionAgreement_Detail; }
            set
            {
                if (this._PersonalInformationCollectionAgreement_Detail != value)
                {
                    this._PersonalInformationCollectionAgreement_Detail = value; this.NotifyPropertyChanged();
                }
            }
        }

        private bool _IsNew;

        public bool IsNew
        {
            get { return _IsNew; }
            set
            {
                if (this._IsNew != value)
                {
                    this._IsNew = value; this.NotifyPropertyChanged();

                    this.JoinEmail = "";
                    this.ChangeEmail = "";
                    this.EmailRetry = "";
                    this.AccessNumberOld = "";
                    this.AccessNumber = "";
                    this.AccessNumberRetry = "";
                    this.NickName = "";

                    if (this.Core != null)
                        (this.Core as IAction).ControlAction(this, new AtomusControlArgs("Join.ClearPassword", null));
                }
            }
        }

        private string _JoinEmail;

        [Required]
        [EmailAddress]
        [Compare("EmailRetry")]
        [Display(Name = "이메일")]
        public string JoinEmail
        {
            get { return _JoinEmail; }
            set
            {
                if (this._JoinEmail != value)
                {
                    this._JoinEmail = value; this.NotifyPropertyChanged();

                    if (this.IsNew)
                        this.NotifyPropertyChanged("EmailRetry");
                }
            }
        }

        private string _ChangeEmail;

        [Required]
        [EmailAddress]
        [Display(Name = "이메일")]
        public string ChangeEmail
        {
            get { return _ChangeEmail; }
            set
            {
                if (this._ChangeEmail != value)
                {
                    this._ChangeEmail = value; this.NotifyPropertyChanged();
                }
            }
        }

        private string _EmailRetry;

        [Required]
        [EmailAddress]
        [Compare("JoinEmail")]
        [Display(Name = "이메일 확인")]
        public string EmailRetry
        {
            get { return _EmailRetry; }
            set
            {
                if (this._EmailRetry != value)
                {
                    this._EmailRetry = value; this.NotifyPropertyChanged();

                    if (this.IsNew)
                        this.NotifyPropertyChanged("JoinEmail");
                }
            }
        }

        private string _AccessNumberOld;

        [Required]
        [Different("AccessNumber")]
        [Different("AccessNumberRetry")]
        [Display(Name = "기존 비밀번호")]
        public string AccessNumberOld
        {
            get { return _AccessNumberOld; }
            set
            {
                if (this._AccessNumberOld != value)
                {
                    this._AccessNumberOld = value; this.NotifyPropertyChanged();

                    this.NotifyPropertyChanged("AccessNumber");
                    this.NotifyPropertyChanged("AccessNumberRetry");
                }
            }
        }

        private string _AccessNumber;

        [Required]
        [Compare("AccessNumberRetry")]
        [Different("AccessNumberOld")]
        [Display(Name = "비밀번호")]
        public string AccessNumber
        {
            get { return _AccessNumber; }
            set
            {
                if (this._AccessNumber != value)
                {
                    this._AccessNumber = value; this.NotifyPropertyChanged();

                    this.NotifyPropertyChanged("AccessNumberRetry");
                    this.NotifyPropertyChanged("AccessNumberOld");
                }
            }
        }

        private string _AccessNumberRetry;

        [Required]
        [Compare("AccessNumber")]
        [Different("AccessNumberOld")]
        [Display(Name = "비밀번호 확인")]
        public string AccessNumberRetry
        {
            get { return _AccessNumberRetry; }
            set
            {
                if (this._AccessNumberRetry != value)
                {
                    this._AccessNumberRetry = value; this.NotifyPropertyChanged();

                    this.NotifyPropertyChanged("AccessNumber");
                    this.NotifyPropertyChanged("AccessNumberOld");
                }
            }
        }

        private string _NickName;

        [Required]
        [Display(Name = "별명")]
        public string NickName
        {
            get { return _NickName; }
            set
            {
                if (this._NickName != value)
                {
                    this._NickName = value; this.NotifyPropertyChanged();
                }
            }
        }

        private bool _IsEnabledControl;

        public bool IsEnabledControl
        {
            get { return _IsEnabledControl; }
            set
            {
                if (this._IsEnabledControl != value)
                {
                    this._IsEnabledControl = value; this.NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Property
        public ICore Core { get; set; }

        public ICommand JoinCommand { get; set; }
        public ICommand AccessNumberChangeCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        #region INIT
        public DefaultJoinViewModel()
        {
            this.Reset();

            this.JoinCommand = new MVVM.DelegateCommand(async () => { await this.JoinProcess(); }
                                                            , () => { return this.IsEnabledControl; });

            this.AccessNumberChangeCommand = new MVVM.DelegateCommand(async () => { await this.AccessNumberChangeProcess(); }
                                                            , () => { return this.IsEnabledControl; });

            this.CancelCommand = new MVVM.DelegateCommand(() => { this.CancelProcess(); }
                                                            , () => { return this.IsEnabledControl; });
        }
        public DefaultJoinViewModel(ICore core) : this()
        {
            this.Core = core;

            try
            {
                this.UserAgreement_Detail = this.Core.GetAttribute("UserAgreement");
                this.PersonalInformationCollectionAgreement_Detail = this.Core.GetAttribute("PersonalInformationCollectionAgreement");
            }
            catch (Exception ex)
            {
                DiagnosticsTool.MyTrace(ex);
            }
        }
        #endregion

        #region IO
        private async Task JoinProcess()
        {
            IResponse result;
            bool ok;

            ok = false;

            try
            {
                this.IsEnabledControl = false;
                (this.JoinCommand as Atomus.MVVM.DelegateCommand).RaiseCanExecuteChanged();

                this.ValidateResult = true;

                this.NotifyPropertyChanged("UserAgreement");
                this.NotifyPropertyChanged("PersonalInformationCollectionAgreement");

                this.NotifyPropertyChanged("JoinEmail");
                this.NotifyPropertyChanged("EmailRetry");
                this.NotifyPropertyChanged("NickName");

                this.NotifyPropertyChanged("AccessNumber");
                this.NotifyPropertyChanged("AccessNumberRetry");

                if (!this.ValidateResult)
                    return;

                result = await this.Core.SaveAsync(new DefaultJoinSaveModel()
                {
                    EMAIL = this.JoinEmail,
                    ACCESS_NUMBER = ((ISecureHashAlgorithm)this.Core.CreateInstance("SecureHashAlgorithm")).ComputeHashToBase64String(this.AccessNumber),
                    NICKNAME = this.NickName
                });

                if (result.Status == Status.OK)
                {
                    ok = true;

                    this.WindowsMessageBoxShow(Application.Current.Windows[0], "가입이 완료 되었습니다.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    this.WindowsMessageBoxShow(Application.Current.Windows[0], result.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                this.WindowsMessageBoxShow(Application.Current.Windows[0], ex);
            }
            finally
            {
                this.IsEnabledControl = true;
                (this.JoinCommand as Atomus.MVVM.DelegateCommand).RaiseCanExecuteChanged();

                if (ok)
                    this.CancelCommand.Execute(null);
            }
        }
        private async Task AccessNumberChangeProcess()
        {
            IResponse result;
            ISecureHashAlgorithm secureHashAlgorithm;
            bool ok;

            ok = false;

            try
            {
                this.IsEnabledControl = false;
                (this.AccessNumberChangeCommand as Atomus.MVVM.DelegateCommand).RaiseCanExecuteChanged();

                this.ValidateResult = true;

                this.NotifyPropertyChanged("UserAgreement");
                this.NotifyPropertyChanged("PersonalInformationCollectionAgreement");

                this.NotifyPropertyChanged("ChangeEmail");
                this.NotifyPropertyChanged("AccessNumberOld");

                this.NotifyPropertyChanged("AccessNumber");
                this.NotifyPropertyChanged("AccessNumberRetry");

                if (!this.ValidateResult)
                    return;

                secureHashAlgorithm = ((ISecureHashAlgorithm)this.Core.CreateInstance("SecureHashAlgorithm"));

                result = await this.Core.SavePasswordChangeAsync(new DefaultJoinSaveModel()
                {
                    EMAIL = this.ChangeEmail,
                    ACCESS_NUMBER = secureHashAlgorithm.ComputeHashToBase64String(this.AccessNumberOld),
                    NEW_ACCESS_NUMBER = secureHashAlgorithm.ComputeHashToBase64String(this.AccessNumber)
                });

                if (result.Status == Status.OK)
                {
                    ok = true;

                    this.WindowsMessageBoxShow(Application.Current.Windows[0], "비밀번호 변경이 완료 되었습니다.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    this.WindowsMessageBoxShow(Application.Current.Windows[0], result.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                this.WindowsMessageBoxShow(Application.Current.Windows[0], ex);
            }
            finally
            {
                this.IsEnabledControl = true;
                (this.AccessNumberChangeCommand as Atomus.MVVM.DelegateCommand).RaiseCanExecuteChanged();

                if (ok)
                    this.CancelCommand.Execute(null);
            }
        }
        private void CancelProcess()
        {
            try
            {
                this.IsEnabledControl = false;
                (this.CancelCommand as Atomus.MVVM.DelegateCommand).RaiseCanExecuteChanged();

                (this.Core as IAction).ControlAction(this, new AtomusControlArgs("Join.Cancel", null));

                this.Reset();
            }
            catch (Exception ex)
            {
                this.WindowsMessageBoxShow(Application.Current.Windows[0], ex);
            }
            finally
            {
                this.IsEnabledControl = true;
                (this.CancelCommand as Atomus.MVVM.DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private void Reset()
        {
            this.UserAgreement = false;
            this.PersonalInformationCollectionAgreement = false;
            this.IsNew = true;

            this.JoinEmail = "";
            this.ChangeEmail = "";
            this.EmailRetry = "";
            this.AccessNumberOld = "";
            this.AccessNumber = "";
            this.AccessNumberRetry = "";
            this.NickName = "";

            this.IsEnabledControl = true;
        }
        #endregion

        #region ETC
        #endregion
    }
}