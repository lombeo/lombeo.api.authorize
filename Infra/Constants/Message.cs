namespace Lombeo.Api.Authorize.Infra.Constants
{
    public class Message
    {
        public static class CommonMessage
        {
            // Common
            public const string NOT_ALLOWED = "Common_403"; // You are not allowed.
            public const string NOT_AUTHEN = "Common_401";
            public const string NOT_FOUND = "Common_404"; // Not found.
            public const string ERROR_HAPPENED = "Common_500"; // An error occurred, please contact the admin or try again later!
            public const string ACTION_SUCCESS = "Common_200"; // Action successfully.
            public const string MISSING_PARAM = "Common_005"; // Missing input parameters. Please check again!
            public const string INVALID_FORMAT = "Common_006"; //Invalid format. Please check again!;
            public const string PERMISSION_REQUIRED = "Common_007"; //You do not have permission to perform this action!
            public const string ACTION_FAIL = "Common_008"; //Internal server error!
        }
    }
}
