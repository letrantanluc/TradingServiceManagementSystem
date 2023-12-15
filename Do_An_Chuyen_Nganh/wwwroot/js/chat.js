"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Tắt nút gửi cho đến khi kết nối được thiết lập.
document.getElementById("sendButton").disabled = true;

// Declare receiverId outside of the toggleChat function
var receiverId;
connection.on("ReceiveMessage", function (username, message) {
    var ul = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.textContent = `${username}: ${message}`;
    ul.appendChild(li);
});
connection.on("ReceiveUsersWithMessages", function (usernames) {
    console.log("ReceiveUsersWithMessages called:", usernames);

    // Update the HTML to display the list of usernames
    var ul = document.getElementById("usersList");
    ul.innerHTML = ""; // Clear previous usernames

    usernames.forEach(function (user) {
        var li = document.createElement("li");
        li.textContent = user.userName; // Assuming you have userName in your message object
        ul.appendChild(li);

        // Add a click event listener to each li element
        li.addEventListener("click", function () {
            // Set the receiverId when an li is clicked
            receiverId = user.id; // Assuming you have an id property in your message object
            toggleChat(receiverId);
        });
    });
});

console.log("receiverId:", receiverId);

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    // Lấy tin nhắn khi kết nối được thiết lập
    connection.invoke("GetMessages", senderId, receiverId)
        .then(function (messages) {
            console.log("Old messages received:", messages);

            var ul = document.getElementById("messagesList");
            ul.innerHTML = ""; // Xóa tin nhắn cũ

            messages.forEach(function (message) {
                var li = document.createElement("li");
                li.textContent = `${message.senderUsername}: ${message.text}`;
                ul.appendChild(li);
            });
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
        //var ul = document.getElementById("messagesList");
        //var li = document.createElement("li");
        //li.textContent = `You: ${message}`;
        //ul.appendChild(li);

        connection.invoke("SendMessage", receiverId, message).catch(function (err) {
            console.error(err.toString());
        });

       
        document.getElementById("messageInput").value = "";
    }
    event.preventDefault();
});