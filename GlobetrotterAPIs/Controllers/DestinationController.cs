using GlobetrotterAPIs.Context;
using GlobetrotterAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobetrotterAPIs.Controllers
{
    [Route("api/destinations")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly GlobetrotterContext _context;

        public DestinationController(GlobetrotterContext context)
        {
            _context = context;
        }
        [HttpGet("random")]
        public async Task<ActionResult<Destination>> GetRandomDestination()
        {
            var count = await _context.Destinations.CountAsync();
            if (count == 0) return NotFound("No destinations available");

            var random = new Random();
            var skip = random.Next(count);

            var destination = await _context.Destinations.Skip(skip).FirstOrDefaultAsync();
            return Ok(destination);
        }

        [HttpGet("getRandomFact")]
        public async Task<ActionResult<ApiResponse<RandomFact>>> GetRandomFact()
        {
            try
            {
                Random r = new Random();
                int? id = r.Next(1, 100);

                var data = await _context.Destinations
                    .Where(x => x.Id == id)
                    .Select(x => new RandomFact
                    {
                        Destination = x.City,
                        Fact = x.FunFacts,
                        Country = x.Country
                    })
                    .FirstOrDefaultAsync();

                return Ok(new ApiResponse<RandomFact>
                {
                    IsSuccess = true,
                    Message = "Random fact retrieved successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RandomFact>
                {
                    IsSuccess = false,
                    Message = "An error occurred",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("getRandomTrivia")]
        public async Task<ActionResult<ApiResponse<RandomFact>>> GetRandomTrivia()
        {
            try
            {
                Random r = new Random();
                int? id = r.Next(1, 100);

                var data = await _context.Destinations
                    .Where(x => x.Id == id)
                    .Select(x => new RandomFact
                    {
                        Destination = x.City,
                        Fact = x.Trivia,
                        Country = x.Country
                    })
                    .FirstOrDefaultAsync();

                return Ok(new ApiResponse<RandomFact>
                {
                    IsSuccess = true,
                    Message = "Random fact retrieved successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RandomFact>
                {
                    IsSuccess = false,
                    Message = "An error occurred",
                    Error = ex.Message
                });
            }
        }

    }
}
public class RandomFact
{
    public string Destination { get; set; }
    public string Fact { get; set; }
    public string Country { get; set; }
  }