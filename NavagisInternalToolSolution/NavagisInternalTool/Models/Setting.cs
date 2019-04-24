using System.ComponentModel.DataAnnotations;


namespace NavagisInternalTool.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Client ID")]
        public string ClientId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Billing Account Name")]
        public string BillingAccountName { get; set; }
    }
}