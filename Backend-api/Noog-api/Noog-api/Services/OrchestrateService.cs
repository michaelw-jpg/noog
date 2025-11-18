using Microsoft.AspNetCore.Http;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Enums;
using Noog_api.Models.AssemblyAi;
using Noog_api.Services.IServices;
using OpenAI.Responses;
using StreamChat.Models;
using static Noog_api.Services.OpenAiPromptService;

namespace Noog_api.Services
{
    public class OrchestrateService(IAssemblyAiService assemblyService, IOpenAiService openAiService,
        ISummaryService summaryService) : IOrchestrateService
    {
        private readonly IAssemblyAiService _assemblyService = assemblyService;
        private readonly IOpenAiService _openAiService = openAiService;
        private readonly ISummaryService _summaryService = summaryService;


        public async Task<BaseResponseDto<SummaryResponseDto>> OrchestrateAsync(string audioUrl,  Guid projectGroupId,
            string? language = "swedish", PromptType? type = PromptType.MeetingSummary )
            
        {
            if (string.IsNullOrWhiteSpace(audioUrl))
                throw new ArgumentException("Audio URL cannot be empty.", nameof(audioUrl));
            // Step 1: Upload the audio file to AssemblyAI
            string uploadedUrl = await _assemblyService.UploadFileAsync(audioUrl);
            // Step 2: Create a transcript request
            var transcript = await _assemblyService.CreateTranscriptAsync(uploadedUrl, language);
            
            // Step 3: Wait for the transcription to complete
            var completedTranscript = new Transcript();
            try
            {
                completedTranscript = await _assemblyService.WaitForTranscriptToProcess(transcript);

            }
            catch (Exception ex)
            {

                return new BaseResponseDto<SummaryResponseDto>(
                    Enums.StatusCodesEnum.ServerError, "Transcription processing failed: " + ex.Message, null);
            }
            //here we can later save transcript to db if needed



            // Step 4: Get summary from OpenAI          
            var openAiResponse = await _openAiService.GetChatResponseAsync(type.Value,completedTranscript.Text);

            if(openAiResponse.StatusCode != StatusCodesEnum.Success)
            {
                
                return new BaseResponseDto<SummaryResponseDto>(
                    openAiResponse.StatusCode, "Failed to get summary from OpenAI: " + openAiResponse.Message, null);

            }

            var createSummaryDto = new CreateSummaryDto
            {
                Title = "Auto-generated Summary", //need to ask ai for a title later
                Content = openAiResponse.Data.Message,
                ProjectGroupId = projectGroupId
            };
            var summaryResult = await _summaryService.CreateSummaryAsync(createSummaryDto);

            return summaryResult;
        }
    }
}
