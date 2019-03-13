"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/wechatHub")
    .build();

connection.on("Recv", function (data) {
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

$(document).ready(function () {
    $("#btnSend").on("click", () => {
        var userName = $("#userName").val();
        var content = $("#content").val();
        console.log(userName + ":" + content);
        connection.invoke("send", { "Type": 0, "UserName": userName, "Content": content });
    });
});