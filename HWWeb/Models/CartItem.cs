using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HWWeb.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PhoneId { get; set; }

        [ForeignKey("PhoneId")]
        public Phone Phone { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
