
var con;
var oo = [];
var oH = [];

async function MyCon(link) {
    con = new signalR
        .HubConnectionBuilder()
        .withAutomaticReconnect([0, 0, 5000])
        .withUrl(link, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        }).build();

    con.onclose(async () => { console.log('--> Connection Closed.. '); await Start(); })
    con.onreconnecting(msg => {
        console.log('--> onreconnecting '); console.log(msg);
    });
    con.onreconnected(msg => {
        console.log('--> onreconnected ');
        console.log(msg);
    });
    con.on("LogReceiver", function (data) {

        //console.log('LogReceiver->');
        //if (data.indexOf('Dummy') >= 0)
        console.log(data);
    });
    con.on("OpenOrder", function (data) {
        oo = data;
        console.log("OpenOrder");
        console.log(data);
        updateOpenOrders(data);
        ooCount(oo);
    });
    con.on("OrderHistory", function (data) {
        oH = data;
        console.log("OrderHistory", data);
        console.log(data);
        updateOrderHistory(data);
    });
    con.on("TradeHistory", function (data) {
        console.log("TradeHistory", data);
        console.log(data);
        updateTradeHistory(data);
    });
    con.on("RecentTrade", function (data) {
        console.log("RecentTrade");
        console.log(data);
        updateRecentTrades(data);
    });
    con.on("Funds", function (data) {
        console.log("Funds");
        console.log(data);
        updateWalletFunds(data);
    });
}
async function myConStart() {
    try {
        console.log("attempting to connect..");
        if (con == null) {
            console.log("first Time initialize");
            MyCon((new URL(document.location)).origin + '/Stream?mkt=' + __mCode);
        }
        con.start();
        console.log("Connected successfully");
    }
    catch (e) {
        console.log(e);
        // setTimeout(start, 15000);//Try Reconnect after 15 sec
    }
}
async function myConRefresh() {
    try {
        console.log("Refreshing..");
        con.stop();
        //doConnect();
        console.log("Connected successfully");
    }
    catch (e) {
        console.log(e);
        setTimeout(start, 5000);//Try Reconnect after 5 sec
    }
}
function updateWalletFunds(data) {
    if (data != null && data != 'undefined')
        for (var i = 0; i < data.tokens.length; i++) {
            var d = data.tokens[i];
            if (d.code.toLowerCase() == __q.toLowerCase()) {
                _qa = d.amount;
                $('#nquoteavailable').val(d.amount);
            }
            if (d.code.toLowerCase() == __b.toLowerCase()) {
                _ba = d.amount;
                $('#nbaseavailable').val(d.amount);
            }
        }
    /*var txt = 'Available ' + GetFormatedVal(_qa, __qf) + ' ' + __q;
    var txtb = 'Available ' + GetFormatedVal(_ba, __bf) + ' ' + __b;*/
    var adfund = '<a href="/Deposit"><i class="fa fa-plus-circle" aria-hidden="true"></i></a>';

    var txt = 'Avl Bal ' + GetFormatCountMBTQ(_qa, __qf) + ' ' + __q + ' ' + adfund;
    var txtb = 'Avl Bal ' + GetFormatCountMBTQ(_ba, __bf) + ' ' + __b + ' ' + adfund;

    $('.dqa').each(function (d) {
        $(this).html(txt);
        $(this).attr('data-val', _qa);
    });
    $('.dba').each(function (d) {
        $(this).html(txtb);
        $(this).attr('data-val', _ba);
    });
    console.log("SpotWallet Updated");
}
function updateOpenOrders(data) {
    var txt = '';
    var le = data.length;
    console.log('myconclassic', le, data);
    if (le <= 10) {
        $('#openorder33 .openorder33').hide();
    } else {
        $('#openorder33 .openorder33').show();
    }
    if (le == 0) {
        txt += '<tr><td colspan="11"><div class="noheight54"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div></td></tr>';
        $('#tbopenorder').html(txt);
        
    } else {
    if (le > 10) le = 10;
    for (var i = 0; i < le; i++) {
        var d = data[i];
        txt += '<tr>';
        //txt += '<td><div class="form-check">';
        //txt += '       <input class="form-check-input" type="checkbox" value="" id=" "></div>';
        //txt += ' </td>';

        txt += '   <td class="text-left"><div class="datereward44">';
        txt += '        <span> ' + new moment.utc(d.placedOn).local().format('LL') + ' </span> <small> ' + new moment.utc(d.placedOn).local().format('LT') + '</small></div> </td>';
        /*  txt += '  <td class="text-left">' + d.marketCode + '</td>';*/
        txt += '   <td> <span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + d.internalOrderID + '" data-bs-original-title="" title="">' + d.internalOrderID.split('-')[0].toUpperCase() + '..' + '</span>  </td>';
        txt += '    <td>' + d.orderTypeT + '</td>';

        if (d.orderSideT == 'Buy')
            txt += '   <td><span class="basegreen">' + d.orderSideT + '</span> </td>';
        else
            txt += '   <td><span class="basered">' + d.orderSideT + '</span> </td>';

        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.price, __qf) + '</span> <small> ' + __q + ' </small></div></td>';
        /* txt += '   <td>' + GetFormatedVal(d.price,__qf) + ' <small> ' + __q + ' </small></td>';*/
        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.originalVolume, __bf) + '</span> <small> ' + __b + ' </small></div></td>';
        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.currentVolume, __bf) + '</span> <small> ' + __b + ' </small></div></td>';
        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.currentVolume * d.price, __qf) + '</span> <small> ' + __q + ' </small></div></td>';
        //txt += '    <td nowrap="nowrap">';
        //txt += '        <div class="css5asset44">';
        //txt += '           <span> 0.9998 BTC </span> <span> <small> 19996 USDT</small></span>';
        //txt += '        </div>';
        //txt += '   </td>';

        txt += '<td><div class="datettrade44"><span> ' +  GetFormatedVal(d._OrderSwapTradeValue, __qf) + '</span> <small> USDT </small></div></td>';
        if (d._OrderTrigger > 0) {
            txt += '    <td>' + d._OrderTrigger + ' </td>';
        } else {
            txt += '    <td>N/A </td>';
        }

        txt += '   <td class="csscancel44"><button class="btnFetch btnFetch-' + d.internalOrderID +'" onclick="cancelOrder(\'' + d.internalOrderID + '\')"> Cancel </button></td>    </tr>';

    }
    var obj = $('#tbopenorder');
    obj.html(txt);
    }
}
function cancelOrder(id) {
    var data = { mcode: __mCode, id: id };
    //disable button
    $('.btnFetch-' + id).prop("disabled", true);
    //ad spinner to button
    $('.btnFetch-' + id).html('<i class="fa fa-circle-o-notch fa-spin"></i> Canceling...');
    $.ajax({
        type: "POST",
        url: '/Trade/CancelMyOrder',
        data: data,
        success: function (d) {
            //alert("delete request sent.." + d.toString());
            $('.btnFetch-' + id).prop("disabled", true);
            $('.btnFetch-' + id).html('Canceled');
           
           
        },
        error: function (error) {
            console.log(error.statusCode + ':' + error.statusText + ':' + error.responseText);
           // alert("delete request failed..");
        }
    });
   
}
function updateOrderHistory(data) {
    console.log('updateOrderHistory', data);
    var txt = '';
    var le = data.length;
    if (le == 0) {
        txt += '<tr><td colspan="12"><div class="noheight54"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div></td></tr>';
        $('#tbOrderHist').html(txt);
    } else {
    if (le > 10) le = 10;
    for (var i = 0; i < le; i++) {
        var d = data[i];
        txt += '<tr class="text-left">';
        txt += '   <td><div class="datereward44">';
        txt += '        <span> ' + new moment.utc(d.placedOn).local().format('LL') + ' </span> <span> <small class="me-0"> ' + new moment.utc(d.placedOn).local().format('LT') + '</small></span></div> </td>';
        /*txt += '  <td class="text-left">' + d.marketCode + '</td>';*/
        txt += '   <td> <span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + d.internalOrderID + '" data-bs-original-title="" title="">' + d.internalOrderID.split('-')[0].toUpperCase() + '..' + '</span>  </td>';
        txt += '    <td>' + d.orderTypeT + '</td>';
        if (d.orderSide == 0)
            txt += '   <td><span class="basegreen">' + d.orderSideT + '</span> </td>';
        else
            txt += '   <td><span class="basered">' + d.orderSideT + '</span> </td>';

        txt += '   <td> <div class="datettrade44">' + GetFormatedVal(d.price, __qf) + ' <small> ' + __q + ' </small></div></td>';
        //Amount
        txt += '    <td> <div class="datettrade44">' + GetFormatedVal(d.originalVolume, __bf) + ' <small>' + __b + '</small></div></td>';
        //Filled
        txt += '   <td nowrap="nowrap"> <div class="datettrade44">' + GetFormatedVal(d.processedVolume, __bf) + '<small> ' + __b + '</small></div></td>';
        //Total

        txt += '    <td> <div class="datettrade44">' + GetFormatedVal(d.processedVolume * d.price, __qf) + ' <small> ' + __q + '</small></div></td>';
        //Asset Amount
        if (d._OrderAssetAmount > 0)
            txt += '<td><div class="dflex22"><span> ' + GetFormatedVal(d.originalVolume, __bf) + '</span>  <small>  ' + __b + ' </small></div></td>';
        else
            txt += '<td><div class="dflex22"><span class="me-0"> -- </span>  <small>  ' + __b + ' </small></div></td>';
        //SWAP trade Value
        txt += '    <td nowrap="nowrap"> <div class="datettrade44">' + GetFormatedVal(d._OrderSwapTradeValue, ___qf) + ' <small> USDT</small></div></td>';
        if (d.trigger > 0) {
            txt += '    <td>' + d.trigger + ' </td>';
        } else {
            txt += '    <td>N/A </td>';
        }


        txt += '<td nowrap="nowrap"> ' + d.statusT + '</td></tr>';
    }
    var obj = $('#tbOrderHist');
    obj.html(txt);
    }
}
function updateRecentTrades(data) {
    console.log('updateRecentTrades',data)
    var txt = '<div class="row"><div class="price-list"><ul class="price-ul"><li>Pair<span> </span></li><li>Price <span> </span></li><li>Amount <span> </span></li><li>Time </li></ul>';
    var le = data.length;
   // if (le > 5)
  /* var advScreen = $('.advance.active');
    
    console.log('advScreen', advScreen);
    if (advScreen) {
        le = 15;
    } else { 
      le = 15; 
    }
*/
    
    le = 15;
    
    
    for (var i = 0; i < le; i++) {
        var d = data[i];
        txt += '<ul class="row-div">';
        if (d.orderSide == 0)
            txt += '<li class="css44-up"> ' + d.marketCode + '</li>';
        else
            txt += '<li class="css44-down"> ' + d.marketCode + '</li>';

        txt += '<li>' + GetFormatedValAdj(d.tradePrice, _fsl) +'</li>';
        txt += '<li> ' + GetFormatedValAdj(d.tradeVolumn, _fsl) +' </li>';
        txt += '<li> ' + new moment.utc(d.dateTimeUTC).local().format('hh:mm') + ' </li>';
        txt += '</ul> ';
    }
    txt += '</div>';
    txt += '</div>';
    var obj = $('#tabred');
    obj.html(txt);    
}
function updateTradeHistory(data) {
    console.log('updateTradeHistory', data)
    var txt = '';
    var le = data.length;
    if (le == 0) {
        txt += '<tr><td colspan="12"><div class="noheight54"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div></td></tr>';
        $('#tbTradeHist').html(txt);
    } else {
    // if (le > 10) le = 10;
    for (var i = 0; i < le; i++) {
        var d = data[i];
        txt += '<tr>';
        txt += '   <td><div class="datereward44">';
        txt += '        <span> ' + new moment.utc(d.dateTimeUTC).local().format('LL') + ' </span> <span> <small> ' + new moment.utc(d.dateTimeUTC).local().format('LT') + '</small></span></div> </td>';
        //txt += '  <td class="text-left">' + d.marketCode + '</td>';
        txt += '   <td> <span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + d.tradeId + '" data-bs-original-title="" title="">' + d.tradeId.split('-')[0].toUpperCase() + '..' + '</span>  </td>';
        if (d.orderSide == 0)
            txt += '    <td class="basegreen">' + d.orderSideT + '</td>';
        else
            txt += '    <td class="basered">' + d.orderSideT + '</td>';

        txt += '    <td>' + d.orderTypeT + '</td>';

        txt += '   <td><div class="datettrade44"><span>' + GetFormatedVal(d.tradePrice, __qf) + '</span> <small> ' + __q + ' </small></div></td>';
        txt += '    <td><div class="datettrade44"><span>' + GetFormatedVal(d.tradeVolumn, __bf) + '</span> <small>' + __b + '</small></div></td>';
        txt += '   <td nowrap="nowrap"><div class="datettrade44"><span>' + GetFormatedVal(d.tradeValue, __qf) + '</span><small> ' + __q + '</small></div></td>';
        //txt += '    <td>' + GetFormatedVal(d.currentVolume * d.price, __qf) + ' <small> ' + __q + '</small></td>';

        //txt += '    <td nowrap="nowrap">' + d._OrderSwapTradeValue + ' <small> USDT</small></td>';
        //if (d._OrderTrigger > 0) {
        //    txt += '    <td>' + d._OrderTrigger + ' </td>';
        //} else {
        //    txt += '    <td>N/A </td>';
        //}
        txt += '    <td>' + GetFormatedVal(d._AssetAmountValue, __qf) +' </td>';
        txt += '    <td><div class="datettrade44"><span>' + GetFormatedVal(d._CashBackTechnoSavvyValue, __qf) + '</span><small> USDT</small></div> </td>';
        txt += '    <td>' + GetFormatedVal(d._PoolRefund, __qf) +' </td>';
        txt += '    <td>' + GetFormatedVal((d._AssetAmount * d.tradePrice) + d._CashBackTechnoSavvyValue, __qf) +' </td>';
        txt += '   <td> <span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + d.orderID + '" data-bs-original-title="" title="">' + d.orderID.split('.')[2] + '-' + d.orderID.split('.')[3] + '</span>  </td>';
        //txt += '<td nowrap="nowrap"> ' + d.statusT + '</td></tr>';
    }
    var obj = $('#tbTradeHist');
    obj.html(txt);
    }
}
function XXupdateTradeList(data) {
    var txt = '';
    if (lp > data.tradePrice) {
        txt = '<ul class="row-div" ><li class="green">' + GetFormatedValWithSep(data.tradePrice, __qf) + ' </li><li> ' + GetFormatedVal(data.tradeVolumn, __bf) + ' </li><li> ' + moment.utc(data.dateTimeUTC.toString()).local().format('hh:mm:ss') + ' </li></ul>';
    }
    else {
        txt = '<ul class="row-div" ><li class="green">' + GetFormatedValWithSep(data.tradePrice, __qf) + ' </li><li> ' + GetFormatedVal(data.tradeVolumn, __bf) + ' </li><li> ' + moment.utc(data.dateTimeUTC.toString()).local().format('hh:mm:ss') + ' </li></ul>';
    }
    var obj = $('#tradetab77');
    if (obj.children().length >= 14)
        obj.children()[obj.children().length - 1].remove();
    obj.append(txt);
    obj.children()[1].before(obj.children()[obj.children().length - 1]);

    var i = new moment.utc(data.dateTimeUTC).local().startOf('second').fromNow();
    //console.log(i + ' | ' + data.tradeID.toString());
}

function ooCount(x) {
    var ooLength = $('#tbopenorder tr').length;
    $('#ooCount').html(x.length);
}

