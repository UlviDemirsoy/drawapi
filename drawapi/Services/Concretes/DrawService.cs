using drawapi.Data.Models;
using drawapi.Services.Abstracts;
using drawapi.Repositories.Abstracts;
using drawapi.Data.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drawapi.Services.Concretes
{
    public class DrawService : IDrawService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;
        private readonly ILogger<DrawService> _logger;

        public DrawService(IUnitOfWork unitOfWork, ITeamService teamService, IMapper mapper, ILogger<DrawService> logger)
        {
            _unitOfWork = unitOfWork;
            _teamService = teamService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<DrawDTO>> ListAllDrawsAsync()
        {
            var draws = await _unitOfWork.DrawRepository.ListAllDrawsAsync();
            return _mapper.Map<List<DrawDTO>>(draws);
        }

        public async Task<DrawDTO> GetDrawByIdAsync(int id)
        {
            var draw = await _unitOfWork.DrawRepository.GetDrawByIdAsync(id);
            return _mapper.Map<DrawDTO>(draw);
        }

        public async Task<DrawDTO> CreateDrawAsync(string drawerName, int groupCount)
        {
            _logger.LogInformation("Starting draw creation for {DrawerName} with {GroupCount} groups", drawerName, groupCount);

            if (groupCount != 4 && groupCount != 8)
            {
                _logger.LogWarning("Invalid group count: {GroupCount}", groupCount);
                throw new ArgumentException("Group count must be either 4 or 8.");
            }

            var teams = (await _teamService.GetTeamsAsync()).ToList();
            if (teams.Count != 32)
            {
                _logger.LogError("Invalid team count: Expected 32, found {TeamCount}", teams.Count);
                throw new InvalidOperationException("There must be exactly 32 teams to conduct the draw.");
            }

            var random = new Random();

            await _unitOfWork.BeginTransactionAsync(); 

            var draw = new Draw
            {
                DrawerName = drawerName,
                DrawDate = DateTime.UtcNow,
                Groups = new List<Group>()
            };

            await _unitOfWork.DrawRepository.CreateDrawAsync(draw);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created draw with ID {DrawId}", draw.Id);

            var groupNames = groupCount == 8
                ? new[] { "A", "B", "C", "D", "E", "F", "G", "H" }
                : new[] { "A", "B", "C", "D" };

            var groups = new List<Group>();

            foreach (var name in groupNames)
            {
                var group = new Group { Name = $"Group {name}", DrawId = draw.Id };
                await _unitOfWork.GroupRepository.AddGroupAsync(group);
                groups.Add(group);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created {GroupCount} groups for Draw ID {DrawId}", groupCount, draw.Id);

            var countryGroups = teams.GroupBy(t => t.Country)
                                     .ToDictionary(g => g.Key, g => new Queue<Team>(g));

            var assignedTeams = new Dictionary<int, List<Team>>();
            for (int i = 0; i < groupCount; i++)
                assignedTeams[i] = new List<Team>();

            bool teamsRemaining = true;

            if (groupCount == 8)
            {
                _logger.LogInformation("Using group distribution for 8 groups: 4 different countries per group");
               
                var assignedCountries = new Dictionary<int, HashSet<string>>();
                for (int i = 0; i < groupCount; i++)
                    assignedCountries[i] = new HashSet<string>(); 

                while (teamsRemaining)
                {
                    teamsRemaining = false;

                    for (int i = 0; i < groupCount; i++)
                    {
                        var group = groups[i];

                        var availableTeams = teams
                            .Where(t => !assignedCountries[i].Contains(t.Country))
                            .OrderBy(_ => random.Next()) 
                            .ToList();

                        if (availableTeams.Count > 0 && assignedTeams[i].Count < 4)
                        {
                            var selectedTeam = availableTeams.First();
                            assignedTeams[i].Add(selectedTeam);
                            assignedCountries[i].Add(selectedTeam.Country);
                            teams.Remove(selectedTeam); 
                            teamsRemaining = true;
                        }
                    }
                }
            }
            else if (groupCount == 4)
            {
                _logger.LogInformation("Using group distribution for 4 groups: 8 different countries per group");

                var assignedCountries = new Dictionary<int, HashSet<string>>();
                for (int i = 0; i < groupCount; i++)
                    assignedCountries[i] = new HashSet<string>(); 

                while (teamsRemaining)
                {
                    teamsRemaining = false;

                    for (int i = 0; i < groupCount; i++)
                    {
                        var group = groups[i];

                        
                        var availableTeams = teams
                            .Where(t => !assignedCountries[i].Contains(t.Country)) 
                            .OrderBy(_ => random.Next()) 
                            .ToList();

                        if (availableTeams.Count > 0 && assignedTeams[i].Count < 8)
                        {
                            var selectedTeam = availableTeams.First(); 
                            assignedTeams[i].Add(selectedTeam);
                            assignedCountries[i].Add(selectedTeam.Country);
                            teams.Remove(selectedTeam); 
                            teamsRemaining = true;
                        }
                    }
                }
            }

            _logger.LogInformation("Teams distributed into groups");

            for (int i = 0; i < groupCount; i++)
            {
                foreach (var team in assignedTeams[i])
                {
                    var groupId = groups[i].Id;
                    var groupTeam = new GroupTeam { GroupId = groupId, TeamId = team.Id };

                    await _unitOfWork.GroupRepository.AddTeamToGroupAsync(groupTeam);
                }
            }

            await _unitOfWork.SaveChangesAsync(); 

            draw = await _unitOfWork.DrawRepository.GetDrawByIdAsync(draw.Id);
            await _unitOfWork.CommitTransactionAsync(); 

            _logger.LogInformation("Draw successfully completed for {DrawerName} with {GroupCount} groups", drawerName, groupCount);

            return _mapper.Map<DrawDTO>(draw);
        }



    }
}
