using AutoMapper;
using Core.CustomExceptions;
using Core.Models;
using Core.Services.Abstraction;
using Domain.Interfaces;
using Dtos;
using FluentValidation;
using IG.Core.Data.Entities;
using Infrastructure.Data.EFCore.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using service.server.Dtos;
using service.server.HelperClasses;
using service.server.Models;
using service.server.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementation
{
    public class PersonManagementsService : IPersonManagementsService
    {
        private readonly PersonValidation _validator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepo<Person> _personsRepo;
        private readonly IRepo<ConnectedPerson> _connectedPersonRepo;
        private readonly ILogger<PersonManagementsService> _logger;
        private readonly IOptions<FilePathConfig> _filePathConfig;


        public PersonManagementsService(IOptions<FilePathConfig> filePathConfig,ILogger<PersonManagementsService> logger,IUnitOfWork unitOfWork,
            PersonValidation validator,IMapper mapper,IRepo<Person> repo, IRepo<ConnectedPerson> connectedPersonRepo)
        {
            _filePathConfig = filePathConfig ?? throw new ArgumentNullException(nameof(filePathConfig));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _personsRepo = repo ?? throw new ArgumentNullException(nameof(repo));
            _connectedPersonRepo = connectedPersonRepo ?? throw new ArgumentNullException(nameof(connectedPersonRepo));
        }

        public async Task<ServiceResponse<ReadConnectedPerson>> AddConnectedPerson(int id, WriteConnectedPerson connectedPerson)
        {
            var response = new ServiceResponse<ReadConnectedPerson>();
            _unitOfWork.BeginTransaction();

            try
            {
                var theConnectedPerson = _mapper.Map<ConnectedPerson>(connectedPerson);

                var person = await _personsRepo.GetById(id);

                if (person == null)
                {
                    _logger.LogError($"Person not found with id {id}");

                    throw new PersonNotFoundException($"Person not found with id {id}");
                }

                theConnectedPerson.PersonId = id;

                person.ConnectedPerson.Add(theConnectedPerson);

                await _personsRepo.Update(person);

                await _unitOfWork.CommitAsync();

                response.Success = true;
                response.Data = _mapper.Map<ReadConnectedPerson>(theConnectedPerson);
                return response;
            }
            catch (PersonNotFoundException ex)
            {
                _logger.LogError($"Person not found: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"Person not found: {ex.Message}";

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }

        public async Task<ServiceResponse<ReadPersonData>> AddPerson(CreatePerson createPerson)
        {
            var response = new ServiceResponse<ReadPersonData>();
            _unitOfWork.BeginTransaction();
            try
            {
                var thePerson = _mapper.Map<Person>(createPerson);

                var validateResult = await _validator.ValidateAsync(thePerson);

                await _validator.ValidateAndThrowAsync(thePerson);

                if (!validateResult.IsValid)
                {
                    response.Success = false;
                    response.ErrorMessage = string.Join(", ", validateResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                if (!AgeValidation(createPerson.BirthDate))
                {
                    response.Success = false;
                    response.ErrorMessage = "Invalid Age";
                    return response;
                }

                var result = await _personsRepo.Add(thePerson);
                await _unitOfWork.CommitAsync();

                response.Success = true;
                response.Data = _mapper.Map<ReadPersonData>(result);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }



        public async Task<ServiceResponse<bool>> DeletePerson(int personId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var person = await _personsRepo.GetById(personId);

                if (person == null)
                {
                    _logger.LogError($"Person not found with id {personId}");

                    response.Success = false;
                    response.ErrorMessage = $"Person not found with id {personId}";

                    return response;
                }

                await _personsRepo.Remove(personId);
                await _unitOfWork.CommitAsync();

                response.Success = true;
                response.Data = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ReadPersonData>>> GetAllPersons(PagingParametrs pagingParametrs)
        {
            var response = new ServiceResponse<IEnumerable<ReadPersonData>>();

            try
            {
                var persons = await _personsRepo.GetAll();

                var thePersons = persons.Select(p => _mapper.Map<ReadPersonData>(p));

                var result = PagedList<ReadPersonData>
                    .ToPagedList(thePersons.AsQueryable(), pagingParametrs.PageNumber, pagingParametrs.PageSize);

                response.Success = true;
                response.Data = result;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ConectinedPersonsReport>>> GetConnectedPersonsReport(ConnectedPersonType connectingType)
        {
            var response = new ServiceResponse<IEnumerable<ConectinedPersonsReport>>();

            try
            {
                var persons = await _personsRepo.GetAll();
                var report = new List<ConectinedPersonsReport>();

                foreach (var item in persons)
                {
                    var predicate = GenericExpressionTree.CreateWhereClause<ConnectedPerson>("PersonId", item.Id);

                    var connectedPersons = await _connectedPersonRepo.GetByQueryAsync(predicate);

                    var thePersons = connectedPersons.Where(s => s.PersonType.Equals(connectingType)).Select(s => s).AsEnumerable();

                    report.Add(new ConectinedPersonsReport
                    {
                        Firstname = item.FirstNameENG,
                        Laststname = item.LastNameENG,
                        ConnectinedPersonCount = thePersons.Count()
                    });
                }

                response.Success = true;
                response.Data = report;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }

        public async Task<ServiceResponse<ReadPersonData>> GetPersonById(int personId)
        {
            var response = new ServiceResponse<ReadPersonData>();

            try
            {
                var result = await _personsRepo.GetById(personId);

                if (result == null)
                {
                    _logger.LogError($"Person not found with id {personId}");

                    response.Success = false;
                    response.ErrorMessage = $"Person not found with id {personId}";

                    return response;
                }

                var thePerson = _mapper.Map<ReadPersonData>(result);

                var predicate = GenericExpressionTree.CreateWhereClause<ConnectedPerson>("PersonId", personId);

                var connectedPersons = await _connectedPersonRepo.GetByQueryAsync(predicate);

                thePerson.connectedPersons = connectedPersons?.ToList() ?? new List<ConnectedPerson>();

                response.Success = true;
                response.Data = thePerson;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }

        public async Task<ServiceResponse<bool>> RemoveConnectedPerson(int connectedPersonId)
        {
            var response = new ServiceResponse<bool>();
            _unitOfWork.BeginTransaction();
            try
            {
                var thePerson = await _connectedPersonRepo.GetById(connectedPersonId);

                if (thePerson == null)
                {
                    _logger.LogError($"Person not found with id {connectedPersonId}");

                    response.Success = false;
                    response.ErrorMessage = $"Person not found with id {connectedPersonId}";

                    return response;
                }

                await _connectedPersonRepo.Remove(connectedPersonId);
                await _unitOfWork.CommitAsync();

                response.Success = true;
                response.Data = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ReadPersonData>>> Search(DetailedSearchParametrs detailSearchParametrs)
        {
            var response = new ServiceResponse<IEnumerable<ReadPersonData>>();

            try
            {
                var orderByFunctions = new Dictionary<person, Func<IQueryable<Person>, IOrderedQueryable<Person>>>
        {
            { person.FirstNameENG, q => q.OrderBy(p => p.FirstNameENG) },
            { person.LastNameENG, q => q.OrderBy(p => p.LastNameENG) },
            { person.FirstNameGEO, q => q.OrderBy(p => p.FirstNameGEO) },
            { person.LastNameGEO, q => q.OrderBy(p => p.LastNameGEO) },
            { person.PhoneNumber, q => q.OrderBy(p => p.PhoneNumber) },
            { person.PersonalNumber, q => q.OrderBy(p => p.PersonalNumber) },
        };

                // Determine the orderByFunc using the dictionary or fallback to a default order
                var orderByFunc = orderByFunctions.GetValueOrDefault(detailSearchParametrs.OrderPersonsby, q => q.OrderBy(p => p.FirstNameENG));

                var predicate = GenericExpressionTree.CreateWhereClause<Person>(detailSearchParametrs.SearchPersonsBy.ToString(), detailSearchParametrs.SearchValue);

                var persons = await _personsRepo.GetByQueryAsync(predicate, orderByFunc);

                var thePersons = persons.Select(p => _mapper.Map<ReadPersonData>(p));

                var result = PagedList<ReadPersonData>
                    .ToPagedList(thePersons.AsQueryable(), detailSearchParametrs.pagingParametrs.PageNumber, detailSearchParametrs.pagingParametrs.PageSize);

                response.Success = true;
                response.Data = result;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }

        public async Task<ServiceResponse<ReadPersonData>> UpdatePerson(UpdatePerson person)
        {
            var response = new ServiceResponse<ReadPersonData>();
            _unitOfWork.BeginTransaction();

            try
            {
                var thePerson = _mapper.Map<Person>(person);

                var validateResult = await _validator.ValidateAsync(thePerson);

                await _validator.ValidateAndThrowAsync(thePerson);

                if (!validateResult.IsValid)
                {
                    response.Success = false;
                    response.ErrorMessage = string.Join(", ", validateResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                thePerson = await _personsRepo.Update(thePerson);
                await _unitOfWork.CommitAsync();

                response.Success = true;
                response.Data = _mapper.Map<ReadPersonData>(thePerson);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }


        public async Task<ServiceResponse<string>> UpdatePersonPicture(int id, IFormFile formFile)
        {
            var response = new ServiceResponse<string>();
            _unitOfWork.BeginTransaction();

            try
            {
                var thePerson = await _personsRepo.GetById(id);

                if (thePerson == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "Person not found with the specified id";
                    return response;
                }

                if (formFile.Length <= 0)
                {
                    response.Success = false;
                    response.ErrorMessage = "Picture file is empty or null";
                    return response;
                }

                if (File.Exists(thePerson.Picture))
                {
                    File.Delete(thePerson.Picture);
                }

                var basePath = _filePathConfig.Value.BasePath;

                string filePath = Path.Combine(basePath, formFile.FileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                thePerson.Picture = filePath;

                await _personsRepo.Update(thePerson);
                await _unitOfWork.CommitAsync();

                response.Success = true;
                response.Data = filePath;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }


        public async Task<ServiceResponse<string>> UploadPersonPicture(int id, IFormFile formFile)
        {
            var response = new ServiceResponse<string>();
            _unitOfWork.BeginTransaction();

            try
            {
                var thePerson = await _personsRepo.GetById(id);

                if (thePerson == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "Person not found with the specified id";
                    return response;
                }

                if (formFile.Length <= 0)
                {
                    response.Success = false;
                    response.ErrorMessage = "Picture file is empty or null";
                    return response;
                }

                var basePath = _filePathConfig.Value.BasePath;
                string filePath = Path.Combine(basePath, formFile.FileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                thePerson.Picture = filePath;

                await _personsRepo.Update(thePerson);
                await _unitOfWork.CommitAsync();
                
                response.Success = true;
                response.Data = filePath;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                _unitOfWork.Rollback();

                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";

                return response;
            }
        }


        private static bool AgeValidation(DateTime birthDate)
        {
            var dateTimeNow = DateTime.Now;

            var age= dateTimeNow.AddYears(-birthDate.Year);

            return age.Year > 18;
        }
    }
}
