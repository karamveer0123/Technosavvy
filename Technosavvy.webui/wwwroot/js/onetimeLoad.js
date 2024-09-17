//$.get(MktStream + 'GetBars?mCode=TechnoSavvyUSDT&pulse=1Sec', (data) => BarLoad(data));
//$.get(MktAPI + 'GetBars', { "mCode": "TechnoSavvyUSDT", "pulse": "1Min" }, (data) => BarLoad(data));
$.get(MktAPI + 'GetMarketSummary', (data) => MarketLoad(data));
//GetCurrencies
function BarLoad(data) {
    $('#Three').css("color", "lime");

    console.log(data[0]);
    $('#Three').html(GetBarTable(data));
    console.log(data);
}

function MarketLoad(data) {
    console.log(data);
    
}

function GetBarTable(array) {
    var arrlen = array.length;
    var selected = '';
    var row = '';
    if (arrlen > 0) {
        selected = '<table class="table table-responsive">';
        selected = selected + '<thead class="trasaction-table-title">';
        selected = selected + '<tr><th class="css566-table-text"><div class="name-css46"><span>Code</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="price-css46"><span>Type</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="price-css46"><span>Time</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="price-css46"><span>Stamp</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="change24-css46"><span>Open</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>High</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Low</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Close</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Volume</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Value</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Cashback</span></div></th>';
        selected = selected + '</tr></thead>';

        for (let i = 0; i < arrlen; i++) {
            var newD = new Date(0);
            newD.setUTCSeconds(array[i].time);
            var reported = newD.toUTCString();
            row = row + '<tbody class="trasaction-table">';
            row = row + '<tr><td><div class="coin-name-css46">' + array[i].code + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].type + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].time + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + newD + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].open + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].high + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].low + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].close + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].volume + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].tradesValue + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].cashBack + '</div></td>';
            row = row + '</tr></tbody>';

        }
        selected = selected + row + '</div>';
    }
    return selected;
}


$.get(MktAPI + 'GetCurrencies', (data) => CurrenciesLoad(data));//GetTokenSummary




var connection;
var isClosing = false;
async function SetCon(link) {
    connection = new signalR
        .HubConnectionBuilder()
        .withUrl(link, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        }).build();

    connection.onclose(async () => {  await Start(); })
    connection.on("UpdateMarket", function (data) {
        updateTMS(data);
        //console.log('UpdateMarket->');
       // console.log(data);
    });
    connection.on("UpdateAllMarkets", function (data) {

        console.log('UpdateAllMarkets->');
        console.log(data);
    });
    connection.on("UpdateTrades", function (data) {

        //console.log('UpdateTrades-> at ' + Date.now().toString());
        //console.log(data.length);UpdateTradingCashback
        console.log('received..');
        updateTrades(data);//UpdateOrders
    });
    connection.on("UpdateOrders", function (data) {

        //console.log('UpdateTrades-> at ' + Date.now().toString());
        //console.log(data.length);UpdateTradingCashback
        updateOrders(data);
    });
    connection.on("UpdateTradingCashback", function (data) {

        //console.log('UpdateTrades-> at ' + Date.now().toString());
        //console.log(data.length);
        updateCB(data);
    });
    connection.on("CurrencyValUpdate1", function (data) {

        console.log('CurrencyValUpdate->' );
        console.log( data);
    });
    connection.on("TokenValUpdate1", function (data) {

        console.log('TokenValUpdate->');
        console.log(data);
    });
    connection.on("TokenSummary", function (data) {

        console.log('TokenSummary->');
        console.log(data);
    });
    connection.on("LogReceiver", function (data) {

        console.log('LogReceiver->');
        console.log(data);
    });
}
async function Start() {
    try {
        console.log("attempting to connect..");
        connection.start();
        console.log("Connected successfully");
    }
    catch (e) {
        console.log(e);
        setTimeout(start, 5000);//Try Reconnect after 5 sec
    }
}
function LogData(data) {
    var parsed = JSON.parse(data);

}



//-------------- Market Home Page..Move to new Script file
