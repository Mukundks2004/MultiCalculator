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
            var openAiApiKey = "sk-TAotAypTGxc_Vaa9tntvWdxoc7AvP0ODDsaP1eZUbNT3BlbkFJf6J_VPy-GlqxYp0ARcECC9O0tmrzS-5OLpcSMZ7uMA"; // I put $10 into this so we can use it.

            APIAuthentication aPIAuthentication = new APIAuthentication(openAiApiKey);
            OpenAIAPI openAiApi = new OpenAIAPI(aPIAuthentication);

            try
            {
                string model = "gpt-4o-mini"; // We gonna have to use gpt-4o-mini seems to be the cheapest at around $0.015 for 1000 output tokens & $0.005 for 1000 input tokens. 
                int maxTokens = 50; // So do not spam use this for no reason now -- Still needs testing, but the API Link and Model should be fine? (Model not 100% sure but the API does work and has $10 on that account.

                var completionRequest = new CompletionRequest
                {
                    Prompt = prompt,
                    Model = model,
                    MaxTokens = maxTokens,
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