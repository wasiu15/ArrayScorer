using ArrayScorer.Dtos;

namespace ArrayScorer.Services
{
    public interface IGetTotalScoreService
    {
        TotalScoreResponseDto GetTotalScore(List<int> request);
    }
}
