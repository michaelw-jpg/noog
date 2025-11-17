using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class OpenAiPromptService : IOpenAiPromptService
    {
        private readonly IOpenAiService _openAiService;

        public OpenAiPromptService(IOpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        public async Task<string> CreatePromptAsync(PromptType type)
        {
            string transcript = "Genereate a radmom meeting transcript of 1000 words";
            if (!Prompts.TryGetValue(type, out var template))
            {
                throw new ArgumentException($"No prompt found for {type}");

            }

            string finalPrompt = template.Replace("{transcript}", transcript);

            var response = await _openAiService.GetChatResponseAsync(finalPrompt);

            return response.Data?.Message ?? string.Empty;
        }
        
        public enum PromptType
        {
            MeetingSummary, //general summary
            ExecutiveSummary,//Highligt main topics with action items and assigend owners and deadlines
            DetailedSummary,//Agenda, key points, decisions Made followuup and action items
            InsightExtractionSummary,//Extract insight challanges and oppertuinites and emphasis impact on strategy budget and timelines
            ClientCallSummary,//Create a client simmary include client geals conserns objections and propores solutions and next steps use professional tone

        }

        public static readonly Dictionary<PromptType, string> Prompts = new()
        {
            {
            PromptType.MeetingSummary, "Gereral meeting summary"
            },
            {
                PromptType.ExecutiveSummary, "Highligt main topics with action items and assigend owners and deadlines. Keep it under 300 words"
            },
            {
                PromptType.DetailedSummary, "Provide a structured summary under these sections. 1. Agenda, 2. Key Discussion Points, 3. Decisions Made, 4. Action Items(With who and when if possible), 5. Open Questions or Follow-ups"
            },
            {
                PromptType.InsightExtractionSummary, "Extract insight challanges and oppertuinites and emphasis impact on strategy budget and timelines"
            },
            {
                PromptType.ClientCallSummary, "Create a client summary include client geals conserns objections and propores solutions and next steps. Use a professional tone"
            }
        };
    }
}
