//-------Stop Market Buy----

function PlaceStopMarketBuyOrder() {
    if (!ValidateStopMarketBuy()) return;
    var amount = $('#smbTotal').val();

    var data = BuildStopMarketBuyOrderPackage();
    $.ajax({
        type: "POST",
        url: '/Order/StopMarketOrderBuy',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        success: function (d) {
            if (d.item1 == 'true' || d.item1 == true) {
                // console.log(_q + ' available:' + UpdateQuoteQty(-amount));
                TupleMsg(d, true);
            }
            else {
                TupleEr(d, true);
            }

        },
        error: function (error) {
            console.log(error.statusCode + ':' + error.statusText + ':' + error.responseText);
           // alert("danger");
           // $('.input-btn-css').removeClass('inputDisabled');
        }
    });
    $(this).parent('.input-btn-css').addClass('inputDisabled');
    
}

function ValidateStopMarketBuy() {
    var iserror = false;
    var stopPrice = $('#smbStopPrice').val();
    if (+stopPrice <= 0) {
        $('#smbStopPriceV').text('Invalid Trigger Price');
        $("#smbStopPriceV").removeClass("d-none");
        iserror = true;
    }
    var amount = $('#smbAmount').val();
    var allowedamount = $('#nquoteavailable').val();
    if (+amount <= 0) {
        $('#smbAmountV').text('Invalid Amount');
        $("#smbAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#smbAmountV").addClass("d-none"); }
    var price = lastPrice;//= $('#smbPrice').val();
    if (price <= 0) { $('#smbPriceV').text('Invalid Price'); $("#smbPriceV").removeClass("d-none"); iserror = true; } else { $("#smbPriceV").addClass("d-none"); }
    var total = $('#smbTotal').val();
    // var tReserve = (+amount * price) * 1.05;
    if (total <= 0 || (+total > +allowedamount)) { $('#smbTotalV').text('Insufficent/Invalid Total'); $("#smbTotalV").removeClass("d-none"); iserror = true; } else { $("#smbTotalV").addClass("d-none"); }

    var smp = $('#smbStopPrice').val();
    if (smp >= lastPrice) {
        $('#smbStopPriceV').text('Invalid Price');
        $('#smbStopPriceV').removeClass("d-none");
        iserror = true;
    } else {
        $('#smbStopPriceV').text('');
        $('#smbStopPriceV').addClass("d-none");
    }
    return !iserror;
}
var mcode = ''; var bcode = ''; var qcode = '';
function BuildStopMarketBuyOrderPackage() {
    var stopPrice = $('#smbStopPrice').val();
    var amount = $('#smbAmount').val();
    var price = lastPrice;
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice: stopPrice, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice: stopPrice, OrderId: '?' };
        return data;
    }
}
function StopMarketBuyRest() {

    $('#smbStopPrice').val('');
    $("#smbAmount").val('');
    $("#smbTotal").val('');
    $("#smbTechnoSavvyV").val('');
    $("#smbSTV").val('');
    $("#smbassamt").val('');
    $("#smbStopPriceV").addClass("d-none");
    $("#smbPriceV").addClass("d-none");
    $("#smbAmountV").addClass("d-none");
    $("#smbTotalV").addClass("d-none");
}
function StopMarketBuyUpdate1() {
    //p = lastPrice;
    if ($("#smbPrice").val() == '')
        $("#smbPrice").val(lastPrice);
    p = $("#smbPrice").val();
    a = $("#smbAmount").val();
    t = $("#smbTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#smbTotal").val(t);
    StopMarketBuyInfo();
}
function StopMarketBuyUpdate2() {
    //p = lastPrice;
    if ($("#smbPrice").val() == '')
        $("#smbPrice").val(lastPrice);
    p = $("#smbPrice").val();
    a = $("#smbAmount").val();
    t = $("#smbTotal").val();
    if (p >= 0 && t >= 0)
        a = t / p;
    $("#smbAmount").val(GetFormatedVal(a, __qf));
    StopMarketBuyInfo();
}

function StopMarketBuyInfo() {
    var v = $("#smbAmount").val();
    if (v > 0) {
        v -= v * _bsr;
    }
    else v = 0;    
    $("#smbassamt").html(GetFormatedVal(v, __bf) + ' ' + __b);

    var n = (_cbt / _bsr) * (_bsr * $("#smbTotal").val());
    //should be HTML
    if (__b == 'TechnoSavvy') {
        $("#smbTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#smbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
    /*$("#smbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);*/
    var n = (v * $("#smbPrice").val()) + n;    
    $("#smbSTV").html(GetFormatedVal(n, __qf) + ' ' + __q);
}
function StopMarketPriceBuyInfo() {
    var smp = $('#smbStopPrice').val();    
    if (smp >= lastPrice) {
        $('#smbStopPriceV').text('Invalid Price');
        $('#smbStopPriceV').removeClass("d-none");
        iserror = true;
    } else {
        $('#smbStopPriceV').text('');
        $('#smbStopPriceV').addClass("d-none");
    }
}
$('#smbStopPrice').keyup(StopMarketPriceBuyInfo);
$("#smbPrice").keyup(StopMarketBuyUpdate1);
$("#smbAmount").keyup(StopMarketBuyUpdate1);
$("#smbTotal").keyup(StopMarketBuyUpdate2);
$("#smbPrice").change(StopMarketBuyUpdate1);
$("#smbAmount").change(StopMarketBuyUpdate1);
$("#smbTotal").change(StopMarketBuyUpdate2);
$("#btnBuyStopMarket").click(PlaceStopMarketBuyOrder);
$("#btnBuyStopMarketReset").click(StopMarketBuyRest);
StopMarketBuyInfo();
StopMarketSellInfo();
//------- Market Sell----
function PlaceStopMarketSellOrder() {
    if (!ValidateStopMarketSell()) return;
    var data = BuildStopMarketSellOrderPackage();
    var amount = $('#smsTotal').val();

    $.ajax({
        type: "POST",
        url: '/Order/StopMarketOrderSell',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        success: function (d) {
            if (d.item1 == 'true' || d.item1 == true) {
                //console.log(_q + ' available:' + UpdateBaseQty(-amount));
                TupleMsg(d, true);
            }
            else {
                TupleEr(d, true);
            }
        },
        error: function (error) {
            console.log(error.statusCode + ':' + error.statusText + ':' + error.responseText);
          //  alert("danger");
           // $('.input-btn-css').removeClass('inputDisabled');
        }
    });
    $(this).parent('.input-btn-css').addClass('inputDisabled');
    
}
function ValidateStopMarketSell() {
    var iserror = false;
    var stopPrice = $('#smsStopPrice').val();
    if (+stopPrice <= 0) {
        $('#smsStopPriceV').text('Invalid Trigger Price');
        $("#smsStopPriceV").removeClass("d-none");
        iserror = true;
    }
    var amount = $('#smsAmount').val();
    var allowedamount = $('#nbaseavailable').val();
    if (+amount <= 0 || _ab < +amount) {
        $('#smsAmountV').text('Invalid Amount');
        $("#smsAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#smsAmountV").addClass("d-none"); }
    var price = lastPrice;// $('#smsPrice').val();
    if (price <= 0) {
        $('#smsPriceV').text('Invalid Price'); $("#smsPriceV").removeClass("d-none"); iserror = true;
    } else { $("#smsPriceV").addClass("d-none"); }
    var total = $('#smsTotal').val();
    if (total <= 0 || (+amount * price) <= 0) {
        $('#smsTotalV').text('Invalid Total');
        $("#smsTotalV").removeClass("d-none");
        iserror = true;
    } else { $("#smsTotalV").addClass("d-none"); }

    var smp = $('#smsStopPrice').val();
    if (smp <= lastPrice) {
        $('#smsStopPriceV').text('Invalid Price');
        $('#smsStopPriceV').removeClass("d-none");
        iserror = true;
    } else {        
        $('#smsStopPriceV').addClass("d-none");
    }
    return !iserror;
}
function BuildStopMarketSellOrderPackage() {
    var amount = $('#smsAmount').val();
    var stopPrice = $('#smsStopPrice').val();

    var price = lastPrice;
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice: stopPrice, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice: stopPrice, OrderId: '?' };
        return data;
    }
}
function StopMarketSellRest() {
    $('#smsStopPrice').val('');
    $("#smsAmount").val('');
    $("#smsTotal").val('');
    $("#smsTechnoSavvyV").val('');
    $("#smsSTV").val('');
    $("#smsassamt").val('');
    $("#smsStopPriceV").addClass("d-none");
    $("#smsPriceV").addClass("d-none");
    $("#smsAmountV").addClass("d-none");
    $("#smsTotalV").addClass("d-none");
}
function StopMarketSellUpdate1() {
    //p = lastPrice;
    if ($("#smsPrice").val() == '')
        $("#smsPrice").val(lastPrice);
    p = $("#smsPrice").val();

    a = $("#smsAmount").val();
    t = $("#smsTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#smsTotal").val(t);
    StopMarketSellInfo();
}
function StopMarketSellUpdate2() {
    //p = lastPrice;
    if ($("#smsPrice").val().length <= 0 && lastPrice > 0)
        $("#smsPrice").val(lastPrice)
    p = $("#smsPrice").val();


    a = $("#smsAmount").val();
    t = $("#smsTotal").val();

    if (p >= 0 && t >= 0)
        a = t / p;
    $("#smsAmount").val(GetFormatedVal(a, __qf));
    StopMarketSellInfo();
}
function StopMarketSellInfo() {
    var v = $("#smsTotal").val();
    if (v > 0) {
        v -= v * _asr;
    }
    else v = 0;
    $("#smsassamt").html(GetFormatedVal(v, __qf) + ' ' + __q);

    var n = (_cbt / _asr) * (_asr * $("#smsTotal").val());
    //should be HTML
    if (__b == 'TechnoSavvy') {
        $("#smsTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#smsTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
    
    $("#smsSTV").html(GetFormatedVal(v + n, __qf) + ' ' + __q);
}
function TupleMsg(d, o) {
   
    if (o === true) {
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
        $('.input-btn-css').removeClass('inputDisabled');
    }        
}
function TupleEr(d, o) {
   
    if (o === true) {
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
       // $('.input-btn-css').removeClass('inputDisabled');
    }        
}

function StopMarketPriceSellInfo() {
    var smp = $('#smsStopPrice').val();
    if (smp <= lastPrice) {
        $('#smsStopPriceV').text('Invalid Price');
        $('#smsStopPriceV').removeClass("d-none");
    } else {
        $('#smsStopPriceV').text('');
        $('#smsStopPriceV').addClass("d-none");
    }
}

$("#smsPrice").prop('disabled', true);
$("#smbPrice").prop('disabled', true);
$('#smsStopPrice').keyup(StopMarketPriceSellInfo)
$("#smsAmount").keyup(StopMarketSellUpdate1);
$("#smsTotal").keyup(StopMarketSellUpdate2);
//$("#smsPrice").change(StopMarketSellUpdate1);
$("#smsAmount").change(StopMarketSellUpdate1);
$("#smsTotal").change(StopMarketSellUpdate2);
$("#btnSellStopMarket").click(PlaceStopMarketSellOrder);
$("#btnSellStopMarketReset").click(StopMarketSellRest);

