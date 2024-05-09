﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.core.Entities.Order_Aggregate
{
    public class Address
    {
        [Required]
        public  string FirstName { get; set; } 
        public string LastName { get; set; } = null!;
        public string Street { get; set; }= null!;
        public string City { get; set; }= null!;
        public string Country { get; set; }= null!;

    }
}
