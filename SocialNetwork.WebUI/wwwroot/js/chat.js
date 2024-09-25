﻿"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.start().then(function () {
    GetAllUsers();
    GetAllUsersLayout();
})
    .catch(function (err) {
        return console.error(err.toString());
    })

const element = document.querySelector("#alert");
element.style.display = "none";


connection.on("Connect", function (info) {
    GetAllUsers();
    GetAllUsersLayout();
    element.style.display = "block";
    element.innerHTML = info;
    setTimeout(() => {
        element.innerHTML = "";
        element.style.display = "none";
    }, 5000);
})

connection.on("Disconnect", function (info) {
    GetAllUsers();
    GetAllUsersLayout();
    element.style.display = "block";
    element.innerHTML = info;
    setTimeout(() => {
        element.innerHTML = "";
        element.style.display = "none";
    }, 5000);
})

async function SendFollowCall(id) {
    await connection.invoke("SendFollow",id);
}

connection.on("ReceiveNotification", function () {
    GetMyRequests();
    GetAllUsers();
    GetAllFriends();
})