
var tkList = [];
function LoadCoin() {
    var v = $('#fromWalletType').val();
    if (v.length <= 0) return;
    $.get(window.location.origin + '/wallet/TransferWallet?wtype=' + v,
        function (data) {
            var coinOpt = '';// '<option>Select Token</option>';
            let opt =  JSON.parse(data);
            console.log(opt);
            var isCoin = false;
            tkList = opt;
            $('#TokenList').val(tkList);
            for (var i = 0; i < tkList.length; i++) {
                {
                    isCoin = true;
                    coinOpt += '<option data-img-src="/images/coin/' + tkList[i].Code.toUpperCase() + '.png" value="' + tkList[i].CoinId + '">' + tkList[i].Code + '</option>';
                }
            }
            if (isCoin == false) return;
            $('#selectedCoin').empty();
            $('#selectedCoin').append('<option>', coinOpt);
            $('#selectedCoin').removeClass('d-none');
            $('#selectedCoin').addClass('my-select');
            $('#selectedCoin').chosen({ width: "100%" });
        }
    );
    Check();
}
var tkVal = 0;
function GetBalance() {
    var id = $('#selectedCoin').val();
    for (var i = 0; i < tkList.length; i++) {
        if (id == tkList[i].CoinId) {
            tkVal = tkList[i].Amount;
            $('#dbal').html('Available Balance :' + tkList[i].Amount + ' ' + tkList[i].Code);
            $('#Code').val(tkList[i].Code);
        }
    }
    Check();
}
function Check() {
    var isTrue = $('#fromWalletType').val() != $('#toWalletType').val();
    isTrue = isTrue && $('#toWalletType').val() > 0;
    isTrue = isTrue  && $('#fromWalletType').val() > 0;
    isTrue = isTrue && $('#selectedCoin').val().length > 0;
    isTrue = isTrue  && $('#Amount').val() > 0;
    isTrue = isTrue  && $('#Amount').val() <= tkVal;
    if (isTrue) 
        $('#btnGo').prop('disabled', false);
    else
        $('#btnGo').prop('disabled', true);
}