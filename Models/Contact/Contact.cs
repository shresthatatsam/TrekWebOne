using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserRoles.Models.Contact
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
		[NotMapped]
		public string? Type { get; set; }
	}
}
