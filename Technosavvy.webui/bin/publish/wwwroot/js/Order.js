//------- Market Buy----

function PlaceMarketBuyOrder() {
    if (!ValidateMarketBuy()) return;
    var amount = $('#mbTotal').val();

    var data = BuildMarketBuyOrderPackage();
    $.ajax({
        type: "POST",
        url: '/Order/MarketOrderBuy',
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
            //alert("danger");
           // $('.input-btn-css').removeClass('inputDisabled');
        }
    });
    $(this).parent('.input-btn-css').addClass('inputDisabled');
    
}
function UpdateQuoteQty(Qty) {
    var allowedamount = $('#nquoteavailable').val();
    allowedamount = +allowedamount + Qty;
    $("#mbqa").text(allowedamount); 
    $('#nquoteavailable').val(allowedamount);
    __qa = allowedamount;
    return allowedamount;
}
function UpdateBaseQty(Qty) {
    var allowedamount = $('#nbaseavailable').val();
    allowedamount = +allowedamount + Qty;
    $("#msba").text(allowedamount); 
    $('#nbaseavailable').val(allowedamount);
    _ba = allowedamount;
    return allowedamount;
}
function ValidateMarketBuy() {
    var iserror = false;
    var amount = $('#mbAmount').val();
    var allowedamount = $('#nquoteavailable').val();
    if (+amount <= 0) {
        $('#mbAmountV').text('Invalid Amount');
        $("#mbAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#mbAmountV").addClass("d-none"); }
    var price = lastPrice;//= $('#mbPrice').val();
    if (price <= 0) { $('#mbPriceV').text('Invalid Price'); $("#mbPriceV").removeClass("d-none"); iserror = true; } else { $("#mbPriceV").addClass("d-none"); }
    var total = $('#mbTotal').val();
   // var tReserve = (+amount * price) * 1.05;
    if ((total <= 0) || (+total > +allowedamount)) { $('#mbTotalV').text('Insufficent/Invalid Total'); $("#mbTotalV").removeClass("d-none"); iserror = true; } else { $("#mbTotalV").addClass("d-none"); }

    return !iserror;
}
var mcode = ''; var bcode = ''; var qcode = '';
function BuildMarketBuyOrderPackage() {
    var amount = $('#mbAmount').val();
    var price = lastPrice;
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
}
function MarketBuyRest() {
    
    $("#mbAmount").val('');
    $("#mbTotal").val('');
    $("#mbTechnoSavvyV").val('');
    $("#mbSTV").val('');
    $("#mbassamt").val('');
    $("#mbPriceV").addClass("d-none");
    $("#mbAmountV").addClass("d-none");
    $("#mbTotalV").addClass("d-none");
}
function MarketBuyUpdate1() {
   // p = lastPrice;
    if ($("#mbPrice").val() == '')
        $("#mbPrice").val(lastPrice);
    p = $("#mbPrice").val();

    a = $("#mbAmount").val();
    t = $("#mbTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#mbTotal").val(t);
    MarketBuyInfo();
}
function MarketBuyUpdate2() {
    p = lastPrice;
    //p = $("#lbPrice").val();
    a = $("#mbAmount").val();
    t = $("#mbTotal").val();
    if (p >= 0 && t >= 0)
        a = t / p;
    $("#mbAmount").val(GetFormatedVal(a, __qf));
    MarketBuyInfo();
}

function MarketBuyInfo() {
    var v = $("#mbAmount").val();
    if (v > 0) {
        v -= v * _bsr;
    }
    else v = 0;
    
        $("#mbassamt").html(GetFormatedVal(v, __bf) + ' ' + __b);

    var n = (_cbt / _bsr) * (_bsr * $("#mbTotal").val());
    //should be HTML
    if (__b == 'TechnoSavvy') {
        $("#mbTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#mbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
        /*$("#mbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);*/

    console.log('Market Order', n, v);
    var nn = (v * $("#mbPrice").val()) + n;
    $("#mbSTV").html(GetFormatedVal(nn, __qf) + ' ' + __q);
}
$("#mbPrice").keyup(MarketBuyUpdate1);
$("#mbAmount").keyup(MarketBuyUpdate1);
$("#mbTotal").keyup(MarketBuyUpdate2);
$("#mbPrice").change(MarketBuyUpdate1);
$("#mbAmount").change(MarketBuyUpdate1);
$("#mbTotal").change(MarketBuyUpdate2);
$("#btnBuyMarket").click(PlaceMarketBuyOrder);
$("#btnBuyMarketReset").click(MarketBuyRest);
MarketBuyInfo();
MarketSellInfo();
//------- Market Sell----
function PlaceMarketSellOrder() {
    if (!ValidateMarketSell()) return;
    var data = BuildMarketSellOrderPackage(); 
    var amount = $('#msTotal').val();

    $.ajax({
        type: "POST",
        url: '/Order/MarketOrderSell',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        success: function (d) {
            if (d.item1 == 'true' || d.item1 == true) {
               // console.log(_q + ' available:' + UpdateBaseQty(-amount));
                TupleMsg(d, true);
            }
            else {
                TupleEr(d, true);
            }
        },
        error: function (error) {
            console.log(error.statusCode + ':' + error.statusText + ':' + error.responseText);
            //alert("danger");
            //$('.input-btn-css').removeClass('inputDisabled');
        }
    });
    $(this).parent('.input-btn-css').addClass('inputDisabled');
    
}
function ValidateMarketSell() {
    var iserror = false;
    var amount = $('#msAmount').val();
    var allowedamount = $('#nbaseavailable').val();
    if (+amount <= 0||+amount> allowedamount) {
        $('#msAmountV').text('Invalid Amount');
        $("#msAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#msAmountV").addClass("d-none"); }
    var price = lastPrice;// $('#msPrice').val();
    if (price <= 0) {
        $('#msPriceV').text('Invalid Price'); $("#msPriceV").removeClass("d-none"); iserror = true;
    } else { $("#msPriceV").addClass("d-none"); }
    var total = $('#msTotal').val();
    if (total <= 0 &&(+amount*price)<=0) { $('#msTotalV').text('Invalid Total'); $("#msTotalV").removeClass("d-none"); iserror = true; } else { $("#msTotalV").addClass("d-none"); }

    return !iserror;
}
function BuildMarketSellOrderPackage() {
    var amount = $('#msAmount').val();
    var price = lastPrice;
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
}
function MarketSellRest() {
    $("#msAmount").val('');
    $("#msTotal").val('');
    $("#msTechnoSavvyV").val('');
    $("#msSTV").val('');
    $("#msassamt").val('');
    $("#msPriceV").addClass("d-none");
    $("#msAmountV").addClass("d-none");
    $("#msTotalV").addClass("d-none");
}
function MarketSellUpdate1()
{
   // p = lastPrice;

    if ($("#msPrice").val() == '')
        $("#msPrice").val(lastPrice);
    p = $("#msPrice").val();

    a = $("#msAmount").val();
    t = $("#msTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#msTotal").val(t);
    MarketSellInfo();
}
function MarketSellUpdate2() {
    //p = lastPrice;
    if ($("#msPrice").val().length <= 0 && lastPrice > 0)
        $("#msPrice").val(lastPrice)
    p = $("#msPrice").val();

    a = $("#msAmount").val();
    t = $("#msTotal").val();

    if (p >= 0 && t >= 0)
        a = t / p;
    $("#msAmount").val(GetFormatedVal(a, __qf));
    MarketSellInfo();
}
function MarketSellInfo() {
    var v = $("#msTotal").val();
    if (v > 0) {
        v -= v * _asr;
    }
    else v = 0;
    $("#msassamt").html(GetFormatedVal(v, __qf) + ' ' + __q);
    
    var n = (_cbt / _asr) * (_asr * $("#msTotal").val());
    //should be HTML
    if (__b == 'TechnoSavvy') {
        $("#msTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#msTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
    /*$("#msTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);*/
    $("#msSTV").html(GetFormatedVal(v + n, __qf) + ' ' + __q);
}
function TupleMsg(d, o) {
   // console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
    if (o === true)
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
    $('.input-btn-css').removeClass('inputDisabled');
}
function TupleEr(d, o) {
    //console.error('Order Status:' + d.item1 + ', Reason:' + d.item2);
    if (o === true)
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
    //$('.input-btn-css').removeClass('inputDisabled');
    
}



$("#msPrice").prop('disabled', true);
$("#mbPrice").prop('disabled', true);

$("#msAmount").keyup(MarketSellUpdate1);
$("#msTotal").keyup(MarketSellUpdate2);
//$("#msPrice").change(MarketSellUpdate1);
$("#msAmount").change(MarketSellUpdate1);
$("#msTotal").change(MarketSellUpdate2);
$("#btnSellMarket").click(PlaceMarketSellOrder);
$("#btnSellMarketReset").click(MarketSellRest);