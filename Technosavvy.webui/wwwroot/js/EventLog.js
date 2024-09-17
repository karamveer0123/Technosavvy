const watchdogapi = watchdogapiurl;


var connection = new signalR
    .HubConnectionBuilder()
    .withUrl(watchdogapi + "broadcastevent", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    }).build();
console.log(' WatchDog : ' + watchdogapi + "broadcastevent");
connection.start().then(() => console.log("Connected successfully")).catch(() => console.log(error));//establish connection


connection.on("RefreshEventData", (errorData) => {

    var parsed = JSON.parse(errorData);
    if (parsed.length > 0) {
        $('#comunity-css566-box').empty();
        var arrErr = errArray(parsed);
        var htm = arrhtmlErrTable(arrErr);
        $("#comunity-css566-box").append(htm);
    }

});
connection.on("TimeData", (errorData) => {

    var parsed = JSON.parse(errorData);
    console.log(parsed);


});

function arrhtmlErrTable(array) {
    var arrlen = array.length;
    var selected = '';
    var row = '';
    if (arrlen > 0) {
        selected = '<table class="table table-responsive">';
        selected = selected + '<thead class="trasaction-table-title">';
        selected = selected + '<tr><th class="css566-table-text"><div class="name-css46"><span>App Id</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="price-css46"><span>Message</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="change24-css46"><span>Reported On</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Type</span></div></th>';
        selected = selected + '</tr></thead>';

        for (let i = 0; i < arrlen; i++) {
            var newD = new Date(array[i][2]);
            var reported = newD.toUTCString();
            row = row + '<tbody class="trasaction-table">';
            row = row + '<tr><td><div class="coin-name-css46">' + array[i][0] +'</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i][1] + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">'+ reported + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">'+ array[i][3] +'</div></td>';
            row = row + '</tr></tbody>';

        }
        selected = selected + row + '</div>';
    }
    return selected;
}

function errArray(erra) {
    let a = erra.length;
    let b = 4;
    let arr = [];


    for (let i = 0; i < a; i++) {
        for (let j = 0; j < b; j++) {
            arr[i] = [];
        }
    }

    for (let i = 0; i < a; i++) {
        arr[i][0] = erra[i]['AppId'];
        arr[i][1] = erra[i]['Message'];
        arr[i][2] = erra[i]['ReportedOn'];
        arr[i][3] = erra[i]['Type'];
    }
    return arr;
}

//---------
var api = "http://localhost:5096";
var myConnection = new signalR
    .HubConnectionBuilder()
    .withUrl(api + "my?a1", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        Headers: { "sToken": "ABC" }
    }).build();

myConnection.start().then(() => console.log("Connected successfully")).catch(() => console.log(error));//establish

myConnection.on("RelayMyData", (errorData) => {

    var parsed = JSON.parse(errorData);
    console.log(parsed);

});