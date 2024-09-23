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
    GetAllUsersLayout();
    GetAllUsers();
    element.style.display = "block";
    element.innerHTML = info;
    setTimeout(() => {
        element.innerHTML = "";
        element.style.display = "none";
    }, 5000);
})

connection.on("Disconnect", function (info) {
    GetAllUsersLayout();
    GetAllUsers();
    element.style.display = "block";
    element.innerHTML = info;
    setTimeout(() => {
        element.innerHTML = "";
        element.style.display = "none";
    }, 5000);
})