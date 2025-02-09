using AutoMapper;
using drawapi.Data.Dtos;
using drawapi.Data.Models;
using drawapi.Repositories.Abstracts;
using drawapi.Services.Concretes;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class GroupServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GroupService _groupService;

    public GroupServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _groupService = new GroupService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllGroupsAsync_ShouldReturnGroupDTOList()
    {
        // Arrange
        var groups = new List<Group>
        {
            new Group { Id = 1, Name = "Group A", DrawId = 1 },
            new Group { Id = 2, Name = "Group B", DrawId = 1 }
        };

        var expectedGroupDTOs = groups.Select(g => new GroupDTO { Id = g.Id, Name = g.Name, DrawId = g.DrawId }).ToList();

        _mockUnitOfWork.Setup(u => u.GroupRepository.GetAllGroupsAsync()).ReturnsAsync(groups);
        _mockMapper.Setup(m => m.Map<List<GroupDTO>>(It.IsAny<List<Group>>())).Returns(expectedGroupDTOs);

        // Act
        var result = await _groupService.GetAllGroupsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedGroupDTOs.Count, result.Count);
        Assert.Equal(expectedGroupDTOs.First().Name, result.First().Name);
    }

    [Fact]
    public async Task GetGroupByIdAsync_ShouldReturnGroupDTO()
    {
        // Arrange
        var group = new Group { Id = 1, Name = "Group A", DrawId = 1 };
        var expectedGroupDTO = new GroupDTO { Id = group.Id, Name = group.Name, DrawId = group.DrawId };

        _mockUnitOfWork.Setup(u => u.GroupRepository.GetGroupByIdAsync(group.Id)).ReturnsAsync(group);
        _mockMapper.Setup(m => m.Map<GroupDTO>(group)).Returns(expectedGroupDTO);

        // Act
        var result = await _groupService.GetGroupByIdAsync(group.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedGroupDTO.Id, result.Id);
        Assert.Equal(expectedGroupDTO.Name, result.Name);
        Assert.Equal(expectedGroupDTO.DrawId, result.DrawId);
    }

    [Fact]
    public async Task CreateGroupAsync_ShouldReturnCreatedGroup()
    {
        // Arrange
        var group = new Group { Name = "Group X", DrawId = 1 };

        _mockUnitOfWork.Setup(u => u.GroupRepository.AddGroupAsync(It.IsAny<Group>()))
                       .Callback<Group>(g => g.Id = new Random().Next(1, 100)); 
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _groupService.CreateGroupAsync(group);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(0, result.Id); 
        Assert.Equal(group.Name, result.Name);
    }

    [Fact]
    public async Task AddTeamToGroupAsync_ShouldSaveGroupTeam()
    {
        // Arrange
        var groupTeam = new GroupTeam { GroupId = 1, TeamId = 5 };

        _mockUnitOfWork.Setup(u => u.GroupRepository.AddTeamToGroupAsync(groupTeam)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await _groupService.AddTeamToGroupAsync(groupTeam);

        // Assert
        _mockUnitOfWork.Verify(u => u.GroupRepository.AddTeamToGroupAsync(groupTeam), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllGroupsByDrawIdAsync_ShouldReturnGroups()
    {
        // Arrange
        var drawId = 1;
        var groups = new List<Group>
        {
            new Group { Id = 1, Name = "Group A", DrawId = drawId },
            new Group { Id = 2, Name = "Group B", DrawId = drawId }
        };

        _mockUnitOfWork.Setup(u => u.GroupRepository.GetAllGroupsByDrawIdAsync(drawId)).ReturnsAsync(groups);

        // Act
        var result = await _groupService.GetAllGroupsByDrawIdAsync(drawId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(groups.Count, result.Count);
        Assert.All(result, g => Assert.Equal(drawId, g.DrawId));
    }
}
