﻿
using System.ComponentModel.DataAnnotations;
namespace atelier3.Models.ViewModels
{
        public class CreateRoleViewModel
        {
            [Required]
            [Display(Name = "Role")]

            public string RoleName { get; set; }

        }
    }


