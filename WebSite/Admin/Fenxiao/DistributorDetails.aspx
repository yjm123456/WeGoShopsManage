<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.ControlPanel.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .yejiItem {
            text-align: center;
            line-height: 30px;
            float: left;
            margin: 5px;
            padding: 10px 40px;
            border-left: 1px solid #b9caca;
        }

            .yejiItem:first-child {
                border-left: 0px;
            }

            .yejiItem .money {
                color: #125acb;
                font-weight: bold;
                font-size: 18px;
            }

            .yejiItem .yejitxt {
                color: #444451;
                font-weight: bold;
                font-size: 18px;
            }

        .infodiv {
            float: left;
        }

        .infosdetail {
            width: 250px;
            margin-left: 10px;
            line-height: 23px;
        }

        .infostitle {
            width: 90px;
            text-align: center;
        }

        .infosdetail ul label {
            width: 90px;
            text-align: right;
            margin-right: 10px;
            font-weight: normal;
        }

        .infosdetailLong {
            width: 500px;
        }
    </style>
    <script src="../js/ZeroClipboard.min.js"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>分销商详情</h2>
    </div>

    <form runat="server">

        <!--基本信息-->
        <h3 class="templateTitle">基本信息</h3>
        <div class="set-switch clearfix">
            <div class="infodiv">
                <div class="qrCode">
                    <Hi:HiImage ID="ListImage1" ImageUrl="/Templates/common/images/user.png" runat="server" Width="90" Height="90" />
                </div>
                <div class="infostitle">个人头像</div>
            </div>

            <div class="infodiv infosdetail">
                <ul>
                    <li>
                        <label>联系人：</label><span id="txtRealName" runat="server">-</span></li>
                    <li>
                        <label>手机号码：</label><span id="txtCellPhone" runat="server">-</span></li>
                    <li>
                        <label>用户名：</label><span id="txtUserBindName" runat="server">-</span></li>
                    <li>
                        <label>微信昵称：</label><span id="txtMicroName" runat="server">-</span></li>
                </ul>
            </div>

            <div class="infodiv">
                <div class="qrCode">
                    <Hi:HiImage ID="StoreCode" runat="server" Width="90" Height="90" />
                </div>
                <div class="infostitle">店铺二维码</div>
            </div>


            <div class="infodiv infosdetail infosdetailLong">
                <ul>
                    <li>
                        <a id="copybutton" data-clipboard-target="ctl00_ContentPlaceHolder1_txtUrl" style="cursor: pointer; color: #0f63ac; float: right;">复制链接</a><label>店铺名：</label><span id="txtStoreName" runat="server">-</span> </li>
                    <li>
                        <label>分销商等级：</label><span id="txtName" runat="server">-</span></li>
                    <li style="overflow: hidden">
                        <label>店铺链接：</label><span id="txtUrl" runat="server" style="width: 300px; overflow-x: hidden; margin: 0px">-</span></li>
                    <li>
                        <label>申请时间：</label><span id="txtCreateTime" runat="server">-</span></li>
                </ul>
            </div>



        </div>


        <!--业绩-->
        <h3 class="templateTitle">业绩</h3>
        <div class="set-switch  clearfix" style="height: 120px">

            <div class="yejiItem">
                <div class="money" id="ReferralOrders" runat="server">0</div>
                <div class="yejitxt">成交单数</div>
            </div>

            <div class="yejiItem">
                <div class="money" id="OrdersTotal" runat="server">￥0</div>
                <div class="yejitxt">销售金额</div>
            </div>

            <div class="yejiItem">
                <div class="money" id="TotalReferral" runat="server">￥0</div>
                <div class="yejitxt">佣金总额</div>
            </div>

            <div class="yejiItem">
                <div class="money" id="ReferralBlance" runat="server">￥0</div>
                <div class="yejitxt">可提现佣金余额</div>
            </div>

            <div class="yejiItem">
                <div class="money" id="ReferralRequestBalance" runat="server">￥0</div>
                <div class="yejitxt">已提现佣金总额</div>
            </div>

        </div>

        <!--团队奖-->
        <h3 class="templateTitle">团队奖</h3>
        <div class="set-switch  clearfix" style="height: 120px">

            <div class="yejiItem">
                <div class="money" id="Div1" runat="server">

                    <label for="templateTitle">筛选时间：</label>
                    <input type="date" id="calendarStartDate" cssclass="form-control resetSize inputw150" />
                    至
                                <input type="date" id="calendarEndDate" cssclass="form-control resetSize inputw150" />
                    &nbsp &nbsp <a onclick="DestributorSelect()" class="btn btn-primary">查询</a>
                </div>
                <div class="yejitxt"></div>
            </div>
            <div class="yejiItem">
                <div class="money" id="Div2" runat="server">¥&nbsp<span id="TeamMoney" runat="server">0</span></div>
                <div class="yejitxt">团队奖总额</div>
            </div>
        </div>
        <!--数据列表-->
        <h3 class="templateTitle">提现记录</h3>

        <div>
            <table class="table table-hover mar table-bordered" style="table-layout: fixed">
                <thead>
                    <tr>
                        <th width="120">申请时间</th>
                        <th width="120">提现金额</th>
                        <th width="100">帐号类型</th>
                        <th width="100">帐号</th>
                        <th width="100">收款人</th>
                        <th width="120">支付日期</th>
                    </tr>
                </thead>
                <tbody>

                    <asp:Repeater ID="reCommissions" runat="server">
                        <ItemTemplate>
                            <tr class="td_bg">
                                <td width="200">&nbsp; <%# Eval("RequestTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td>&nbsp;￥<%# Eval("Amount", "{0:F2}")%></td>
                                <td>&nbsp;<%# VShopHelper.GetCommissionPayType(Eval("RequestType").ToString())%>
                                </td>
                                <td>&nbsp;<%# Eval("MerchantCode") %></td>
                                <td>&nbsp;<%# Eval("AccountName") %></td>
                                <td>&nbsp;<%# Eval("CheckTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>



        <!--数据列表底部功能区域-->
        <br />
        <div class="select-page clearfix">
            <div class="form-horizontal fl">
                <a onclick="javascript:history.go(-1)" class="btn btn-primary">返回</a>
            </div>
            <div class="page fr">
                <div class="pageNumber">
                    <div class="pagination" style="margin: 0px">
                        <UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="5" ID="pager" />
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix" style="height: 30px"></div>


    </form>
     <script language="javascript">

        function DestributorSelect() {
            var mydate = new Date();
            var nowDate = mydate.getFullYear() + "-" + (mydate.getMonth() + 1) + "-" + mydate.getDate();


            var startDate = document.getElementById("calendarStartDate").value;
            var endDate = document.getElementById("calendarEndDate").value;
            if (startDate == "" || startDate == null) {
                alert("请选择开始时间！");
                return false;
            } if (endDate == "" || endDate == null) {
                alert("请选择结束时间！");
                return false;
            }
            var startNum = parseInt(startDate.replace(/-/g, ''), 10);
            var endNum = parseInt(endDate.replace(/-/g, ''), 10);
            var nowNum = parseInt(nowDate.replace(/-/g, ''), 10);
            if (startNum > endNum) {
                alert("结束时期需比开始时期大，请重新选择！");
                return false;
            };
            //DistributorsBrower.GetTeamMoneysBrowser(userIdDistributors.UserId, ss, Convert.ToDateTime("2000-01-01"))
            GetTeamMoney(startDate,endDate);
          
        }
        //获取团队奖数据
        function GetTeamMoney(startTime,endTime)
        {
            var userid = document.getElementById("ctl00_ContentPlaceHolder1_txtUrl").innerHTML;
            userid = userid.split('=')[1];
            $.ajax({
                url: "DistributorDetails.aspx?UserId=" + userid + "&StartTime=" + startTime + "&EndTime=" + endTime,
                type: "post",
                data: {
                },
                datatype: "json",
                success: function (json) {
                    var Money = json.split('<')[0];
                    Money = Money.replace(/[\r\n]/g, "")
                    document.getElementById("ctl00_ContentPlaceHolder1_TeamMoney").innerText =Money;
                }
            });
        }

        $(function () {
            //得到当前时间
            var date_now = new Date();
            //得到当前年份
            var year = date_now.getFullYear();
            //得到当前月份
            //注：
            //  1：js中获取Date中的month时，会比当前月份少一个月，所以这里需要先加一
            //  2: 判断当前月份是否小于10，如果小于，那么就在月份的前面加一个 '0' ， 如果大于，就显示当前月份
            var month = date_now.getMonth() + 1 < 10 ? "0" + (date_now.getMonth() + 1) : (date_now.getMonth() + 1);
            //得到当前日子（多少号）
            var date = date_now.getDate() < 10 ? "0" + date_now.getDate() : date_now.getDate();
            //设置input标签的max属性
            $("#calendarStartDate").attr("max", year + "-" + month + "-" + date);
            $("#calendarEndDate").attr("max", year + "-" + month + "-" + date);
        })
        $(function () {

            var copy = new ZeroClipboard($("#copybutton")[0], {
                moviePath: "../js/ZeroClipboard.swf"
            });

            copy.on('complete', function (client, args) {
                HiTipsShow("已复制:" + args.text, 'success');
            });

        });
    </script>
</asp:Content>
