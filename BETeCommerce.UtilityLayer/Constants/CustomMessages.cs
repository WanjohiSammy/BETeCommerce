
namespace BETeCommerce.UtilityLayer.Constants
{
    public class CustomMessages
    {
        public static string Success { get; } = "Success";
        public static string UserRegistered { get; } = "User has been Registered Successfully!";

        public static string Error { get; } = "Error";
        public static string InvalidRequestMessage { get; } = "Invalid Request: {0}";
        public static string InvalidRequestLoggingError { get; } = "MethodName: {0}\n Request: {1}\n ErrorMessage: {2}";
        public static string ExceptionLoggingError { get; } = "MethodName: {0}\n Request: {1}\n Exception: {1}";
        public static string FailedDelete { get; } = "Failed To Delete";
        public static string InvalidLoginCred { get; } = "Invalid Email and Password";
        public static string UnableImageUpdate { get; } = "Unable to update Product Image";
    }
}
