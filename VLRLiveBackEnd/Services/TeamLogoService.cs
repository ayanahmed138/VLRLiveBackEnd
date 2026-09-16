using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using VLRLiveBackEnd.Data;

namespace VLRLiveBackEnd.Services
{
    public class TeamLogoService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly HttpClient _httpClient;

        public TeamLogoService(
            ApplicationDbContext db,
            IWebHostEnvironment environment,
            HttpClient httpClient)
        {
            _db = db;
            _environment = environment;
            _httpClient = httpClient;
        }

        public async Task GenerateTeamIconAsync(string teamId)
        {
            var team = await _db.Teams
                .FirstOrDefaultAsync(t => t.VlrTeamId == teamId);

            if (team == null)
                throw new Exception($"Team {teamId} not found.");

            if (string.IsNullOrWhiteSpace(team.LogoUrl))
                throw new Exception($"Team {teamId} has no logo URL.");

            // wwwroot/team-icons
            var iconsFolder = Path.Combine(
                _environment.WebRootPath,
                "team-icons");

            Directory.CreateDirectory(iconsFolder);

            var fileName = $"{team.VlrTeamId}.webp";
            var filePath = Path.Combine(iconsFolder, fileName);

            // Download original logo
            var imageBytes = await _httpClient.GetByteArrayAsync(team.LogoUrl);

            using var inputStream = new MemoryStream(imageBytes);

            using var image = await Image.LoadAsync(inputStream);

            // Resize while keeping aspect ratio
            image.Mutate(x =>
            {
                x.Resize(new ResizeOptions
                {
                    Size = new Size(128, 128),
                    Mode = ResizeMode.Max
                });
            });

            // Save as WebP
            await image.SaveAsync(
                filePath,
                new WebpEncoder
                {
                    Quality = 90
                });

            // Store URL path for the API
            team.IconPath = $"/team-icons/{fileName}";

            team.LastUpdated = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}