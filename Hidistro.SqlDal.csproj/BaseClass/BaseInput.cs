using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseClass
{
   public  class BaseInput
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public string TenantID { get; set; }
    }
    /// <summary>
    /// 分页基类
    /// </summary>
    public class BaseListInput
    {
        /// <summary>
        /// 默认每页数量，当前第一页
        /// </summary>
        public BaseListInput()
        {
            PageSize = 10;
            PageIndex = 1;
        }
        /// <summary>
        /// 每页页数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }


        /// <summary>
        /// 租户编号
        /// </summary>
        public string TenantId { get; set; }
    }
}
