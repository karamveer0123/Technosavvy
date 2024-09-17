//GetTokenSummary
//GetCurrencies--> GeneralConfig is dealing with Currencies
//$.get(MktAPI + 'GetCurrencies', (data) => CurrenciesLoad(data));

function MarketLoad(data) {
    console.log(data);
}
//Temp Test
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

var connection;
var isClosing = false;
async function SetCon(link) {
    connection = new signalR
        .HubConnectionBuilder()
        .withUrl(link, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        }).build();

    connection.onclose(async () => { if (!isClosing) await Start(); })
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

        console.log('CurrencyValUpdate->');
        console.log(data);
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
        console.info("attempting to connect..");
        connection.start();
        console.info("Connected successfully");
    }
    catch (e) {
        console.log(e);
        setTimeout(start, 5000);//Try Reconnect after 5 sec
    }
}
function LogData(data) {
    var parsed = JSON.parse(data);

}
//--------------------
function load() {
    doInitialLoad();
    doConnect();
    doStartup();
}
function doInitialLoad() {
    //Load infor
    Loadinfo();
    //Set local variable
    //Status publish
}
function doConnect() {
    let para = (new URL(document.location)).searchParams;
    SetCon(MktStream + para);
    Start();
}
function doStartup() {
    //Apply formate
    var txt = 'Available ' + GetFormatedVal(_qa, _qf) + ' ' + _q;
    var txtb = 'Available ' + GetFormatedVal(_ba, _bf) + ' ' + _b;

    $('.dqa').each(function (d) {
        $(this).html(txt);
    });
    $('.dba').each(function (d) {
        $(this).html(txtb);
    });
    $('.dqf').focusout(function (d) {
        var v = $(this).val();
        if (v > 0)
            $(this).val(GetFormatedVal(v, _qf));
    });
    $('.dbf').focusout(function (d) {
        var v = $(this).val();
        if (v > 0)
            $(this).val(GetFormatedVal(v, _bf));
    });
    //Apply fraction validation
}
function Loadinfo() {
    $.get(MktAPI + 'GetMarketSummary', (data) => MarketLoad(data));
    $.get(MktAPI + 'GetMarketSummary', (data) => updateTMS(data));//GetTopVolumnTokenSummary
    $.get(MktAPI + 'Cashback', (data) => updateCB(data));//GetTopVolumnTokenSummary

}
function updateOrders(bdata, adata) {
    //console.log('Orders count started at ' + Date.now().toString() + '->' + data[0].length)
    //console.log(data);
    for (var i = 0; i < bdata.length; i++) {
        updateBuyOrder(bdata[i]);
    }
    for (var i = 0; i < adata.length; i++) {
        updateSellOrder(adata[i]);
    }
}
function updateBuyOrder(data) {
    var txt = '<ul class="row-div"><li> ' + GetFormatedVal(data.price, 2) + ' </li><li> ' + GetFormatedVal(data.amount, 6) + ' </li><li> ' + GetFormatedVal(data.price * data.amount, 2) + ' </li></ul>';

    var obj = $('#mBuyOrders');
    if (obj.children().length >= 17)
        obj.children()[0].remove();
    obj.append(txt);
}
function updateSellOrder(data) {
    var txt = '<ul class="row-div"><li> ' + GetFormatedVal(data.price, 2) + ' </li><li> ' + GetFormatedVal(data.amount, 6) + ' </li><li> ' + GetFormatedVal(data.price * data.amount, 2) + ' </li></ul>';

    var obj = $('#mSellOrders');
    if (obj.children().length >= 17)
        obj.children()[0].remove();
    obj.append(txt);
}
function updateTrades(data) {
    //if (data.length > 0)
    console.log('Trades count started at ' + Date.now().toString() + '->' + data.trade.length)
    updateOrders(data.bids, data.asks)
    updateLP(data.trade);
    updateTradeList(data.trade);
    lp = data.trade.tradePrice;
    console.log('Bids count: ' + data.bids.length.toString())
    console.log('Asks count: ' + data.asks.length.toString())
    console.log('Trades count Finished at ' + Date.now().toString())
}
//function updateTrades(data) {
//    if (data.length > 0)
//        console.log('Trades count started at ' + Date.now().toString() + '->' + data.length)
//    for (var i = 0; i < data.length; i++) {
//        updateLP(data[i]);
//        updateTradeList(data[i]);
//        lp = data[i].tradePrice;
//    }
//    //console.log('Trades count Finished at '+Date.now().toString())
//}
var lp = 0;
function updateLP(data) {
    if (lp <= data.tradePrice) {
        var txt = '<div class="price-number green"> ' + GetFormatedVal(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-up "></i> </div><div class="sub-number">' + GetCurrVal(data.tradePrice, 2) + ' </div>';
        var mtxt = '<div  class="price-txt_div green"> ' + GetFormatedVal(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-up"></i> </div>'
        $('#ptxtm').html(mtxt);
        $('#toplastprice').html(txt);

    }
    else {
        var txt = '<div class="price-number red"> ' + GetFormatedVal(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-down "></i> </div><div class="sub-number">' + GetCurrVal(data.tradePrice, 2) + ' </div>';
        var mtxt = '<div  class="price-txt_div red"> ' + GetFormatedVal(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-down"></i> </div>'

        $('#ptxtm').html(mtxt);
        $('#toplastprice').html(txt);
    }
    //console.log(data.tradePrice);
}
function updateTradeList(data) {
    var txt = '';
    if (lp > data.tradePrice) {
        txt = '<ul class="row-div" ><li>' + GetFormatedVal(data.tradePrice, 2) + ' </li><li> ' + GetFormatedVal(data.tradeVolumn, _bf) + ' </li><li> ' + moment(data.dateTimeUTC.toString()).format('hh:mm:ss') + ' </li></ul>';
    }
    else {
        txt = '<ul class="row-div" ><li class="green">' + GetFormatedVal(data.tradePrice, 2) + ' </li><li> ' + GetFormatedVal(data.tradeVolumn, _bf) + ' </li><li> ' + moment(data.dateTimeUTC.toString()).format('hh:mm:ss') + ' </li></ul>';
    }
    var obj = $('#tradetab77');
    if (obj.children().length >= 14)
        obj.children()[obj.children().length - 1].remove();
    obj.append(txt);
    obj.children()[1].before(obj.children()[obj.children().length - 1]);

}
function updateTMS(data) {
    console.log('updateTMS Actioned..');
    var txt = '<div class="number-div"><ul><li> 24h <br> Change <span class="css44-up"> ' + GetPercentFormat(data.change, 2) + '</span>  </li><li> 24h<br> High <span> ' + GetFormatedVal(data.highest24H, 2) + ' </span></li><li> 24h<br> Low <span> ' + GetFormatedVal(data.lowest24H, 2) + ' </span> </li><li> 24h<br> Volume(BTC) <span> ' + GetFormatedVal(data.volume, 6) + ' </span> </li><li> 24h <br>Volume(USDT) <span> ' + GetFormatedVal(data.value, 2) + ' </span> </li></ul></div>';
    $('#mTop24h').html(txt);
    $('#mName').html(data.name);
    //console.log('my name is :');
    //console.log(data);
}
function updateCB(data) {
    console.log(data);
    var txt = '<div class="col-detnavtop"><div class="col-detnav"><span><img src="../images/nav-icon.png"> </span><div class="cssnavpr44"><span class="pricecss33-text">' + GetFormatedVal(data.TechnoSavvyPrice, 6) + '</span></div></div><div class="colfloor77">Floor Price: ' + GetFormatedVal(data.TechnoSavvyFloorPrice, 6) + ' </div></div>';
    txt += '<div class="col-cashnav"><span class="colcash77" > Cashback</span><span class="colamoun77"> ' + GetFormatedVal(data.cashBack24H, 6) + ' (24h)</span><span class="colamoun77"> ' + GetFormatedVal(data.cashBackYTD, 2) + ' (YTD)</span></div> ';
    $('#mTopTechnoSavvy').html(txt);
}