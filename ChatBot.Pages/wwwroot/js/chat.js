


window.ChatFunctions =
{
    ScrollToLast: () => {
        let messagesList = document.getElementById("messagesList");
        messagesList.scrollTop = messagesList.scrollHeight;
    }
}