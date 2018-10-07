using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Repositories;

namespace MyCode.Project.Services.Implementation
{
    public class PictureService
    {
        #region 初始化
        private readonly IBasPictureGroupRepository _basPictureGroupRepository;
        private readonly IBasPictureStockRepository _basPictureStockRepository;
        private readonly IBasPictureUseRepository _basPictureUseRepository;

        public PictureService(IBasPictureGroupRepository basPictureGroupRepository,
            IBasPictureStockRepository basPictureStockRepository,
            IBasPictureUseRepository basPictureUseRepository)
        {

            _basPictureGroupRepository = basPictureGroupRepository;
            _basPictureStockRepository = basPictureStockRepository;
            _basPictureUseRepository = basPictureUseRepository;

        }
        #endregion
    }
}
