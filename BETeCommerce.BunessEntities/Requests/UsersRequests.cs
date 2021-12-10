using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BETeCommerce.BunessEntities.Requests
{
    [DataContract]
    public class AddOrUdateUserRequest
    {
        [DataMember(Name = "fullName", IsRequired = true)]
        [Required(AllowEmptyStrings = false)]
        public string FullName { get; set; }

        [DataMember(Name = "email", IsRequired = true)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [DataMember(Name = "password", IsRequired = true)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$", 
            ErrorMessage = "Password must have Minimum 6 characters, at least one uppercase letter, one lowercase letter and one number")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(Name = "confirmPassword", IsRequired = true)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
            ErrorMessage = "Password must have Minimum 6 characters, at least one uppercase letter, one lowercase letter and one number")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserRequest
    {
        [DataMember(Name = "email", IsRequired = true)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [DataMember(Name = "password", IsRequired = true)]
        // Minimum 6 characters, at least one uppercase letter, one lowercase letter and one number
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
            ErrorMessage = "Password must have Minimum 6 characters, at least one uppercase letter, one lowercase letter and one number")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
