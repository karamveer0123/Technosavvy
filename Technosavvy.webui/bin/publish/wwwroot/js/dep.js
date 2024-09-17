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
            //$('.fitCur').each(function () {
            //    $(this).append('<option>', opt);
            //    //$(this).addClass('my-select');
            //    //$(this).chosen({ width: "100%" });
            //});

        }
    );
}
function AssetSelected() {
    var v = $('#selectedCoin').val()
    var networkOpt = '';
    for (var i = 0; i < tkList.length; i++) {
        if (tkList[i].TokenId == v) {
            if (tkList[i].SupportedCoin.length > 0) {
                $('#dFiat').hide();
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
            $('#dFiat').show();
            $('#frmDeposit').submit();
        }
    }
    $('#dWallet').addClass('d-none');

    if (networkOpt.length > 0) {
        //var obj = $('#selectedNetwork');
        //if (obj.length==0) {
        $('#dNetwork').empty();
        $('#dNetwork').removeClass('d-none');
        var txt = '<label for="exampleInputSpend" class="lable-name22">Network</label > ';
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
    $.get(window.location.origin + '/deposit/GetMyNetWallet?networkid=' + nId, data => ProcessAndDisplayNetWallet(data));
}
var isNetWalDisplay = false;
var checkCount = 0;
function CheckAfter() {
    console.log('CheckAfter called..' + checkCount.toString());
    var nId = $('#selectedNetwork').val();
    $.get(window.location.origin + '/deposit/GetMyNetWallet?networkid=' + nId, data => ProcessAndDisplayNetWallet(data));
    if (!isNetWalDisplay && checkCount < 5) {
        checkCount++;
        setTimeout(CheckAfter, 3000);
        console.log('CheckAfter Set again..');
    }
    else {
        $('#showLoading').addClass('d-none');
        if (isNetWalDisplay)
            console.log('Address Received..');
        else
            console.log('Address Not Received but Auth Check Stopped after 15 seconds..');
    }
}
function GenerateNetWallet() {
    var nId = $('#selectedNetwork').val();
    $('#showLoading').removeClass('d-none');
    $.get(window.location.origin + '/deposit/GenerateMyNetWallet?networkid=' + nId, d => RepeatCheck(d));
}
function RepeatCheck(status) {
    if (status == "true") {
        checkCount = 0;
        isNetWalDisplay = false;
        setTimeout(CheckAfter, 3000);
        console.log('First Check set for 3 sec');
    }
    else {
        //notify user to try again for network wallet request in few seconds since system might be busy
    }
}

function ProcessAndDisplayNetWallet(data) {
    if (data == null || data == false || data == "false") {
        //No Network wallat yet
        $('#dWallet').removeClass('d-none');
        var txt = '<a href="javascript:void(0)" onClick="GenerateNetWallet()" id="generateWallet" class="css55-enable-btn">Click to Generate Wallet</a>';
        $('#btnDeposit').removeClass('d-none');
        $('#btnDeposit').addClass('d-none');
        $('#dWalletTxt').addClass('d-none');
        $('#qrholder').empty();
        $('#qrholder').append(txt);
    }
    else {
        let opt = JSON.parse(data);
        console.log(opt);
        var txt = '<span>QR Image here</span>';
        isNetWalDisplay = true;
        $('#dWallet').removeClass('d-none');
        $('#dWalletTxt').removeClass('d-none');
        $('#btnDeposit').removeClass('d-none');

        $('#nWallet').val(opt.Address);
        $('#qrholder').empty();
        $('#qrholder').append(txt);

        var qrContent = opt.Address;
        var qrcode = new QRCode(document.getElementById("qrholder"), {
            text: qrContent,
            width: 200,
            height: 200
        });
    }
}
function balCheck() {

}
function INRStep2() {
    var err = false; ClearValMsg();
    var v = $('#IsUPI').prop('checked') || $('#IsBankDeposits').prop('checked');
    if (!v) {
        ValMsg("Please select a 'Deposit Method'");
        err = true;
    }
    if ($('#Amount').val() <= 0) {
        ValMsg("Please enter an amount");
        err = true;
    }
    if (err)
        return;
    $('#frmDeposit').submit();
}
function ClearValMsg() { $('#Msg').html(''); }
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
   var v= $('#isBack').val();
    $('#frmDeposit').prop('action', v);
    $('#frmDeposit').submit();
});


////-------
function checkboxDropdown(el) {
    var $el = $(el)

    function updateStatus(label, result) {
        if (!result.length) {
            label.html('Select');
        }
    };

    $el.each(function (i, element) {
        var $list = $(this).find('.dropdown-list'),
            $label = $(this).find('.dropdown-label'),
            $checkAll = $(this).find('.check-all'),
            $inputs = $(this).find('.check'),
            defaultChecked = $(this).find('input[type=checkbox]:checked'),
            result = [];

        updateStatus($label, result);
        if (defaultChecked.length) {
            defaultChecked.each(function () {
                result.push($(this).next().text());
                $label.html(result.join(", "));
            });
        }

        $label.on('click', () => {
            $(this).toggleClass('open');
        });

        $checkAll.on('change', function () {
            var checked = $(this).is(':checked');
            var checkedText = $(this).next().text();
            result = [];
            if (checked) {
                $('#INRUserOptions_selUPI_UPIid').val($(this).data('upiid'));
                result.push(checkedText);
               // $label.html(result);
                $label.html(checkedText);
                $inputs.prop('checked', false);
            } else {
                $label.html(result);
            }
            updateStatus($label, result);
        });

        $inputs.on('change', function () {
            var checked = $(this).is(':checked');
            var checkedText = $(this).next().text();
            if ($checkAll.is(':checked')) {
                result = [];
            }
            if (checked) {
                result.push(checkedText);
                $label.html(result.join(", "));
                $checkAll.prop('checked', false);
            } else {
                let index = result.indexOf(checkedText);
                if (index >= 0) {
                    result.splice(index, 1);
                }
                $label.html(result.join(", "));
            }
            updateStatus($label, result);
        });

        $(document).on('click touchstart', e => {
            if (!$(e.target).closest($(this)).length) {
                $(this).removeClass('open');
            }
        });
    });
};

checkboxDropdown('.dropdown');

function UPIChanged() {
    var bval = $('#IB_INRUserOptions_selUPI_UPIid').val();
    $('#selUPI').html(bval);
}
function BankChanged() {
    var bval = $('#IB_INRUserOptions_selBankDeposits_AccountNumber').val();
    $('#selUPI').html(bval);
}
