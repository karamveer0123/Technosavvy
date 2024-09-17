
// Other Market section
try {
    $.get(MktAPI + 'GetMarketSummary', (data) => { buildOthersMarkets(data); loadotherMarkets(data); });
    setInterval(function () {
        $.get(MktAPI + 'GetMarketSummary', (data) => { InjectOtherMarketData(data) });
    }, 30000)
} catch (e) {
    console.log('Other Market Catch Error', e);
}

var MarketsTrade = [];
var MarketQuote = [];
var MarketBase = [];
var MarketCoin = [];


function buildOthersMarkets(data) {

    if (data.length <= 0) return;
    data.forEach(function (x) {
        if (jQuery.inArray(x.quote, MarketQuote) <= -1)
            MarketQuote.push(x.quote);

        if (jQuery.inArray(x.base, MarketBase) <= -1)
            MarketBase.push(x.base);

        MargeCoin = MarketQuote.concat(MarketBase);
        MarketCoin = MargeCoin.filter((item, idx) => MargeCoin.indexOf(item) === idx)

        MarketsTrade.push({
            name: x.name, code: x.mCode, base: x.base, quote: x.quote, high: x.highest24H, low: x.lowest24H, cat: x.category, price: x.value, volume: x.volume, change: x.change, cashback: x.cashBackYTD, poly: x.poly24Hr, marketCap: x.marketCap
        });
    });

}


function loadotherMarkets(data) {
    tradeQuoteList(data);
    tradeQuoteMarketList(data);
    searchMarketCoin(data);
    priceLiveMarket(data);

}
// Trade quote 
function tradeQuoteList(d) {

    var isFirst2 = false;
    var txt = '';
    $('#eventmark44').html('');
    txt += '<li class="nav-item">';
    txt += '<a class="nav-link" data-bs-toggle="tab" href="#star11"><i class="fa fa-star"></i></a>';
    txt += '</li>';
    // console.log('other market quote', MarketQuote);
    MarketQuote.forEach(function (mq, index) {
       
        txt += '<li class="nav-item">';
        txt += '<a class="nav-link" data-bs-toggle="tab" href="#' + mq + '">' + mq + ' </a>';
        txt += '</li>';
        
        $('#eventmark44').html(txt);
        $('#eventmark44 .nav-item:nth-child(2) .nav-link').addClass('active');
    });
};

function tradeQuoteMarketList(d) {
    txt = '';
    txt2 = '';
    $('#otherMarketList').html('');

    MarketQuote.forEach(function (d) {
        console.log('d', d);
        txt += '<div id="star11" class="tab-pane pe-1"><table class="row-divtr4 table-responsive w-100"></table></div>';
        txt += '<div id="' + d + '" class="tab-pane pe-1">';
        txt += '<table class="row-divtr4 table-responsive w-100">';
        MarketsTrade.forEach(function (cm) {
            if (d == cm.quote) {
                // console.log('mq', cm);
                txt += '<tr>';
                txt += '<td><a  href="/trade?cat=trade&t=' + cm.base + '&q=usdt" traget="_blank" class="linkColor"><span> ' + cm.name + ' </span></a>';
                txt += '</td>';
                if (0 <= cm.change) {
                    txt += '<td class="green">' + GetFormatedValWithSep(cm.price, _fsl) + ' </td>';
                    txt += '<td class="green">' + GetPercentFormat(cm.change, 2) + '</td>';
                } else {
                    txt += '<td class="red">' + GetFormatedValWithSep(cm.price, _fsl) + '</span> </td>';
                    txt += '<td class="red">' + GetPercentFormat(cm.change, 2) + '</td>';
                }

                txt += '</tr>';
            }
        });
        txt += '</table>';
        txt += '</div>';

        $('#otherMarketList').html(txt);
        $('#otherMarketList .tab-pane:nth-child(2)').addClass('active show');
    });

    if (favList.length == 0) {
        txt2 += '<tr><td colspan="3" class="text-white text-center"><div class="pt-4"><img src="../images/no-data.png" width="30"><p class="p-noRecords"> No records found</p></div></td></tr>';
        $('#otherMarketList #star11 table').html(txt2);
    } else {

        MarketsTrade.forEach(function (d) {
        if (favList.indexOf(d.code.toLowerCase()) > -1) {
            txt2 += '<tr>';
            txt2 += '<td><span class="cssact-star22"><a  href="/trade?cat=trade&t=' + d.base + '&q=usdt" traget="_blank" class="linkColor"><i class="fa fa-star  active"></i></span><span> ' + d.name + ' </span></a>';
            txt2 += '</td>';
            if (0 <= d.change) {
                txt2 += '<td class="green">' + GetFormatedValWithSep(d.price, _fsl) + '</td>';
                txt2 += '<td class="green">' + GetPercentFormat(d.change, 2) + '</td>';
            } else {
                txt2 += '<td class="red">' + GetFormatedValWithSep(d.price, _fsl) + '</td>';
                txt2 += '<td class="red">' + GetPercentFormat(d.change, 2) + '</td>';
            }
            txt2 += '</tr>';
        } 

        $('#otherMarketList #star11 table').html(txt2);

    });
    }

};


function InjectOtherMarketData(data) {

    var MarketsTrade2 = [];
    var MarketQuote2 = [];
    var MarketBase2 = [];
    var MarketCoin2 = [];


    if (data.length <= 0) return;
    data.forEach(function (x) {
        if (jQuery.inArray(x.quote, MarketQuote2) <= -1)
            MarketQuote2.push(x.quote);

        if (jQuery.inArray(x.base, MarketBase2) <= -1)
            MarketBase2.push(x.base);

        MargeCoin2 = MarketQuote2.concat(MarketBase2);
        MarketCoin2 = MargeCoin2.filter((item, idx) => MargeCoin2.indexOf(item) === idx)

        MarketsTrade2.push({
            name: x.name, code: x.mCode, base: x.base, quote: x.quote, high: x.highest24H, low: x.lowest24H, cat: x.category, price: x.value, volume: x.volume, change: x.change, cashback: x.cashBackYTD, poly: x.poly24Hr, marketCap: x.marketCap
        });
    });


    var txt = '';
    var txt2 = '';
    MarketQuote2.forEach(function (d) {
        $('#' + d).children('table').html('');
        console.log('Auto Inject', d);
        MarketsTrade2.forEach(function (cm) {
            if (d == cm.quote) {
               
                txt += '<tr>';
                txt += '<td><a  href="/trade?cat=trade&t=' + cm.base + '&q=usdt" traget="_blank" class="linkColor"><span> ' + cm.name + ' </span></a>';
                txt += '</td>';
                if (0 <= cm.change) {
                    txt += '<td class="green">' + GetFormatedValWithSep(cm.price, _fsl) + ' </td>';
                    txt += '<td class="green">' + GetPercentFormat(cm.change, 2) + '</td>';
                } else {
                    txt += '<td class="red">' + GetFormatedValWithSep(cm.price, _fsl) + '</span> </td>';
                    txt += '<td class="red">' + GetPercentFormat(cm.change, 2) + '</td>';
                }

                txt += '</tr>';
            }
        });
        
        $('#' + d).children('table').html(txt);
    })


    if (favList.length == 0) {
        txt2 += '<tr><td colspan="3" class="text-white text-center"><div class="pt-4"><img src="../images/no-data.png" width="30"><p class="p-noRecords"> No records found</p></div></td></tr>';
        $('#otherMarketList #star11 table').html(txt2);
    } else {

        MarketsTrade2.forEach(function (d) {
            if (favList.indexOf(d.code.toLowerCase()) > -1) {
                txt2 += '<tr>';
                txt2 += '<td><span class="cssact-star22"><a  href="/trade?cat=trade&t=' + d.base + '&q=usdt" traget="_blank" class="linkColor"><i class="fa fa-star  active"></i></span><span> ' + d.name + ' </span></a>';
                txt2 += '</td>';
                if (0 <= d.change) {
                    txt2 += '<td class="green">' + GetFormatedValWithSep(d.price, _fsl) + '</td>';
                    txt2 += '<td class="green">' + GetPercentFormat(d.change, 2) + '</td>';
                } else {
                    txt2 += '<td class="red">' + GetFormatedValWithSep(d.price, _fsl) + '</td>';
                    txt2 += '<td class="red">' + GetPercentFormat(d.change, 2) + '</td>';
                }
                txt2 += '</tr>';
            }

            $('#otherMarketList #star11 table').html(txt2);

        });
    }

}






function SearchMarketByCoin() {
    $('.selectOtherMarket').hide();
    var searchValue = $('#searchMarketByCoin');
    searchValue.on('change', function () {
        if ($(this).val() == 'Select Coin' || $(this).val() == '') {
            $('.onLoadedOtherMarket').show();
            $('.selectOtherMarket').hide();
        } else {
            $('.onLoadedOtherMarket').hide();
            $('.selectOtherMarket').show();
        }

        var marketCoinValue = $(this).val();
        var marketCoinValueText = marketCoinValue.toUpperCase();
        txt = '';
        $('#selectOtherMarketList table').html('');
        MarketsTrade.forEach(function (cm) {

            if (cm.quote.toUpperCase() == marketCoinValueText || cm.base.toUpperCase() == marketCoinValueText) {
                txt += '<tr data-base="' + cm.base + '" data-quote="' + cm.quote + '">';
                txt += '<td><span class="cssact-star22"><a href="/trade?cat=trade&t=' + cm.base + '&q=usdt" traget="_blank" class="linkColor">';

                if (favList.indexOf(cm.code.toLowerCase()) > 0) {
                    txt += '<i class="fa fa-star  active"></i>';
                }

                txt += '</span><span> ' + cm.name + ' </span></a > ';
                txt += '</td>';
                if (0 <= cm.change) {
                    txt += '<td class="green">' + GetFormatedValWithSep(cm.price, _fsl) + '</td>';
                    txt += '<td  class="green">' + GetPercentFormat(cm.change, 2) + '</td>';
                } else {
                    txt += '<td class="red">' + GetFormatedValWithSep(cm.price, _fsl) + '</td>';
                    txt += '<td class="red">' + GetPercentFormat(cm.change, 2) + '</td>';
                }
                txt += '</tr>';
            }

            $('#selectOtherMarketList table').html(txt)
        });


    });
}


function searchMarketCoin(c) {
    $('#searchbox435').html('');
    var txt = '';
    txt += '<select id="searchMarketByCoin" class="my-select" aria-label="Default select example" style="display: none;">';
    txt += '<option selected>Select Coin</option>';
    MarketCoin.forEach(function (coin) {
        txt += '<option data-img-src="../images/coin/coin/' + coin.toUpperCase() + '.png">' + coin + '</option>';
    });
    txt += '</select>';
    $('#searchbox435').html(txt);
    selectDropdown();    
    SearchMarketByCoin();
    
}

function selectDropdown() {
    $("#searchMarketByCoin").chosen({ width: "100%" });
}

// End Other Market section

// Theme change section hide and show click on outside of section
const $menu = $('.trade-light-icon')

const onMouseUp = e => {
    if (!$menu.is(e.target)
        && $menu.has(e.target).length === 0) {
        $menu.removeClass('is-active')
    }
}

$('.sidebarIconToggle').on('click', () => {
    $menu.toggleClass('is-active').promise().done(() => {
        if ($menu.hasClass('is-active')) {
            $(document).on('mouseup', onMouseUp)
        } else {
            $(document).off('mouseup', onMouseUp)
        }
    })
});

function priceLiveMarket(live) {
    $('#priceLiveMarket').html('');
    var sortedByChange = live.sort(function (a, b) {
        let x = new Date(b.change) - new Date(a.change);
        return x;
    });

    txt = "";
    var count = 1;
    var maxCount = 3;
    sortedByChange.forEach(function (li) {
        if (count <= maxCount) {
            txt += '<li><span>' + li.name + '</span>';
            if (0 <= li.change) {
                txt += '<span class="green">' + GetPercentFormat(li.change, 2) + '</span>';
                txt += '<span class="green"> ' + GetFormatedValWithSep(li.value, __qf)  + '</span >';
            } else {
                txt += '<span class="red">' + GetPercentFormat(li.change, 2) + '</span>';
                txt += '<span class="red"> ' + GetFormatedValWithSep(li.value, __qf) + '</span >';
            }
            txt += '</li > ',
                count++;
        }
    });
    $('#priceLiveMarket').html(txt);

}

