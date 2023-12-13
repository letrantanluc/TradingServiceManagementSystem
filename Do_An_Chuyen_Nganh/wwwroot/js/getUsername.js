"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Tắt nút gửi cho đến khi kết nối được thiết lập.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveUsersWithMessages", function (messages) {
    console.log("ReceiveUsersWithMessages called:", messages);

    // Update the HTML to display the list of usernames
    //var ul = document.getElementById("usersList");
    //ul.innerHTML = ""; // Clear previous usernames

    //usernames.forEach(function (username) {
    //    var li = document.createElement("li");
    //    li.textContent = username;
    //    ul.appendChild(li);
    //});
    messages.forEach(function (message) {
        var li = document.createElement("li");
        li.className = "user-item";

        var card = document.createElement("div");
        card.className = "card";

        var cardBody = document.createElement("div");
        cardBody.className = "card-body";

        var button = document.createElement("button");
        button.className = "btn";
        button.setAttribute("style", "width: 100%;");
        button.setAttribute("onclick", `toggleChat('${message.id}')`);

        var span = document.createElement("span");
        span.textContent = message.userName; // Assuming you have userName in your message object

        // Append everything together
        button.appendChild(span);
        cardBody.appendChild(button);
        card.appendChild(cardBody);
        li.appendChild(card);

        document.getElementById("usersList").appendChild(li);
    });
});

console.log("receiverId:", receiverId);

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    // Lấy tin nhắn khi kết nối được thiết lập
    connection.invoke("GetUsersWithMessages", senderId)
        .then(function (messages) {
            console.log("Old messages received:", messages);

            // Hiển thị tin nhắn trong chat-body
            //messages.forEach(function (message) {
            //    var li = document.createElement("li");
            //    li.textContent = `${message.senderUsername}: ${message.text}`;
            //    document.getElementById("messagesList").appendChild(li);
            //});

            //<li class="user-item">
            //    <div class="card">
            //        <div class="card-body">
            //            <button class="btn" style="width: 100%;" >
            //                <span>@user.userName</span>
            //            </button>
            //        </div>
            //    </div>
            //</li>
        })
        .catch(function (err) {
            console.error(err.toString());
        });
}).catch(function (err) {
    console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var username = document.getElementById("userInput").value; // Make sure the id is correct
    var message = document.getElementById("messageInput").value;

    // Use console.log for logging in JavaScript
    console.log(`Sent message from ${username}: ${message}`);

    connection.invoke("SendMessage", username, message).catch(function (err) {
        console.error(err.toString());
    });

    event.preventDefault();
    document.getElementById("messageInput").value = "";
});