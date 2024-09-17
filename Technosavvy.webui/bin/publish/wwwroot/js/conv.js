
$('#PaySelectedCoin').on('change', AssetSelected);
$('#selectedCoin').on('change', AssetSelected);

function AssetSelected() {
    var v = $('#selectedCoin').val()
    var v2 = $('#PaySelectedCoin').val()
    if (v.length > 0 && v2.length > 0) {
        $('#frmConvert').attr('action', '/convert/');
        $('#frmConvert').submit();
    }
}
$('#BuyAmount').on('change', function () {
    var v = $('#BuyAmount').val();
    if (MinToken!='NaN' && MinToken > v) {
        ValMsg("Minimum Convert must be "+MinToken+' '+dd.q);
        return;
    }
    if (v > 0) {
        $('#PayAmount').val(GetFormatedVal(v * rate, qfv));
        $('#BuyAmount').val(GetFormatedVal(v, bfv));

        $.get(window.location.origin + '/Convert/Getgoingrate?b=' + b + '&q=' + q + '&Amt=' + v, data => UpdatePayAmount(data));

    }
});
$('#PayAmount').on('change', PayAmountChange);
function PayAmountChange() {
    var v = $('#PayAmount').val();
    if (v > 0) {
        $('#PayAmount').val(GetFormatedVal(v, qfv));
        $('#BuyAmount').val(GetFormatedVal(v / rate, bfv));
        var v = $('#BuyAmount').val();
        $.get(window.location.origin + '/Convert/Getgoingrate?b=' + b + '&q=' + q + '&Amt=' + v, data => UpdateBuyAmount(data));
    }
}
var dd = '';
function UpdatePayAmount(r) {
    ClearValMsg();
      dd = JSON.parse(r);
    rate = dd.Rate;
    MinToken = dd.MinimumTrade;
    $('#drate').html(GetFormatedVal(rate, qfv));
    var v = $('#BuyAmount').val();
    if (v > 0) {
        $('#PayAmount').val(GetFormatedVal(v * rate, qfv));
    }
} function UpdateBuyAmount(r) {
    ClearValMsg();
      dd = JSON.parse(r);
    rate = dd.Rate;
    $('#drate').html(GetFormatedVal(rate, qfv));
    var v = $('#PayAmount').val();
    if (v > 0) {
        var nv = GetFormatedVal(v / rate, bfv);
        $('#BuyAmount').val(nv);
        if (MinToken > nv) {
            ValMsg("Minimum Convert of 10 USDT");
        }
    }
}
$('#IsAll').on('change', function () {
    var v = $(this).is(':checked');
    if (v) {
        $('#abal').html(GetFormatedVal(+bal + +adBal, qfv));
        $('#PayAmount').val(GetFormatedVal(+bal + +adBal, qfv));
    }
    else {
        $('#abal').html(GetFormatedVal(bal, qfv));
        $('#PayAmount').val(GetFormatedVal(bal, qfv));
    }
    PayAmountChange();
});


function INRStep2() {
    var err = false; ClearValMsg();
    var v = $('#selectedCoin').val()

    if (!$('#selectedCoin').val().length > 0) {
        ValMsg("Please select a 'Token'");
        err = true;
    }
    if (!$('#Amount').val() > 0) {
        ValMsg("Please enter an 'Amount'");
        err = true;
    }
    //if (!($('#Amount').val() > MinCount && $('#Amount').val() < MaxCount)) {
    //    ValMsg("Please enter an 'Amount' between " + MinCount + ' and ' + MaxCount);
    //    err = true;
    //}
    if ($('#IsFiat').val() == 'True') {
        var v = ($('#IsUPI').prop('checked') || $('#IsBankDeposits').prop('checked'));
        if (!v) {
            ValMsg("Please select a 'Deposit Method'");
            err = true;
        }
    }
    else {
        if (!$('#selectedNetwork').val().length > 0) {
            ValMsg("Please select a 'Network'");
            err = true;
        }
    }
    if (err)
        return;
    $('#frmWithdraw').submit();

}
function ClearValMsg() { $('#Msg').html(''); $('#sMsg').html(''); }
function ValMsg(m) {
    var h = $('#Msg').html();
    m = '<div>' + m + '</div>'
    $('#Msg').html(h + m);
}


$('input[name="previous"]').click(function () {
    var v = $('#isBack').val();
    $('#frmWithdraw').prop('action', v);
    $('#frmWithdraw').submit();
});

function callValue() {
    var v = $('#BuyAmount').val();
    if (v > 0) {
        $.get(window.location.origin + '/Convert/Getgoingrate?b=' + b + '&q=' + q + '&Amt=' + v, data => UpdatePayAmount(data));
    }
}
var seconds = 10;
function updateTimer() {
    $('#countdowntimer').text(seconds);
    if (seconds === 0) {
        callValue();
    } else {
        seconds--;
    }
}
var timerInterval = setInterval(updateTimer, 1000);
setInterval(function () {
    seconds = 10;
    callValue();
}, 10000)