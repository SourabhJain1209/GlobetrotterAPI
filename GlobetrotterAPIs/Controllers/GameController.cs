using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlobetrotterAPIs.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using GlobetrotterAPIs.Context;
using System.Globalization;

namespace GlobetrotterAPI.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GlobetrotterContext _context;

        public GameController(GlobetrotterContext context)
        {
            _context = context;
        }

        [HttpGet("CreateQuiz")]
        public async Task<ApiResponse<QuizDTO>> CreateQuiz()
        {
            var r = new Random();
            int? id = r.Next(1, 100);

            // Fetch the correct city
            var correctCity = _context.Destinations
                .Where(x => x.Id == id)
                .Select(x => new { x.City, x.Clues })
                .FirstOrDefault();

            if (correctCity == null)
            {
                return new ApiResponse<QuizDTO>
                {
                    IsSuccess = false,
                    Message = "No city found!"
                };
            }

            // Fetch 3 random incorrect cities
            var incorrectCities = _context.Destinations
                .Where(x => x.Id != id)
                .OrderBy(x => Guid.NewGuid()) // Randomize the order
                .Take(3)
                .Select(x => x.City)
                .ToList();

            // Ensure we have 3 incorrect cities
            if (incorrectCities.Count < 3)
            {
                return new ApiResponse<QuizDTO>
                {
                    IsSuccess = false,
                    Message = "Not enough cities in database!"
                };
            }

            // Create the quiz options
            List<QuizOptionDTO> options = new List<QuizOptionDTO>
            {
                new QuizOptionDTO { Option = correctCity.City, IsCorrect = true },
                new QuizOptionDTO { Option = incorrectCities[0], IsCorrect = false },
                new QuizOptionDTO { Option = incorrectCities[1], IsCorrect = false },
                new QuizOptionDTO { Option = incorrectCities[2], IsCorrect = false }
            };

            // Shuffle the options
            options = options.OrderBy(x => Guid.NewGuid()).ToList();

            // Build the QuizDTO
            QuizDTO quizDTO = new QuizDTO
            {
                Options = options,
                Clue = correctCity.Clues
            };

            return new ApiResponse<QuizDTO>
            {
                IsSuccess = true,
                Data = quizDTO
            };
        }



        //[HttpGet("/api/destinations/random")]
        //public async Task<IActionResult> GetRandomDestination()
        //{
        //    var random = new Random();
        //    var count = await _context.Destinations.CountAsync();
        //    var destination = await _context.Destinations.Skip(random.Next(count)).FirstOrDefaultAsync();

        //    if (destination == null)
        //        return NotFound();

        //    var options = new List<string> { destination.City, "Wrong Option 1", "Wrong Option 2", "Wrong Option 3" };
        //    options = options.OrderBy(_ => random.Next()).ToList();

        //    return Ok(new { destination, options });
        //}

        //[HttpPost("verify")]
        //public async Task<IActionResult> VerifyAnswer([FromBody] VerifyAnswerDto request)
        //{
        //    var destination = await _context.Destinations.FindAsync(request.DestinationId);
        //    if (destination == null)
        //        return NotFound();

        //    bool isCorrect = destination.City.Equals(request.Answer, StringComparison.OrdinalIgnoreCase);
        //    var user = await _context.Users.FindAsync(request.UserId);

        //    if (user != null)
        //    {
        //        if (isCorrect)
        //        {
        //            user.Score += 10;
        //            user.CorrectAnswers += 1;
        //        }
        //        else
        //        {
        //            user.IncorrectAnswers += 1;
        //        }
        //        await _context.SaveChangesAsync();
        //    }

        //    return Ok(new { correct = isCorrect, funFact = destination.FunFacts });
        //}
    }
}

public class QuizDTO
{
    public List<QuizOptionDTO> Options { get; set; }
    public string Clue { get; set; }
}

public class QuizOptionDTO
{
    public string Option { get; set; }
    public bool IsCorrect { get; set; }
}