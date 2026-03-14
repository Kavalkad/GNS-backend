using GNS.Contracts.Requests;
using GNS.Data.Entities;
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
        private readonly UnitOfWork _unitOfWork;

        public CyberClubService(
            ICyberClubsRepository cyberClubsRepository,
            IOwnersRepository ownersRepository,
            IHttpContextAccessor contextAccessor,
            UnitOfWork unitOfWork)
        {
            _cyberClubsRepository = cyberClubsRepository;
            _contextAccessor = contextAccessor;
            _ownersRepository = ownersRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(AddCyberClubRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var ownerId = _contextAccessor.GetHttpUserId();
                var cyberClubEntity = new CyberClubEntity
                {
                    Name = request.Name,
                    City = request.City,
                    Address = request.Address,
                    OwnerId = ownerId
                };
                await _cyberClubsRepository.Add(cyberClubEntity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
            }

        }
        public async Task<bool> VerifyOwner(Guid ownerId, string cyberClubName)
        {
            var cyberClubs = await _cyberClubsRepository.GetByOwnerId(ownerId);

            return cyberClubs.Any(cc => cc.Name == cyberClubName);
        }

        public async Task<List<CyberClubDto>> GetAllClubs()
        {
            var cyberClubs = await _cyberClubsRepository.GetAllClubs();
            return cyberClubs
                .Select(cc => new CyberClubDto(cc))
                .ToList();

        }
        public async Task<CyberClubDto> GetById(Guid cyberClubId)
        {
            var cyberClub = await _cyberClubsRepository.GetById(cyberClubId) ?? throw new Exception("CyberClub not found");
            return new CyberClubDto(cyberClub);
        }
        public async Task<CyberClubEntity> GetByCCName(string cyberClubName)
        {

            return await _cyberClubsRepository.GetByCCName(cyberClubName);
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