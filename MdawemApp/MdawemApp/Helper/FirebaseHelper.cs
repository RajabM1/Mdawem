using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
