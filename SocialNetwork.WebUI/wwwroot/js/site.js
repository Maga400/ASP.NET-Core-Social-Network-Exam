﻿

function GetAllUsers()
{
    $.ajax({
        url: "/Home/GetAllUsers",
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
                    <div class="card" style="${style};width:220px;margin:5px;">

                        <img style="width:100%;height:220px;" src="/images/${data[i].image}" />
                        <div class="card-body">
                            <h5 class="card-title">${data[i].userName}</5>
                            <p class="card-text">${data[i].email} </p>
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