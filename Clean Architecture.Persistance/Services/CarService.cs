using AutoMapper;
using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Features.CarFeatures.Queries.GetAllCars;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Persistance.Context;
using EntityFrameworkCorePagination.Nuget.Pagination;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistance.Services
{
    public sealed class CarService : ICarService
    {
        private readonly AppDbContext _context;
        private readonly ICarRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CarService(AppDbContext context, IMapper mapper, IUnitOfWork unitOfWork, ICarRepository repository)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CreateCarCommand request, CancellationToken cancellationToken)
        {
            Car car = _mapper.Map<Car>(request);

            //Unit Of Work - Repository Pattern
            await _repository.AddAsync(car, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);


            //await _context.Set<Car>().AddAsync(car, cancellationToken);
            //await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginationResult<Car>> GetAllAsync(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            PaginationResult<Car> cars = await _repository
                .GetWhere(x => x.Name.ToLower()
                .Contains(request.Search.ToLower()))
                .OrderBy(x => x.Name)
                .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
            return cars;
        }
    }
}
