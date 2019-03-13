"use strict";
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.on("ReceiveMessage", function (user, message) {
    var msg = user + ":" + message;
    var li = document.createElement("li");
    li = $(li).addClass("list-group-item").text(msg);
    $("#messageList").append(li);
});

connection.start().then(function () {
    console.log("SignalR Connected");
}).catch(function (err) {
    console.log("error");
    return console.error(err.toString());
});

$("#btnSend").on("click", function () {
    var user = $("#userName").val();
    var message = $("#message").val();
    console.log(user, message);
    connection.invoke("SendMessage", { "UserName": user, "Content": message }).catch(function (err) {
        return console.error(err.toString());
    });
});