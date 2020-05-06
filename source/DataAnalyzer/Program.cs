using CustomVisionInteraction;
using DrugCheckingCrawler.ContentWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DataAnalyzer
{
    internal class Program
    {
        internal static void Main()
        {
            //Temporarly here for testing

            //var trainerCommunicator = new TrainerCommunicator(context);
            //trainerCommunicator.Init().Wait();

            //var inputData = new List<(byte[] image, string name)>();
            //foreach (var directory in Directory.GetDirectories(@"C:\Users\Yanic\source\repos\PillWarningImageRecognition\DrugCheckingResources"))
            //{
            //    var content = File.ReadAllText(Path.Combine(directory, "Info.txt"));
            //    var info = JsonSerializer.Deserialize<InfoFileContent>(content);

            //    inputData.Add((File.ReadAllBytes(Path.Combine(directory, "Image.jpg")), info.Name));
            //};

            //var trainer = new PillRecognitionTrainer(trainerCommunicator);
            //try
            //{
            //    trainer.Train(inputData).Wait();
            //}
            //catch (AggregateException aggregateException)
            //{
            //    foreach (var exception in aggregateException.InnerExceptions)
            //    {
            //        Console.Error.WriteLine(exception.Message);
            //        Console.Error.WriteLine(exception.StackTrace);
            //    }
            //}
        }
    }
}
