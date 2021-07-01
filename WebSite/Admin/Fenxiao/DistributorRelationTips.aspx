<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorRelationTips.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorRelationTips" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        em {
            color: #FF6600;
        }
    </style>
    <link href="../themes/bootstrap/easyui.css" rel="stylesheet" />
    <link href="../themes/icon.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/easyui-lang-zh_CN.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script>
        function resetform() {
            document.getElementById("aspnetForm").reset();
        }

        $(function () {

            $('.allselect').change(function () {
                $('.content-table input[type="checkbox"]').prop('checked', $(this)[0].checked);
            });

            var tableTitle = $('.title-table').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= tableTitle) {
                    $('.title-table').css({
                        position: 'fixed',
                        top: '58px'
                    })
                }
                if ($(document).scrollTop() + $('.title-table').height() + 58 <= tableTitle) {
                    $('.title-table').removeAttr('style');
                }
            });



        })
        //$(function () {
        //    $('#test').treegrid({
        //        url: 'AjaxDistributorRelationTips.aspx?UserID=325',
        //        idField: 'id',
        //        treeField: 'text',
        //        rownumbers: true,
        //        columns: [[
        //                    { field: 'text', title: '店铺名称', width: 250 },
        //                    { field: 'CreateDate', title: '注册时间', width: 50 },
        //                    { field: 'LastOrderDate', title: '近期消费', width: 150 }
        //        ]],
        //    });
        //})

        function Select() {
            var UserName = document.getElementById("txtStoreName").value;
            $.ajax({
                url: "AjaxDistributorRelationTips.aspx?UserName=" + UserName,
                dataType:'json',
                success: function (data) {
                    if (data != "0")
                    {
                        $('#test').treegrid({
                            data: data,
                            idField: 'id',
                            treeField: 'text',
                            rownumbers: true,
                            columns: [[
                                        { field: 'text', title: '店铺名称', width: 250 },
                                        { field: 'CreateDate', title: '注册时间', width: 50 },
                                        { field: 'LastOrderDate', title: '近期消费', width: 150 }
                            ]],
                        });
                    }
                }
            })
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>分销商管理</h2>
    </div>
    <form runat="server">
        <div id="mytabl">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs">
                    <li><a href="DistributorList.aspx">
                        <asp:Literal ID="ListActive" Text="分销商列表(10)" runat="server"></asp:Literal></a></li>
                    <li><a href="DistributorFrozenList.aspx">
                        <asp:Literal ID="Listfrozen" Text="已冻结(0)" runat="server"></asp:Literal></a></li>
                    <li class="active"><a href="#home">
                        <asp:Literal ID="LiteralRelation" Text="分销商关系" runat="server"></asp:Literal></a></li>
                </ul>

            </div>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">

                    <div class="set-switch">
                        <div class="form-inline mb10">
                            <div class="form-group mr20">
                                <label for="sellshop1">店铺名：</label>
                                <%--<asp:TextBox ID="txtStoreName" CssClass="form-control resetSize inputw150" runat="server" />--%>
                                <input type="text" class="form-control resetSize inputw150" id="txtStoreName" />
                            </div>
                            <%--    <div class="form-group mr20">
                                <label for="sellshop5">　　昵称：</label>
                                <asp:TextBox ID="txtUserName" CssClass="form-control  resetSize  inputw150" runat="server" autocomplete="off" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop2">　　联系人：</label>
                                <asp:TextBox ID="txtRealName" CssClass="form-control  resetSize" runat="server"
                                    Width="150" />
                            </div>
                            <div class="form-group ">
                                <label for="txtStoreName">所属上级：</label>
                                <asp:TextBox ID="txtStoreName1" CssClass="form-control  resetSize  inputw150" placeholder="输入店铺名称" runat="server" />
                            </div>--%>
                            <div class="form-group" style="margin-left: 20px">
                                <input type="button" onclick="Select()" id="Selects" class="btn btn-primary  resetSize " value="查询" />
                                <%--<asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="btn btn-primary  resetSize " />--%>
                            </div>
                        </div>

                    </div>
                    <div class="select-page clearfix" style="margin-top: 20px;">
                    </div>
                    <div class="sell-table">
                        <div class="title-table">
                        </div>
                          <table id="test" title="分销商所有下级列表" class="easyui-treegrid" style="width:400px;height:300px">
                         <%--<table id="test" title="分销商关系列表" class="easyui-treegrid" style="width: 1046px; height: 300px"
                                url="AjaxDistributorRelationTips.aspx?UserID=325"
                                rownumbers="true"
                                idfield="ParentId" treefield="text">
                                <thead>
                                    <tr>
                                        <th field="text" width="600">店铺名称</th>
                                        <th field="CreateDate" width="123" align="right">注册时间</th>
                                        <th field="LastOrderDate" width="123">近期消费</th>
                                    </tr>
                                </thead>--%>
                            </table>
                        <%--<div class="content-table">
                           
                        </div>--%>
                    </div>

                </div>
                <div class="tab-pane"></div>
            </div>
        </div>

    </form>
</asp:Content>
