using AutoMapper;
using Core.CustomExceptions;
using Core.Services.Abstraction;
using Dtos;
using FluentValidation;
using IG.Core.Data.Entities;
using Microsoft.AspNetCore.Http;
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
    public class PersonManagmentService : IPersonManagmentService
    {
        private readonly PersonValidation _validator;
        private readonly IMapper _mapper;
        private readonly IRepo<Person> _personsRepo;
        private readonly IRepo<ConnectedPerson> _connectedPersonrepo;

        public PersonManagmentService(PersonValidation validator,IMapper mapper,IRepo<Person> repo, IRepo<ConnectedPerson> connectedPersonrepo)
        {
            this._personsRepo = repo;

            this._mapper = mapper;

            this._connectedPersonrepo = connectedPersonrepo;

            this._validator = validator;
        }

        public async Task<ReadConnectedPerson> AddConnectinedPerson(int id,WriteConnectedPerson connectedPerson)
        {
            var theconnectedPerson = _mapper.Map<ConnectedPerson>(connectedPerson);

            var person = await _personsRepo.GetById(id);

            if(person==null)
                throw new PersonNotFoundException($"Person not found with id {id}");

            theconnectedPerson.PersonId = id;

            person.ConnectedPerson.Add(theconnectedPerson);


            await _personsRepo.Update(person);

            return await  Task.FromResult(_mapper.Map<ReadConnectedPerson>(theconnectedPerson));

        }

      

        public async Task<ReadPersonData> AddPerson(CreatePerson createPerson)
        {


            var theperson = _mapper.Map<Person>(createPerson);

            var validateresult = await _validator.ValidateAsync(theperson);


            await _validator.ValidateAndThrowAsync(theperson);

            if (!validateresult.IsValid)
            {
                 validateresult.Errors.AsEnumerable();
            }

            if (!AgeValidation(createPerson.BirthDate))
                throw new Exception("Invalid Age");


            var result =  await  _personsRepo.Add(theperson);

           return  _mapper.Map<ReadPersonData>(result);


        }


        public async Task<bool> DeletePerson(int PersonId)
        {
            var person = await _personsRepo.GetById(PersonId);

            if (person == null)
                throw new PersonNotFoundException($"Person not found with id {PersonId}");


            var result =  await _personsRepo.Remove(PersonId);

            return result;
        }

        public async Task<IEnumerable<ReadPersonData>> GetAllPersons(PagingParametrs pagingParametrs)
        {
            var persons = await _personsRepo.GetAll();

            var thepersons = persons.Select(p => _mapper.Map<ReadPersonData>(p));

            var result = PagedList<ReadPersonData>
              .ToPagedList(thepersons.AsQueryable(), pagingParametrs.PageNumber, pagingParametrs.PageSize);

            return result;
        }

        public async Task<IEnumerable<ConectinedPersonsReport>> GetConectinedPersonsReport(ConnectedPersonType ConnectingType)
        {

            var persons = await _personsRepo.GetAll();

            var report = new List<ConectinedPersonsReport>();

            foreach (var item in persons)
            {
            var predicate = GenericExpressionTree.CreateWhereClause<ConnectedPerson>("PersonId", item.Id);

            var connectedpersons = await _connectedPersonrepo.GetByQueryAsync(predicate);

               var thePersons= connectedpersons.Where(s => s.PersonType.Equals(ConnectingType)).Select(s=>s).AsEnumerable();

                report.Add(new ConectinedPersonsReport {Firstname= item.FirstNameENG, Laststname=item.LastNameENG, ConnectinedPersonCount=thePersons.Count() });
            }


            return report;

        }

        public async Task<ReadPersonData> GetPersonById(int PersonId)
        {
            var result= await _personsRepo.GetById(PersonId);

            if (result == null)
                throw new PersonNotFoundException($"Person not found with id {PersonId}");
            try
            {

            var theperson = _mapper.Map<ReadPersonData>(result);
           

            var predicate = GenericExpressionTree.CreateWhereClause<ConnectedPerson>("PersonId", PersonId);


            var connectedpersons =  _connectedPersonrepo.GetByQueryAsync(predicate).Result.ToList();

            
            theperson.connectinedPersons = connectedpersons??new List<ConnectedPerson>();


            return theperson;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> RemoveConnectinedPerson(int ConnectinedPersonId)
        {
            var theperson = await _connectedPersonrepo.GetById(ConnectinedPersonId);

            if (theperson == null)
                throw new PersonNotFoundException($"Person not found with id {ConnectinedPersonId}");

          return  await _connectedPersonrepo.Remove(ConnectinedPersonId);


        }

        public async Task<IEnumerable<ReadPersonData>> Search(DetailedSearchParametrs detailSearchParametrs)
        {
            Func<IQueryable<Person>, IOrderedQueryable<Person>> orderByFunc = null;

            switch (detailSearchParametrs.OrderPersonsby)
            {
                case person.FirstNameENG:
                    orderByFunc = q => q.OrderBy(p => p.FirstNameENG);
                    break;
                case person.LastNameENG:
                    orderByFunc = q => q.OrderBy(p => p.LastNameENG);

                    break;
                case person.FirstNameGEO:
                    orderByFunc = q => q.OrderBy(p => p.FirstNameGEO);

                    break;
                case person.LastNameGEO:
                    orderByFunc = q => q.OrderBy(p => p.LastNameGEO);

                    break;
                case person.PhoneNumber:
                    orderByFunc = q => q.OrderBy(p => p.PhoneNumber);

                    break;
                case person.PersonalNumber:
                    orderByFunc = q => q.OrderBy(p => p.PersonalNumber);

                    break;
                

                default:
                    orderByFunc = q => q.OrderBy(p => p.FirstNameENG);
                    break;


            }

            var predicate = GenericExpressionTree.CreateWhereClause<Person>(detailSearchParametrs.SearchPersonsBy.ToString(), detailSearchParametrs.SearchValue);


            var persons = await _personsRepo.GetByQueryAsync(predicate, orderByFunc);


            var thepersons = persons.Select(p => _mapper.Map<ReadPersonData>(p));

            var result = PagedList<ReadPersonData>
                .ToPagedList(thepersons.AsQueryable(), detailSearchParametrs.pagingParametrs.PageNumber, detailSearchParametrs.pagingParametrs.PageSize);

            return result;

        }

        public async Task<ReadPersonData> UpdatePerson(UpdatePerson person)
        {
            
          
            var theperson = _mapper.Map<Person>(person);

            var validateresult = await _validator.ValidateAsync(theperson);


            await _validator.ValidateAndThrowAsync(theperson);

            if (!validateresult.IsValid)
            {
                validateresult.Errors.AsEnumerable();
            }

            theperson = await _personsRepo.Update(theperson);

            return _mapper.Map<ReadPersonData>(theperson);

        }

        public async Task<string> UpdatePersonPicture(int id, IFormFile formFile)
        {
            
            
                var theperson = await _personsRepo.GetById(id);

                if (theperson == null)
                    throw new ArgumentNullException("Id was null");



                if (formFile.Length <= 0)
                    throw new ArgumentNullException("Picture was null");


                if (File.Exists(theperson.Picture))
                {
                    File.Delete(theperson.Picture);
                }


            string filePath = Path.Combine(@"C:\Users\John\Desktop", formFile.FileName);
            try
            {

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            theperson.Picture = filePath;


            await _personsRepo.Update(theperson);


            return filePath;
        }

        public async Task<string> UploadPersonPicture(int id, IFormFile formFile)
        {


         var theperson =  await _personsRepo.GetById(id);

            if (theperson == null)
                throw new ArgumentNullException("Id was null");



            if (formFile.Length<=0)
                throw new ArgumentNullException("Picture was null");



            string filePath = Path.Combine(@"C:\Users\John\Desktop", formFile.FileName);
            try
            {

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            }
            catch (Exception ex)
            {

                throw;
            }

             
                theperson.Picture = filePath;

            await _personsRepo.Update(theperson);


            return filePath;
        }


        private bool AgeValidation(DateTime birthDate)
        {
            var datetimenow = DateTime.Now;

            var age= datetimenow.AddYears(-birthDate.Year);

            return age.Year > 18;

        }


    }
}
