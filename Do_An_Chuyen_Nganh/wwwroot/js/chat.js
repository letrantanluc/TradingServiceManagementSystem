"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (username, message) {
    var li = document.createElement("li");
    li.textContent = `${username}: ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
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