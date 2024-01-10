using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    public class UpdatePersonPictureCommand : IRequest<string>
    {
        public int Id { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
