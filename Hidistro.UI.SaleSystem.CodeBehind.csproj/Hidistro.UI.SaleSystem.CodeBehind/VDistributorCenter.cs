using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VDistributorCenter : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litTodayOrdersNum;

		private System.Web.UI.WebControls.Literal litStroeName;

		private System.Web.UI.WebControls.Literal litMysubMember;

		private System.Web.UI.WebControls.Literal litMysubFirst;

		private System.Web.UI.WebControls.Literal litMysubSecond;

		private System.Web.UI.WebControls.Literal litProtuctNum;

		private System.Web.UI.WebControls.Literal litUserId;

		private System.Web.UI.WebControls.Literal litUserId1;

		private System.Web.UI.WebControls.Literal litUserId2;

		private System.Web.UI.WebControls.Literal litUserId3;

		private System.Web.UI.WebControls.Literal litUserId4;

		private System.Web.UI.WebControls.Literal litrGradeName;

		private System.Web.UI.WebControls.Literal litReferralBlance;

		private System.Web.UI.WebControls.Image imglogo;

		private System.Web.UI.WebControls.Literal litdistirbutors;

		private System.Web.UI.WebControls.Literal litOrders;

		private System.Web.UI.WebControls.Literal fxCenter;

		private System.Web.UI.WebControls.Literal commissionName1;

		private System.Web.UI.WebControls.Literal commissionName2;

		private System.Web.UI.WebControls.Literal fxTeamName;

		private System.Web.UI.WebControls.Literal shopName;

		private System.Web.UI.WebControls.Literal firstShop;

		private System.Web.UI.WebControls.Literal secondShop;

		private System.Web.UI.WebControls.Literal myCommission;

		private System.Web.UI.WebControls.Literal fxExplain;

		private FormatedMoneyLabel saletotal;

		private FormatedMoneyLabel refrraltotal;

		private System.Web.UI.HtmlControls.HtmlContainerControl UpClassInfo;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorCenter.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string distributorCenterName = masterSettings.DistributorCenterName;
			string commissionName = masterSettings.CommissionName;
			this.fxCenter = (System.Web.UI.WebControls.Literal)this.FindControl("fxCenter");
			this.fxCenter.Text = distributorCenterName;
			this.commissionName1 = (System.Web.UI.WebControls.Literal)this.FindControl("commissionName1");
			this.commissionName2 = (System.Web.UI.WebControls.Literal)this.FindControl("commissionName2");
			this.commissionName1.Text = commissionName;
			this.commissionName2.Text = commissionName;
			this.fxTeamName = (System.Web.UI.WebControls.Literal)this.FindControl("fxTeamName");
			this.fxTeamName.Text = masterSettings.DistributionTeamName;
			this.shopName = (System.Web.UI.WebControls.Literal)this.FindControl("shopName");
			this.shopName.Text = masterSettings.MyShopName;
			this.firstShop = (System.Web.UI.WebControls.Literal)this.FindControl("firstShop");
			this.firstShop.Text = masterSettings.FirstShopName;
			this.secondShop = (System.Web.UI.WebControls.Literal)this.FindControl("secondShop");
			this.secondShop.Text = masterSettings.SecondShopName;
			this.myCommission = (System.Web.UI.WebControls.Literal)this.FindControl("myCommission");
			this.myCommission.Text = masterSettings.MyCommissionName;
			this.fxExplain = (System.Web.UI.WebControls.Literal)this.FindControl("fxExplain");
			this.fxExplain.Text = masterSettings.DistributionDescriptionName;
			PageTitle.AddSiteNameTitle(distributorCenterName);
			int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
			if (userIdDistributors == null)
			{
				System.Web.HttpContext.Current.Response.Redirect("DistributorRegCheck.aspx");
			}
			else if (userIdDistributors.ReferralStatus != 0)
			{
				System.Web.HttpContext.Current.Response.Redirect("MemberCenter.aspx");
			}
			else
			{
				this.imglogo = (System.Web.UI.WebControls.Image)this.FindControl("image");
				if (!string.IsNullOrEmpty(userIdDistributors.Logo))
				{
					this.imglogo.ImageUrl = userIdDistributors.Logo;
				}
				if (masterSettings.IsShowDistributorSelfStoreName)
				{
					this.imglogo.Attributes.Add("onclick", "window.location.href = 'DistributorInfo.aspx'");
				}
				this.litStroeName = (System.Web.UI.WebControls.Literal)this.FindControl("litStroeName");
				this.litStroeName.Text = userIdDistributors.StoreName;
				this.litrGradeName = (System.Web.UI.WebControls.Literal)this.FindControl("litrGradeName");
				DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
				if (distributorGradeInfo != null)
				{
					this.litrGradeName.Text = distributorGradeInfo.Name;
				}
				this.litReferralBlance = (System.Web.UI.WebControls.Literal)this.FindControl("litReferralBlance");
				this.litReferralBlance.Text = userIdDistributors.ReferralBlance.ToString("F2");
				this.litUserId = (System.Web.UI.WebControls.Literal)this.FindControl("litUserId");
				this.litUserId1 = (System.Web.UI.WebControls.Literal)this.FindControl("litUserId1");
				this.litUserId2 = (System.Web.UI.WebControls.Literal)this.FindControl("litUserId2");
				this.litUserId3 = (System.Web.UI.WebControls.Literal)this.FindControl("litUserId3");
				this.litUserId4 = (System.Web.UI.WebControls.Literal)this.FindControl("litUserId4");
				this.litUserId.Text = userIdDistributors.UserId.ToString();
				this.litUserId1.Text = userIdDistributors.UserId.ToString();
				this.litUserId2.Text = userIdDistributors.UserId.ToString();
				this.litUserId3.Text = userIdDistributors.UserId.ToString();
				this.litUserId4.Text = userIdDistributors.UserId.ToString();
				this.litTodayOrdersNum = (System.Web.UI.WebControls.Literal)this.FindControl("litTodayOrdersNum");
				OrderQuery orderQuery = new OrderQuery();
				orderQuery.UserId = new int?(currentMemberUserId);
				orderQuery.Status = OrderStatus.Today;
				this.litTodayOrdersNum.Text = DistributorsBrower.GetDistributorOrderCount(orderQuery).ToString();
				this.refrraltotal = (FormatedMoneyLabel)this.FindControl("refrraltotal");
				this.refrraltotal.Money = DistributorsBrower.GetUserCommissions(userIdDistributors.UserId, System.DateTime.Now, null, null, null, "");
				this.saletotal = (FormatedMoneyLabel)this.FindControl("saletotal");
                //月团队奖参数
                ////定义时间
                //int getMonth = DateTime.Now.Month;
                //DateTime Starttime = DateTime.Now;
                //DateTime ss;
                //int CheckDate = Starttime.Day;
                //if (CheckDate >= 15)
                //{
                //    ss = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + getMonth.ToString() + "-15");
                //}
                //else
                //{
                //    ss = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + (getMonth - 1).ToString() + "-15");
                //}
                var ss =  DateTime.Now.AddDays(1 - DateTime.Now.Day);
                this.saletotal.Money = DistributorsBrower.GetTeamMoneysBrowser(userIdDistributors.UserId, ss, Convert.ToDateTime("2000-01-01"));
                this.litMysubMember = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubMember");
				this.litMysubFirst = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubFirst");
				this.litMysubSecond = (System.Web.UI.WebControls.Literal)this.FindControl("litMysubSecond");
				DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
				if (distributorsSubStoreNum != null || distributorsSubStoreNum.Rows.Count > 0)
				{
					this.litMysubMember.Text = distributorsSubStoreNum.Rows[0]["memberCount"].ToString();
					this.litMysubFirst.Text = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
					this.litMysubSecond.Text = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
				}
				else
				{
					this.litMysubMember.Text = "0";
					this.litMysubFirst.Text = "0";
					this.litMysubSecond.Text = "0";
				}
				this.litProtuctNum = (System.Web.UI.WebControls.Literal)this.FindControl("litProtuctNum");
				this.litProtuctNum.Text = ProductBrowser.GetProductsNumber(true).ToString();
				orderQuery.Status = OrderStatus.All;
				this.litOrders = (System.Web.UI.WebControls.Literal)this.FindControl("litOrders");
				this.litOrders.Text = DistributorsBrower.GetDistributorOrderCount(orderQuery).ToString();
				this.UpClassInfo = (System.Web.UI.HtmlControls.HtmlContainerControl)this.FindControl("UpClassInfo");
				System.Collections.Generic.IList<DistributorGradeInfo> distributorGradeInfos = VShopHelper.GetDistributorGradeInfos();
				DistributorGradeInfo distributorGradeInfo2 = null;
				foreach (DistributorGradeInfo current in distributorGradeInfos)
				{
					if (!(distributorGradeInfo.CommissionsLimit >= current.CommissionsLimit))
					{
						if (distributorGradeInfo2 == null)
						{
							distributorGradeInfo2 = current;
						}
						else if (distributorGradeInfo2.CommissionsLimit > current.CommissionsLimit)
						{
							distributorGradeInfo2 = current;
						}
					}
				}
				if (distributorGradeInfo2 == null)
				{
					this.UpClassInfo.Visible = false;
				}
				else
				{
					decimal d = distributorGradeInfo2.CommissionsLimit - userIdDistributors.ReferralBlance - userIdDistributors.ReferralRequestBalance;
					if (d < 0m)
					{
						d = 0.01m;
					}
					this.UpClassInfo.InnerHtml = string.Concat(new string[]
					{
						"再获得<span> ",
						d.ToString("F2"),
						" 元</span>",
						commissionName,
						"升级为 <span>",
						distributorGradeInfo2.Name,
						"</span>"
					});
				}
			}
		}
	}
}
