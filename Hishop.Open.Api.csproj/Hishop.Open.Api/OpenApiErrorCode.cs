using System;
using System.ComponentModel;

namespace Hishop.Open.Api
{
	public enum OpenApiErrorCode
	{
		[Description("开发者权限不足")]
		Insufficient_ISV_Permissions = 1,
		[Description("用户权限不足")]
		Insufficient_User_Permissions,
		[Description("远程服务出错")]
		Remote_Service_Error,
		[Description("缺少方法名参数")]
		Missing_Method,
		[Description("不存在的方法名")]
		Invalid_Method,
		[Description("非法数据格式")]
		Invalid_Format,
		[Description("缺少签名参数")]
		Missing_Signature,
		[Description("非法签名")]
		Invalid_Signature,
		[Description("缺少AppKey参数")]
		Missing_App_Key,
		[Description("非法的AppKey参数")]
		Invalid_App_Key,
		[Description("缺少时间戳参数")]
		Missing_Timestamp = 12,
		[Description("非法的时间戳参数")]
		Invalid_Timestamp,
		[Description("缺少必选参数")]
		Missing_Required_Arguments,
		[Description("非法的参数")]
		Invalid_Arguments,
		[Description("请求被禁止")]
		Forbidden_Request,
		[Description("参数错误")]
		Parameter_Error,
		[Description("系统错误")]
		System_Error,
		[Description("缺少参数")]
		Missing_Parameters = 501,
		[Description("需要绑定用户昵称")]
		Need_Binding_User,
		[Description("参数格式错误")]
		Parameters_Format_Error,
		[Description("交易不存在")]
		Trade_not_Exists,
		[Description("非法交易")]
		Trade_is_Invalid,
		[Description("用户不存在")]
		User_not_Exists,
		[Description("非法的交易订单（或子订单）ID")]
		Biz_Order_ID_is_Invalid,
		[Description("交易备注超出长度限制")]
		Trade_Memo_Too_Long,
		[Description("页码条数超出长度限制")]
		Page_Size_Too_Long,
		[Description("开始时间晚于结束时间")]
		Time_Start_End,
		[Description("结束时间晚于当前时间")]
		Time_End_Now,
		[Description("开始时间晚于当前时间")]
		Time_Start_Now,
		[Description("物流公司不存在")]
		Company_not_Exists,
		[Description("运单号太长")]
		Out_Sid_Too_Long,
		[Description("商品库存不足")]
		Product_Stock_Lack,
		[Description("订单状态不允许进行发货")]
		Trade_Status_Send,
		[Description("配送方式不存在")]
		ShippingMode_not_Exists,
		[Description("交易标记值不在指定范围之内")]
		Trade_Flag_Too_Long,
		[Description("状态不在指定范围之内")]
		Trade_Status_is_Invalid,
		[Description("查询条件(修改时间)跨度不能超过一天")]
		Time_StartModified_AND_EndModified,
		[Description("来自门店的订单")]
		Trade_is_Store,
		[Description("订单状态不允许打印")]
		Trade_Status_Print,
		[Description("订单打印失败")]
		Trade_Print_Faild,
		[Description("商品不存在")]
		Product_Not_Exists = 601,
		[Description("状态不在指定范围之内")]
		Product_Status_is_Invalid,
		[Description("修改商品库存失败")]
		Product_UpdateeQuantity_Faild,
		[Description("修改商品状态失败")]
		Product_ApproveStatus_Faild
	}
}
