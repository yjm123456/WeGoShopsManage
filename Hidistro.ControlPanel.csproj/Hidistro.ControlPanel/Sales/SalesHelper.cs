using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Sales;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.ControlPanel.Sales
{
	public sealed class SalesHelper
	{
		private SalesHelper()
		{
		}

		public static bool AddExpressTemplate(string expressName, string xmlFile)
		{
			return (new ExpressTemplateDao()).AddExpressTemplate(expressName, xmlFile);
		}

		public static bool AddShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return (new ShipperDao()).AddShipper(shipper);
		}

		public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
		{
			PaymentModeActionStatus paymentModeActionStatu;
			if (null != paymentMode)
			{
				Globals.EntityCoding(paymentMode, true);
				paymentModeActionStatu = (new PaymentModeDao()).CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
			}
			else
			{
				paymentModeActionStatu = PaymentModeActionStatus.UnknowError;
			}
			return paymentModeActionStatu;
		}

		public static bool DeleteExpressTemplate(int expressId)
		{
			return (new ExpressTemplateDao()).DeleteExpressTemplate(expressId);
		}

		public static bool DeletePaymentMode(int modeId)
		{
			PaymentModeInfo paymentModeInfo = new PaymentModeInfo()
			{
				ModeId = modeId
			};
			return (new PaymentModeDao()).CreateUpdateDeletePaymentMode(paymentModeInfo, DataProviderAction.Delete) == PaymentModeActionStatus.Success;
		}

		public static bool DeleteShipper(int shipperId)
		{
			return (new ShipperDao()).DeleteShipper(shipperId);
		}

		public static DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			return (new DateStatisticDao()).GetDaySaleTotal(year, month, saleStatisticsType);
		}

		public static decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
		{
			return (new DateStatisticDao()).GetDaySaleTotal(year, month, day, saleStatisticsType);
		}

		public static DataTable GetExpressTemplates()
		{
			return (new ExpressTemplateDao()).GetExpressTemplates(null);
		}

		public static DataTable GetIsUserExpressTemplates()
		{
			return (new ExpressTemplateDao()).GetExpressTemplates(new bool?(true));
		}

		public static DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			return (new SaleStatisticDao()).GetMemberStatistics(query, out totalProductSales);
		}

		public static DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
		{
			return (new SaleStatisticDao()).GetMemberStatisticsNoPage(query);
		}

		public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			return (new DateStatisticDao()).GetMonthSaleTotal(year, month, saleStatisticsType);
		}

		public static DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			return (new DateStatisticDao()).GetMonthSaleTotal(year, saleStatisticsType);
		}

		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return (new PaymentModeDao()).GetPaymentMode(modeId);
		}

		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return (new PaymentModeDao()).GetPaymentMode(gateway);
		}

		public static IList<PaymentModeInfo> GetPaymentModes()
		{
			return (new PaymentModeDao()).GetPaymentModes();
		}

		public static DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			DataTable productSales;
			if (productSale != null)
			{
				productSales = (new SaleStatisticDao()).GetProductSales(productSale, out totalProductSales);
			}
			else
			{
				totalProductSales = 0;
				productSales = null;
			}
			return productSales;
		}

		public static DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			DataTable productSalesNoPage;
			if (productSale != null)
			{
				productSalesNoPage = (new SaleStatisticDao()).GetProductSalesNoPage(productSale, out totalProductSales);
			}
			else
			{
				totalProductSales = 0;
				productSalesNoPage = null;
			}
			return productSalesNoPage;
		}

		public static DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			return (new SaleStatisticDao()).GetProductVisitAndBuyStatistics(query, out totalProductSales);
		}

		public static DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
		{
			return (new SaleStatisticDao()).GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
		}

		public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
		{
			return (new SaleStatisticDao()).GetSaleOrderLineItemsStatistics(query);
		}

		public static DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
		{
			return (new SaleStatisticDao()).GetSaleOrderLineItemsStatisticsNoPage(query);
		}

		public static DbQueryResult GetSaleTargets()
		{
			return (new SaleStatisticDao()).GetSaleTargets();
		}

		public static ShippersInfo GetShipper(int shipperId)
		{
			return (new ShipperDao()).GetShipper(shipperId);
		}

		public static IList<ShippersInfo> GetShippers(bool includeDistributor)
		{
			return (new ShipperDao()).GetShippers(includeDistributor);
		}

		public static IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
		{
			return (new DateStatisticDao()).GetUserAdd(year, month, days);
		}

		public static OrderStatisticsInfo GetUserOrders(OrderQuery userOrder)
		{
			return (new SaleStatisticDao()).GetUserOrders(userOrder);
		}

		public static OrderStatisticsInfo GetUserOrdersNoPage(OrderQuery userOrder)
		{
			return (new SaleStatisticDao()).GetUserOrdersNoPage(userOrder);
		}

		public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
		{
			IList<UserStatisticsInfo> userStatistics;
			if (page != null)
			{
				userStatistics = (new SaleStatisticDao()).GetUserStatistics(page, out totalProductSaleVisits);
			}
			else
			{
				totalProductSaleVisits = 0;
				userStatistics = null;
			}
			return userStatistics;
		}

		public static DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
		{
			return (new DateStatisticDao()).GetWeekSaleTota(saleStatisticsType);
		}

		public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			return (new DateStatisticDao()).GetYearSaleTotal(year, saleStatisticsType);
		}

		public static bool IsExistExpress(string ExpressName)
		{
			return (new ExpressTemplateDao()).IsExistExpress(ExpressName);
		}

		public static void SetDefalutShipper(int shipperId)
		{
			(new ShipperDao()).SetDefalutShipper(shipperId);
		}

		public static bool SetExpressIsUse(int expressId)
		{
			return (new ExpressTemplateDao()).SetExpressIsUse(expressId);
		}

		public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			(new PaymentModeDao()).SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}

		public static bool SwapShipper(int ShipperId, string ShipperTag)
		{
			return (new ShipperDao()).SwapShipper(ShipperId, ShipperTag);
		}

		public static bool UpdateExpressTemplate(int expressId, string expressName)
		{
			return (new ExpressTemplateDao()).UpdateExpressTemplate(expressId, expressName);
		}

		public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
		{
			PaymentModeActionStatus paymentModeActionStatu;
			if (null != paymentMode)
			{
				Globals.EntityCoding(paymentMode, true);
				paymentModeActionStatu = (new PaymentModeDao()).CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
			}
			else
			{
				paymentModeActionStatu = PaymentModeActionStatus.UnknowError;
			}
			return paymentModeActionStatu;
		}

		public static bool UpdateShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return (new ShipperDao()).UpdateShipper(shipper);
		}
	}
}