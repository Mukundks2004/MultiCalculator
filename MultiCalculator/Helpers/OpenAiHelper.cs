using OpenAI_API.Completions;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiCalculator.Database.Services;
using MultiCalculator.Database.Models;

namespace MultiCalculator.Helpers
{
    public class OpenAiHelper
    {
        private string openAiResponse;
        readonly DatabaseService _databaseService;

        public OpenAiHelper(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        async void OpenAi(string prompt)
        {
            var openAiApiKey = "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"; // May have to replace currently using the gmail one I sent.

            APIAuthentication aPIAuthentication = new APIAuthentication(openAiApiKey);
            OpenAIAPI openAiApi = new OpenAIAPI(aPIAuthentication);


            try
            {
                string model = "gpt-3.5-turbo-0613"; // We gonna have to use gpt-4o-mini seems to be the cheapest at around $0.015 for 1000 output tokens & $0.005 for 1000 input tokens. 

                var completionRequest = new CompletionRequest
                {
                    Prompt = prompt,
                    Model = model,
                };

                var completionResult = await openAiApi.Completions.CreateCompletionAsync(completionRequest);
                var generatedText = completionResult.Completions[0].Text;

                Console.WriteLine("Generated text:");
                Console.WriteLine(generatedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public string SubmitAndGetApiResponse(string prompt, UserModel sendingUser)
        {
            OpenAi(prompt);
            _databaseService.AddOpenAiQuestion(new OpenAiQuestionsModel() { Id = new Guid(), Question = prompt, Answer = openAiResponse, QuestionSender = sendingUser });
            return openAiResponse;
        }
    }
}