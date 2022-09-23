

"use strict";

/* js functions */
function sendMessage() {
    var message = document.getElementById("messageInput").value;
    var chatMessage = {
        text: message,
        userId: loggedUserId,
        userName: loggedUserName
    };
    connection.invoke("SendAll", chatMessage).catch(function (err) {
        return console.error(err.toString());
    });
}

function scrollToLastMessages() {
    let messagesList = document.getElementById("messagesList");
    messagesList.scrollTop = messagesList.scrollHeight;
}

/* Starting SignalR connection */
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("receive", function (message) {
    let container = document.createElement("div");
    container.className = "container";

    let outterRow = document.createElement("div");
    outterRow.className = "row";

    let paintedDiv = document.createElement("div");
    paintedDiv.className = "col-5 message " + (message.userID === loggedUserId ? "mine offset-7" : "")

    let messageText = document.createElement("p");
    messageText.textContent = message.text;

    let innerRow = document.createElement("div");
    innerRow.className = "row";

    let sender = document.createElement("div");
    sender.className = "col-6 sender";
    sender.textContent = message.userName;

    let time = document.createElement("div");
    time.className = "col-6 time";
    let date = new Date(message.sentAt);
    let hour = date.getHours();
    let min = date.getMinutes();
    let seconds = date.getSeconds();
    time.textContent = "At " + date.toLocaleDateString() + " " + `${hour}:${min}:${seconds}`;

    innerRow.appendChild(sender);
    innerRow.appendChild(time);

    paintedDiv.appendChild(messageText);
    paintedDiv.appendChild(innerRow);

    outterRow.appendChild(paintedDiv);

    container.appendChild(outterRow);

    let messagesList = document.getElementById("messagesList");
    messagesList.appendChild(container);

    /* Scroll down to end of list*/
    scrollToLastMessages();

    /* Ensures that only 50 messages are displayed */
    if (messagesList.children.length > 50) {
        let first = messagesList.children[0];
        messagesList.remove(first);
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    scrollToLastMessages();

}).catch(function (err) {
    return console.error(err.toString());
});

document.querySelector("#messageInput").addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        sendMessage();
        document.querySelector("#messageInput").value = "";
        e.preventDefault();
    }
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    sendMessage();
    event.preventDefault();
});

