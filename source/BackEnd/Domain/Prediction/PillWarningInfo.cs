using Domain.Interface;

namespace Domain.Prediction
{
    public class PillWarningInfo : IPillWarningInfo
    {
        public PillWarningInfo(string title, string info)
        {
            Title = title;
            Info = info;
        }

        public string Title { get; }
        public string Info { get; }
    }
}
