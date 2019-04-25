"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/wechatHub")
    .build();

connection.on("RecvAsync", function (data) {
    var li = document.createElement("li");
    li = $(li).text(data.userName + "：" + data.content);
    $("#msgList").append(li);
});

connection.on("HeartbeatAsync", (data) => {
    console.log(data);
});

connection.start()
    .then(function () {
        console.log("客户端已连接");
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
});