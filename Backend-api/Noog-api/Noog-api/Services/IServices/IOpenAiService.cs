using Azure;
using Azure.AI.OpenAI;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Enums;
using Noog_api.Models.AssemblyAi;
using Noog_api.Services.IServices;
using OpenAI.Chat;
using System.Net.Http.Headers;
using System.Text.Json;
namespace Noog_api.Services.IServices
{
    public interface IOpenAiService
    {
        Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(string prompt);
        //Add Transcript class input
        Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(PromptType type, string transcript, string language);
    }
}
