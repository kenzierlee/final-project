using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace keepr.Models
{
    public class Board
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string UserId { get; set; }
    }
    public class BoardPin
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string BoardId { get; set; }
        [Required]
        public string PinId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}