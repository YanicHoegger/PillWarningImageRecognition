using Domain.Interface;

namespace Domain.Prediction
{
    public interface IProbabilityToLikelinessConverter
    {
        Likeliness Convert(double probability);
    }
}
