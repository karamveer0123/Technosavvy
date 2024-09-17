//-------Stop Limit Buy----

var rn = 5;
function PlaceStopLimitBuyOrder() {
    if (!ValidateStopLimitBuy()) return;
    var amount = $('#slbTotal').val();

    var data = BuildStopLimitBuyOrderPackage();
    $.ajax({
        type: "POST",
        url: '/Order/StopLimitOrderBuy',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        success: function (d) {
            if (d.item1 == 'true' || d.item1 == true) {
                //console.log(_q + ' available:' + UpdateQuoteQty(-amount));
                StopLimitBuyRest();
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
//function UpdateQuoteQty(Qty) {
//    var allowedamount = $('#nquoteavailable').val();
//    allowedamount = +allowedamount + Qty;
//    $("#mbqa").text(allowedamount); // ToDo: Naveen, All Display Text should be covered
//    $('#nquoteavailable').val(allowedamount);
//    _qa = allowedamount;
//    return allowedamount;
//}
//function UpdateBaseQty(Qty) {
//    var allowedamount = $('#nbaseavailable').val();
//    allowedamount = +allowedamount + Qty;
//    $("#slsba").text(allowedamount); // ToDo: Naveen, All Display Text should be covered
//    $('#nbaseavailable').val(allowedamount);
//    _ba = allowedamount;
//    return allowedamount;
//}
function ValidateStopLimitBuy() {
    var iserror = false;
    var stopPrice = $('#slbStopPrice').val();
    if (+stopPrice <= 0) {
        $('#slbStopPriceV').text('Invalid Trigger Price');
        $("#slbStopPriceV").removeClass("d-none");
        iserror = true;
    }
    var amount = $('#slbAmount').val();
    var allowedamount = $('#nquoteavailable').val();
    if (+amount <= 0) {
        $('#slbAmountV').text('Invalid Amount');
        $("#slbAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#slbAmountV").addClass("d-none"); }
    var price = $('#slbPrice').val();
    if (price <= 0) { $('#slbPriceV').text('Invalid Price'); $("#slbPriceV").removeClass("d-none"); iserror = true; } else { $("#slbPriceV").addClass("d-none"); }
    var total = $('#slbTotal').val();
    if (total <= 0 || +total > +allowedamount) { $('#slbTotalV').text('Insufficent/Invalid Total'); $("#slbTotalV").removeClass("d-none"); iserror = true; } else { $("#slbTotalV").addClass("d-none"); }

    var sp = $('#slbStopPrice').val();
    if (sp <= lastPrice) {
        $('#slbStopPriceV').text('Invalid Price');
        $("#slbStopPriceV").removeClass("d-none");
        iserror = true;
    } else {
        $('#slbStopPriceV').text('');
        $("#slbStopPriceV").addClass("d-none");
    }   

    return !iserror;
}
var mcode = ''; var bcode = ''; var qcode = '';
function BuildStopLimitBuyOrderPackage() {
    var amount = $('#slbAmount').val();
    var price = $('#slbPrice').val();
    var stopPrice = $('#slbStopPrice').val();
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice:stopPrice, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice:stopPrice, OrderId: '?' };
        return data;
    }
}
function StopLimitBuyRest() {
    $("#slbStopPrice").val('');
    $("#slbPrice").val('');
    $("#slbAmount").val('');
    $("#slbTotal").val('');
    $("#slbTechnoSavvyV").val('');
    $("#slbSTV").val('');
    $("#slbassamt").val('');
    $("#slbStopPriceV").addClass("d-none");
    $("#slbPriceV").addClass("d-none");
    $("#slbAmountV").addClass("d-none");
    $("#slbTotalV").addClass("d-none");
}
function StopLimitBuyUpdate1() {
    if ($("#slbPrice").val() == '')
        $("#slbPrice").val(lastPrice);
    p = $("#slbPrice").val();

    a = $("#slbAmount").val();
    t = $("#slbTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#slbTotal").val(t);
    StopLimitBuyInfo();
}
function StopLimitBuyUpdate2() {
    if ($("#slbPrice").val() == '')
        $("#slbPrice").val(lastPrice);    
    p = $("#slbPrice").val();
    a = $("#slbAmount").val();
    t = $("#slbTotal").val();
    if (p >= 0 && t >= 0)
        a = t / p;
    $("#slbAmount").val(GetFormatedVal(a, __qf));
    StopLimitBuyInfo();
}
function StopLimitBuyInfo() {
    var v = $("#slbAmount").val();
    if (v > 0) {
        v -= v * _bsr;
    }
    else v = 0;
    
    $("#slbassamt").html(GetFormatedVal(v, __bf) + ' ' + __b);

    var n = (_cbt / _bsr) * (_bsr * $("#slbTotal").val());
    //should be HTML
    if (__b == 'TechnoSavvy') {
        $("#slbTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#slbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
    /*$("#slbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);*/
    var n = (v * $("#slbPrice").val()) + n;    
    $("#slbSTV").html(GetFormatedVal(n, __qf) + ' ' + __q);
}
function StopPriceLimitBuyInfo() {
    var sp = $('#slbStopPrice').val();    
    if (+sp <= lastPrice) {
        $('#slbStopPriceV').text('Invalid Price');
        $("#slbStopPriceV").removeClass("d-none");
    } else {
        $('#slbStopPriceV').text('');
        $("#slbStopPriceV").addClass("d-none");
    }   
}
$('#slbStopPrice').keyup(StopPriceLimitBuyInfo);
$("#slbPrice").keyup(StopLimitBuyUpdate1);
$("#slbAmount").focus(StopLimitBuyUpdate1);
$("#slbAmount").keyup(StopLimitBuyUpdate1);
$("#slbTotal").keyup(StopLimitBuyUpdate2);
$("#slbPrice").change(StopLimitBuyUpdate1);
$("#slbAmount").change(StopLimitBuyUpdate1);
$("#slbTotal").change(StopLimitBuyUpdate2);
$("#btnBuyStopLimit").click(PlaceStopLimitBuyOrder);
$("#btnBuyStopLimitReset").click(StopLimitBuyRest);

//------- Limit Sell----
function PlaceStopLimitSellOrder() {
    if (!ValidateStopLimitSell()) return;
    var data = BuildStopLimitSellOrderPackage();
    var amount = $('#slsTotal').val();

    $.ajax({
        type: "POST",
        url: '/Order/StopLimitOrderSell',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        success: function (d) {
            if (d.item1 == 'true' || d.item1 == true) {
                StopLimitSellRest();
                // console.log(__q + ' available:' + UpdateBaseQty(-amount));
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
function ValidateStopLimitSell() {
    var iserror = false;
    var stopPrice = $('#slsStopPrice').val();
    if (+stopPrice <= 0)
    {
        $('#slsStopPriceV').text('Invalid Trigger Price');
        $("#slsStopPriceV").removeClass("d-none");
        iserror = true;
    }
    var amount = $('#slsAmount').val();
    var allowedamount = $('#nbaseavailable').val();
    if (+amount <= 0 ||+amount>allowedamount) {
        $('#slsAmountV').text('Invalid Amount');
        $("#slsAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#slsAmountV").addClass("d-none"); }
    var price = $('#slsPrice').val();
    if (price <= 0) { $('#slsPriceV').text('Invalid Price'); $("#slsPriceV").removeClass("d-none"); iserror = true; } else { $("#slsPriceV").addClass("d-none"); }
    var total = $('#slsTotal').val();
    if (total <= 0) { $('#slsTotalV').text('Invalid Total'); $("#slsTotalV").removeClass("d-none"); iserror = true; } else { $("#slsTotalV").addClass("d-none"); }

    var sp = $('#slsStopPrice').val();
    if (+sp >= lastPrice) {
        $('#slsStopPriceV').text('Invalid Price');
        $("#slsStopPriceV").removeClass("d-none");
        iserror = true;
    } else {
        $('#slsStopPriceV').text('');
        $("#slsStopPriceV").addClass("d-none");
    }


    return !iserror;
}
function BuildStopLimitSellOrderPackage() {
    var amount = $('#slsAmount').val();
    var price = $('#slsPrice').val();
    var stopPrice = $('#slsStopPrice').val();
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice:stopPrice, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, stopPrice:stopPrice, OrderId: '?' };
        return data;
    }
}
function StopLimitSellRest() {
    $("#slsStopPrice").val('');
    $("#slsPrice").val('');
    $("#slsAmount").val('');
    $("#slsTotal").val('');
    $("#slsTechnoSavvyV").val('');
    $("#slsSTV").val('');
    $("#slsassamt").val('');
    $("#slsStopPriceV").addClass("d-none");
    $("#slsPriceV").addClass("d-none");
    $("#slsAmountV").addClass("d-none");
    $("#slsTotalV").addClass("d-none");

}
function StopLimitSellUpdate1() {
    if ($("#slsPrice").val() == '')
        $("#slsPrice").val(lastPrice);
    p = $("#slsPrice").val();
    a = $("#slsAmount").val();
    t = $("#slsTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#slsTotal").val(t);
    StopLimitSellInfo();
}
function StopLimitSellUpdate2() {
    if ($("#slsPrice").val().length <= 0 && lastPrice > 0)
        $("#slsPrice").val(lastPrice)    
    p = $("#slsPrice").val();
    a = $("#slsAmount").val();
    t = $("#slsTotal").val();

    if (p >= 0 && t >= 0)
        a = t / p;
    $("#slsAmount").val(GetFormatedVal(a, __qf));
    StopLimitSellInfo();
}
function StopLimitSellInfo() {
    var v = $("#slsTotal").val();
    if (v > 0) {
        v -= v * _asr;
    }
    else v = 0;
    $("#slsassamt").html(GetFormatedVal(v, __qf) + ' ' + __q);
    
    var n = (_cbt / _asr) * (_asr * $("#slsTotal").val());
    //should be HTML
    if (__b == "TechnoSavvy") {
        $("#slsTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#slsTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
    
    $("#slsSTV").html(GetFormatedVal(v + n, __qf) + ' ' + __q);
}
function TupleMsg(d, o) {
   // console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
    if (o === true) {
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
        $('.input-btn-css').removeClass('inputDisabled');
    }
}
function TupleEr(d, o) {
   // console.error('Order Status:' + d.item1 + ', Reason:' + d.item2);
    if (o === true) {
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
       // $('.input-btn-css').removeClass('inputDisabled');
    }
}
function StopPriceLimitSellInfo() {
    var sp = $('#slsStopPrice').val();
    if (+sp >= lastPrice) {
        $('#slsStopPriceV').text('Invalid Price');
        $("#slsStopPriceV").removeClass("d-none");
    } else {
        $('#slsStopPriceV').text('');
        $("#slsStopPriceV").addClass("d-none");
    }
}


StopLimitSellInfo();
StopLimitBuyInfo();
$('#slsStopPrice').keyup(StopPriceLimitSellInfo)
$("#slsPrice").keyup(StopLimitSellUpdate1);
$("#slsAmount").keyup(StopLimitSellUpdate1);
$("#slsAmount").focus(StopLimitSellUpdate1);
$("#slsTotal").keyup(StopLimitSellUpdate2);
$("#slsPrice").change(StopLimitSellUpdate1);
$("#slsAmount").change(StopLimitSellUpdate1);
$("#slsTotal").change(StopLimitSellUpdate2);
$("#btnSellStopLimit").click(PlaceStopLimitSellOrder);
$("#btnSellStopLimitReset").click(StopLimitSellRest);