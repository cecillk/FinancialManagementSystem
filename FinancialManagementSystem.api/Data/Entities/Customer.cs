﻿using System.ComponentModel.DataAnnotations;

namespace FinancialManagementSystem.api.Data.Entities
{
    public class Customer
    {
        public string CustomerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Account> Accounts { get; set; }

    }




}

