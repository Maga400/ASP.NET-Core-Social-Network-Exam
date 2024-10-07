"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.start().then(function () {
    GetAllUsers();
    GetAllUsersLayout();
})
    .catch(function (err) {
        return console.error(err.toString());
    })



connection.on("Connect", function (info) {
    GetAllUsers();
    GetAllUsersLayout();
    const element = document.querySelector("#alert");
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
    const element = document.querySelector("#alert");
    element.style.display = "block";
    element.innerHTML = info;
    setTimeout(() => {
        element.innerHTML = "";
        element.style.display = "none";
    }, 5000);
})


connection.on("ReceiveMessages", function (receiverId, senderId) {
    GetMessages(receiverId, senderId);
    var audio = document.querySelector("#mySound2");
    setTimeout(() => {
        audio.play();

    },1500)
})

async function GetMessageCall(receiverId, senderId) {
    await connection.invoke("GetMessages", receiverId, senderId);
}
async function SendFollowCall(id) {
    await connection.invoke("SendFollow",id);
}
async function SendNotificationCall(currentId) {
    await connection.invoke("SendNotification",currentId);
}
async function UnFollowUserCall(id) {
    await connection.invoke("UnFollow", id);
}
async function SharePostCall() {
    await connection.invoke("SharePost");
}
async function GetAllPostsCall() {
    await connection.invoke("GetAllPosts");
}

connection.on("ReceiveAllPosts", function () {
    GetAllPosts();
    GetMyPosts();
    GetNotifications();

})
connection.on("ReceiveUnFollowNotification", function ()
{
    window.location.href = '/Message/GoChat';

})
connection.on("ReceivePostNotification", function () {
    GetNotifications();
    GetAllPosts();
})

connection.on("ReceiveNotification", function () {
    GetMyRequests();
    GetAllUsers();
    GetAllFriends();
    //window.location.href = '/Message/GoChat';

})

connection.on("ReceiveMyNotification", function () {
    GetMyNotifications();
})