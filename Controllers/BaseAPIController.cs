using Lombeo.Api.Authorize.DTO.Common;
using Lombeo.Api.Authorize.Infra.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Lombeo.Api.Authorize.Controllers
{
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        public BaseAPIController()
        {
        }

        public async Task<ResponseDTO<T>> HandleException<T>(Task<T> task)
        {
            try
            {
                var data = await task;
                return new ResponseDTO<T>() { Success = true, Data = data };
            }
            catch (ApplicationException ex)
            {
                //Serilog.Log.Debug(ex, ex.Message);
                return new ResponseDTO<T>() { Success = false, Code = 200, Message = ex.Message };
            }
            catch (KeyNotFoundException ex)
            {
                //Serilog.Log.Debug(ex, ex.Message);
                return new ResponseDTO<T>() { Success = false, Code = 404, Message = ex.Message };
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ResponseDTO<T>() { Success = false, Code = 403, Message = ex.Message };
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return new ResponseDTO<T>() { Success = false, Code = 500, Message = Message.CommonMessage.ERROR_HAPPENED };
            }
        }
    }
}
