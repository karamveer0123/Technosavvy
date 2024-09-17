//------- Limit Buy----

var rn = 5;
function PlaceLimitBuyOrder() {
    if (!ValidateLimitBuy()) return;
    var amount = $('#lbTotal').val();

    var data = BuildLimitBuyOrderPackage();
    $.ajax({
        type: "POST",
        url: '/Order/LimitOrderBuy',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        success: function (d) {
            if (d.item1 == 'true' || d.item1 == true) {
                //console.log(_q + ' available:' + UpdateQuoteQty(-amount));
                LimitBuyRest();
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
function UpdateQuoteQty(Qty) {
    var allowedamount = $('#nquoteavailable').val();
    allowedamount = +allowedamount + Qty;
    $("#mbqa").text(allowedamount); 
    $('#nquoteavailable').val(allowedamount);
    _qa = allowedamount;
    return allowedamount;
}
function UpdateBaseQty(Qty) {
    var allowedamount = $('#nbaseavailable').val();
    allowedamount = +allowedamount + Qty;
    $("#lsba").text(allowedamount); 
    $('#nbaseavailable').val(allowedamount);
    _ba = allowedamount;
    return allowedamount;
}
function ValidateLimitBuy() {
    var iserror = false;
    var amount = $('#lbAmount').val();
    var allowedamount = $('#nquoteavailable').val();
    if (+amount <= 0) {
        $('#lbAmountV').text('Invalid Amount');
        $("#lbAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#lbAmountV").addClass("d-none"); }
    var price = $('#lbPrice').val();
    if (price <= 0) { $('#lbPriceV').text('Invalid Price'); $("#lbPriceV").removeClass("d-none"); iserror = true; } else { $("#lbPriceV").addClass("d-none"); }
    var total = $('#lbTotal').val();
    if (total <= 0 || +total > +allowedamount) { $('#lbTotalV').text('Insufficent/Invalid Total'); $("#lbTotalV").removeClass("d-none"); iserror = true; } else { $("#lbTotalV").addClass("d-none"); }

    return !iserror;
}
var mcode = ''; var bcode = ''; var qcode = '';
function BuildLimitBuyOrderPackage() {
    var amount = $('#lbAmount').val();
    var price = $('#lbPrice').val();
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 0, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
}
function LimitBuyRest() {
    $("#lbPrice").val('');
    $("#lbAmount").val('');
    $("#lbTotal").val('');
    $("#lbTechnoSavvyV").val('');
    $("#lbSTV").val('');
    $("#lbassamt").val('');
    $("#lbPriceV").addClass("d-none");
    $("#lbAmountV").addClass("d-none");
    $("#lbTotalV").addClass("d-none");
}
function LimitBuyUpdate1() {
    if ($("#lbPrice").val() == '')
        $("#lbPrice").val(lastPrice);
    p = $("#lbPrice").val();

    a = $("#lbAmount").val();
    t = $("#lbTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#lbTotal").val(t);
    LimitBuyInfo();
}
function LimitBuyUpdate2() {
    p = lastPrice; //$("#lbPrice").val();
    a = $("#lbAmount").val();
    t = $("#lbTotal").val();
    if (p >= 0 && t >= 0)
        a = t / p;
    $("#lbAmount").val(GetFormatedVal(a, __qf));
    LimitBuyInfo();
}
function LimitBuyInfo() {
    var v = $("#lbAmount").val();
    if (v > 0) {
        v -= v * _bsr;
    }
    else v = 0;    
    $("#lbassamt").html(GetFormatedVal(v, __bf) + ' ' + __b);

    var n = (_cbt/_bsr) * (_bsr * $("#lbTotal").val());
    //should be HTML
    if (__b == 'TechnoSavvy') {
        $("#lbTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#lbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
    /*$("#lbTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);*/
    var n = (v * $("#lbPrice").val()) + n;
   
    $("#lbSTV").html(GetFormatedVal(n, __qf) + ' ' + __q);
}
$("#lbPrice").keyup(LimitBuyUpdate1);
$("#lbAmount").focus(LimitBuyUpdate1);
$("#lbAmount").keyup(LimitBuyUpdate1);
$("#lbTotal").keyup(LimitBuyUpdate2);
$("#lbPrice").change(LimitBuyUpdate1);
$("#lbAmount").change(LimitBuyUpdate1);
$("#lbTotal").change(LimitBuyUpdate2);
$("#btnBuyLimit").click(PlaceLimitBuyOrder);
    
$("#btnBuyLimitReset").click(LimitBuyRest);

//------- Limit Sell----
function PlaceLimitSellOrder() {
    if (!ValidateLimitSell()) return;
    var data = BuildLimitSellOrderPackage();
    var amount = $('#lsTotal').val();

    $.ajax({
        type: "POST",
        url: '/Order/LimitOrderSell',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        success: function (d) {
            if (d.item1 == 'true' || d.item1 == true) {
                LimitSellRest();
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
function ValidateLimitSell() {
    var iserror = false;
    var amount = $('#lsAmount').val();
    var allowedamount = $('#nbaseavailable').val();
    if (+amount <= 0 ||+amount>allowedamount) {
        $('#lsAmountV').text('Invalid Amount');
        $("#lsAmountV").removeClass("d-none");
        iserror = true;
    } else { $("#lsAmountV").addClass("d-none"); }
    var price = $('#lsPrice').val();
    if (price <= 0) { $('#lsPriceV').text('Invalid Price'); $("#lsPriceV").removeClass("d-none"); iserror = true; } else { $("#lsPriceV").addClass("d-none"); }
    var total = $('#lsTotal').val();
    if (total <= 0) { $('#lsTotalV').text('Invalid Total'); $("#lsTotalV").removeClass("d-none"); iserror = true; } else { $("#lsTotalV").addClass("d-none"); }

    return !iserror;
}
function BuildLimitSellOrderPackage() {
    var amount = $('#lsAmount').val();
    var price = $('#lsPrice').val();
    if (+price > 0) {
        var data = { Amount: amount, Price: price, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
    else {
        var data = { Amount: amount, Price: 0, side: 1, mCode: __mCode, quoteCode: __q, qTag: _qTag, baseCode: __b, bTag: _bTag, OrderId: '?' };
        return data;
    }
}
function LimitSellRest() {
    $("#lsPrice").val('');
    $("#lsAmount").val('');
    $("#lsTotal").val('');
    $("#lsTechnoSavvyV").val('');
    $("#lsSTV").val('');
    $("#lsassamt").val('');
    $("#lsPriceV").addClass("d-none");
    $("#lsAmountV").addClass("d-none");
    $("#lsTotalV").addClass("d-none");

}
function LimitSellUpdate1() {
    if ($("#lsPrice").val() == '')
        $("#lsPrice").val(lastPrice);
    p = $("#lsPrice").val();
    a = $("#lsAmount").val();
    t = $("#lsTotal").val();
    if (p >= 0 && a >= 0)
        t = p * a;
    $("#lsTotal").val(t);
    LimitSellInfo();
}
function LimitSellUpdate2() {
    if ($("#lsPrice").val().length <= 0 && lastPrice > 0)
        $("#lsPrice").val(lastPrice)
    p = $("#lsPrice").val();
    a = $("#lsAmount").val();
    t = $("#lsTotal").val();

    if (p >= 0 && t >= 0)
        a = t / p;
    $("#lsAmount").val(GetFormatedVal(a, __qf));
    LimitSellInfo();
}
function LimitSellInfo() {
    var v = $("#lsTotal").val();
    if (v > 0) {
        v -= v * _asr;
    }
    else v = 0;
    $("#lsassamt").html(GetFormatedVal(v, __qf) + ' ' + __q);
    
    var n = (_cbt / _asr ) * (_asr * $("#lsTotal").val());
    //should be HTML
    if (__b == 'TechnoSavvy') {
        $("#lsTechnoSavvyV").html('--' + ' ' + __q);
    } else {
        $("#lsTechnoSavvyV").html(GetFormatedVal(n, __qf) + ' ' + __q);
    }
    $("#lsSTV").html(GetFormatedVal(v + n, __qf) + ' ' + __q);
}
function TupleMsg(d, o) {
   // console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
    if (o === true)
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
    $('.input-btn-css').removeClass('inputDisabled');
    
    
}
function TupleEr(d, o) {
   // console.error('Order Status:' + d.item1 + ', Reason:' + d.item2);
    if (o === true)
        console.log('Order Status:' + d.item1 + ', Reason:' + d.item2);
        //$('.input-btn-css').removeClass('inputDisabled');
    
    
}



LimitSellInfo();
LimitBuyInfo();
$("#lsPrice").keyup(LimitSellUpdate1);
$("#lsAmount").keyup(LimitSellUpdate1);
$("#lsAmount").focus(LimitSellUpdate1);
$("#lsTotal").keyup(LimitSellUpdate2);
$("#lsPrice").change(LimitSellUpdate1);
$("#lsAmount").change(LimitSellUpdate1);
$("#lsTotal").change(LimitSellUpdate2);
$("#btnSellLimit").click(PlaceLimitSellOrder);
$("#btnSellLimitReset").click(LimitSellRest);