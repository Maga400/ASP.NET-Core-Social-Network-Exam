
function GetAllUsersLayout() {
    $.ajax({
        url: "/Home/GetAllUsersForLayout",
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
                    <div class="card" style="${style};width:100px;height:100px;margin:5px;border-radius:50%">

                        <img style="width:100%;height:100%;border-radius:50%" src="/images/${data[i].image}" />

                    </div>
                `;
                content += item;

            }
            $("#allUsers2").html(content);

        }

    })
}