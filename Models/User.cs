﻿using System.ComponentModel.DataAnnotations;

namespace usingLinq.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
