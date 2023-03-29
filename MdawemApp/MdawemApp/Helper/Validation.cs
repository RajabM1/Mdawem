using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MdawemApp.Helper
{
    public class Validation
    {
        public bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(email);

            return match.Success;

        }

        public bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(password);

            return match.Success;
        }

        public bool IsValidName(string name)
        {
            string pattern = @"^[A-Za-z]+$";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(name);

            return match.Success;
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(phoneNumber);

            return match.Success;

        }
    }
}

