"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Tắt nút gửi cho đến khi kết nối được thiết lập.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveUsersWithMessages", function (usernames) {
    console.log("ReceiveUsersWithMessages called:", usernames);

    // Update the HTML to display the list of usernames
    var ul = document.getElementById("usersList");
    ul.innerHTML = ""; // Clear previous usernames

    usernames.forEach(function (username) {
        var li = document.createElement("li");
        li.textContent = username;
        ul.appendChild(li);
    });
});

console.log("receiverId:", receiverId);

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    // Lấy tin nhắn khi kết nối được thiết lập
    connection.invoke("GetMessages", senderId, receiverId)
        .then(function (messages) {
            console.log("Old messages received:", messages);

            // Hiển thị tin nhắn trong chat-body
            messages.forEach(function (message) {
                var li = document.createElement("li");
                li.textContent = `${message.senderUsername}: ${message.text}`;
                document.getElementById("messagesList").appendChild(li);
            });
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