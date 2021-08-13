const hubNotifyConnection = new signalR.HubConnectionBuilder()
	.withUrl("/update")
	.build();

hubNotifyConnection.on("Send", function (data) {
	let content = data.split(" ");
	let message = 'A new vote ' + content[2] + ' in poll ' + content.slice(4).join(" ") + ' by ' + content[3];
	$.notify(
		{
			message: message
		},
		{
			element: 'body',
			placement: {
				from: "bottom",
				align: "right"
			},

			animate: {
				enter: "animated flipInX",
				exit: "animated zoomOut"
			},

			type: "success"
		});
});

hubNotifyConnection.start();