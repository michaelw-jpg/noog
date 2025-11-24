using Noog_api.Enums;
using Noog_api.Models.AssemblyAi;
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

        public async Task<string> CreatePromptAsync(PromptType type, string transcript, string language)
        {
            if (!Prompts.ContainsKey(type))
            {
                throw new ArgumentException($"No prompt found for {type}");
            }

            var response = await _openAiService.GetChatResponseAsync(type, transcript, language);

            return response.Data?.Message ?? string.Empty;
        }
        

        public static readonly Dictionary<PromptType, string> Prompts = new()
        {
            {
            PromptType.MeetingSummary, "General meeting summary"
            },
            {
                PromptType.ExecutiveSummary, "Highlight main topics with action items and assigned owners and deadlines. Keep it under 300 words"
            },
            {
                PromptType.DetailedSummary, "Provide a structured summary under these sections. 1. Agenda, 2. Key Discussion Points, 3. Decisions Made, 4. Action Items(With who and when if possible), 5. Open Questions or Follow-ups"
            },
            {
                PromptType.InsightExtractionSummary, "Extract insight, challanges, opportunities and emphasis impact on strategic budget and timelines"
            },
            {
                PromptType.ClientCallSummary, "Create a client summary include client goals, concerns, objections and propose solutions and next steps. Use a professional tone"
            }
        };
    }
}
