using System.ComponentModel.DataAnnotations.Schema;

namespace UserRoles.Models.Team
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; 

        [NotMapped]
        public IFormFile? ImageFile { get; set; } 

        public string FacebookUrl { get; set; } = string.Empty;
        public string TwitterUrl { get; set; } = string.Empty;
        public string LinkedInUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
