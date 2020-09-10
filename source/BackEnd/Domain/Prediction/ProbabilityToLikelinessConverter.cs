using System;
using Domain.Interface;

namespace Domain.Prediction
{
    public class ProbabilityToLikelinessConverter : IProbabilityToLikelinessConverter
    {
        public Likeliness Convert(double probability)
        {
            return probability switch
            {
                { } d when d < 0.2 => Likeliness.NotAtAll,
                { } d when d < 0.6 => Likeliness.Maybe,
                { } d when d < 0.9 => Likeliness.Very,
                { } d when d <= 1 => Likeliness.Sure,

                _ => throw new ArgumentException($"{nameof(probability)} can not be bigger then 1")
            };
        }
    }
}
