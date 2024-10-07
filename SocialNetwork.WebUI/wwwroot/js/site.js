
function GetAllUsers() {
    $.ajax({
        url: "/Home/GetAllUsers",
        method: "GET",
        success: function (data) {
            let content = "";
            for (var i = 0; i < data.length; i++) {

                let style = '';
                let subContent = '';
                if (data[i].hasRequestPending) {
                    subContent = `<button class="btn btn-outline-secondary" onclick="TakeRequest('${data[i].id}')">Already Sent</button>`
                }
                else {
                    if (data[i].isFriend) {
                        subContent = `<button class='btn btn-outline-secondary' onclick="UnfollowUser('${data[i].id}')" >UnFollow</button>`
                    }
                    else {
                        subContent = `<button onclick="SendFollow('${data[i].id}')" class='btn btn-outline-primary'>Follow</button>`

                    }
                }

                if (data[i].isOnline) {
                    style = 'border: 5px solid springgreen';
                }
                else {
                    style = "border: 5px solid red";

                }

                const item = `
                    <div class="card" style="${style};width:300px;margin-top:50px;margin-right:30px">

                        <img style="width:100%;height:250px;" src="/images/${data[i].image}" />
                        <div class="card-body">
                            <h5 class="card-title">${data[i].userName}</5>
                            <p class="card-text">${data[i].email} </p>
                            ${subContent}
                        </div>

                    </div>
                `;
                content += item;

            }
            $("#allUsers").html(content);

        }

    })
}

GetAllUsers();
GetMyRequests();
GetNotifications();
GetMyNotifications();
function GetMessages(receiverId, senderId) {
    $.ajax({
        url: `/Message/GetAllMessages?receiverId=${receiverId}&senderId=${senderId}`,
        method: "GET",
        success: function (data) {
            let content = "";
            for (var i = 0; i < data.messages.length; i++) {
                let dateTime = new Date(data.messages[i].dateTime);
                let hour = dateTime.getHours();
                let minute = dateTime.getMinutes();
                let item = '';
                if (data.messages[i].receiverId == data.currentUserId) {
                    item = `<section style="display:flex;margin-top:25px;border:2px solid black;
    margin-left:10px;border-radius:10px;background-color:lightgrey;min-width:20%;max-width:50%;">

                                            <h5 style="margin-left:10px;margin-top:15px;margin-right:10px;font-size:1em;">${data.messages[i].content}</h5>
                                            <p style="margin-top:20px;margin-right:10px;font-size:0.9em">${hour}:${minute}</p>
                                        
                                        </section>`;

                }
                else {
                    item = `<section style="display:flex;margin-top:25px;border:2px solid black;
    margin-left:50%;border-radius:10px;background-color:blue;min-width:20%;max-width:50%;">

                                            <h5 style="margin-left:10px;margin-top:15px;margin-right:10px;font-size:1em;color:white">${data.messages[i].content}</h5>
                                            <p style="margin-top:20px;margin-right:10px;font-size:0.9em;color:white">${hour}:${minute}</p>
                                        
                                        </section>`;
                }
                content += item;
            }
            console.log(data);
            $("#currentMessages").html(content);
        }
    })
}

function SendMessage(receiverId, senderId) {
    const content = document.querySelector("#message-input");
    let obj = {
        receiverId: receiverId,
        senderId: senderId,
        content: content.value
    };

    $.ajax({
        url: `/Message/AddMessage`,
        method: "POST",
        data: obj,
        success: function (data) {
            GetMessageCall(receiverId, senderId);
            content.value = "";
            var audio = document.querySelector("#mySound");
            audio.play();
        }
    })
}

function GetAllFriends() {
    $.ajax({
        url: "/Friends/GetAllFriends",
        method: "GET",
        success: function (data) {
            let content = "";
            for (var i = 0; i < data.length; i++) {

                let style = '';

                if (data[i].isOnline) {
                    style = 'border: 5px solid springgreen';
                }
                else {
                    style = "border: 5px solid red";

                }

                const item = `
                    <div class="card" style="${style};width:300px;margin:5px;">

                        <img style="width:100%;height:180px;" src="/images/${data[i].image}" />
                        <div class="card-body">
                            <h5 class="card-title">${data[i].userName}</5>
                            <p class="card-text">${data[i].email} </p>
                            <div style='display:flex;justify-content:center;'>
                                <button class='btn btn-outline-secondary' style="width:45%;height:30px;font-size:0.6em;" onclick="UnfollowUser('${data[i].id}')" >UnFollow</button>
                                <a class='btn btn-outline-primary' style="width:45%;height:30px;font-size:0.6em;margin-left:10%;" href='/Message/GoChat/${data[i].id}' >Send Message</a>
                            
                            </div>
                        </div>

                    </div>
                `;
                content += item;

            }
            $("#allFriends").html(content);

        }
    })
}

GetAllFriends();
function TakeRequest(id) {
    const element = document.querySelector("#alert");
    element.style.display = "none";
    $.ajax({
        url: `/Home/TakeRequest?id=${id}`,
        method: "DELETE",
        success: function (data) {
            element.style.display = "block";
            element.innerHTML = "You take your request successfully";
            SendFollowCall(id);
            GetAllUsers();
            setTimeout(() => {
                element.innerHTML = "";
                element.style.display = "none";
            }, 5000);
        }
    })
}

function SendFollow(id) {
    const element = document.querySelector("#alert");
    element.style.display = "none";
    $.ajax({
        url: `/Home/SendFollow/${id}`,
        method: "GET",
        success: function (data) {
            element.style.display = "block";
            element.innerHTML = "Your friend request sent successfully";
            SendFollowCall(id);
            GetAllUsers();
            setTimeout(() => {
                element.innerHTML = "";
                element.style.display = "none";
            }, 5000);
        }
    })
}


function SharePost(e) {
    e.preventDefault();
    var textArea = document.querySelector("#textArea");
    $.ajax({
        url: `/Home/SharePost?text=${textArea.value}`,
        method: "GET",
        success: function (data) {
            textArea.value = "";
            GetAllPosts();
            SharePostCall();

        }
    })
}

function GetMyRequests() {
    $.ajax({
        url: "/Home/GetAllRequests",
        method: "GET",
        success: function (data) {
            let content = '';
            let subContent = '';
            for (let i = 0; i < data.length; i++) {
                if (data[i].status == "Request") {
                    subContent = `
                    <div class="card-body" style="display:flex;justify-content:start;">
                        <button style="width:25%;font-size:1em;" class="btn btn-success" onclick="AcceptRequest('${data[i].senderId}','${data[i].receiverId}',${data[i].id})" >Accept</button>
                        <button style="width:25%;margin-left:10%;font-size:1em;" class="btn btn-secondary" onclick="DeclineRequest(${data[i].id},'${data[i].senderId}')">Decline</button>
                    </div>`;
                }
                else {
                    subContent = `
                    <div class="card-body">
                        <button class="btn btn-warning" onclick="DeleteRequest(${data[i].id})">Delete</button>
                    </div>`;
                }

                let item = `
                <div class="card" style="width:100%;background-color:lightgrey;margin-top:50px;">
                    <div class="card-body">
                        <h5 style="color:red;">${data[i].status}</h5>
                        <ul class="list-group list-group-flush">
                            <li style="font-size:1em;list-style:none;">${data[i].content}</li>
                        </ul>
                        ${subContent}
                    </div>
                </div>`;

                content += item;
            }
            $("#requests").html(content);
        }
    });
}

function GetMyNotifications() {
    $.ajax({
        url: "/Home/GetMyNotifications",
        method: "GET",
        success: function (data) {
            let content = '';
            let subContent = '';
            for (let i = 0; i < data.length; i++) {

                subContent = `
                    <div class="card-body">
                        <button class="btn btn-warning" onclick="DeleteNotification(${data[i].id})">Delete</button>
                    </div>`;

                let item = `
                <div class="card" style="width:100%;background-color:lightgrey;margin-top:50px;">
                    <div class="card-body">
                        <h5 style="color:red;">${data[i].status}</h5>
                        <ul class="list-group list-group-flush">
                            <li style="font-size:1em;list-style:none;">${data[i].content}</li>
                        </ul>
                        ${subContent}
                    </div>
                </div>`;

                content += item;
            }
            $("#myNotifications").html(content);
        }
    });
}
function GetNotifications() {
    $.ajax({
        url: "/Home/GetAllNotification",
        method: "GET",
        success: function (data) {
            let content = '';
            let subContent = '';
            for (let i = 0; i < data.notifications.length; i++) {
                if (data.currentId != data.notifications[i].userId) {
                    let item = `
                    <div class="card" style="width:100%;background-color:lightgrey;margin-top:50px;">
                        <div class="card-body">
                            <h5 style="color:red;">${data.notifications[i].status}</h5>
                            <ul class="list-group list-group-flush">
                                <li style="font-size:1em;list-style:none;">${data.notifications[i].content}</li>
                            </ul>
                        
                        </div>
                    </div>`;
                    content += item;

                }
            }
            $("#notifications").html(content);
        }
    });
}

function SendComment(id, e,senderId) {
    e.preventDefault();

    let textArea = document.querySelector(`#message${id}`).value;

    $.ajax({
        url: `/Home/SendComment?id=${id}&message=${textArea}&senderId=${senderId}`,
        method: "GET",
        success: function (data) {
            //SharePostCall();
            SendNotificationCall(senderId);
            GetAllPosts();
            GetMyPosts();
            GetAllPostsCall();
        }
    });
}

function SendLike(id,senderId) {

    $.ajax({
        url: `/Home/SendLike?id=${id}&currentId=${senderId}`,
        method: "GET",
        success: function (data) {
            //SharePostCall();
            SendNotificationCall(senderId);
            GetAllPosts();
            GetMyPosts();
            GetAllPostsCall();
        }
    });
}

function SendCommentLike(id,senderId) {

    $.ajax({
        url: `/Home/SendCommentLike?id=${id}&senderId=${senderId}`,
        method: "GET",
        success: function (data) {
            //SharePostCall();
            SendNotificationCall(senderId);
            GetAllPosts();
            GetMyPosts();
            GetAllPostsCall();
        }
    });
}

function GetAllPosts() {
    $.ajax({
        url: "/NewsFeed/GetAllPosts",
        method: "GET",
        success: function (data) {
            let content = '';

            for (let i = 0; i < data.posts.length; i++) {

                //subContent = `
                //    <div class="card-body">
                //        <button class="btn btn-warning" onclick="DeleteRequest(${data[i].id})">Delete</button>
                //    </div>`;
                let subContent = '';
                let likeContent = '<i class="fa-regular fa-thumbs-up"></i>';


                if (data.currentId != data.posts[i].senderId) {

                    if (data.posts[i].comments.length != 0) {
                        for (var j = 0; j < data.posts[i].comments.length; j++) {
                            let likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                            var comment = data.posts[i].comments[j];
                            let dateTime = new Date(comment.writingDate);
                            let year = dateTime.getFullYear();
                            let month = dateTime.getMonth() + 1;
                            let day = dateTime.getDate();
                            let hours = dateTime.getHours();
                            let minutes = dateTime.getMinutes();

                            if (data.likedComments.length != 0) {

                                for (var n = 0; n < data.likedComments.length; n++) {
                                    if (data.likedComments[n].userId == data.currentId && data.likedComments[n].commentId == comment.id) {
                                        likeContent2 = '<i class="fa-solid fa-thumbs-up"></i>';
                                        break;
                                    }

                                    likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                                }
                            }

                            subContent += `
                            <div class="post-comment-list">
                                <div class="comment-list" style="display:flex;justify-content:start;width:100%;padding:30px">
                                    <div class="comment-image" style="width:5%;">
                                        <a href="my-profile.html"><img style="height:50px;width:50px;" src="/images/${comment.sender.image}" class="rounded-circle" alt="image"></a>
                                    </div>
                                    <div class="comment-info" style="width:85%;margin-left:3%;">
                                        <h3>
                                            <a href="my-profile.html">${comment.sender.userName}</a>
                                        </h3>
                                        <span>${year}-${month}-${day} ${hours}:${minutes}</span>
                                        <p>${comment.content}</p>
                                        
                                    </div>
                                    <div class="post-react" onclick="SendCommentLike(${comment.id},'${comment.senderId}')">
                                        <div>${likeContent2}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${comment.likeCount}</span></div>

                                    </div>
                                </div>
                            </div>
                        `;
                        }
                    }

                    if (data.likedPosts.length != 0) {

                        for (var l = 0; l < data.likedPosts.length; l++) {
                            if (data.likedPosts[l].userId == data.currentId && data.likedPosts[l].postId == data.posts[i].id) {
                                likeContent = '<i class="fa-solid fa-thumbs-up"></i>';
                                break;
                            }

                            likeContent = '<i class="fa-regular fa-thumbs-up"></i>';
                        }
                    }



                    let item = `
                    <div class="news-feed news-feed-post" style="background-color:white;margin-top:50px;">
                        <div class="post-header d-flex justify-content-between align-items-center" style="padding:30px;">
                            <div class="image">
                                <a href="#"><img src="/images/${data.posts[i].sender.image}" class="rounded-circle" style="height:150px;width:150px;" alt="image"></a>
                            </div>
                            <div class="info ms-3">
                                <span class="name" style="display:block;"><a href="#" style="font-weight:bold;font-size:1.5em">${data.posts[i].sender.userName}</a></span>
                                <span class="small-text" style="display:block;margin-top:15px;"><a href="#">${data.posts[i].shareDate}</a></span>
                            </div>
                            <div class="dropdown">
                                <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="flaticon-menu"></i></button>
                                <ul class="dropdown-menu">
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-edit"></i> Edit Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-private"></i> Hide Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-trash"></i> Delete Post</a></li>
                                </ul>
                            </div>
                        </div>

                        <div class="post-body">
                            <p style="font-size:1.2em;font-weight:bold;margin:30px;">${data.posts[i].text}</p>
                            
                            
                            <ul class="post-meta-wrap d-flex justify-content-between align-items-center" style="padding:30px;list-style:none;">
                                <li class="post-react" onclick="SendLike(${data.posts[i].id},'${data.posts[i].senderId}')">
                                    <div>${likeContent}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${data.posts[i].likeCount}</span></div>

                                </li>
                                <li class="post-comment">
                                    <div><i class="flaticon-comment"></i><span style="margin-left:5px;">Comment</span> <span style="margin-left:5px;" class="number">${data.posts[i].commentCount}</span></div>
                                </li>
                                <li class="post-share">
                                    <div><i class="flaticon-share"></i><span style="margin-left:5px;">Share</span> <span style="margin-left:5px;" class="number">0</span></div>
                                </li>
                            </ul>

                            ${subContent}

                            <form class="post-footer" style="display:flex;justify-content:start;width:100%;padding:30px;">
                                <div class="footer-image">
                                    <a href="#"><img src="/images/${data.currentImage}" class="rounded-circle" style="width:50px;height:50px;" alt="image"></a>
                                </div>
                                <div class="form-group" style="display:flex;justify-content:start;width:98%;margin-left:3%">
                                    <textarea style="width:80%%;" id="message${data.posts[i].id}" name="message" class="form-control" placeholder="Write a comment..."></textarea>
                      
                                    <button type="submit" style="width:15%;background-color:red;color:white;font-size:1em;margin-left:20px;" onclick="SendComment(${data.posts[i].id},event,'${data.posts[i].senderId}')">Send</button>
                                </div>
                            </form>
                        </div>
                    </div>`;
                    content += item;
                    //317 share image part
                    //<div class="post-image">
                    //    <img src="assets/images/news-feed-post/post-4.jpg" alt="image">
                    //</div>

                }

            }

            $("#posts").html(content);
        }
    });
}

function GetAllPosts2() {
    $.ajax({
        url: "/NewsFeed/GetAllPosts",
        method: "GET",
        success: function (data) {
            let content = '';

            for (let i = 0; i < data.posts.length; i++) {

                //subContent = `
                //    <div class="card-body">
                //        <button class="btn btn-warning" onclick="DeleteRequest(${data[i].id})">Delete</button>
                //    </div>`;
                let subContent = '';
                let likeContent = '<i class="fa-regular fa-thumbs-up"></i>';

                if (data.currentId != data.posts[i].senderId) {

                    if (data.posts[i].comments.length != 0) {
                        for (var j = 0; j < data.posts[i].comments.length; j++) {
                            let likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                            var comment = data.posts[i].comments[j];
                            let dateTime = new Date(comment.writingDate);
                            let year = dateTime.getFullYear();
                            let month = dateTime.getMonth() + 1;
                            let day = dateTime.getDate();
                            let hours = dateTime.getHours();
                            let minutes = dateTime.getMinutes();

                            if (data.likedComments.length != 0) {

                                for (var l = 0; l < data.likedComments.length; l++) {
                                    if (data.likedComments[l].userId == data.currentId && data.likedComments[l].commentId == comment.id) {
                                        likeContent2 = '<i class="fa-solid fa-thumbs-up"></i>';
                                        break;
                                    }

                                    likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                                }
                            }

                            subContent += `
                            <div class="post-comment-list">
                                <div class="comment-list" style="display:flex;justify-content:start;width:100%;padding:30px">
                                    <div class="comment-image" style="width:5%;">
                                        <a href="my-profile.html"><img style="height:50px;width:50px;" src="/images/${comment.sender.image}" class="rounded-circle" alt="image"></a>
                                    </div>
                                    <div class="comment-info" style="width:85%;margin-left:3%;">
                                        <h3>
                                            <a href="my-profile.html">${comment.sender.userName}</a>
                                        </h3>
                                        <span>${year}-${month}-${day} ${hours}:${minutes}</span>
                                        <p>${comment.content}</p>
                                        
                                    </div>
                                    <div class="post-react" onclick="SendCommentLike(${comment.id},'${comment.senderId}')">
                                        <div>${likeContent2}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${comment.likeCount}</span></div>

                                    </div>
                                </div>
                            </div>
                        `;
                        }
                    }

                    if (data.likedPosts.length != 0) {

                        for (var l = 0; l < data.likedPosts.length; l++) {
                            if (data.likedPosts[l].userId == data.currentId && data.likedPosts[l].postId == data.posts[i].id) {
                                likeContent = '<i class="fa-solid fa-thumbs-up"></i>';
                                break;
                            }

                            likeContent = '<i class="fa-regular fa-thumbs-up"></i>';
                        }
                    }

                    let item = `
                    <div class="news-feed news-feed-post" style="background-color:white;margin-top:50px;">
                        <div class="post-header d-flex justify-content-between align-items-center" style="padding:30px;">
                            <div class="image">
                                <a href="#"><img src="/images/${data.posts[i].sender.image}" class="rounded-circle" style="height:150px;width:150px;" alt="image"></a>
                            </div>
                            <div class="info ms-3">
                                <span class="name" style="display:block;"><a href="#" style="font-weight:bold;font-size:1.5em">${data.posts[i].sender.userName}</a></span>
                                <span class="small-text" style="display:block;margin-top:15px;"><a href="#">${data.posts[i].shareDate}</a></span>
                            </div>
                            <div class="dropdown">
                                <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="flaticon-menu"></i></button>
                                <ul class="dropdown-menu">
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-edit"></i> Edit Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-private"></i> Hide Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-trash"></i> Delete Post</a></li>
                                </ul>
                            </div>
                        </div>

                        <div class="post-body">
                            <p style="font-size:1.2em;font-weight:bold;margin:30px;">${data.posts[i].text}</p>
                            
                            
                            <ul class="post-meta-wrap d-flex justify-content-between align-items-center" style="padding:30px;list-style:none;">
                                <li class="post-react" onclick="SendLike(${data.posts[i].id},'${data.posts[i].senderId}')">
                                    <div>${likeContent}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${data.posts[i].likeCount}</span></div>

                                </li>
                                <li class="post-comment">
                                    <div><i class="flaticon-comment"></i><span style="margin-left:5px;">Comment</span> <span style="margin-left:5px;" class="number">${data.posts[i].commentCount}</span></div>
                                </li>
                                <li class="post-share">
                                    <div><i class="flaticon-share"></i><span style="margin-left:5px;">Share</span> <span style="margin-left:5px;" class="number">0</span></div>
                                </li>
                            </ul>

                            ${subContent}

                            <form class="post-footer" style="display:flex;justify-content:start;width:100%;padding:30px;">
                                <div class="footer-image">
                                    <a href="#"><img src="/images/${data.currentImage}" class="rounded-circle" style="width:50px;height:50px;" alt="image"></a>
                                </div>
                                <div class="form-group" style="display:flex;justify-content:start;width:98%;margin-left:3%">
                                    <textarea style="width:80%%;" id="message${data.posts[i].id}" name="message" class="form-control" placeholder="Write a comment..."></textarea>
                      
                                    <button type="submit" style="width:15%;background-color:red;color:white;font-size:1em;margin-left:20px;" onclick="SendComment(${data.posts[i].id},event,'${data.posts[i].senderId}')">Send</button>
                                </div>
                            </form>
                        </div>
                    </div>`;
                    content += item;
                    //317 share image part
                    //<div class="post-image">
                    //    <img src="assets/images/news-feed-post/post-4.jpg" alt="image">
                    //</div>

                }

            }
            var posts = document.querySelector("#posts");
            posts.style = "display:block;";

            var addPost = document.querySelector("#newPost");
            addPost.style = "display:none;";

            var myPosts = document.querySelector("#myPosts");
            myPosts.style = "display:none;";

            $("#posts").html(content);
        }
    });
}

function AddNewPost() {
    var newPost = document.querySelector("#newPost");
    newPost.style = "display:block;margin-top:50px;margin-left:25%;";
    var posts = document.querySelector("#posts");
    posts.style = "display:none;";
    var myPosts = document.querySelector("#myPosts");
    myPosts.style = "display:none;";
}

function GetMyPosts() {
    $.ajax({
        url: "/NewsFeed/GetMyPosts",
        method: "GET",
        success: function (data) {
            let content = '';

            for (let i = 0; i < data.posts.length; i++) {

                let subContent = '';
                let likeContent = '<i class="fa-regular fa-thumbs-up"></i>';

                if (data.posts[i].comments.length != 0) {
                    for (var j = 0; j < data.posts[i].comments.length; j++) {
                        let likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                        var comment = data.posts[i].comments[j];
                        let dateTime = new Date(comment.writingDate);
                        let year = dateTime.getFullYear();
                        let month = dateTime.getMonth() + 1;
                        let day = dateTime.getDate();
                        let hours = dateTime.getHours();
                        let minutes = dateTime.getMinutes();

                        if (data.likedComments.length != 0) {

                            for (var l = 0; l < data.likedComments.length; l++) {
                                if (data.likedComments[l].userId == data.currentId && data.likedComments[l].commentId == comment.id) {
                                    likeContent2 = '<i class="fa-solid fa-thumbs-up"></i>';
                                    break;
                                }

                                likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                            }
                        }

                        subContent += `
                            <div class="post-comment-list">
                                <div class="comment-list" style="display:flex;justify-content:start;width:100%;padding:30px">
                                    <div class="comment-image" style="width:5%;">
                                        <a href="my-profile.html"><img style="height:50px;width:50px;" src="/images/${comment.sender.image}" class="rounded-circle" alt="image"></a>
                                    </div>
                                    <div class="comment-info" style="width:85%;margin-left:3%;">
                                        <h3>
                                            <a href="my-profile.html">${comment.sender.userName}</a>
                                        </h3>
                                        <span>${year}-${month}-${day} ${hours}:${minutes}</span>
                                        <p>${comment.content}</p>
                                        
                                    </div>
                                    <div class="post-react" onclick="SendCommentLike(${comment.id},'${comment.senderId}')">
                                        <div>${likeContent2}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${comment.likeCount}</span></div>

                                    </div>
                                </div>
                            </div>
                        `;
                    }
                }

                if (data.likedPosts.length != 0) {

                    for (var l = 0; l < data.likedPosts.length; l++) {
                        if (data.likedPosts[l].userId == data.currentId && data.likedPosts[l].postId == data.posts[i].id) {
                            likeContent = '<i class="fa-solid fa-thumbs-up"></i>';
                            break;
                        }

                        likeContent = '<i class="fa-regular fa-thumbs-up"></i>';
                    }
                }

                let item = `
                    <div class="news-feed news-feed-post" style="background-color:white;margin-top:50px;">
                        <div class="post-header d-flex justify-content-between align-items-center" style="padding:30px;">
                            <div class="image">
                                <a href="#"><img src="/images/${data.posts[i].sender.image}" class="rounded-circle" style="height:150px;width:150px;" alt="image"></a>
                            </div>
                            <div class="info ms-3">
                                <span class="name" style="display:block;"><a href="#" style="font-weight:bold;font-size:1.5em">${data.posts[i].sender.userName}</a></span>
                                <span class="small-text" style="display:block;margin-top:15px;"><a href="#">${data.posts[i].shareDate}</a></span>
                            </div>
                            <div class="dropdown">
                                <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="flaticon-menu"></i></button>
                                <ul class="dropdown-menu">
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-edit"></i> Edit Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-private"></i> Hide Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-trash"></i> Delete Post</a></li>
                                </ul>
                            </div>
                        </div>

                        <div class="post-body">
                            <p style="font-size:1.2em;font-weight:bold;margin:30px;">${data.posts[i].text}</p>
                            
                            
                            <ul class="post-meta-wrap d-flex justify-content-between align-items-center" style="padding:30px;list-style:none;">
                                <li class="post-react" onclick="SendLike(${data.posts[i].id})">
                                    <div>${likeContent}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${data.posts[i].likeCount}</span></div>

                                </li>
                                <li class="post-comment">
                                    <div><i class="flaticon-comment"></i><span style="margin-left:5px;">Comment</span> <span style="margin-left:5px;" class="number">${data.posts[i].commentCount}</span></div>
                                </li>
                                <li class="post-share">
                                    <div><i class="flaticon-share"></i><span style="margin-left:5px;">Share</span> <span style="margin-left:5px;" class="number">0</span></div>
                                </li>
                            </ul>

                            ${subContent}

                            
                        </div>
                    </div>`;
                content += item;
                //317 share image part
                //<div class="post-image">
                //    <img src="assets/images/news-feed-post/post-4.jpg" alt="image">
                //</div>



            }
            $("#myPosts").html(content);
        }
    });
}

function GetMyPosts2() {
    $.ajax({
        url: "/NewsFeed/GetMyPosts",
        method: "GET",
        success: function (data) {
            let content = '';

            for (let i = 0; i < data.posts.length; i++) {

                let subContent = '';
                let likeContent = '<i class="fa-regular fa-thumbs-up"></i>';

                if (data.posts[i].comments.length != 0) {
                    for (var j = 0; j < data.posts[i].comments.length; j++) {
                        let likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                        var comment = data.posts[i].comments[j];
                        let dateTime = new Date(comment.writingDate);
                        let year = dateTime.getFullYear();
                        let month = dateTime.getMonth() + 1;
                        let day = dateTime.getDate();
                        let hours = dateTime.getHours();
                        let minutes = dateTime.getMinutes();

                        if (data.likedComments.length != 0) {

                            for (var l = 0; l < data.likedComments.length; l++) {
                                if (data.likedComments[l].userId == data.currentId && data.likedComments[l].commentId == comment.id) {
                                    likeContent2 = '<i class="fa-solid fa-thumbs-up"></i>';
                                    break;
                                }

                                likeContent2 = '<i class="fa-regular fa-thumbs-up"></i>';
                            }
                        }

                        subContent += `
                            <div class="post-comment-list">
                                <div class="comment-list" style="display:flex;justify-content:start;width:100%;padding:30px">
                                    <div class="comment-image" style="width:5%;">
                                        <a href="my-profile.html"><img style="height:50px;width:50px;" src="/images/${comment.sender.image}" class="rounded-circle" alt="image"></a>
                                    </div>
                                    <div class="comment-info" style="width:85%;margin-left:3%;">
                                        <h3>
                                            <a href="my-profile.html">${comment.sender.userName}</a>
                                        </h3>
                                        <span>${year}-${month}-${day} ${hours}:${minutes}</span>
                                        <p>${comment.content}</p>
                                        
                                    </div>
                                    <div class="post-react" onclick="SendCommentLike(${comment.id},'${comment.senderId}')">
                                        <div>${likeContent2}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${comment.likeCount}</span></div>

                                    </div>
                                </div>
                            </div>
                        `;
                    }
                }

                if (data.likedPosts.length != 0) {

                    for (var l = 0; l < data.likedPosts.length; l++) {
                        if (data.likedPosts[l].userId == data.currentId && data.likedPosts[l].postId == data.posts[i].id) {
                            likeContent = '<i class="fa-solid fa-thumbs-up"></i>';
                            break;
                        }

                        likeContent = '<i class="fa-regular fa-thumbs-up"></i>';
                    }
                }

                let item = `
                    <div class="news-feed news-feed-post" style="background-color:white;margin-top:50px;">
                        <div class="post-header d-flex justify-content-between align-items-center" style="padding:30px;">
                            <div class="image">
                                <a href="#"><img src="/images/${data.posts[i].sender.image}" class="rounded-circle" style="height:150px;width:150px;" alt="image"></a>
                            </div>
                            <div class="info ms-3">
                                <span class="name" style="display:block;"><a href="#" style="font-weight:bold;font-size:1.5em">${data.posts[i].sender.userName}</a></span>
                                <span class="small-text" style="display:block;margin-top:15px;"><a href="#">${data.posts[i].shareDate}</a></span>
                            </div>
                            <div class="dropdown">
                                <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="flaticon-menu"></i></button>
                                <ul class="dropdown-menu">
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-edit"></i> Edit Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-private"></i> Hide Post</a></li>
                                    <li><a class="dropdown-item d-flex align-items-center" href="#"><i class="flaticon-trash"></i> Delete Post</a></li>
                                </ul>
                            </div>
                        </div>

                        <div class="post-body">
                            <p style="font-size:1.2em;font-weight:bold;margin:30px;">${data.posts[i].text}</p>
                            
                            
                            <ul class="post-meta-wrap d-flex justify-content-between align-items-center" style="padding:30px;list-style:none;">
                                <li class="post-react" onclick="SendLike(${data.posts[i].id})">
                                    <div>${likeContent}<span style="margin-left:5px;">Like</span> <span style="margin-left:5px;" class="number">${data.posts[i].likeCount}</span></div>

                                </li>
                                <li class="post-comment">
                                    <div><i class="flaticon-comment"></i><span style="margin-left:5px;">Comment</span> <span style="margin-left:5px;" class="number">${data.posts[i].commentCount}</span></div>
                                </li>
                                <li class="post-share">
                                    <div><i class="flaticon-share"></i><span style="margin-left:5px;">Share</span> <span style="margin-left:5px;" class="number">0</span></div>
                                </li>
                            </ul>

                            ${subContent}

                            
                        </div>
                    </div>`;
                content += item;
                //317 share image part
                //<div class="post-image">
                //    <img src="assets/images/news-feed-post/post-4.jpg" alt="image">
                //</div>



            }
            var myPosts = document.querySelector("#myPosts");
            myPosts.style = "display:block;";

            var newPost = document.querySelector("#newPost");
            newPost.style = "display:none;";

            var allPosts = document.querySelector("#posts");
            allPosts.style = "display:none;";

            $("#myPosts").html(content);
        }
    });
}
function DeclineRequest(id, senderId) {
    //window.location.href = '/Notification/Index';

    $.ajax({
        url: `/Home/DeclineRequest?id=${id}&senderId=${senderId}`,
        method: "GET",
        success: function (data) {

            element.innerHTML = "You declined request";

            SendFollowCall(senderId);
            GetAllUsers();
            GetMyRequests();

            setTimeout(() => {
                element.innerHTML = "";
                element.style.display = "none";
            }, 5000);
        }
    });
}

function AcceptRequest(id, id2, requestId) {
    //window.location.href = '/Notification/Index';
    $.ajax({
        url: `/Home/AcceptRequest?userId=${id}&senderId=${id2}&requestId=${requestId}`,
        method: "GET",
        success: function (data) {

            SendFollowCall(id);
            SendFollowCall(id2);
            //GetAllUsers();
            //GetMyRequests();
            //GetAllFriends();
        }
    });
}
function DeleteRequest(id) {
    $.ajax({
        url: `/Home/DeleteRequest/${id}`,
        method: "DELETE",
        success: function (data) {
            GetMyRequests();
            //GetAllNotifications();
        }
    });
}

function DeleteNotification(id) {
    $.ajax({
        url: `/Home/DeleteNotification/${id}`,
        method: "DELETE",
        success: function (data) {
            GetMyNotifications();
            //GetAllNotifications();
        }
    });
}

function UnfollowUser(id) {
    $.ajax({
        url: `/Home/UnfollowUser?id=${id}`,
        method: "DELETE",
        success: function (data) {
            SendFollowCall(id);
            UnFollowUserCall(id);
            GetAllUsers();
            GetAllFriends();
            //window.location.href = '/Message/GoChat';
            //GetAllNotifications();
        }
    });
}


