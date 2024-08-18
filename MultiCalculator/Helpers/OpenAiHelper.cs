using MultiCalculator.Database.Models;
using MultiCalculator.Database.Services;
using OpenAI_API;
using OpenAI_API.Completions;

namespace MultiCalculator.Helpers
{
    public class OpenAiHelper
    {
        readonly IDatabaseService _databaseService;
        private string openAiResponse;

        public OpenAiHelper(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<string> OpenAi(string prompt)
        {
            var openAiApiKey = "sk-TAotAypTGxc_Vaa9tntvWdxoc7AvP0ODDsaP1eZUbNT3BlbkFJf6J_VPy-GlqxYp0ARcECC9O0tmrzS-5OLpcSMZ7uMA";

            OpenAIAPI openAiApi = new OpenAIAPI(openAiApiKey);

            try
            {
                var completionRequest = new CompletionRequest()
                {
                    Prompt = prompt,
                    Model = OpenAI_API.Models.Model.DefaultModel,
                    MaxTokens = 200,
                };

                var completionResult = await openAiApi.Completions.CreateCompletionAsync(completionRequest);
                openAiResponse = completionResult.Completions[0].Text;
                return completionResult.Completions[0].Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return "";
        }

        public string SubmitAndGetApiResponse(string prompt, UserModel sendingUser)
        {
            var result = OpenAi(prompt).ToString();
            _databaseService.AddOpenAiQuestion(new OpenAiQuestionsModel() { Id = new Guid(), Question = prompt, Answer = openAiResponse, QuestionSender = sendingUser });
            return openAiResponse;
        }
    }
}