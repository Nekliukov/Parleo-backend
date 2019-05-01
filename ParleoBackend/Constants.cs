namespace ParleoBackend
{
    public static class Constants
    {
        public static class Errors
        {
            public const string EMAIL_ALREADY_EXISTS = "User with such email is already exists";
            public const string EMAIL_NOT_FOUND = "User with such email does not exist";
            public const string EMAIL_NOT_EXISTS = "Such email does not exist";
          
            public const string INVALID_PASSWORD = "Invalid password";
            public const string USER_CREATION_FAILED = "Unable to create user, something went wrong";
            public const string USER_NOT_FOUND = "User is not found";
            public const string WRONG_GUID_FORMAT = "Wrong GUID format";
            public const string TOKEN_ID_NOT_MATCH_URL_ID = "Id from url do not match Id from token";
            public const string INVALID_LOCATION = "Invalid location format";

            public const string EVENT_NOT_FOUND = "Event with such id is not found";
            public const string EXCEEDED_PARTICIPANTS_COUNT_LIMIT = "Exceeded the limit of participants";
            public const string NO_UPDATE_RIGHTS = "You are not an event creator";
            public const string USER_ALREADY_PARTICIPATE = "At least one user already participates";
          
            public const string EXPIRED_TOKEN = "Token expired";
            public const string NOT_VALID_TOKEN = "Token is not valid";
            public const string TOKEN_ALREADY_SENT = "Token has already been sent";

            public const string INVALID_LANGUAGE = "At least one language has incorrect code";
            public const string INVALID_HOBBY = "At least one hobby doesn't exist";
            public const string INCORRECT_LANGUAGE_LEVEL = "Language level must be greater than 0 and less than 5 inclusively";
        }
    }
}
