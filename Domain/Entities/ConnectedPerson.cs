using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IG.Core.Data.Entities
{
    public class ConnectedPerson:BaseEntity
    {

        public ConnectedPersonType PersonType { get; set; }

        public int PersonId { get; set; }
    }
}
