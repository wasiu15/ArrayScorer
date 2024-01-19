using ArrayScorer.Dtos;

namespace ArrayScorer.Services
{
    public class GetTotalScoreService : IGetTotalScoreService
    {
        private readonly ILogger<GetTotalScoreService> _logger;
        private readonly IConfiguration _configuration;

        public GetTotalScoreService(ILogger<GetTotalScoreService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public TotalScoreResponseDto GetTotalScore(List<int> request)
        {
            try
            {
                // Retrieve scoring points from configuration with error handling
                var evenNumberPoint = GetConfigValueOrDefault("evenNumberPoint");
                var oddNumberPoint = GetConfigValueOrDefault("oddNumberPoint");
                var numberEightPoint = GetConfigValueOrDefault("numberEightPoint");


                // Calculate totals based on scoring points
                var evenNumbersTotal = request.Count(evenNumber => evenNumber % 2 == 0) * evenNumberPoint;
                var oddNumbersTotal = request.Count(oddNumber => oddNumber % 2 != 0) * oddNumberPoint;
                var numberEightTotal = request.Count(numberEight => numberEight == 8) * numberEightPoint;

                // Sum up the totals
                var sumOfTotals = evenNumbersTotal + oddNumbersTotal + numberEightTotal;

                // Return the result in a TotalResponseDto
                return new TotalScoreResponseDto
                {
                    IsSuccessful = true,
                    Result = sumOfTotals
                };
            }
            catch (Exception ex)
            {
                // Log the exception with details
                _logger.LogError(ex, "An error occurred while calculating the total score.");

                // Return a response indicating failure
                return new TotalScoreResponseDto
                {
                    IsSuccessful = false,
                    Result = 0
                };
            }
        }

        // Helper method for parsing configuration values or providing defaults
        private int GetConfigValueOrDefault(string key)
        {
            if (int.TryParse(_configuration[key], out var value))
            {
                return value;
            }

            // Log an error for failed parsing and return a default value
            _logger.LogError($"Failed to parse '{key}' from configuration.");
            return 0; // Set a default value or take appropriate action
        }
    }
}