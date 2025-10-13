using Azure;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Mappers;
using Noog_api.Models;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class SummaryService(ISummaryRepository repo) : ISummaryService
    {
        private readonly ISummaryRepository _repo = repo;
        

        public async Task<BaseResponseDto<List<SummaryResponseDto>>> GetAllSummariesAsync()
        {
            var result = await _repo.GetAllSummariesAsync();

            var response = new BaseResponseDto<List<SummaryResponseDto>>();

            if (result == null || !result.Any())
            {
                 response.Data = new List<SummaryResponseDto>();
                 response.StatusCode = Enums.StatusCodesEnum.Success;
                 response.Message = "No summaries found";
            }
            else 
            {
                var dtoList = GenericMapper.ToDtoList<Summary, SummaryResponseDto>(result);

                response.Data = dtoList;
                response.StatusCode = Enums.StatusCodesEnum.Success;
                response.Message = "Summaries loaded successfully";
            }
           
            return response;
        }

        public async Task<BaseResponseDto<SummaryResponseDto>> GetSummaryByIdAsync(int id)
        {
            var result = await _repo.GetSummaryByIdAsync(id);
            var response = new BaseResponseDto<SummaryResponseDto>();
            
            if(result == null)
            {
                
                response.StatusCode = Enums.StatusCodesEnum.NotFound;
                response.Message = "Summary not found";
                return response;
            }
            else
            {
                var dto = GenericMapper.ToDto<Summary, SummaryResponseDto>(result);
                response.Data = dto;
                response.StatusCode = Enums.StatusCodesEnum.Success;
                response.Message = "Summary loaded successfully";
                return response;
            }

        }

        public async Task<BaseResponseDto<SummaryResponseDto>> CreateSummaryAsync(CreateSummaryDto Request)
        {
            var summary = new Summary();
            var response = new BaseResponseDto<SummaryResponseDto>();
           

            GenericMapper.ApplyCreate(summary, Request);
            try
            {
                await _repo.CreateSummaryAsync(summary);
            }
            catch (Exception ex)
            {
                response.Data = GenericMapper.ToDto<Summary, SummaryResponseDto>(summary);
                response.StatusCode = Enums.StatusCodesEnum.ServerError;
                response.Message = ex.Message;
                return response;
            }

            
            response.Data = GenericMapper.ToDto<Summary, SummaryResponseDto>(summary);
            response.StatusCode = Enums.StatusCodesEnum.Created;
            response.Message = "Summary created successfully";
            

            return response;
        }

        public async Task<BaseResponseDto<SummaryResponseDto>> UpdateSummaryAsync(int id, PatchSummaryDto updatedSummary)
        {
            var summary = await _repo.GetSummaryByIdAsync(id);
            if(summary == null)
            {
                return new BaseResponseDto<SummaryResponseDto>
                {
                    StatusCode = Enums.StatusCodesEnum.NotFound,
                    Message = "Summary not found"
                };
            }

            var response = new BaseResponseDto<SummaryResponseDto>();
            GenericMapper.ApplyPatch(summary, updatedSummary);

            try
            {
                await _repo.UpdateSummaryAsync(id, summary);
            }
            catch (Exception ex)
            {
                response.Data = GenericMapper.ToDto<Summary, SummaryResponseDto>(summary);
                response.StatusCode = Enums.StatusCodesEnum.ServerError;
                response.Message = ex.Message;
                return response;

            }
            response.Data = GenericMapper.ToDto<Summary, SummaryResponseDto>(summary);
            response.StatusCode = Enums.StatusCodesEnum.Success;
            response.Message = "Summary Created Sucessfully";
            return response;

            
        }

        public async Task<BaseResponseDto<int>> DeleteSummaryAsync(int id)
        {
            var result =  new BaseResponseDto<int>();
            bool success = false;
            try
            {
                success = await _repo.DeleteSummaryAsync(id);
            }
            catch (Exception ex)
            {
                result = new BaseResponseDto<int>
                {
                    StatusCode = Enums.StatusCodesEnum.ServerError,
                    Message = ex.Message,
                };
                return result;
            }

            if (success)
            {
                result.Data = id;
                result.StatusCode = Enums.StatusCodesEnum.Success;
                result.Message = "Summary deleted successfully";
                return result;
            }

            else
            {
                result.StatusCode = Enums.StatusCodesEnum.NotFound;
                result.Message = "Summary Not found";
                return result;
            }
            
           
        }
    }
}
