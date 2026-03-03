using Microsoft.AspNetCore.Identity;

namespace StockSense.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? EmployeeId { get; set; }
        public string? BranchLocation { get; set; }
    }

}
