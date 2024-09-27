namespace Lombeo.Api.Authorize.Infra.Constants
{
    public class Message
    {
        public static class CommonMessage
        {
            // Common
            public const string NOT_ALLOWED = "Bạn không được phép"; // You are not allowed.
            public const string NOT_AUTHEN = "Common_401";
            public const string NOT_FOUND = "Không tìm thấy"; // Not found.
            public const string ERROR_HAPPENED = "Đã có lỗi xảy ra, vui lòng liên hệ với quản trị viên hoặc thử lại sau!"; // An error occurred, please contact the admin or try again later!
            public const string ACTION_SUCCESS = "Thành công"; // Action successfully.
            public const string MISSING_PARAM = "Bạn chưa nhập đủ trường, hãy kiểm tra lại!"; // Missing input parameters. Please check again!
            public const string INVALID_FORMAT = "Định dạng chưa đúng, hãy kiểm tra lại!"; //Invalid format. Please check again!;
            public const string PERMISSION_REQUIRED = "Bạn không có quyền để thực hiện hành động này!"; //You do not have permission to perform this action!
            public const string ACTION_FAIL = "Common_008"; //Internal server error!
        }

        public static class AuthenMessage
        {
            public const string EXIST_EMAIL = "Email của bạn đã tồn tại"; //Your email already exists.
            public const string EXIST_USERNAME = "Tên người dùng của bạn đã tồn tại"; //Your username already exists.
            public const string INVALID_EMAIL = "Định dạng email không đúng"; //The email you just entered is not in the correct format.
            public const string INVALID_USERNAME = "Tên người dùng của bạn không thể chứa dấu cách"; //Your username cannot contain spaces.
            public const string INVALID_PASSWORD = "Mật khẩu của bạn cần ít nhất 6 ký tự, 1 ký tự thường, 1 ký tự in hoa và 1 ký tự đặc biệt!"; 
            //Your password must contain at least 6 characters, one lowercase letter, one uppercase letter, 1 special character and one number
            public const string INVALID_LOGIN = "Tài khoản hoặc mật khẩu không đúng!"; //Invalid username or password.
            public const string INVALID_USER = "You need to update your profile!";
        }
    }
}
