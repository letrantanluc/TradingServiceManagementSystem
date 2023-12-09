"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Tắt nút gửi cho đến khi kết nối được thiết lập.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (username, message) {
    console.log("ReceiveMessage called:", username, message);

    var li = document.createElement("li");
    li.textContent = `${username}: ${message}`;
    document.getElementById("messagesList").appendChild(li);
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
                li.textContent = `${message.senderID}: ${message.text}`;
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

function getReceiversBySender(senderId) {
    // Call a hub method to get receivers for the specified sender
    connection.invoke("GetReceiversBySender", senderId)
        .then(function (receivers) {
            console.log(`Receivers who messaged ${senderId}:`, receivers);

            // Update the list-receiver with the received data
            updateReceiverList(receivers);
        })
        .catch(function (err) {
            console.error(err.toString());
        });
}

function updateReceiverList(receivers) {
    var listReceiver = document.querySelector('.list-receiver');
    listReceiver.innerHTML = ''; // Clear the existing list

    // Iterate through the receivers and update the list
    for (var i = 0; i < receivers.length; i++) {
        var listItem = document.createElement('li');
        listItem.textContent = receivers[i];
        listReceiver.appendChild(listItem);
    }
}