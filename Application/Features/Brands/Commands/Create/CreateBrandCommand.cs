using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Commands.Create
{
    public class CreateBrandCommand : IRequest<CreatedBrandResponse>
    {
        public string Name { get; set; }


        public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
        {
            private IBrandRepository _brandRepository;
            private IMapper _mapper;

            public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            public async Task<CreatedBrandResponse>? Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {
                Brand brand = _mapper.Map<Brand>(request);
                brand.Id = Guid.NewGuid();

                await _brandRepository.AddAsync(brand);
                CreatedBrandResponse response = _mapper.Map<CreatedBrandResponse>(brand);

                return response;
            }
        }
    }
}
