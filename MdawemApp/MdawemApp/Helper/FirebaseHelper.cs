using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MdawemApp.Helper
{
    public class FirebaseHelper
    {
        string webAPIKey = "AIzaSyDuxpf83oL4rNwmPBV06DEid9xUWPNyOWU";
        FirebaseAuthProvider authProvider;

        public  FirebaseHelper()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }

        public async Task<bool> Register(string email, string password)
        {
            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }
            return false;
        }
    }
}
