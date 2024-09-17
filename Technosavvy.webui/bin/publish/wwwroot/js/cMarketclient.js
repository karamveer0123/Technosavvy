var Markets = [];
var MarketQuote = [];
var Tokens = [];
var Cat = [];
var _paginationLimit = 10;

$.get(MktAPI + 'GetMarketSummary', (data1) => { buildMarkets(data1); });
$.get(MktAPI + 'GetTopVolumnTokenSummary', (data) => {
    GetT1tile(data, 'mktTopV');
});//GetTopVolumnTokenSummary
$.get(MktAPI + 'GetTopGainerTokenSummary', (data) => GetT1tile(data, 'mktGainer'));//GetTopVolumnTokenSummary
$.get(MktAPI + 'GetTop24HCashbackTokenSummary', (data) => GetT1tile(data, 'mktCashback24'));//GetTopVolumnTokenSummary
$.get(MktAPI + 'GetMarketSummary', (data) => GetT2tile(data, 'mktHighLightCoin'));
$.get(MktAPI + 'GetMarketSummary', (data) => { GetT2tile(data, 'mktNewListing'); });

/* Get All Markets For Tabs Table with Category and Call Methof ApplySearch
 */
//
function MarketUpdataed() {
    $.get(MktAPI + 'GetTopVolumnTokenSummary', (data) => { GetT1tile(data, 'mktTopV'); });
    $.get(MktAPI + 'GetTopGainerTokenSummary', (data) => GetT1tile(data, 'mktGainer'));
    $.get(MktAPI + 'GetTop24HCashbackTokenSummary', (data) => GetT1tile(data, 'mktCashback24'));
    $.get(MktAPI + 'GetMarketSummary', (data) => { GetT2tile(data, 'mktHighLightCoin'); });
    $.get(MktAPI + 'GetMarketSummary', (data) => { GetT2tile(data, 'mktNewListing'); });
}
setInterval(function () {
    MarketUpdataed()
}, 60000)
//builMarketTabs();
function builMarketTabs() {
    buildTokens();
    buildSPOT();
    HighlightMarketTable();
    buildNewListing();
}
$('table tfoot').hide();
function GetT1tile(tok, name) {
    console.log('GetTopVolumnTokenSummary', tok);

    var str = '';
    var le = tok.length;
    if (le == 0) {
        str += '<div class="noFound noheight54 w-100 comunity-css566-box"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div>'
    } else {
        console.log('le', le)

        if (le > 4) le = 4;
        for (var i = 0; i < le; i++) {
            str += doTileT1(tok[i], name);
        }
    }
    $('#' + name).html(str);

    // Market Top Gainer Coins
    /* var str2 = '';
     for (var i = 0; i < tok.length; i++) {
         if (i < 5) { str2 += doTileT2(tok[i]); }
     }
     var ht2 = '<div class="market-tab-box">';
     ht2 += str2;
     ht2 += '</div>';
     $('#top-coin').html(ht2); */


}


function GetT2tile(tok, name) {
    console.log('GetMarketSummary', tok)

    var str2 = '';
    var le = tok.length;
    var curDT = moment.utc(new Date()).valueOf();
    console.log('curDT', curDT, le);

    if (le == 0) {
        str2 += '<div class="noFound noheight54 w-100 comunity-css566-box"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div>'
    } else {
        if (le > 4) le = 4;
        for (var i = 0; i < le; i++) {
            if (name == 'mktHighLightCoin') {
                var hLDate = moment.utc(tok[i].highlightTill).valueOf();
            } else if (name == "mktNewListing") {
                var hLDate = moment.utc(tok[i].newListingTill).valueOf();
            }
            console.log('hLDate', hLDate,)
            if (curDT <= hLDate) {
                str2 += doTileT1(tok[i], name);
            } else {

            }
        }
    }


    $('#' + name).html(str2);
    $('#' + name).find('.noFound:not(:last)').hide();

    var mktLen = $('#mktHighLightCoin .market-detail22').length;
    console.log('mktLen', mktLen)
    if (mktLen <= 0) {
        var txt = '<div class="noFound noheight54 w-100 comunity-css566-box"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div>';
        $('#mktHighLightCoin').html(txt);
    } else { }
    var mktLen1 = $('#mktNewListing .market-detail22').length;
    console.log('mktLen1', mktLen1)
    if (mktLen1 <= 0) {
        var txt = '<div class="noFound noheight54 w-100 comunity-css566-box"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div>';
        $('#mktNewListing').html(txt);
    } else { }
}



function dspNotFound() {
    var txt = '';
    txt += '<div class="noFound noheight54 w-100 comunity-css566-box"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div>'
    return txt;
}

function doTileT1(tok, name) {
    var selected = '';
    selected += '<div class="market-detail22 positionrelative33">';
    selected += '<div class="overlay-div">';
    if (name === 'mktHighLightCoin' || name === 'mktNewListing') {
        selected += '<div class="name-m22 favno11"> <span> <img src="../images/coin/coin/' + tok.base.toUpperCase() + '.png"> ' + tok.base + '<small> ' + tok.quote + ' </small></span><span></span></div>';
    } else {
        selected += '<div class="name-m22 favno11"> <span> <img src="../images/coin/coin/' + tok.abbr.toUpperCase() + '.png"> ' + tok.abbr + '<small> ' + tok.name + ' </small></span><span></span></div>';
    }

    selected += '<div class="price-m22"><span class="price-font11"> Price </span> <span class="css44-up dyCur" data-oval="' + tok.value + '">' + GetFormatedCurrency(tok.value, __nf) + '</span> </div>';
    if (tok.change >= 0) {
        selected += '<div class="price-m22"><span> 24h Change </span> <span class="css44-up">' + GetPercentFormat(tok.change, 2) + ' </span> </div>';
    }
    else {
        selected += '<div class="price-m22"><span> 24h Change </span> <span class="css44-down">' + GetPercentFormat(tok.change, 2) + ' </span> </div>';
    }
    if (name === 'mktCashback24') {
        selected += '<div class="price-m22"><span>Cashback 24h</span> <span class="dyCurLen" data-oval="' + tok.cashBack24h + '">' + GetFormatedCurrencyMBTQ(tok.cashBack24h) + '</span> </div>';
    } else {
        selected += '<div class="price-m22"><span>Volume 24h</span> <span class="dyCurLen-1" data-oval="' + tok.volume + '">' + GetFormatCountMBTQ(tok.volume, 2) + '</span> </div>';
    }
    if (name === 'mktHighLightCoin' || name === 'mktNewListing') {
        selected += '<div class="price-m22"><span><a href="/token/?tname=' + tok.base.toLowerCase() + '"  target="_blank">';
        selected += 'Detail </a></span> <span><a href="/trade?cat=trade&t=' + tok.base + '&q=usdt" target="_blank">Trade </a></span></div>';
    } else {
        if (tok.abbr.toLowerCase() == 'TechnoSavvy') {
            selected += '<div class="price-m22"><span><a href="/about-TechnoSavvy"  target="_blank">Detail </a></span>';
        } else {
            selected += '<div class="price-m22"><span><a href="/token/?tname=' + tok.abbr.toLowerCase() + '"  target="_blank">Detail </a></span>';
        }
        selected += ' <span><a href="/trade?cat=trade&t=' + tok.abbr + '&q=usdt" target="_blank">Trade </a></span></div>';
    }

    selected += '</div>';
    selected += '</div>';


    return selected;


}
/*
function doTileT2(tok) {
    console.log('doTileT2', tok);
    var selected = '';
    selected += '<div class="market33-box-css positionrelative33">';
    selected += '<div class="overlay-div">';
    selected += '<div class="name-m22">';
    selected += '<span> <img src="images/coin/coin/' + tok.abbr.toUpperCase() + '.png"> ' + tok.abbr + '</span>';
    ///if (tok.cashBackYTD > 0) {
       // selected += '<span>';
       // selected += '<a href="#" class="icon-m22"> <i class="fa fa-money" aria-hidden="true"></i></a>';
       // selected += '</span>';
    //}
    selected += '</div>';
    selected += '<div class="mareket-head-33">';
    selected += '<span>';
    selected += ' <svg viewBox="0 0 500 100" class="chart">';
    selected += '<polyline fill="none" stroke="#d5960f" stroke-width="10" points="';
    if (tok.poly24Hr != null) {
        for (var i = 0; i < tok.poly24Hr.length; i++) {
            selected += (i * 20) + ',' + (tok.poly24Hr[i]) + ' '
        }
    }
    selected += '"></polyline>';
    selected += '</svg>';
    selected += '</span> <span>';
    selected += '  <span class=" dyCur" data-oval="' + tok.value +'">' + GetFormatedCurrency(tok.value) + '</span>';
    if (tok.change < 0) { selected += ' <span class="css44-down">' + GetPercentFormat(tok.change, 2) + '</span>'; }
    else { selected += ' <span class="css44-up">' + GetPercentFormat(tok.change, 2) + '</span>'; }
    selected += '</span>';
    selected += '</div>';
    selected += '</div>';
    selected += '</div>';
    return selected;
}*/

//AllTokens
function buildTokens() {
    if (Markets.length <= 0) return;
    $('#myMktTaball-566 tbody tr').remove();
    Markets.forEach(function (x) {
        if (x.quote.toLowerCase() == __base.toLowerCase()) {
            var txt = getHtmlAllCrypto(x, 'allCrypto');
            $('#myMktTaball-566').append(txt);
            var obj = $('#myMktTaball-566');
            //obj.children()[1].before(obj.children()[obj.children().length - 1]);
            console.log(x.quote + ' ' + x.mCode + ' isgood for Token Summary');
        }
        else {
            console.log(x.base + ' ' + x.mCode + ' isgood for Market Summary');
        }
        //ToDo:Naveen,
        //This is where we should do needfull to Add Category value in the List
    });

    $('#all-crypto-data').simplePagination({
        items_per_page: _paginationLimit,
    });

    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
    sortTableOrder();
}

function buildNewListing() {
    var curDT = moment.utc(new Date()).valueOf();
    if (Markets.length <= 0) return;
    $('#new-listing--566 tbody tr').remove();
    Markets.forEach(function (x) {
        var nLDate = moment.utc(x.newList).valueOf();
        if (curDT <= nLDate) {
            if (x.quote.toLowerCase() == __base.toLowerCase()) {
                var txt = getHtmlAllCrypto(x, 'newList');
                $('#new-listing-body-566').append(txt);
                var obj = $('#new-listing-body-566');
                //obj.children()[1].before(obj.children()[obj.children().length - 1]);
                console.log(x.quote + ' ' + x.mCode + ' isgood for Token Summary');
            }
            else {
                console.log(x.base + ' ' + x.mCode + ' isgood for Market Summary');
            }
        } else {

        }
    });

    var trLen = $('#new-listing--566 tbody tr').length;
    if (trLen <= 0) {
        var txt = dspNotFound();
        $('#newlisting-main .nomarket-11').append(txt);
        $('#newlisting-main .my-navigation').hide();
    } else {
        $('#newlisting-main .nomarket-11').hide();
        $('#newlisting-main .my-navigation').show();
    }
    $('#newlisting-main').find('.noFound:not(:last)').hide();

    $('#newlisting-main').simplePagination({
        items_per_page: _paginationLimit,
    });
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
    sortTableOrder();
}


function HighlightMarketTable() {
    // Highlight coin

    var curDT = moment.utc(new Date()).valueOf();
    if (Markets.length <= 0) return;
    $('#highLightMarket-table tbody tr').remove();
    Markets.forEach(function (x) {
        var nLDate = moment.utc(x.highlight).valueOf();
        if (curDT <= nLDate) {
            if (x.quote.toLowerCase() == __base.toLowerCase()) {
                var txt = getHtmlAllCrypto(x, 'highLight');
                $('#highLightMarket-566').append(txt);
                var obj = $('#highLightMarket-566');
                //obj.children()[1].before(obj.children()[obj.children().length - 1]);
                console.log(x.quote + ' ' + x.mCode + ' isgood for Token Summary');
            }
            else {
                console.log(x.base + ' ' + x.mCode + ' isgood for Market Summary');
            }
        } else {

        }
    });

    var trLen = $('#highLightMarket-table tbody tr').length;
    console.log('trLen', trLen);
    if (trLen <= 0) {
        var txt = dspNotFound();
        $('#highlightmarket-main .nomarket-11').append(txt);
        $('#highlightmarket-main .my-navigation').hide();
    } else {
        $('#highlightmarket-main .nomarket-11').hide();
        $('#highlightmarket-main .my-navigation').show();
    }
    $('#highlightmarket-main').find('.noFound:not(:last)').hide();


    $('#highlightmarket-main').simplePagination({
        items_per_page: _paginationLimit,
    });
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
    sortTableOrder();

}

//SPOT Market
function buildSPOT() {
    buildTabSelection();
    buildTabBODY();
    buildTabSeach();
    ApplySearch();
    sportMarketUl();
}
function buildTabSeach() {
    var isfirst = false;

    $('#spot-market-search').html('');

    MarketQuote.forEach(function (nd) {
        var txt2 = '';
        if (!isfirst) {
            txt2 += '<div class="tab-pane fade show active ' + nd + '-s1" >';
            isfirst = true;
        }
        else {
            txt2 += '<div class="tab-pane fade ' + nd + '-s1" >';
        }
        txt2 += buildSearchBox(nd + '-5661');
        txt2 += '</div>';
        $('#spot-market-search').append(txt2);
    })
}
function buildTabBODY() {
    //var txt = '';
    //txt = 'myMktTaball-567'
    var isfirst = false;
    $('#spot-market-tab-content').html('');

    MarketQuote.forEach(function (nd) {
        var txt = '';
        if (!isfirst) {
            txt += '<div class="tab-pane fade show active ' + nd + '-t1" >';
            isfirst = true;
        }
        else {
            txt += '<div class="tab-pane fade ' + nd + '-t1" >';
        }
        txt += '<div id="comunity-css566-box" class="comunity-css566-box">';
        //txt += buildSearchBox(nd + '-5661');
        txt += '<div id="sortMain' + nd + '">'
        txt += '<table id="#t' + nd + '-5661" class="table table-responsive  table-sortable table-class"><thead class="trasaction-table-title">';
        txt += '<tr>' + buildTitleHeader('Name') + buildVolumnHeader('Price') + buildChangeHeader('24h Change(%)') + buildChangeHeader('24h Volume') + buildChangeHeader('24h High/24h Low') + buildCashbackTopHeader('Cashback') + '</tr> ' + buildCashbackHeader('24h', 'YTD') + ' </thead>'
        txt += '<tbody id="myTable-' + nd + '-5661" class="trasaction-table">';
        Markets.forEach(function (x) {
            if (x.quote != nd) return;
            txt += '<tr><td class="td-sticky-left"><a href="/trade?cat=trade&t=' + x.base + '&q=usdt" target="_blank"><div class="coin-name-css46">',
                txt += ' <span><img src="../images/coin/coin/' + x.base.toUpperCase() + '.png" width="18" class="z-top"></span>',
                txt += '<span class="paircoin-img"><img width="16" src="../images/coin/coin/' + x.quote.toUpperCase() + '.png"></span>',
                txt += '<span class="ethcss44"> ' + x.name + ' </span></div ></a></td >';

            if (0 <= x.change) {
                txt += '<td><div class="coin-24change-css46 green dyCur" data-oval="' + x.price + '">' + GetFormatedCurrency(x.price) + '</div></td>';
                txt += '<td><div class="coin-24change-css46 green">' + GetPercentFormat(x.change, 2) + '</div></td>';
            } else {
                txt += '<td><div class="coin-24change-css46 red dyCur" data-oval="' + x.price + '">' + GetFormatedCurrency(x.price) + '</div></td>';
                txt += '<td><div class="coin-24change-css46 red">' + GetPercentFormat(x.change, 2) + '</div></td>';
            }
            txt += '<td><div class="coin-24change-css46 dyCurLen-1"  data-oval="' + x.volume + '">' + GetFormatCountMBTQ(x.volume, 2) + '</div></td><td><div class="coin-24change-css46"><span class="dyCur" data-oval="' + x.high + '">' + GetFormatedCurrency(x.high) + '</span> / <span class="dyCur" data-oval="' + x.low + '">' + GetFormatedCurrency(x.low) + '</span> </div></td><td><div class="coin-24change-css46 dyCurLen" data-mainvalue="' + x.cashback24h + '" data-oval="' + x.cashback24h + '">' + GetFormatedCurrencyMBTQ(x.cashback24h) + '</div></td><td nowrap="nowrap"><div class="market-trade-css46 white defCursor dyCurLen" data-mainvalue="' + x.cashback + '" data-oval="' + x.cashback + '">' + GetFormatedCurrencyMBTQ(x.cashback) + ' </div></td></tr > ';
        });
        txt += '</tbody>';
        txt += buildFooter();
        txt += '</table > ';
        txt += buildPagination();
        txt += '</div></div></div>';
        $('#spot-market-tab-content').append(txt);

        $('#sortMain' + nd).simplePagination({
            items_per_page: _paginationLimit,
        });
    });
    sortTableOrder();

}


function buildPagination() {
    //ToDo:Naveen, Build Pagination Logic

    return '<div class="my-navigation"><div class="simple-pagination-first"></div><div class="simple-pagination-previous"></div><div class="simple-pagination-page-numbers"></div><div class="simple-pagination-next"></div><div class="simple-pagination-last"></div></div>';

    //return '<div class="row"><nav id="pagination-css"><ul class="pagination justify-content-end"><li class="page-item disabled"><a class="page-link" href="#" tabindex="-1" aria-disabled="true">Previous</a></li><li class="page-item"><a class="page-link" href="#">1</a></li><li class="page-item"><a class="page-link" href="#">2</a></li><li class="page-item"><a class="page-link" href="#">3</a></li><li class="page-item"><a class="page-link" href="#">Next</a></li></ul></nav></div>';
}
function buildSearchBox(target) {
    return '<span class="css33-rightpan  mb-3"><form class="positionrelative" autocomplete="off"><input type="search" class="market-search" placeholder="Search.." name="search" data-tab="myTable-' + target + '"><span class="searchbtn22"><i class="fa fa-search" aria-hidden="true"></i></span></form></span>';
}
function buildTitleHeader(name) {
    return '<th nowrap="nowrap" rowspan="2" class="css566-table-text text-start th-sticky-left">' + name + '</th>';
}
function buildVolumnHeader(name) {
    return '<th nowrap="nowrap" rowspan="2" class="css566-table-text">' + name + '</th>';
}
function buildPriceHeader(name) {
    return '<th nowrap="nowrap" rowspan="2" class="css566-table-text">' + name + '</th>';
}
function buildChangeHeader(name) {
    return '<th nowrap="nowrap" rowspan="2" class="css566-table-text">' + name + '</th>';
}
function buildCashbackTopHeader(name) {
    return '<th nowrap="nowrap" colspan="2" class="css566-table-text text-center orange">' + name + '</th>';
}
function buildCashbackHeader(name1, name2) {
    return '<tr><th nowrap="nowrap" class="css566-table-text w-150"><div class="volume-css46"><span> ' + name1 + '</span> </div></th><th nowrap="nowrap" class="css566-table-text w-150"><div class="volume-css46"><span>' + name2 + '</span> </div></th></tr>';
}
function buildFooter() {
    return '<tfoot><tr><td colspan="7"><div class="noheight54"><div class="norecord-div44"><span><img src="images/no-data.png"></span><span> No records found</span></div></div></td></tr ></tfoot >'
}

function sortTableOrder() {
    document.querySelectorAll(".table-sortable th").forEach(headerCell => {
        headerCell.addEventListener("click", () => {
            const tableElement = headerCell.parentElement.parentElement.parentElement;
            const headerIndex = Array.prototype.indexOf.call(headerCell.parentElement.children, headerCell);
            const currentIsAscending = headerCell.classList.contains("th-sort-asc");
            sortTableByColumn(tableElement, headerIndex, !currentIsAscending);
        });
    });
}
function buildTabSelection() {
    var isfirst = false;
    var txt = '';
    MarketQuote.forEach(function (x) {
        if (!isfirst) {
            txt += '<li class="nav-item"><a class="nav-link active" data-bs-toggle="tab" data-bs-target=".' + x + '-t1, .' + x + '-s1" href="#' + x + '-5661">' + x + ' </a></li>';
            isfirst = true;
        }
        else {
            txt += '<li class="nav-item"><a class="nav-link " data-bs-toggle="tab" data-bs-target=".' + x + '-t1, .' + x + '-s1" href="#' + x + '-5661">' + x + ' </a></li>';
        }
    });
    $('#spot-market-ul').html(txt);

}

function sportMarketUl() {
    $('#spot-market-ul a[data-bs-toggle="tab"]').on('show.bs.tab', function (e) {
        let target = $(e.target).data('bs-target');
        console.log('target', target);
        $(target)
            .addClass('active show')
            .siblings('.tab-pane.active')
            .removeClass('active show')
    });

}
function buildMarkets(data) {
    console.log('buildMarkets', data)
    if (data.length <= 0) return;

    data.forEach(function (x) {
        if (jQuery.inArray(x.quote, MarketQuote) <= -1)
            MarketQuote.push(x.quote);

        Markets.push({
            name: x.name, code: x.mCode, base: x.base, quote: x.quote, high: x.highest24H, low: x.lowest24H, cat: x.category, price: x.value, volume: x.volume, change: x.change, cashback: x.cashBackYTD, cashback24h: x.cashBack24h, poly: x.poly24Hr, marketcap: x.marketCap, highlight: x.highlightTill, newList: x.newListingTill
        });
    })
    builMarketTabs();
    console.log('Markets', Markets);

}
function ApplySearch() {
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
                // $('.my-navigation').show();
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
            $('.my-navigation').show();
        });
    });
}
//ApplySearch();
function getHtmlAllCrypto(d, name) {
    console.log('market dta', d, name)
    var fi = favList.indexOf(d.code.toLowerCase());
    var wi = watchList.indexOf(d.code.toLowerCase());
    var txt = '<tr> <td class="td-sticky-left"> <div class="coin-name-css46">';
    if (d.base == 'TechnoSavvy') {
        txt += '<a href = "/about-TechnoSavvy" class="d-flex me-2" target = "_blank" >';
    } else {
        txt += '<a href = "/token/?tname=' + d.base.toLowerCase() + '" class="d-flex me-2" target = "_blank" >';
    }

    txt += '<span><img src="../images/coin/coin/' + d.base.toUpperCase() + '.png"></span> <span class="ethcss44">' + d.base + '</span> </a > <span class="arrow-css444">';
    if (fi != undefined && fi > -1)
        txt += '<span class="proper"> <a class="f_' + d.code + '" href="javascript:TogFav(\'' + d.code + '\')"> <i class="fa fa-star" aria-hidden="true"></i> </a></span>';
    else
        txt += '<span class="proper"> <a class="f_' + d.code + '" href="javascript:TogFav(\'' + d.code + '\')"> <i class="fa fa-star-o" aria-hidden="true"></i> </a></span>';
    var st = d.cat.match(/StableStable/i);
    if (!(st != null && st.index == 0))


        txt += '<span class="proper"  tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover" data-bs-content="Community Market: Cashback available on Trade" data-bs-original-title="" title="" ><img src="images/cash.png"></span> ';
    /*
    if (wi != undefined && wi > -1)
        txt += ' <a class="w_' + d.code + '" href="javascript:TogWatch(\'' + d.code + '\')"> <i class="fa fa-clock-o" aria-hidden="true"></i> </a>';
    else
        txt += ' <a class="w_' + d.code + '" href="javascript:TogWatch(\'' + d.code + '\')"> <i class="fa fa-clock-o" aria-hidden="true"></i> <sup class="supdiv43"> <i class="fa fa-plus" aria-hidden="true"></i> </sup></a>';
*/
    txt += ' </span></div> </td> <td > <div class="text-center"><span class="graph-css444"> <svg viewBox="0 0 500 100" class="chart"> <polyline fill="none" stroke="#d5960f" stroke-width="10" points="';
    if (d.poly != null) {
        for (var i = 0; i < d.poly.length; i++) {
            txt += (i * 20) + ',' + (d.poly[i]) + ' '
        }
    }
    txt += '"></polyline> </svg> </span></div> </td>';

    if (0 <= d.change) {
        txt += ' <td><div class="coin-24change-css46 green dyCur" data-oval="' + d.price + '">' + GetFormatedCurrency(d.price) + '</div></td >';
        txt += ' <td> <div class="coin-24change-css46 green">' + GetPercentFormat(d.change, 2) + '</div> </td>';
    } else {
        txt += ' <td><div class="coin-24change-css46 red dyCur" data-oval="' + d.price + '">' + GetFormatedCurrency(d.price) + '</div></td >';
        txt += ' <td> <div class="coin-24change-css46 red">' + GetPercentFormat(d.change, 2) + '</div> </td>';
    }
    txt += '<td> <div class="coin-24change-css46 dyCurLen-1" data-mainvalue="' + d.volume + '" data-oval="' + d.volume + '" >' + GetFormatCountMBTQ(d.volume, 2) + '</div> </td>';
    if (name == 'newList') {
        txt += '<td> <div class="coin-24change-css46"><span class="dyCur" data-oval="' + d.high + '">' + GetFormatedCurrency(d.high) + '</span> / <span class="dyCur" data-oval="' + d.low + '">' + GetFormatedCurrency(d.low) + '</span></div> </td>';
    }
    txt += '<td> <div class="coin-24change-css46 dyCurLen" data-mainvalue="' + d.cashback24h + '" data-oval="' + d.cashback24h + '">' + GetFormatedCurrencyMBTQ(d.cashback24h) + '</div> </td> <td nowrap="nowrap"> <div class="market-trade-css46 white defCursor dyCurLen" data-mainvalue="' + d.cashback + '" data-oval="' + d.cashback + '">' + GetFormatedCurrencyMBTQ(d.cashback) + ' </div> </td> ';

    txt += ' </tr>';
    return txt;

}


function TogWatch(d) {
    var fi = favList.indexOf(d.toLowerCase());
    if (fi < 0) {
        $.ajax({
            url: '../home/AddtoWatch', data: { "str": d },
            success: function (data) {
                if (data.match(/ackn/i) != null) {
                    watchList.push(d.toLowerCase());
                    //update HTML
                    $('.w_' + d).html(' <i class="fa fa-clock" aria-hidden="true"></i> ');
                }
            }
        });
    }
    else
        $.ajax({
            url: '../home/RemoveFromWatch', data: { "str": d },
            success: function (data) {
                if (data.match(/ackn/i) != null) {
                    watchList.splice(watchList.indexOf(d.toLowerCase()), 1);
                    //update HTML
                    $('.w_' + d).html(' <i class="fa fa-clock-o" aria-hidden="true"></i> <sup class="supdiv43"> <i class="fa fa-plus" aria-hidden="true"></i> </sup>');

                }
            }
        });

}

function TogFav(d) {
    var fi = favList.indexOf(d.toLowerCase());
    if (fi < 0) {
        $.ajax({
            url: '../home/AddtoFav', data: { "str": d },
            success: function (data) {
                if (data.match(/ackn/i) != null) {
                    favList.push(d.toLowerCase());
                    //update HTML
                    $('.f_' + d).html(' <i class="fa fa-star" aria-hidden="true"></i> ');
                }
            }
        });
    }
    else
        $.ajax({
            url: '../home/RemoveFromFav', data: { "str": d },
            success: function (data) {
                if (data.match(/ackn/i) != null) {
                    favList.splice(favList.indexOf(d.toLowerCase()), 1);
                    //update HTML
                    $('.f_' + d).html(' <i class="fa fa-star-o" aria-hidden="true"></i> ');

                }
            }
        });

}


$.get(MktAPI + 'GetMarketSummary', (data) => { MarketLoad(data) });
setInterval(() => {
    $.get(MktAPI + 'GetMarketSummary', (data) => { MarketLoad(data) });
}, 60000);
function MarketLoad(data) {
    homeMarketData(data);
    homeMarketMobileViewData(data)
    formateAll;
}

// Home Page Market section on Destop view
function homeMarketData(data) {

    $('#myMktTable-home tbody tr').remove();

    var fl = favList.indexOf(data.mCode);
    var wi = watchList.indexOf(data.mCode);

    var sortedByDate = data.sort(function (a, b) {
        let x = new Date(b.change) - new Date(a.change);
        return x;
    });

    var homeMarket = '';
    var count = 1;
    var maxCount = 10;

    sortedByDate.forEach(function (myValue) {

        if (count <= maxCount) {
            homeMarket += '<tr>';
            homeMarket += '<td><div class="coin-name-css46 td-sticky-left">';
            homeMarket += '<span><img src="../images/coin/coin/' + myValue.base.toUpperCase() + '.png" width="18" class="z-top"></span>',
                homeMarket += '<span class="paircoin-img"><img width="16" src="../images/coin/coin/' + myValue.quote.toUpperCase() + '.png"></span>',
                homeMarket += '<span class="ethcss44">' + myValue.name + '</span>';
            homeMarket += '<span class="arrow-css444">';
            if (favList.indexOf(myValue.mCode.toLowerCase()) > -1) {
                homeMarket += '<span class="proper">  <a class="defCursor f_' + myValue.mCode + '" href="javascript:void(0)"> <i class="fa fa-star" aria-hidden="true"></i> </a></span>';
            } else {
                homeMarket += '<span class="proper">  <a class="defCursor f_' + myValue.mCode + '" href="javascript:void(0)"> <i class="fa fa-star-o" aria-hidden="true"></i> </a></span>';
            }
            var st = myValue.category.match(/StableStable/i);

            if (!(st != null && st.index == 0))

                homeMarket += '<span class="proper" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Community Market: Cashback available on Trade" data-bs-original-title="" title=""> <img src="images/cash.png"></span>';
            /*if (wi != undefined && wi > -1)
                homeMarket += '<span class="proper" > <a class="defCursor w_' + myValue.mCode + '" href="javascript:TogWatch(\'' + myValue.mCode + '\')"><i class="fa fa-clock-o" aria-hidden="true"></i> </a></span>';
            else
                homeMarket += '<span class="proper" > <a class="defCursor w_' + myValue.mCode + '" href="javascript:TogWatch(\'' + myValue.mCode + '\')"><i class="fa fa-clock-o" aria-hidden="true"></i> </a></span>';*/
            homeMarket += '</span>';
            homeMarket += '</div></td>';
            if (0 <= myValue.change) {

                homeMarket += '<td> <span class="green dyCur" data-oval="' + myValue.value + '">' + GetFormatedCurrency(myValue.value, 6) + '</span></td>';
                homeMarket += '<td><span class="green">' + GetPercentFormat(myValue.change, 2) + '</span></td>';
            } else {

                homeMarket += '<td> <span class="red dyCur" data-oval="' + myValue.value + '">' + GetFormatedCurrency(myValue.value, 6) + '</span></td>';
                homeMarket += '<td><span class="red">' + GetPercentFormat(myValue.change, 2) + '</span></td>';
            }
            homeMarket += '<td> <span class="class="dyCurLen" data-oval="' + myValue.cashBack24h + '"> ' + GetFormatedCurrencyMBTQ(myValue.cashBack24h) + '</td>';
            homeMarket += '<td><span class="dyCurLen" data-oval="' + myValue.cashBackYTD + '">' + GetFormatedCurrencyMBTQ(myValue.cashBackYTD) + '</span></td>';
            /*homeMarket += '<td> <span class="class="curBmk dyCur" data-oval="' + myValue.cashBack24h + '"> ' + myValue.cashBack24h + '</td>';
            homeMarket += '<td><span class="curBmk" data-oval="' + myValue.cashBackYTD + '">' + myValue.cashBackYTD + '</span></td>';*/
            homeMarket += '</tr>';

            count++;
        }

    });

    //INSERTING ROWS INTO TBODY OF TABLE 
    $('#myMktTable-home tbody').html(homeMarket);
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
}

// Home Page Market Section on Mobile View
function homeMarketMobileViewData(data) {


    $('#marketMobileViewData .market-detail22').remove();

    var fl = favList.indexOf(data.mCode);
    var wi = watchList.indexOf(data.mCode);

    var sortedByDateMobile = data.sort(function (a, b) {
        let x = new Date(b.time) - new Date(a.time);
        return x;
    });


    var homeMobileView = '';
    var countMobile = 1;
    var maxCountMobile = 10;

    $.each(sortedByDateMobile, function (key, myValue) {

        if (countMobile <= maxCountMobile) {

            homeMobileView += '<div class="market-detail22 positionrelative33">';
            homeMobileView += '<div class="overlay-div">';
            homeMobileView += '<div class="name-m221 d-flex">';
            homeMobileView += '<span class="coin-name-css46">',
                homeMobileView += '<span><img src="../images/coin/coin/' + myValue.base.toUpperCase() + '.png" width="18" class="z-top"></span>',
                homeMobileView += '<span class="paircoin-img"><img width="16" src="../images/coin/coin/' + myValue.quote.toUpperCase() + '.png"></span>',
                homeMobileView += ' <small> ' + myValue.name + ' </small>';
            homeMobileView += '</span>';
            homeMobileView += '<span class="d-flex align-items-center gx-3">';
            if (favList.indexOf(myValue.mCode.toLowerCase()) > -1)
                homeMobileView += '<a class="f_' + myValue.mCode + '" href="javascript:void(0)"> <i class="fa fa-star" aria-hidden="true"></i> </a>';
            else
                homeMobileView += '<a class="f_' + myValue.mCode + '" href="javascript:void(0)"> <i class="fa fa-star-o" aria-hidden="true"></i> </a>';
            var st = myValue.category.match(/StableStable/i);

            if (!(st != null && st.index == 0))
                homeMobileView += '<a tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Community Market: Cashback available on Trade" data-bs-original-title="" title=""> <img src="images/cash.png" class="img-cashback me-0"></a>';

            homeMobileView += '</span>';
            homeMobileView += '</div>';
            if (0 <= myValue.change) {

                homeMobileView += '<div class="price-m22"><span> Last Price </span> <span class=" green dyCur" data-oval="' + myValue.value + '">' + GetFormatedCurrency(myValue.value, 6) + ' </span> </div>';
                homeMobileView += '<div class="price-m22"><span class="price-font11"> 24h Change(%) </span> <span class=" green">' + GetPercentFormat(myValue.change, 2) + '</span> </div>';
            } else {

                homeMobileView += '<div class="price-m22"><span> Last Price </span> <span class=" red dyCur" data-oval="' + myValue.value + '">' + GetFormatedCurrency(myValue.value, 6) + ' </span> </div>';
                homeMobileView += '<div class="price-m22"><span class="price-font11"> 24h Change(%) </span> <span class=" red">' + GetPercentFormat(myValue.change, 2) + '</span> </div>';
            }
            homeMobileView += '<div class="price-m22"><span>Cashback 24h</span> <span class="dyCurLen" data-oval="' + myValue.cashBack24h + '"> ' + GetFormatedCurrencyMBTQ(myValue.cashBack24h) + '</span> </div>';
            homeMobileView += '<div class="price-m22"><span>cashback YTD</span> <span class="dyCurLen" data-oval="' + myValue.cashBackYTD + '">' + GetFormatedCurrencyMBTQ(myValue.cashBackYTD) + '</span> </div>';
            homeMobileView += '</div>';
            homeMobileView += '</div>';
            countMobile++
        };



        //INSERTING ROWS INTO TBODY OF TABLE 
        $('#marketMobileViewData').html(homeMobileView);
        var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
        var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
            return new bootstrap.Popover(popoverTriggerEl)
        });
    });
};




