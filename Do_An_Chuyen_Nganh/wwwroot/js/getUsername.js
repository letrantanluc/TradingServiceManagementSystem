"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var receiverId;
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
        button.onclick = function () {
            setReceiverId(message.id);
            document.getElementById('chatWindow').querySelector('.chat-header h5').textContent = message.userName;
        };

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
    var message = document.getElementById("messageInput").value;

    if (receiverId && message) {
        // Hiển thị tin nhắn vừa gửi trong chat-body
        var ul = document.getElementById("messagesList");
        var li = document.createElement("li");
        // Thay thế 'YourUsername' bằng tên người dùng hiện tại, nếu bạn có
        li.textContent = `You: ${message}`;
        ul.appendChild(li);

        // Gửi tin nhắn
        connection.invoke("SendMessage", receiverId, message).catch(function (err) {
            console.error(err.toString());
        });

        // Xóa nội dung tin nhắn
        document.getElementById("messageInput").value = "";
    }

    event.preventDefault();
});

function setReceiverId(id) {
    receiverId = id;
    document.getElementById('receiverInput').value = id;
    console.log("Receiver ID set to:", receiverId);
    toggleChat(receiverId);
}