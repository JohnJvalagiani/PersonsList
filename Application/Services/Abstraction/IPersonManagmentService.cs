using Dtos;
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
  public interface IPersonManagmentService
    {

        Task<ReadPersonData> AddPerson(CreatePerson createPerson);
        Task<ReadPersonData> UpdatePerson(UpdatePerson updatePerson);
        Task<string> UploadPersonPicture(int id, IFormFile formFile);
        Task<string> UpdatePersonPicture(int id, IFormFile formFile);
        Task<ReadConnectedPerson> AddConnectinedPerson(int id,WriteConnectedPerson connectedPerson);
        Task<bool> RemoveConnectinedPerson(int ConnectinedPersonId);
        Task<ReadPersonData> GetPersonById(int PersonId);
        Task<IEnumerable<ConectinedPersonsReport>> GetConectinedPersonsReport(ConnectedPersonType ConnectingType);
        Task<IEnumerable<ReadPersonData>> GetAllPersons(PagingParametrs pagingParametrs);

        Task<IEnumerable<ReadPersonData>> Search(DetailedSearchParametrs searchParametrs);
        Task<bool> DeletePerson(int PersonId);

    }
}
