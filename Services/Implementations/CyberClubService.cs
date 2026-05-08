using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Exceptions;
using GNS.Extensions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class CyberClubService(
        ICyberClubsRepository cyberClubsRepository,
        IHttpContextAccessor contextAccessor,
        IMapper mapper,
        IUnitOfWork unitOfWork) : ICyberClubService
    {
        private readonly ICyberClubsRepository _cyberClubsRepository = cyberClubsRepository;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddAsync(CreateCyberClubRequest request, CancellationToken token = default)
        {
            var ownerId = _contextAccessor.TryGetHttpUserId();

            var cyberClubEntity = new CyberClubEntity
            {
                Name = request.Name,
                City = request.City,
                Address = request.Address,
                OwnerId = ownerId
            };

            await _cyberClubsRepository.AddAsync(cyberClubEntity, token);
            await _unitOfWork.SaveChangesAsync(token);

        }

        public async Task<List<CyberClubDto>> GetAllClubsAsync(CancellationToken token = default)
        {
            var cyberClubs = await _cyberClubsRepository.GetAllAsync(token);

            return _mapper.MapToCyberClubDto(cyberClubs);
        }
        public async Task<CyberClubEntity> GetClubByIdAsync(Guid cyberClubId, CancellationToken token = default)
        {
            return await _cyberClubsRepository.GetByIdAsync(cyberClubId, token)
                ?? throw new EntityNotFoundException("CyberClub", cyberClubId.ToString());
        }
        public async Task<CyberClubEntity> FindByCyberClubNameAsync(string cyberClubName, CancellationToken token = default)
        {
            return await _cyberClubsRepository.FindAsync(cc => cc.Name == cyberClubName, token)
                ?? throw new EntityNotFoundException("CyberClub", $"name was {cyberClubName}");
        }
        public async Task<List<CyberClubDto>> GetByCityAsync(string city, CancellationToken token = default)
        {
            var cyberClubs = await _cyberClubsRepository.GetByExpressionAsync(cc => cc.City == city, token)
                ?? throw new EntityNotFoundException("CyberClub", city);

            return _mapper.MapToCyberClubDto(cyberClubs);
        }
        public async Task<List<CyberClubDto>> GetOwnerCyberClubsAsync(Guid ownerId, CancellationToken token = default)
        {

            var cyberClubs = await _cyberClubsRepository.GetByExpressionAsync(cc => cc.OwnerId == ownerId, token);

            return _mapper.MapToCyberClubDto(cyberClubs);
        }

        public async Task UpdateCyberClubNameAsync(UpdateCyberClubNameRequest request, CancellationToken token = default)
        {
            var cyberClubId = request.CyberClubId;

            var cyberClub = await _cyberClubsRepository.GetByIdAsync(cyberClubId, token)
                ?? throw new EntityNotFoundException("CyberClub", request.CyberClubId.ToString());

            cyberClub.Name = request.Name;

            _cyberClubsRepository.Update(cyberClub);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task UpdateCyberClubCityAsync(UpdateCyberClubCityRequest request, CancellationToken token = default)
        {
            var cyberClubId = request.CyberClubId;

            var cyberClub = await _cyberClubsRepository.GetByIdAsync(cyberClubId, token)
                ?? throw new EntityNotFoundException("CyberClub", request.CyberClubId.ToString());

            cyberClub.City = request.City;

            _cyberClubsRepository.Update(cyberClub);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task UpdateCyberClubAddressAsync(UpdateCyberClubAddressRequest request, CancellationToken token = default)
        {
            var cyberClubId = request.CyberClubId;

            var cyberClub = await _cyberClubsRepository.GetByIdAsync(cyberClubId, token)
                ?? throw new EntityNotFoundException("CyberClub", request.CyberClubId.ToString());

            cyberClub.Address = request.Address;

            _cyberClubsRepository.Update(cyberClub);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task DeleteClubByIdAsync(Guid cyberClubId, CancellationToken token = default)
        {
            var cyberClub = await _cyberClubsRepository.GetByIdAsync(cyberClubId, token)
                ?? throw new EntityNotFoundException("CyberClub", cyberClubId.ToString());
            _cyberClubsRepository.Delete(cyberClub);
            await _unitOfWork.SaveChangesAsync(token);
        }


    }
}