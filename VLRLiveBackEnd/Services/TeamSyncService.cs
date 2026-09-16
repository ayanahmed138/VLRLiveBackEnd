using Microsoft.EntityFrameworkCore;
using VLRLiveBackEnd.Data;
using VLRLiveBackEnd.Models;

namespace VLRLiveBackEnd.Services
{
    public class TeamSyncService
    {
        private readonly VLRapiService _vlrApiService;
        private readonly ApplicationDbContext _db;
        private readonly TeamLogoService _teamLogoService;

        public TeamSyncService(
            VLRapiService vlrApiService,
            ApplicationDbContext db,
            TeamLogoService teamLogoService)
        {
            _vlrApiService = vlrApiService;
            _db = db;
            _teamLogoService = teamLogoService;
        }

        public async Task SyncTeamAsync(string teamId)
        {
            var vlrTeam = await _vlrApiService.GetTeamAsync(teamId);

            var existingTeam = await _db.Teams
                .FirstOrDefaultAsync(t => t.VlrTeamId == vlrTeam.Id);

            if (existingTeam == null)
            {
                var team = new Team
                {
                    VlrTeamId = vlrTeam.Id,
                    Name = vlrTeam.Name,
                    Tag = vlrTeam.Tag,
                    Country = vlrTeam.Country,
                    Region = vlrTeam.Region,
                    LogoUrl = vlrTeam.Logo,
                    LastUpdated = DateTime.UtcNow
                };

                _db.Teams.Add(team);
            }
            else
            {
                existingTeam.Name = vlrTeam.Name;
                existingTeam.Tag = vlrTeam.Tag;
                existingTeam.Country = vlrTeam.Country;
                existingTeam.Region = vlrTeam.Region;
                existingTeam.LogoUrl = vlrTeam.Logo;
                existingTeam.LastUpdated = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            await _teamLogoService.GenerateTeamIconAsync(teamId);
        }

        public async Task SyncTeamsFromEventAsync(string eventId)
        {
            var teams = await _vlrApiService.GetTeamsFromEventAsync(eventId);

            foreach (var vlrTeam in teams)
            {
                var existingTeam = await _db.Teams
                    .FirstOrDefaultAsync(t => t.VlrTeamId == vlrTeam.Id);

                var fullTeam = await _vlrApiService.GetTeamAsync(vlrTeam.Id);

                if (existingTeam == null)
                {
                    var team = new Team
                    {
                        VlrTeamId = fullTeam.Id,
                        Name = fullTeam.Name,
                        Tag = fullTeam.Tag,
                        Country = fullTeam.Country,
                        Region = fullTeam.Region,
                        LogoUrl = fullTeam.Logo,
                        LastUpdated = DateTime.UtcNow
                    };

                    _db.Teams.Add(team);
                    
                }
                else
                {
                    existingTeam.Name = fullTeam.Name;
                    existingTeam.Tag = fullTeam.Tag;
                    existingTeam.Country = fullTeam.Country;
                    existingTeam.Region = fullTeam.Region;
                    existingTeam.LogoUrl = fullTeam.Logo;
                    existingTeam.LastUpdated = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            // Now generate the icons
            foreach (var vlrTeam in teams)
            {
                await _teamLogoService.GenerateTeamIconAsync(vlrTeam.Id);
            }


        }
    }
}