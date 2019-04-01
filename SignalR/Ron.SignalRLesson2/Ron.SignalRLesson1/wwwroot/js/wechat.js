"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/wechatHub")
    .build();

connection.on("Recv", function (data) {
    debugger;
    console.log(data);
    var li = document.createElement("li");
    li = $(li).text(data.userName + "：" + data.content)
    $("#msgList").append(li);
});

connection.on("官方房间", function (data) {
    debugger;
    console.log(data);
    var li = document.createElement("li");
    li = $(li).text(data.userName + "：" + data.content)
    $("#msgList").append(li);
});

connection.start()
    .then(function () {
        console.log("SignalR 已连接");
    }).catch(function (err) {
        console.log(err);
    });

$.postJSON = function (url, data, callback) {
    return jQuery.ajax({
        'type': 'POST',
        'url': url,
        'contentType': 'application/json',
        'data': JSON.stringify(data),
        'dataType': 'json',
        'success': callback
    });
};

$(document).ready(function () {
    $("#btnSend").on("click", () => {
        var userName = $("#userName").val();
        var content = $("#content").val();
        console.log(userName + ":" + content);
        connection.invoke("send", { "Type": 0, "UserName": userName, "Content": content });
    });

    $("#btnSendToUser").on("click", () => {
        var userName = $("#txtUserName").val();
        var usercontent = $("#txtUserContent").val();
        $.postJSON("/User/SendToUser", { UserName: userName, Content: usercontent }, (data) => {
            console.log(data);
        }, "json");
    });

    $("#btnJoin").on("click", () => {
        var groupName = $("#txtRoom").val();
        $.postJSON("/User/Group-Join", { Name: groupName }, (data) => {
            console.log(data);
        }, "json");
    });

    $("#btnLeave").on("click", () => {
        var groupName = $("#txtRoom").val();
        $.postJSON("/User/Group-Leave", { Name: groupName }, (data) => {
            console.log(data);
        }, "json");
    });

    $("#btnSendToGroup").on("click", () => {
        var groupName = $("#txtRoom").val();
        var content = $("#groupContent").val();
        $.postJSON("/User/SendToGroup", { GroupName: groupName, Content: content }, (data) => {
            console.log(data);
        }, "json");
    });
});