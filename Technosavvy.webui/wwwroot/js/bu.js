 

function AssetSelected() {
    var v = $('#selectedCoin').val()
    var v2 = $('#PaySelectedCoin').val()
    if (v.length > 0 && v2.length > 0) {
        $('#frmBuy').attr('action', '/Buy/');
        $('#frmBuy').submit();
    }
} 
$('#BuyAmount').on('change', function () {
    var v = $('#BuyAmount').val();
    if (v > 0) {
        $('#PayAmount').val(GetFormatedVal(v * rate, 3));
    }
});
$('#PayAmount').on('change', function () {
    var v = $('#PayAmount').val();
    if (v > 0) {
        $('#BuyAmount').val(GetFormatedVal(v / rate, 3) );
    }
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
$('#IsBankDeposits').on('change', function () {
    var v = $('#IsBankDeposits').prop('checked');
    $('#IsUPI').prop('checked', !v);
});
$('#IsUPI').on('change', function () {
    var v = $('#IsUPI').prop('checked');
    $('#IsBankDeposits').prop('checked', !v);
});
$(".view-more-details").slideUp();
$(".view-more").click(function () {
    $(".view-more-details").slideToggle();
});
$('input[name="previous"]').click(function () {
    var v = $('#isBack').val();
    $('#frmWithdraw').prop('action', v);
    $('#frmWithdraw').submit();
});