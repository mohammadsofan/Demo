﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Cart Cart { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = null!;
    }
}