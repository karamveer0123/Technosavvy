Loadmeta();
var tkList = [];
var cuList = [];
function Loadmeta() {
    $.get(window.location.origin + '/deposit/depositmeta',
        function (data) {
            let opt = JSON.parse(data);
            console.log(opt);
            tkList = opt.TokenList;
            cuList = opt.CurrencyList;
        }
    );
}

function AssetSelected() {
    var v = $('#selectedCoin').val()
    $('#IsAll').prop('checked', false);
    var networkOpt = '';
    for (var i = 0; i < tkList.length; i++) {
        if (tkList[i].TokenId == v) {
            if (tkList[i].SupportedCoin.length > 0) {
                if ($('#IsFiat').val() == 'True') {
                    $('#IsFiat').val(false);
                    $('#Amount').val(0);
                    $('#frmWithdraw').prop('action', 'mtype');
                    $('#frmWithdraw').submit();
                }
                $('#abal').html('');
                $('#Amount').val(0);
                $('#dFiat').hide();
                var b = window.location.origin + '/wallet/GetCoinBalInMyWallets?tokenId=' + v;
                $.get(b, data => showAvilableBal(data));
                var sCoin = tkList[i].SupportedCoin
                for (var i2 = 0; i2 < sCoin.length; i2++) {
                    {
                        var sNet = sCoin[i2].RelatedNetwork;
                        networkOpt += '<option data-img-src="/images/coin/' + sNet.NativeCurrencyCode.toUpperCase() + '.png" value="' + sNet.SupportedNetworkId + '">' + sNet.Name + '</option>';
                    }
                }
            }
        }
    }
    for (var i = 0; i < cuList.length; i++) {

        if (cuList[i].token.TokenId == v) {
            //it is fiat coin
            $('#IsFiat').val(true);
            $('#dFiat').show();
            $('#Amount').val(0);
            $('#frmWithdraw').attr('action', '/withdraw/mtype');
            $('#frmWithdraw').submit();
        }
    }
    $('#dWallet').addClass('d-none');

    if (networkOpt.length > 0) {
        //var obj = $('#selectedNetwork');
        //if (obj.length==0) {
        $('#dNetwork').empty();
        $('#dNetwork').removeClass('d-none');
        var txt = '<label class="lable-name22">Network</label > ';
        txt += '<div class="form-select3"><select id = "selectedNetwork" name = "selectedNetwork" onchange="NetworkSelected()"></select></div>';
        $('#dNetwork').append(txt);
        $('#selectedNetwork').append('<option>', networkOpt);
        $('#selectedNetwork').addClass('my-select');
        $('#selectedNetwork').chosen({ width: "100%" });
    }
    else {
        $('#dNetwork').empty();
        $('#dNetwork').addClass('d-none');
    }
}
function NetworkSelected() {
    var nId = $('#selectedNetwork').val();
    var tid = $('#selectedCoin').val();

    $('#dWallet').removeClass('d-none');
    $.get(window.location.origin + '/Addbook/GetMyNetWhiteList?networkid=' + nId, data => ProcessAndDisplayAddBook(data));
    var a = window.location.origin + '/withdraw/WithDrawFee?nid=' + nId + '&tid=' + tid;
    $.get(a, data => shownetfee(data));
    console.debug(a);
}
var MinCount = -1;
var MaxCount = -1;
var ext = 0;
var bal = 0;
var tname = '';
function shownetfee(d) {
    console.debug(d);
    if (d != '' || d != null) {
        $('#nfee').html(d.withdrawalFee + ' ' + d.token.code);
        MaxCount = d.maxWithdrawal;
        MinCount = d.minWithdrawal;
    }
}
function showAvilableBal(da) {
    var d = JSON.parse(da);
    console.debug(d);
    bal = d.FundWallet.Amount;
    $('#Amount').val(bal);

    tname = d.FundWallet.Code;
    $('#abal').html(d.FundWallet.Amount + ' ' + d.FundWallet.Code);
    ext = d.SpotWallet.Amount + d.EarnWallet.Amount;
    $('#dAddi').html(ext + ' ' + d.FundWallet.Code);
    if (ext > 0) {
        AdditionalTokens(d);
        $('#dAddiAvaGrp').show()
    }
    else {
        $('#dAddiAvaGrp').hide()
    }
}
function GetAddressBook() {
    $('#AddressBook').toggle();
}
function ProcessAndDisplayAddBook(data) {
    $('#dWallet').removeClass('d-none');

    if (data == null || data == false || data == "false") {
        //No Data in AddressBook
    }
    else {
        let opt = JSON.parse(data);
        console.log(opt);
        var txt = '<div class="mx-2 my-2"><input type="search" class="form-control form-control-dark" placeholder="Search..." aria-label="Search"></div>';
        opt.forEach(function (x) {
            txt += '<div class="selectdiv43">';
            txt += '<div class="form-check">';
            txt += '<input class="form-check-input ab" type="checkbox" value="" >';
            txt += '<label class="checkbox-custom-label">';
            txt += ' <div class="flexwsbox435">';
            txt += '<span>' + x.Name + '</span> <span class="smalltext ad">' + x.Address + ' </span>';
            txt += '</div>';
            txt += '<div class="Csswd45r"> <span>' + x.Network + '</span></div>';
            txt += '</label>';
            txt += '</div>';
            txt += '</div>';
        })
        $('#ddlAddBook').html(txt);

        $('.ab').on('change', function () {
            var element = $(this);
            element.prop('tt', true);
            var checked = $(this).is(':checked');
            var checkedText = $(this).next().find('.ad').text().trimEnd();
            if (checked) {
                {
                    $('#dAddTo').hide();
                    $('#AddToAddressBook').prop('checked', false);

                    $('#ReceiverAddr').val(checkedText);
                    $('.ab').each(function (i, el) {
                        if ($(el).prop('tt') != true) {
                            $(el).prop('checked', false);
                        }
                    });
                    $(element).prop('tt', false);
                }
            }
        });
    }
}
function AdditionalTokens(d) {
    var txt = ' <div class="selord43ews">'
    if (d.EarnWallet.Amount > 0) {
        txt += '<div class="methdiv432"><div class="form-check dflex20div position-relative"><input class="form-check-input aw" type="checkbox" data-myval=' + d.EarnWallet.Amount + ' id="earnwallet44"><label class="form-check-label" for="earnwallet44">Earn Wallet<span>Include ' + d.EarnWallet.Amount + ' ' + d.EarnWallet.Code + ' balance of Earn wallet.</span><div class="metfee">Avl bal: ' + d.EarnWallet.Amount + ' ' + d.EarnWallet.Code + ' </div></label></div><div class="metfee">Avl bal: ' + d.EarnWallet.Amount + ' ' + d.EarnWallet.Code + ' </div></div > ';
    }
    if (d.SpotWallet.Amount > 0) {
        txt += '<div class="methdiv432"><div class="form-check dflex20div position-relative"><input class="form-check-input aw" type="checkbox" data-myval=' + d.SpotWallet.Amount + ' id="Spowallet44"><label class="form-check-label" for="spotwallet44">Spot Wallet<span>Include ' + d.SpotWallet.Amount + ' ' + d.SpotWallet.Code + ' balance of Spot wallet.</span><div class="metfee">Avl bal: ' + d.SpotWallet.Amount + ' ' + d.SpotWallet.Code + ' </div></label></div><div class="metfee">Avl bal: ' + d.SpotWallet.Amount + ' ' + d.SpotWallet.Code + ' </div></div > ';
    }
    txt += '</div>';
    $('#wallet3288').html(txt);
}

$('#IsAll').on('change', function () {
    var v = $(this).is(':checked');
    if (bal <= 0) { bal = $('#bal').val(); $('#bal').val(0); }
    if (ext <= 0) { ext = $('#AddB').val(); $('#AddB').val(0) }
    if (tname.length <= 0) { tname = $('#na').val(); $('#na').val('') }
    if (v) {
        $('#abal').html(+bal + +ext + ' ' + tname);
        $('#Amount').val(+bal + +ext);
    }
    else {
        $('#abal').html(bal + ' ' + tname);
        $('#Amount').val(bal);
    }
});
$('#ReceiverAddr').on('change', function () {
    var any = false;
    $('.ad').each(function (i, el) {
        if ($('#ReceiverAddr').val() == el.textContent)
            any = true;
    });
    if (any == false) {
        $('#dAddTo').show();
    }
    else
        $('#dAddTo').hide();
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