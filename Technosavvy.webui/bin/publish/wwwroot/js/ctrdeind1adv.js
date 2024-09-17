//GetTokenSummary
//GetCurrencies--> GeneralConfig is dealing with Currencies
//$.get(MktAPI + 'GetCurrencies', (data) => CurrenciesLoad(data));

var lastPrice;
var cb = [];
function MarketLoad(data) {
    if (data == null || data.length <= 0) return;
    ___qf(); ___bf();
    data.forEach(function (d, i) {
        if (d.mCode.toLowerCase() == __mCode.toLowerCase())
            updateTMS(d);
        else {
            //ToDo: Write here for other Market Window update..
        }
    });
    //console.log(data);
}

var connection;
var isClosing = false;
async function SetCon(link) {
    connection = new signalR
        .HubConnectionBuilder()
        .withAutomaticReconnect([0, 0, 5000])
        .withUrl(link, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        }).build();

    connection.onclose(async () => { console.log('--> Connection Closed.. '); await Start(); })
    connection.onreconnecting(msg => {
        console.log('--> onreconnecting '); console.log(msg);
    });
    connection.onreconnected(msg => {
        console.log('--> onreconnected '); console.log(msg);
    });

    connection.on("UpdateMarket", function (data) {
        updateTMS(data);
        //console.log('UpdateMarket->');
        // //console.log(data);UpdateBars
    });

    connection.on("UpdateBars", function (data) {
        var b = buildData(dataBARS.length, data);
        dataBARS.push(b);
        updateBars();
        displayUpdate();
    });
    connection.on("UpdateAllMarkets", function (data) {
        console.log('UpdateAllMarkets received..');
        ////console.log(data);
    });
    connection.on("UpdateTrades", function (data) {
        if (liveOn === false) return;
        ProcessTrade(data);
    });
    connection.on("UpdateBid", function (bdata) {
        if (bdata != null) {
            updateBuyOrder(bdata[0], true);          
            for (var i = 1; i < bdata.length; i++) {
                updateBuyOrder(bdata[i]);               
            }
        }
        else {
            console.log("UpdateBid is null");
        }
        // updateBuyOrder(data);
    });
    connection.on("UpdateAsk", function (adata) {
        console.log('adata',adata);

        if (adata != null) {
            updateSellOrder(adata[0], true);           
            for (var i = 1; i < adata.length; i++) {
                updateSellOrder(adata[i]);               
            }
        }
        else
            console.log("UpdateAsk is null");

        //updateSellOrder(data);
    });
    connection.on("UpdateBid2", function (adata) {
        console.log('adata for 40->', adata);
        //ToDo: Shiv Complete this to display Data
        if (adata != null) {
            updateBuyOrderOnly(adata[0], true);
            for (var i = 1; i < adata.length; i++) {
                updateBuyOrderOnly(adata[i]);
            }
        }
        else {
            console.log("UpdateBid is null");
        }
    });
    connection.on("UpdateAsk2", function (adata) {
        console.log('adata for 40->', adata);
        //ToDo: Shiv Complete this to display Data
        if (adata != null) {
            updateSellOrderOnly(adata[0], true);
            for (var i = 1; i < adata.length; i++) {
                updateSellOrderOnly(adata[i]);
            }
        }
        else
            console.log("UpdateAsk is null");
    });
    connection.on("UpdateOrders", function (data) {
        updateOrders(data);//xx
    });
    connection.on("UpdateTradingCashback", function (data) {

        //console.log('UpdateTrades-> at ' + Date.now().toString());
        //console.log(data.length);
        updateCB(data);
    });
    connection.on("CurrencyValUpdate1", function (data) {

        console.log('CurrencyValUpdate->');
        ////console.log(data);
    });
    connection.on("TokenValUpdate1", function (data) {

        console.log('TokenValUpdate->');
        //console.log(data);
    });
    connection.on("TokenSummary", function (data) {

        console.log('TokenSummary->');
        //console.log(data);
    });
    connection.on("LogReceiver", function (data) {

        //console.log('LogReceiver->');
        //if (data.indexOf('Dummy') >= 0)
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
async function Refresh() {
    try {
        console.log("Refreshing..");
        connection.stop();
        doConnect();
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
//--------------------
function load() {
    doInitialLoad();
    doConnect();
}
function doInitialLoad() {
    //Load infor
    Loadinfo();
    //Set local variable
    //Status publish
}
function doConnect() {
    let para = (new URL(document.location)).searchParams;
    var v = $('#barTime').val();
    if (v == null | v == undefined)
        v = '1min';
    para += '&pl=' + v;
    SetCon(MktStream + para);
    Start();
}
function doStartup() {
    //Apply formate
    var txt = 'Available ' + GetFormatedVal(_qa, __qf) + ' ' + __q;
    var txtb = 'Available ' + GetFormatedVal(_ba, __bf) + ' ' + __b;

    $('.dqa').each(function (d) {
        $(this).html(txt);
    });
    $('.dba').each(function (d) {
        $(this).html(txtb);
    });
    $('.dqf').focusout(function (d) {
        var v = $(this).val();
        if (v > 0)
            $(this).val(GetFormatedVal(v, __qf));
    });
    $('.dbf').focusout(function (d) {
        var v = $(this).val();
        if (v > 0)
            $(this).val(GetFormatedVal(v, __bf));
    });
    //Apply fraction validation
}
function Loadinfo() {
    $.get(MktAPI + 'GetMarketSummary', (data) => MarketLoad(data));
    $.get(MktAPI + 'GetRecentTrades?mcode=' + __mCode, (data) => updateTrades(data));
    $.get(MktAPI + 'Cashback', (data) => cb=data);
    $.get(MktAPI + 'GetCurrentCashBackCycle', (data) => updateCB(data));
    //ToDo:Naveen, Load Initial Orders here..
}
function ProcessTrade(data) {
    var t = getTime(data);
    var eb = dataBARS[dataBARS.length - 1];
    if (eb.time < t) {
        //create a new Bar
        var b = createOrupdateBAR(t, data);
        if (liveBARS.length > 0) {
            if (liveBARS[liveBARS.length - 1].time < b.time) {
                liveBARS.push(b);
            }
            //   console.log('Trade @' + b.x.close.toString() + ' ' + b.date.format("mm:ss"));
            displayUpdate();
            __CheckAndDisplay();
        }
        else {
            liveBARS.push(b);
            //  console.log('Trade @' + b.x.close.toString() + ' ' + b.date.format("mm:ss"));
            displayUpdate();
            __CheckAndDisplay();
        }
        console.log('Last LiveBar timestamp:' + b.time.toString() + '| Trade Time Stamp is:' + t.toString());
    }
    updateTrades(data);//UpdateOrders
}
function updateBars() {
    // console.clear();
    var a = dataBARS[dataBARS.length - 1].y;
    var l = liveBARS.length;
    var na = [];
    for (var i = 0; i < l; i++) {
        if (liveBARS[i].y > a)
            na.push(liveBARS[i]);
    }
    liveBARS = na;
    console.log('UPDATE BARS received..');

}

function updateOrders(bdata, adata) {
    //console.log('Orders count started at ' + Date.now().toString() + '->' + data[0].length)
    ////console.log(data);
    if (bdata != null)
        for (var i = 0; i < bdata.length; i++) {
            updateBuyOrder(bdata[i]);
            updateBuyOrderOnly(bdata[i])
        }
    if (adata != null)
        for (var i = 0; i < adata.length; i++) {
            updateSellOrder(adata[i]);
            updateSellOrderOnly(adata[i]);
        }
}
function updateBuyOrder(data, re) {
    var obj = $('#mBuyOrders');
    if (re) {
        obj.html('');
    }
    var txt = '<ul class="row-div"><li> ' + GetFormatedValWithSepTrad(data.price, __qf) + ' </li><li> ' + GetFormatedValAdj(data.amount, __bf) + ' </li><li> ' + GetFormatedValAdjWithSep(data.price * data.amount, __qf) + ' </li></ul>';

    if (obj.children().length >= 10)
        obj.children()[0].remove();
    obj.append(txt);
}
function updateSellOrder(data, re) {
    var obj = $('#mSellOrders');
    if (re) {
        obj.html('');
    }
    var txt = '<ul class="row-div"><li> ' + GetFormatedValWithSepTrad(data.price, __qf) + ' </li><li> ' + GetFormatedValAdj(data.amount, __bf) + ' </li><li> ' + GetFormatedValAdjWithSep(data.price * data.amount, __qf) + ' </li></ul>';

    if (obj.children().length >= 10)
        obj.children()[0].remove();
    obj.append(txt);
}

//mBuyOrdersOnly
function updateBuyOrderOnly(data, re) {
    var obj = $('#mBuyOrdersOnly');
    if (re) {
        obj.html('');
    }
    var txt = '<ul class="row-div"><li> ' + GetFormatedValWithSepTrad(data.price, __qf) + ' </li><li> ' + GetFormatedValAdj(data.amount, __bf) + ' </li><li> ' + GetFormatedValAdjWithSep(data.price * data.amount, __qf) + ' </li></ul>';

    if (obj.children().length >= 18)
        obj.children()[0].remove();
    obj.append(txt);
}
//mSellOrdersOnly
function updateSellOrderOnly(data, re) {
    var obj = $('#mSellOrdersOnly');
    if (re) {
        obj.html('');
    }
    var txt = '<ul class="row-div"><li> ' + GetFormatedValWithSepTrad(data.price, __qf) + ' </li><li> ' + GetFormatedValAdj(data.amount, __bf) + ' </li><li> ' + GetFormatedValAdjWithSep(data.price * data.amount, __qf) + ' </li></ul>';

    if (obj.children().length >= 18)
        obj.children()[0].remove();
    obj.append(txt);
}

function updateTrades(data) {
    if (data.trade != undefined) {
        updateOrders(data.bids, data.asks)
        updateLP(data.trade);
        updateTradeList(data.trade);
        lp = data.trade;
    }
    else {
        for (var i = 0; i < data.length; i++) {
            updateLP(data[i]);
            updateTradeList(data[i]);
            lp = data[i];
        }

    }
    // console.log('Bids count: ' + data.bids.length.toString())
    // console.log('Asks count: ' + data.asks.length.toString())
    //console.log('Trades count Finished at ' + Date.now().toString())
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
    if (lastPrice <= data.tradePrice) {
        var txt = '<div class="price-number green"> ' + GetFormatedValWithSep(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-up "></i> </div><div class="sub-number dyCur" data-oval="'+data.tradePrice+'">' + GetFormatedCurrency(data.tradePrice) + ' </div>';
        var mtxt = '<div  class="price-txt_div green"> ' + GetFormatedVal(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-up"></i> </div>'
        var pbytxt = ' <li class="price-txt green-txtindex">' + GetFormatedVal(data.tradePrice, 2) + '<i class="fa fa-long-arrow-up" aria-hidden="true" ></i></li><li class="l-height"> </li><li class="l-height float-right"><span class="sub-number"> ' + data.tradePrice + '</span></li>';
        $('#ptxtm').html(mtxt);
        $('#toplastprice').html(txt);
        $('#ptBuyOrder').html(pbytxt);

    }
    else {
        var txt = '<div class="price-number red"> ' + GetFormatedValWithSep(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-down "></i> </div><div class="sub-number dyCur" data-oval="' + data.tradePrice +'">' + GetFormatedCurrency(data.tradePrice) + ' </div>';
        var mtxt = '<div  class="price-txt_div red"> ' + GetFormatedVal(data.tradePrice, 2) + ' <i class="fa fa-long-arrow-down"></i> </div>'
        var pbytxt = ' <li class="price-txt red-txtindex">' + GetFormatedVal(data.tradePrice, 2) + '<i class="fa fa-long-arrow-down" aria-hidden="true" ></i></li><li class="l-height"> </li><li class="l-height float-right"><span class="sub-number"> ' + data.tradePrice + '</span></li>'
        $('#ptxtm').html(mtxt);
        $('#toplastprice').html(txt);
        $('#ptSellOrder').html(pbytxt);
    }
        lastPrice = data.tradePrice;
    //console.log(data.tradePrice);
}

var IsTradeFirst = false;
function updateTradeList(data) {
    if (!IsTradeFirst) {
        //var txt = '<ul class="price-ul"><li>Price ('+__q+') <span> </span></li><li>Amount ('+__b+') <span> </span></li><li>Time </li></ul>';
        var txt = '<div></div>'
        $('#tradetab77').html(txt);
        IsTradeFirst = true;
    }
    var txt = '';
    if (lp.tradePrice > data.tradePrice) {
        txt = '<ul class="row-div" ><li>' + GetFormatedValWithSep(data.tradePrice, __qf) + ' </li><li> ' + GetFormatedVal(data.tradeVolumn, __bf) + ' </li><li> ' + moment.utc(data.dateTimeUTC.toString()).local().format('hh:mm') + ' </li></ul>';
    }
    else {
        txt = '<ul class="row-div" ><li class="green">' + GetFormatedValWithSep(data.tradePrice, __qf) + ' </li><li> ' + GetFormatedVal(data.tradeVolumn, __bf) + ' </li><li> ' + moment.utc(data.dateTimeUTC.toString()).local().format('hh:mm') + ' </li></ul>';
    }
    var obj = $('#tradetab77');
    if (obj.children().length >= 20)
        obj.children()[obj.children().length - 1].remove();
    obj.append(txt);
    obj.children()[1].before(obj.children()[obj.children().length - 1]);

    var i = new moment.utc(data.dateTimeUTC).local().startOf('second').fromNow();
    //console.log(i + ' | ' + data.tradeID.toString());
}
function updateTMS(data) {
    console.log('updateTMS', data);    
    var clsrg = data.change < 0 ? 'red' : 'green';
    var txt = '<div class="number-div"><ul><li> 24h <br> Change <span class="css44-up1 ' + clsrg +'"> ' + GetPercentFormat(data.change, 2) + '</span>  </li><li> 24h<br> High <span> ' + GetFormatedVal(data.highest24H, __qf) + ' </span></li><li> 24h<br> Low <span> ' + GetFormatedVal(data.lowest24H, __qf) + ' </span> </li><li> 24h<br> Volume(' + data.base + ') <span> ' + GetFormatedValWithSep(data.volume, 2) + ' </span> </li><li> 24h <br>Volume(' + data.quote + ') <span> ' + GetFormatedValWithSep(data.trade24Hr, 2) + ' </span> </li></ul></div>';
    $('#mTop24h').html(txt);
    $('#mName').html(data.name);
    var trQuote = '(' + data.quote + ')';
    var trBase = '(' + data.base + ')';
    $('.trQuote').html(trQuote);
    $('.trBase').html(trBase);
    //console.log('my name is :');
    ////console.log(data);
}
function updateCB(data) {
    console.log('CashBack Update..>');
    console.log(data);
   // cb = data;
    var txt = '';
    
    txt += '<div class="col-detnavtop"><div class="col-detnav"><span><img src="../images/nav-icon.png"> </span><div class="cssnavpr44">';
    if (data.TechnoSavvyPrice > data.TechnoSavvyFloorPrice) {
        txt += '<span class="pricecss33-text dyCur" data-oval="' + data.TechnoSavvyPrice + '">' + GetFormatedCurrency(data.TechnoSavvyPrice, __nf) + '</span>';
    } else {
        txt += '<span class="pricecss33-text dyCur" data-oval="' + data.TechnoSavvyFloorPrice + '">' + GetFormatedCurrency(data.TechnoSavvyFloorPrice, __nf) + '</span>';
    }
    

        txt += '</div ></div > <div class="colfloor77">Floor Price: <span class="dyCur" data-oval="' + data.TechnoSavvyFloorPrice + '">' + GetFormatedCurrency(data.TechnoSavvyFloorPrice, 6) + ' </span></div></div > ';
    txt += '<div class="col-cashnav"><span class="colcash77" > Cashback</span><span class="colamoun77"><span class="dyCur" data-oval="' + data.cashBack24H + '" > ' + GetFormatedCurrency(data.cashBack24H, 2) + '</span>(24h)</span><span class="colamoun77"><span class="dyCur" data-oval="' + data.cashBackYTD + '"> ' + GetFormatedCurrency(data.cashBackYTD, 2) + '</span>(YTD)</span></div> ';
    $('#mTopTechnoSavvy').html(txt);
    
    getCountdown(data);
    setInterval(() => {
        getCountdown(data);
        if ($('#tiles span:last-child').text() == 00) {
            $('#tiles, .timrdiv54fr ').hide();
            $('#left').removeClass('d-none');
            $('#left').text('Deposit Soon!');
        } else {
            $('#left').addClass('d-none');
            $('#tiles, .timrdiv54fr ').show();
        }
    }, 1000);
}

$('.advance').addClass('active');
$('.fullscreen').click(function (e) {
    Switch('Fullscreen');
});
$('.classic').click(function (e) {
    Switch('Classic');
}); 