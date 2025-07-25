﻿using System.ComponentModel.DataAnnotations;

namespace APItask
{
    public class Category
    {
        public short Id { get; set;  }
        [Required]
        [MinLength(5), MaxLength(500)]
        public string Name { get; set; }
        public bool IsActive { get; set; }

    
    }
}
