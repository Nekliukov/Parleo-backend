namespace ParleoBackend
{
    public static class Constants
    {
        public static class Errors
        {
            public const string EMAIL_ALREADY_EXISTS = "User with such email is already exists";
            public const string PASSWORDS_DO_NOT_MATCH = "Passwords do not match";
            public const string INVALID_PASSWORD = "Invalid password";
            public const string USER_CREATION_FAILED = "Unable to create user, something went wrong";
            public const string USER_NOT_FOUND = "User is not found";
            public const string EMAIL_NOT_FOUND = "User with such email does not exist";
        }
    }
}
