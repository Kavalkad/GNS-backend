using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Extensions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class CyberClubService : ICyberClubService
    {
        private readonly ICyberClubsRepository _cyberClubsRepository;
        private readonly IOwnersRepository _ownersRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CyberClubService(
            ICyberClubsRepository cyberClubsRepository,
            IOwnersRepository ownersRepository,
            IHttpContextAccessor contextAccessor)
        {
            _cyberClubsRepository = cyberClubsRepository;
            _contextAccessor = contextAccessor;
            _ownersRepository = ownersRepository;
        }

        public async Task Add(AddCyberClubRequest request)
        {
            var ownerId = _contextAccessor.GetHttpUserId();
            await _cyberClubsRepository.Add(ownerId, request.Name, request.City, request.Address);
        }

        public async Task<List<CyberClubDto>> GetAllClubs()
        {
            var cyberClubs = await _cyberClubsRepository.GetAllClubs();
            return cyberClubs
                .Select(cc => new CyberClubDto(cc))
                .ToList();

        }
        public async Task<List<CyberClubDto>> GetByCity(string city)
        {
            var cyberClubs = await _cyberClubsRepository.GetByCity(city);
            return cyberClubs
                .Select(cc => new CyberClubDto(cc))
                .ToList();
        }
        public async Task<List<CyberClubDto>> GetMyCyberClubs()
        {
            var ownerId = _contextAccessor.GetHttpUserId();
            bool isOwner = await _ownersRepository.ContainsOwnerId(ownerId);
            if (!isOwner)
            {
                throw new Exception("User doesn't owe any CyberClub");
            }
            var cyberClubEntities = await _cyberClubsRepository.GetByOwnerId(ownerId);
            return cyberClubEntities
                .Select(cc => new CyberClubDto(cc))
                .ToList();
        }
        public async Task Update(UpdateCyberClubRequest request)
        {
            await _cyberClubsRepository.Update(
                    request.Name,
                    request.NewName,
                    request.NewCity,
                    request.NewAddress
                    );
        }

        public async Task DeleteById(Guid id)
        {
            await _cyberClubsRepository.DeleteById(id);
        }
        public async Task DeleteByName(string name)
        {
            await _cyberClubsRepository.DeleteByName(name);
        }


    }
}