// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
const textarea = document.getElementById('textBox');
const hubConnection = new signalR.HubConnectionBuilder()
.withUrl("/chat")
.build();
textarea.scrollTop = textarea.scrollHeight;
hubConnection.on("Send", function (data) {
	let elem = document.getElementById("textBox");
    elem.value += data;
	textarea.scrollTop = textarea.scrollHeight;
});

hubConnection.start();

document.getElementById("sendBtn").addEventListener("click", function (e) {
	let message = document.getElementById("message").value;
    $.ajax({
        type: "POST",
		url: $("#mySendingUrl").val(),
        data: { message: message },
    })

	document.getElementById("message").value = "";
});

let loadedMessagesCount = 0;
$(document).ready(function () {
	$("#textBox").scrollTop($("#textBox")[0].scrollHeight);
	ajaxChatLoading()
	$('#textBox').scroll(function () {
		if ($('#textBox').scrollTop() == 0) {
			ajaxChatLoading();
		}
	});

});

function ajaxChatLoading() {
	$.ajax({
		url: $("#myLoadingUrl").val(),
		data: { messageCount: loadedMessagesCount },
		dataType: 'text',
		success: function (data) {
			textarea.value = data + textarea.value;
		},
	});
	loadedMessagesCount += 30;
}