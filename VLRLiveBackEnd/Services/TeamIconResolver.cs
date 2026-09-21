using Microsoft.EntityFrameworkCore;
using VLRLiveBackEnd.Data;

namespace VLRLiveBackEnd.Services
{
    /// <summary>
    /// Looks up the icon we generated (Teams.IconPath) for a team.
    /// </summary>
    public class TeamIconResolver
    {
        private readonly ApplicationDbContext _db;

        public TeamIconResolver(ApplicationDbContext db)
        {
            _db = db;
        }

        // VlrTeamId -> IconPath. Used for live matches (we know the team id there).
        public async Task<Dictionary<string, string>> GetIconsByIdAsync()
        {
            var teams = await _db.Teams
                .Where(t => t.IconPath != null)
                .Select(t => new { t.VlrTeamId, t.IconPath })
                .ToListAsync();

            var map = new Dictionary<string, string>();

            foreach (var team in teams)
                map[team.VlrTeamId] = team.IconPath!;

            return map;
        }

        // Team name -> IconPath. Used for upcoming matches (VLR only gives names there).
        public async Task<Dictionary<string, string>> GetIconsByNameAsync()
        {
            var teams = await _db.Teams
                .Where(t => t.IconPath != null)
                .Select(t => new { t.Name, t.IconPath })
                .ToListAsync();

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var team in teams)
                map.TryAdd(team.Name.Trim(), team.IconPath!);

            return map;
        }
    }
}
