const connection = new signalR.HubConnectionBuilder()
    .withUrl("/logouthub")
    .build();


connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("ForceLogout", function () {
    window.location.href = "/Authentication/Login";
});

