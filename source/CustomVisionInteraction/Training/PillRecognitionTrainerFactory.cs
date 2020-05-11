using System.Threading.Tasks;

namespace CustomVisionInteraction.Training
{
    public static class PillRecognitionTrainerFactory
    {
        public static async Task<PillRecognitionTrainer> Create(IContext context)
        {
            var trainerCommunicator = new TrainerCommunicator(context);
            await trainerCommunicator.Init();

            return new PillRecognitionTrainer(trainerCommunicator);
        }
    }
}
