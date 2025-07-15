using AutoMapper;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class LogServices : ILogServices
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        public LogServices(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task<LogListVM> GetLogList(PaginationDTO paginationDTO)
        {
            (List<Log> logs, int totalCount) = await _logRepository.GetListAsync(paginationDTO);
            List<LogVM> logVMs = _mapper.Map<List<LogVM>>(logs);
            LogListVM logListVM = _mapper.Map<LogListVM>(paginationDTO);
            logListVM.Logs = logVMs;
            logListVM.TotalItems = totalCount;
            return logListVM;
        }
    }

}
