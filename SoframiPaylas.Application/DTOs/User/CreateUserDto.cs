using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Application.DTOs
{
    public class CreateUserDto
    {

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public bool IsHost { get; set; }
        public string? ProfilePicture { get; set; }

        public string? About { get; set; }
    }
}