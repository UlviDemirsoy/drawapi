using drawapi.Services.Concretes;
using drawapi.Repositories.Abstracts;
using drawapi.Services.Abstracts;
using drawapi.Data.Models;
using drawapi.Data.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;

public class DrawServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITeamService> _mockTeamService;
    private readonly Mock<ILogger<DrawService>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DrawService _drawService;

    public DrawServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTeamService = new Mock<ITeamService>();
        _mockLogger = new Mock<ILogger<DrawService>>();
        _mockMapper = new Mock<IMapper>();

        _drawService = new DrawService(
            _mockUnitOfWork.Object,
            _mockTeamService.Object,
            _mockMapper.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateDrawAsync_ShouldThrowException_WhenGroupCountIsInvalid()
    {
        // Arrange
        var drawerName = "Ulvi Demirsoy";
        var invalidGroupCount = 5; 

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _drawService.CreateDrawAsync(drawerName, invalidGroupCount));
    }

    [Fact]
    public async Task CreateDrawAsync_ShouldThrowException_WhenTeamsAreNot32()
    {
        // Arrange
        var drawerName = "Ulvi Demirsoy";
        var groupCount = 4;
        var teams = new List<Team> { new Team { Id = 1, Name = "Team A", Country = "USA" } };

        _mockTeamService.Setup(s => s.GetTeamsAsync()).ReturnsAsync(teams);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _drawService.CreateDrawAsync(drawerName, groupCount));
    }

    [Fact]
    public async Task CreateDrawAsync_ShouldCreateDrawSuccessfully_WhenGroupCountIs8()
    {
        // Arrange
        var drawerName = "Ulvi Demirsoy";
        var groupCount = 8;
        var teams = GenerateMockTeams(32);

        _mockTeamService.Setup(s => s.GetTeamsAsync()).ReturnsAsync(teams);
        _mockUnitOfWork.Setup(u => u.DrawRepository.CreateDrawAsync(It.IsAny<Draw>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        _mockUnitOfWork.Setup(u => u.GroupRepository.AddGroupAsync(It.IsAny<Group>()))
                       .Callback<Group>(g => g.Id = new Random().Next(1, 100));

        _mockUnitOfWork.Setup(u => u.DrawRepository.GetDrawByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync(new Draw
                       {
                           Id = 1,
                           DrawerName = drawerName,
                           Groups = new List<Group>
                           {
                           new Group { Id = 1, Name = "Group A", DrawId = 1 },
                           new Group { Id = 2, Name = "Group B", DrawId = 1 },
                           new Group { Id = 3, Name = "Group C", DrawId = 1 },
                           new Group { Id = 4, Name = "Group D", DrawId = 1 },
                           new Group { Id = 4, Name = "Group E", DrawId = 1 },
                           new Group { Id = 4, Name = "Group F", DrawId = 1 },
                           new Group { Id = 4, Name = "Group G", DrawId = 1 },
                           new Group { Id = 4, Name = "Group H", DrawId = 1 }
                           }
                       });

        _mockMapper.Setup(m => m.Map<DrawDTO>(It.IsAny<Draw>()))
                   .Returns((Draw d) => new DrawDTO
                   {
                       Id = d.Id,
                       DrawerName = d.DrawerName,
                       Groups = d.Groups?.Select(g => new GroupDTO
                       {
                           Id = g.Id,
                           Name = g.Name,
                           DrawId = g.DrawId
                       }).ToList() ?? new List<GroupDTO>()
                   });


        // Act
        var result = await _drawService.CreateDrawAsync(drawerName, groupCount);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(drawerName, result.DrawerName);
        Assert.Equal(groupCount, result.Groups.Count);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.AtLeastOnce());
    }



    [Fact]
    public async Task CreateDrawAsync_ShouldCreateDrawSuccessfully_WhenGroupCountIs4()
    {
        var drawerName = "John Doe";
        var groupCount = 4;
        var teams = GenerateMockTeams(32);

        _mockTeamService.Setup(s => s.GetTeamsAsync()).ReturnsAsync(teams);
        _mockUnitOfWork.Setup(u => u.DrawRepository.CreateDrawAsync(It.IsAny<Draw>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        _mockUnitOfWork.Setup(u => u.GroupRepository.AddGroupAsync(It.IsAny<Group>()))
                       .Callback<Group>(g => g.Id = new Random().Next(1, 100)); 

        _mockUnitOfWork.Setup(u => u.DrawRepository.GetDrawByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync(new Draw
                       {
                           Id = 1,
                           DrawerName = drawerName,
                           Groups = new List<Group>
                           {
                           new Group { Id = 1, Name = "Group A", DrawId = 1 },
                           new Group { Id = 2, Name = "Group B", DrawId = 1 },
                           new Group { Id = 3, Name = "Group C", DrawId = 1 },
                           new Group { Id = 4, Name = "Group D", DrawId = 1 }
                           }
                       });

        _mockMapper.Setup(m => m.Map<DrawDTO>(It.IsAny<Draw>()))
                   .Returns((Draw d) => new DrawDTO
                   {
                       Id = d.Id,
                       DrawerName = d.DrawerName,
                       Groups = d.Groups?.Select(g => new GroupDTO
                       {
                           Id = g.Id,
                           Name = g.Name,
                           DrawId = g.DrawId
                       }).ToList() ?? new List<GroupDTO>() 
                   });

        // Act
        var result = await _drawService.CreateDrawAsync(drawerName, groupCount);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(drawerName, result.DrawerName);
        Assert.Equal(groupCount, result.Groups.Count); 
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.AtLeastOnce());
    }


    private List<Team> GenerateMockTeams(int count)
    {
        var teams = new List<Team>();
        for (int i = 1; i <= count; i++)
        {
            teams.Add(new Team { Id = i, Name = $"Team {i}", Country = $"Country {i % 8}" });
        }
        return teams;
    }
}
