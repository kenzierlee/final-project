using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace keepr.Models
{
    public class Pin
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public string Href { get; set; }
        [Required]
        public int Views { get; set; }
        [Required]
        public int Saves { get; set; }
        [Required]
        // 0 = False; 1 = True;
        public int Public { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}