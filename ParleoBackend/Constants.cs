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
            public const string INVALID_LOCATION = "Invalid location format. Latitude between -90 and 90, longitute betwee -180 and 180";

            public const string EVENT_NOT_FOUND = "Event with such id is not found";
            public const string EXCEEDED_PARTICIPANTS_COUNT_LIMIT = "Exceeded the limit of participants";
            public const string NO_UPDATE_RIGHTS = "You are not an event creator";
            public const string USER_ALREADY_PARTICIPATE = "At least one user already participates";
            public const string FRIEND_REQUSET_FAILED = "Unable to add friend. Some error occured";
          
            public const string EXPIRED_TOKEN = "Token expired";
            public const string NOT_VALID_TOKEN = "Token is not valid";
            public const string TOKEN_ALREADY_SENT = "Token has already been sent";

            public const string INVALID_LANGUAGE = "Language has incorrect code";
            public const string INVALID_LANGUAGES = "At least one language has incorrect code";
            public const string INVALID_HOBBY = "At least one hobby doesn't exist";
            public const string INCORRECT_LANGUAGE_LEVEL = "Language level must be greater than 0 and less than 5 inclusively";
            public const string DUPLICATES_ARE_NOT_ALLOWED = "At least one language has duplicate";
            public const string INVALID_BIRTHDATE = "Invalid birth date. You must be from 16 to 100 years old";
            public const string INVALID_START_DATE = "Event should start in a maximum of 60 days";
            public const string INVALID_END_DATE = "Event should end in a maximum of 24 hours";
        }

        public static class Restrictions
        {
            public const int MIN_PAGE_SIZE = 1;
            public const int MAX_PAGE_SIZE = 100;

            public const int MIN_AGE = 16;
            public const int MAX_AGE = 100;

            public const int MAX_LANGUAGE_LEVEL = 5;

            public const int MIN_PARTICIPANTS_COUNT = 1;
            public const int MAX_PARTICIPANTS_COUNT = 30;

            public const int MIN_LATITUDE_VALUE = -90;
            public const int MAX_LATITUDE_VALUE = 90;
            public const int MIN_LONGITUDE_VALUE = -180;
            public const int MAX_LONGITUDE_VALUE = 180;

            public const int MIN_HOURS_TO_START_DATE = 1;
            public const int MAX_DAYS_TO_START_DATE = 60;

            public const int MAX_HOURS_TO_END_DATE = 24;
        }
    }
}
