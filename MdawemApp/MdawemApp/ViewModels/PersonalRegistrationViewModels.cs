using Firebase.Database;
using Firebase.Database.Query;
using MdawemApp.Helper;
using MdawemApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdawemApp.ViewModels
{
    class PersonalRegistrationViewModels : INotifyPropertyChanged
    {
        public Validation validate;

        public FirebaseHelper firebaseHelper;

        public Employee _employee { get; set; }

        public ICommand SaveCommand { get; set; }

        private bool _isFirstNameValid = false;
        private bool _isLastNameValid = false;
        private bool _isEmailValid = false;
        private bool _isPasswordValid = false;
        private bool _isPhoneNumberValid = false;

        public bool FirstNameFlag
        {
            get { return !_isFirstNameValid; }

        }
        public bool LastNameFlag
        {
            get { return !_isLastNameValid; }
        }
        public bool EmailFlag
        {
            get { return !_isEmailValid; }
        }
        public bool PasswordFlag
        {
            get { return !_isPasswordValid; }
        }
        public bool PhoneNumberFlag
        {
            get { return !_isPhoneNumberValid; }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                ValidateFirstName(FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                ValidateLastName(LastName);
            }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                ValidateEmail(EmailAddress);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                ValidatePassword(Password);
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                ValidatePhoneNumber(PhoneNumber);
            }
        }

        public PersonalRegistrationViewModels()
        {
            _employee = new Employee();
            validate = new Validation();
            firebaseHelper = new FirebaseHelper();
            SaveCommand = new Command(async () =>
            {
                await SaveDataToFirebase(_employee);
            });
        }



        private async Task SaveDataToFirebase(Employee data)
        {
            if (IsAllInputValid)
            {
                _employee.FirstName = _firstName;
                _employee.LastName = _lastName;
                _employee.PhoneNumber = _phoneNumber;
                await firebaseHelper.Register(_emailAddress, _password);
                FirebaseClient client = new FirebaseClient(
                "https://mdawemt-default-rtdb.firebaseio.com/"
            );
                await client.Child("Employees").PostAsync(data);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Fill All Inputs", "OK");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsAllInputValid
        {
            get
            {
                return _isFirstNameValid
                    && _isLastNameValid
                    && _isEmailValid
                    && _isPasswordValid
                    && _isPhoneNumberValid;
            }
        }

        public void ValidateFirstName(string firstName)
        {
            _isFirstNameValid = validate.IsValidName(firstName);
            OnPropertyChanged(nameof(FirstNameFlag));
        }

        public void ValidateLastName(string lastName)
        {
            _isLastNameValid = validate.IsValidName(lastName);
            OnPropertyChanged(nameof(LastNameFlag));
        }

        public void ValidateEmail(string email)
        {
            _isEmailValid = validate.IsValidEmail(email);
            OnPropertyChanged(nameof(EmailFlag));
        }

        public void ValidatePassword(string password)
        {
            _isPasswordValid = validate.IsValidPassword(password);
            OnPropertyChanged(nameof(PasswordFlag));
        }

        public void ValidatePhoneNumber(string phoneNumber)
        {
            _isPhoneNumberValid = validate.IsValidPhoneNumber(phoneNumber);
            OnPropertyChanged(nameof(PhoneNumberFlag));
        }
    
    }
}
