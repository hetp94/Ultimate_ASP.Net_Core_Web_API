﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public abstract record EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 Character")]
        public string? Name { get; init; }
        [Required(ErrorMessage = "Age is required field")]
        [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
        public int Age { get; init; }
        [Required(ErrorMessage = "Position field is required")]
        [MaxLength(20, ErrorMessage = "Maximum length for the position is 20 characters")]
        public string? Position { get; init; }
    }
}
