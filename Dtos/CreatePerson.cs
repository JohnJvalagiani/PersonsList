using IG.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace service.server.Dtos
{
    public class CreatePerson
    {
        public string FirstNameENG { get; set; }
        public string LastNameENG { get; set; }
        public string FirstNameGEO { get; set; }
        public string LastNameGEO { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string PersonalNumber { get; set; }
        public string City { get; set; }
        public Gender Sex { get; set; }
        [DisplayName("Phone number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
    }
}
