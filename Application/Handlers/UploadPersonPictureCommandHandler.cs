using AutoMapper;
using Core.Commands;
using Core.CustomExceptions;
using Core.Models;
using Core.Services.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class UploadPersonPictureCommandHandler : IRequestHandler<UploadPersonPictureCommand, string>
    {
        private readonly IOptions<FilePathConfig> _filePathConfig;
        private readonly IPersonsRepo _personsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UploadPersonPictureCommandHandler> _logger;


        public UploadPersonPictureCommandHandler(
            IOptions<FilePathConfig> filePathConfig,
            IPersonsRepo personsRepo,
            IMapper mapper,
            ILogger<UploadPersonPictureCommandHandler> logger
          )
        {
            _filePathConfig = filePathConfig;
            _personsRepo = personsRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<string> Handle(UploadPersonPictureCommand request, CancellationToken cancellationToken)
        {
            var thePerson = await _personsRepo.GetByIdAsync(request.Id) ?? throw new PersonNotFoundException($"Person not found with the specified id {request.Id}");

            if (File.Exists(thePerson.Picture))
            {
                File.Delete(thePerson.Picture);
            }

            var basePath = _filePathConfig.Value.BasePath;
            string filePath = Path.Combine(basePath, request.FormFile.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.FormFile.CopyToAsync(fileStream);
            }

            thePerson.Picture = filePath;

             _personsRepo.Update(thePerson);

            await _personsRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return thePerson.Picture;
        }
    }
}
