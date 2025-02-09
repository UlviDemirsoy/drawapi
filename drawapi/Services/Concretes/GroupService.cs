using AutoMapper;
using drawapi.Data.Dtos;
using drawapi.Data.Models;
using drawapi.Repositories.Abstracts;
using drawapi.Services.Abstracts;

namespace drawapi.Services.Concretes
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _unitOfWork.GroupRepository.GetAllGroupsAsync();
            return _mapper.Map<List<GroupDTO>>(groups); 
        }

        public async Task<GroupDTO> GetGroupByIdAsync(int id)
        {
            var group = await _unitOfWork.GroupRepository.GetGroupByIdAsync(id);
            return _mapper.Map<GroupDTO>(group); 
        }

        public async Task<Group> CreateGroupAsync(Group group)
        {
            await _unitOfWork.GroupRepository.AddGroupAsync(group);
            await _unitOfWork.SaveChangesAsync();
            return group;
        }

        public async Task AddTeamToGroupAsync(GroupTeam groupTeam)
        {
            await _unitOfWork.GroupRepository.AddTeamToGroupAsync(groupTeam);
            await _unitOfWork.SaveChangesAsync(); 
        }

        public async Task<List<Group>> GetAllGroupsByDrawIdAsync(int drawId)
        {
            return await _unitOfWork.GroupRepository.GetAllGroupsByDrawIdAsync(drawId);
        }

       
    }
}
