using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace IG.Core.Data.Entities
{
    public class Person : BaseEntity
    {
        public string FirstNameENG { get; set; }
        public string LastNameENG { get; set; }
        public string FirstNameGEO { get; set; }
        public string LastNameGEO { get; set; }
        public Gender Sex { get; set; }
        public DateTime BirthDate { get; set; }

        public string PersonalNumber { get; set; }
        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<ConnectedPerson> ConnectedPerson { get; set; } = new List<ConnectedPerson>();

        public string  Picture { get; set; }
    }


    public enum ConnectedPersonType
    {
        relative, employee, friend
    }

    public enum Gender
    {
        M,F
    }
}
