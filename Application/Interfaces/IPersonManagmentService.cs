using Core.Models;
using Dtos.Dtos;
using IG.Core.Data.Entities;
using Microsoft.AspNetCore.Http;
using service.server.Dtos;
using service.server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Abstraction
{
    public interface IPersonManagementsService
    {
        Task<ServiceResponse<ReadPersonData>> AddPerson(CreatePerson createPerson);
        Task<ServiceResponse<ReadPersonData>> UpdatePerson(UpdatePerson updatePerson);
        Task<ServiceResponse<string>> UploadPersonPicture(int id, IFormFile formFile);
        Task<ServiceResponse<string>> UpdatePersonPicture(int id, IFormFile formFile);
        Task<ServiceResponse<ReadConnectedPerson>> AddConnectedPerson(int id,WriteConnectedPerson connectedPerson);
        Task<ServiceResponse<bool>> RemoveConnectedPerson(int ConnectedPersonId);
        Task<ServiceResponse<ReadPersonData>> GetPersonById(int PersonId);
        Task<ServiceResponse<IEnumerable<ConectinedPersonsReport>>> GetConnectedPersonsReport(ConnectedPersonType ConnectingType);
        Task<ServiceResponse<IEnumerable<ReadPersonData>>> GetAllPersons(PagingParametrs pagingParameters);
        Task<ServiceResponse<IEnumerable<ReadPersonData>>> Search(DetailedSearchParametrs searchParameters);
        Task<ServiceResponse<bool>> DeletePerson(int PersonId);
    }
}
