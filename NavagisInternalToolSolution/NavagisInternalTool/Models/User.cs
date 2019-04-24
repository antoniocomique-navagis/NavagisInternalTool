using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace NavagisInternalTool.Models
{
    public class User
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index("Username_Index", IsUnique = true)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

    }
}