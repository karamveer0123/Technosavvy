var MarketsFee = [];
var SwapFee = [];

$(function () {
    $.get(window.location.origin + '/Market/GetMarketsFee?abb=' + _selCountry, (data) => {
        var FeeData = JSON.parse(data);
        console.log('SwapData', FeeData);
        buildSwapFee(FeeData)
    });
});

$(function () {
    $.get(window.location.origin + '/token/NetworkFees', (data) => {
        var SwapData = JSON.parse(data);
        console.log('MarketsFee', SwapData);
        buildMarketsFee(SwapData)
    });
});

function ApplySearchWithNoRecord() {
    $('table tfoot').hide();
    $('.market-search').each(function () {
        var tn = $(this).attr('data-tab');
        $(this).on("keyup", function () {
            var value = $(this).val().replace(/\s/g, '').toLowerCase();
            //var value = $(this).val().toLowerCase();
            var fn = '#' + tn + ' tr';
            var fntb = $('#' + tn);
            var matchingRows = $(fn).filter(function () {
                return $(this).text().toLowerCase().indexOf(value) > -1;
            });
            if (matchingRows.length > 0) {
                $(fntb).nextAll('tfoot').hide();
                $('.my-navigation').show();
            } else {
                $(fntb).nextAll('tfoot').show();
                $('.my-navigation').hide();
            }
            $(fn).hide();
            matchingRows.show();
        });
        $('.market-search').on('search', function () {
            $('.market-search').val('');
            $('.market-search').trigger('keyup');
        });
    });
}
ApplySearchWithNoRecord();



$('#myTab a[data-bs-toggle="tab"]').on('show.bs.tab', function (e) {
    let target = $(e.target).data('target');
    $(target)
        .addClass('active show')
        .siblings('.tab-pane.active')
        .removeClass('active show');    
});




function buildMarketsFee(data) {
    console.log('buildMarketsFee', data)
    if (data.length <= 0) return;
    data.forEach(function (x) {


        MarketsFee.push({

            coin: x.Token.Code,
            fullName: x.Token.FullName,
            network: x.SupportedNetwork.Name,
            MinWithdrawal: x.MinWithdrawal,
            MaxWithdraw: x.MaxWithdrawal,
            DepositFee: x.DepositFee,
            PrevaillingGasFees: x.WithdrawalFee,
            Deposit: x.IsDepositAllowed,
            Withdraw: x.IsWithdrawalAllowed

        });
    })
    feeChangeDepoWithdraw()

}

console.log(' >>  ', MarketsFee);
console.log(' >>  ', SwapFee);
/* Deposit & Withdrawal Fees */
function feeChangeDepoWithdraw() {
    
    var txt = '';
    if (MarketsFee.length <= 0) return;
    MarketsFee.forEach(function (f) {
        console.log('f', f)
        var deposit = (f.Deposit) ? 'Enabaled' : 'Disable';
        var withdraw = (f.Withdraw) ? 'Enabaled' : 'Disable';
        txt += '<tr>',
            txt += '<td><div class="coin-name-css46 "><span><img src="../images/coin/coin/' + f.coin + '.png"></span> <span>' + f.coin + ' </span></div></td>',
            txt += '<td>' + f.fullName + '</td>',
            txt += '<td><div class="dflex22">  <span>' + f.network + ' </span></div> </td>',
            txt += '<td><div class="dflex22">  <span>' + f.MinWithdrawal + '</span> <span><img src="../images/coin/coin/' + f.coin + '.png" width="16"></span> <small> ' + f.coin + '</small></div> </td>',
            txt += '<td><div class="dflex22"> <span>' + f.MaxWithdraw + ' </span>  <span><img src="../images/coin/coin/' + f.coin + '.png" width="16"></span> <small>' + f.coin + ' </small> </div> </td>',
            txt += '<td>' + f.DepositFee +'</td>',
            txt += '<td><div class="dflex22"> <span>' + f.PrevaillingGasFees + ' </span> <span><img src="../images/coin/coin/' + f.coin + '.png" width="16"></span> <small>' + f.coin + ' </small></div> </td>',
            txt += '<td>' + deposit + '</td>',
            txt += '<td>' + withdraw +'</td>',
        txt += '</tr>';

        $('#depoWithdrawfee-tbody').html(txt);

        $('#depoWithdrawfee').simplePagination({
            items_per_page: 10,
        });

        ApplySearchWithNoRecord();
    });
}


function buildSwapFee(data) {
    console.log('buildSwapFee', data)
    if (data.length <= 0) return;
    data.forEach(function (x, i) {
        
        SwapFee.push({

            MarketBase: x.BaseToken.Code,
            MarketQuote: x.QuoteToken.Code,
            MinOrderSize: x.MinOrderSizeValueUSD,
            MakerFees1: x.MarketProfile[0]._BaseTokenMakerFee.FeeNonCommunity,
            TakerFees1: x.MarketProfile[0]._BaseTokenTakerFee.FeeNonCommunity,
            MakerFees: x.MarketProfile[0]._BaseTokenMakerFee.FeeNonCommunity,
            TakerFees: x.MarketProfile[0]._BaseTokenTakerFee.FeeNonCommunity,            
            Cashback: x.MarketType,        
            Status: x.IsTradingAllowed,
            ShortName: x.ShortName

        });
    })
    SwapFees()

}

/* Swap Fees */
function SwapFees() {

    var txt = '';

    if (SwapFee.length <= 0) return;
    SwapFee.forEach(function (t) {
        var status = (t.Status) ? 'Active' : 'Disabled';
        txt += '<tr class=" text-center">',
            txt += '<td><div class="coin-name-css46 "><span class="coin-zindex"><img src="../images/coin/coin/' + t.MarketBase + '.png"></span> <span class="paircoin-img"><img src="../images/coin/coin/' + t.MarketQuote + '.png"> </span> <span>' + t.ShortName  + '</span></div></td>',
            txt += '<td><div class="dflex22">  <span>' + t.MinOrderSize + ' </span> <span><img src="../images/coin/coin/' + t.MarketBase + '.png" width="16"></span> <small> ' + t.MarketBase + '</small></div> </td>',
            txt += '<td>' + t.MakerFees1 * 100 + '% </td>',
            txt += '<td>' + t.TakerFees1 * 100 + '%  </td>',
            txt += '<td>' + t.MakerFees * 100 + '%  </td>',
            txt += '<td>' + t.TakerFees * 100 + '%  </td>';
        if (t.MarketType == "StableStable") {
            txt += '<td> ' + (t.TakerFees  * 0)*100 +'% </td>';
        } else {
            txt += '<td> ' + (t.TakerFees * 5)*100 +'% </td>';
        }

        txt += '<td>' + status + '</td>',
            txt += '</tr>',

        $('#tradingFees-tbody').html(txt);

        $('#feesdivcs34').simplePagination({
            items_per_page: 10,
        });

        ApplySearchWithNoRecord();
    });
}
