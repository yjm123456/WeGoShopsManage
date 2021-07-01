using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMemberCenter : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserName;

		private System.Web.UI.WebControls.Literal litBindUser;

		private System.Web.UI.WebControls.Literal litExpenditure;

		private System.Web.UI.WebControls.Literal litrGradeName;

		private System.Web.UI.WebControls.Literal litPoints;

		private System.Web.UI.WebControls.Literal usePoints;

		private System.Web.UI.WebControls.Literal litCoupon;

		private System.Web.UI.WebControls.Literal litAmount;

		private System.Web.UI.WebControls.Literal fxCenter;

		private System.Web.UI.WebControls.Image image;

		private System.Web.UI.HtmlControls.HtmlContainerControl UpClassInfo;

		private System.Web.UI.HtmlControls.HtmlInputHidden IsSign;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtWaitForstr;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowDis;

		private System.Web.UI.HtmlControls.HtmlInputHidden UserBindName;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberCenter.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("会员中心");
			MemberInfo currentMemberInfo = this.CurrentMemberInfo;
			if (currentMemberInfo == null)
			{
				this.Page.Response.Redirect("/logout.aspx");
			}
			else
			{
				int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
				this.UserBindName = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("UserBindName");
				this.UserBindName.Value = currentMemberInfo.UserBindName;
				this.UpClassInfo = (System.Web.UI.HtmlControls.HtmlContainerControl)this.FindControl("UpClassInfo");
				this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
				this.litPoints = (System.Web.UI.WebControls.Literal)this.FindControl("litPoints");
				this.litPoints.Text = currentMemberInfo.Points.ToString();
				this.image = (System.Web.UI.WebControls.Image)this.FindControl("image");
				this.usePoints = (System.Web.UI.WebControls.Literal)this.FindControl("usePoints");
				this.usePoints.Text = currentMemberInfo.Points.ToString();
				this.litAmount = (System.Web.UI.WebControls.Literal)this.FindControl("litAmount");
				this.litAmount.Text = System.Math.Round(currentMemberInfo.AvailableAmount, 2).ToString();
				MemberCouponsSearch memberCouponsSearch = new MemberCouponsSearch();
				memberCouponsSearch.CouponName = "";
				memberCouponsSearch.Status = "0";
				memberCouponsSearch.MemberId = currentMemberUserId;
				memberCouponsSearch.IsCount = true;
				memberCouponsSearch.PageIndex = 1;
				memberCouponsSearch.PageSize = 10;
				memberCouponsSearch.SortBy = "CouponId";
				memberCouponsSearch.SortOrder = SortAction.Desc;
				int num = 0;
				DataTable memberCoupons = CouponHelper.GetMemberCoupons(memberCouponsSearch, ref num);
				this.litCoupon = (System.Web.UI.WebControls.Literal)this.FindControl("litCoupon");
				this.litCoupon.Text = num.ToString();
				this.litBindUser = (System.Web.UI.WebControls.Literal)this.FindControl("litBindUser");
				this.litExpenditure = (System.Web.UI.WebControls.Literal)this.FindControl("litExpenditure");
				this.litExpenditure.SetWhenIsNotNull("￥" + currentMemberInfo.Expenditure.ToString("F2"));
				if (!string.IsNullOrEmpty(currentMemberInfo.UserBindName))
				{
					this.litBindUser.Text = " style=\"display:none\"";
				}
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMemberInfo.GradeId);
				this.litrGradeName = (System.Web.UI.WebControls.Literal)this.FindControl("litrGradeName");
				if (memberGrade != null)
				{
					this.litrGradeName.Text = memberGrade.Name;
				}
				else
				{
					this.litrGradeName.Text = "普通会员";
				}
				this.litUserName.Text = (string.IsNullOrEmpty(currentMemberInfo.OpenId) ? (string.IsNullOrEmpty(currentMemberInfo.RealName) ? currentMemberInfo.UserName : currentMemberInfo.RealName) : currentMemberInfo.UserName);
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				this.fxCenter = (System.Web.UI.WebControls.Literal)this.FindControl("fxCenter");
				this.fxCenter.Text = masterSettings.DistributorCenterName;
				this.IsSign = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("IsSign");
				if (!masterSettings.sign_score_Enable)
				{
					this.IsSign.Value = "-1";
				}
				else if (!UserSignHelper.IsSign(currentMemberInfo.UserId))
				{
					this.IsSign.Value = "1";
				}
				if (!string.IsNullOrEmpty(currentMemberInfo.UserHead))
				{
					this.image.ImageUrl = currentMemberInfo.UserHead;
				}
				this.txtWaitForstr = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtWaitForstr");
				OrderQuery orderQuery = new OrderQuery();
				orderQuery.Status = OrderStatus.WaitBuyerPay;
				int userOrderCount = MemberProcessor.GetUserOrderCount(currentMemberUserId, orderQuery);
				orderQuery.Status = OrderStatus.SellerAlreadySent;
				int userOrderCount2 = MemberProcessor.GetUserOrderCount(currentMemberUserId, orderQuery);
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
				int userOrderCount3 = MemberProcessor.GetUserOrderCount(currentMemberUserId, orderQuery);
				int waitCommentByUserID = ProductBrowser.GetWaitCommentByUserID(currentMemberUserId);
				int userOrderReturnCount = MemberProcessor.GetUserOrderReturnCount(currentMemberUserId);
				this.txtWaitForstr.Value = string.Concat(new string[]
				{
					userOrderCount.ToString(),
					"|",
					userOrderCount3.ToString(),
					"|",
					userOrderCount2.ToString(),
					"|",
					waitCommentByUserID.ToString(),
					"|",
					userOrderReturnCount.ToString()
				});
				DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
				this.txtShowDis = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowDis");
				if (userIdDistributors == null || userIdDistributors.ReferralStatus != 0)
				{
					this.txtShowDis.Value = "false";
				}
				else
				{
					this.txtShowDis.Value = "true";
                }
                var userid = Globals.GetCurrentMemberUserId();
                var member = MemberHelper.GetMember(userid);
                System.Collections.Generic.IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades(member.TenantID);
				MemberGradeInfo memberGradeInfo = null;
				foreach (MemberGradeInfo current in memberGrades)
				{
                    if (memberGrade == null)
                    {
                        memberGradeInfo = current;
                    }
                    else {
                        double? tranVol = memberGrade.TranVol;
                        double? tranVol2 = current.TranVol;
                        if (tranVol.GetValueOrDefault() < tranVol2.GetValueOrDefault() || !(tranVol.HasValue & tranVol2.HasValue) || !(memberGrade.TranTimes >= current.TranTimes))
                        {
                            tranVol = memberGrade.TranVol;
                            tranVol2 = current.TranVol;
                            if ((tranVol.GetValueOrDefault() < tranVol2.GetValueOrDefault() && (tranVol.HasValue & tranVol2.HasValue)) || memberGrade.TranTimes < current.TranTimes)
                            {
                                if (memberGradeInfo == null)
                                {
                                    memberGradeInfo = current;
                                }
                                else
                                {
                                    tranVol = memberGradeInfo.TranVol;
                                    tranVol2 = current.TranVol;
                                    if ((tranVol.GetValueOrDefault() > tranVol2.GetValueOrDefault() && (tranVol.HasValue & tranVol2.HasValue)) || memberGradeInfo.TranTimes > current.TranTimes)
                                    {
                                        memberGradeInfo = current;
                                    }
                                }
                            }
                        }
                    }
				}
				if (memberGradeInfo == null)
				{
					this.UpClassInfo.Visible = false;
				}
				else
				{
					int num2 = 0;
					if (memberGradeInfo.TranTimes.HasValue)
					{
						num2 = memberGradeInfo.TranTimes.Value - currentMemberInfo.OrderNumber;
					}
					if (num2 <= 0)
					{
						num2 = 1;
					}
					decimal d = 0m;
					if (memberGradeInfo.TranVol.HasValue)
					{
						d = (decimal)memberGradeInfo.TranVol.Value - currentMemberInfo.Expenditure;
					}
					if (d <= 0m)
					{
						d = 0.01m;
					}
					this.UpClassInfo.InnerHtml = string.Concat(new string[]
					{
						"再交易<span>",
						num2.ToString(),
						"次 </span>或消费<span> ",
						System.Math.Round(d + 0.49m, 0).ToString(),
						"元 </span>升级"
					});
				}
			}
		}
	}
}
