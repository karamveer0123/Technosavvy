
function ApplySearch() {
    $('.market-search2').each(function () {
        var tn = $(this).attr('data-tab');
        $(this).on("keyup", function () {
            var value = $(this).val().replace(/\s/g, '').toLowerCase();
            //var value = $(this).val().toLowerCase();
            var f = '#' + tn + ' tr';
            $(f).filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            });
            $('.market-search2').on('search', function () {
                $('.market-search2').val('');
                $('.market-search2').trigger('keyup');
            });
        });
    });
}
ApplySearch()
function ApplySearchWithNoRecord() {
    $('table tfoot').hide();
    $('.market-search').each(function () {
        var tn = $(this).attr('data-tab');
        $(this).on("keyup", function () {
            var value = $(this).val().toLowerCase();
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
ApplySearchWithNoRecord()
$('#earnWalletTable-main').simplePagination({
    items_per_page: 10,
    items_per_page_content: {
        '10': 10,
        '50': 50,
        '100': 100,
        '200': 200,
        '500': 500
    },
});
$('#MyStaking33').simplePagination({
    items_per_page: 10,
    items_per_page_content: {
        '10': 10,
        '50': 50,
        '100': 100,
        '200': 200,
        '500': 500
    },
});
$('#flexibleStaking-table').simplePagination({
    items_per_page: 10,
    items_per_page_content: {
        '10': 10,
        '50': 50,
        '100': 100,
        '200': 200,
        '500': 500
    },
});
$('#lockedStaking-table').simplePagination({
    items_per_page: 10,
    items_per_page_content: {
        '10': 10,
        '50': 50,
        '100': 100,
        '200': 200,
        '500': 500
    },
});
$('#StakingOpportunitiesTabs').simplePagination({
    items_per_page: 10,
    items_per_page_content: {
        '10': 10,
        '50': 50,
        '100': 100,
        '200': 200,
        '500': 500
    },
});


$('#mobileMyAssets').simplePagination({
    pagination_container: '#myMktTaball-m-566',
    items_per_page: 10,
    items_per_page_content: {
        '10': 10,
        '50': 50,
        '100': 100,
        '200': 200,
        '500': 500
    },
});


function divSearch() {
    $('#myAssetNoRecodes4').hide();

    $('[data-search]').on('keyup', function () {

        var searchVal = $(this).val();
        var filterItems = $('[data-filter-item]');

        if (searchVal != '') {
            filterItems.addClass('hidden');
            var filterTxt = $('[data-filter-item][data-filter-name*="' + searchVal.toLowerCase() + '"]');
            console.log('searchVal', searchVal, filterItems, filterTxt)
            if (filterTxt.length != 0) {
                $('#myAssetNoRecodes4').hide();
                $('.my-navigation').show();
                $('[data-filter-item][data-filter-name*="' + searchVal.toLowerCase() + '"]').removeClass('hidden');
            } else {
                console.log('no data found')
                $('.my-navigation').hide();
                $('#myAssetNoRecodes4').show();
            }

        } else {
            filterItems.removeClass('hidden');
            $('.my-navigation').show();
        }
    });
    $('[data-search]').on('search', function () {
        $('[data-search]').val('');
        $('[data-search]').trigger('keyup');
    });
}
divSearch();



var EarnWalletData = [];
function myAllStaking() {
    var txt = '';
    EarnWalletData.forEach(function (e) {
        txt += '<tr>',
            txt += '<td class="td-sticky-left"><a href="about-btc.html"><div class="coin-name-css46 ">',
            txt += '<span><img src="/images/coin/coin/@string.Concat(tk.Code.ToUpper(),".png")"></span>',
            txt += '<span>@tk.Code </span> <small> @tk.ShortName</small>',
            txt += '</div></a></td>',
            txt += '<td> 1.000200 </td>',
            txt += '<td>12-01-2022 </td>',
            txt += '<td>Yes </td>',
            txt += '<td>0.000200</td>',
            txt += '<td>0.000400</td>',
            txt += '<td>4.00% </td>',
            txt += '<td class="spot-trade-css44"><a asp-controller="stake" asp-action="redeem"><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Redeem your asset, it may affect your interest rewards." data-bs-original-title="" title="" aria-describedby="popover531066">Redeem</span></a> </td>',
            txt += '</tr>'
    });
    $('#allStaking').html(txt);
    $('#allStaking-m').html(txt);
    ApplySearchWithNoRecord();
    $('#allStaking-table').simplePagination({
        items_per_page: 10,
        items_per_page_content: {
            '10': 10,
            '50': 50,
            '100': 100,
            '200': 200,
            '500': 500
        },
    });
    $('#allStaking-table-m').simplePagination({
        items_per_page: 10,
        items_per_page_content: {
            '10': 10,
            '50': 50,
            '100': 100,
            '200': 200,
            '500': 500
        },
    });
}

//myAllStaking()

// All Staking
var allStaking = $('#allStaking tr').length;
if (allStaking == 0) {
    $('#allStaking-table table tfoot').show();
    $('#allStaking-table .my-navigation').hide();
}


// flexible Staking
var flexibleStaking = $('#FlexibleStaking tr').length;
if (flexibleStaking == 0) {
    $('#flexibleStaking-table table tfoot').show();
    $('#flexibleStaking-table .my-navigation').hide();
}


// Locked Staking
var lockedStaking = $('#lockedStaking tr').length;
if (lockedStaking == 0) {
    $('#lockedStaking-table table tfoot').show();
    $('#lockedStaking-table .my-navigation').hide();
}


// Staking Opportunities
var StakingOpportunities = $('#StakingOpportunities tr').length;
if (StakingOpportunities == 0) {
    $('#StakingOpportunities-table table tfoot').show();
    $('#StakingOpportunities-table .my-navigation').hide();
}

// Earn Cashback
var StakingOpportunities = $('#CashbackStaking tr').length;
if (StakingOpportunities == 0) {
    $('#earnCashback-table table tfoot').show();
    $('#earnCashback-table .my-navigation').hide();
}


$('#myStaking a[data-bs-toggle="tab"]').on('show.bs.tab', function (e) {
    let target = $(e.target).data('bs-target');
    $(target)
        .addClass('active show')
        .siblings('.tab-pane.active')
        .removeClass('active show')
});








var sportData = [];
var TotalPortfolio = [];
try {

    $.get(MktAPI + 'GetMarketSummary', (data) => {
        replaceTotal(data);
        depositHand()
    });

} catch (e) {
    console.log('Catch Error', e);
}

$('#myMktTaball-566 tr').each(function (e) {
    var x = $(this).find('td:nth-child(2) .dyVal').data('oval');
    var y = $(this).find('td:nth-child(1) .coin-name-css46 span:nth-child(2)').text();
    // console.log('value', x, y)
    sportData.push({ value: x, token: $.trim(y) });
});


function depositHand() {
    var Alloval = $('#id_earn .dyCur').attr('data-oval');
    if (Alloval == 0) {
        $('.wallDeposit').addClass("indicate");
    }
}

function replaceTotal(data) {
    var fiatFlag = allCur;
    sportData.forEach((x) => {

        let filterSport = data.filter((s) => {
            return s.base == x.token
        })

        if (filterSport.length > 0) {
            var bVal = filterSport[0].value;
        } else {
            var bVal = 1;
        }

        var flag;
        var curVal;
        if (fiatFlag.length != 0) {
            let filterFiat = fiatFlag.filter((f) => {
                return f.abbr.toUpperCase() == x.token.toUpperCase()
            });
            if (filterFiat.length > 0) {
                flag = filterFiat[0].abbr;
                curVal = filterFiat[0].value;
            } else {
                flag = 'Empty';
                curVal = 1;
            }
        }
        console.log('filter sport', filterSport, x, flag);

        if (flag == x.token) {
            TotalPortfolio.push({ ammount: x.value, tokVal: curVal })
        } else {
            TotalPortfolio.push({ ammount: x.value, tokVal: bVal })
        }
    });

    const AllTotal = TotalPortfolio.reduce((accumulator, currentObject) => {
        return accumulator + (currentObject.ammount * currentObject.tokVal);
    }, 0);

    //console.log('Sport Total', AllTotal);

    var Alloval = $('#id_earn .dyCur').text();
    if (Alloval == '$NaN') {
        $('#id_earn small').replaceWith('<small class="dyCur" data-dval="6" data-oval="' + AllTotal + '">' + AllTotal + '</small>');
    }
}



$.get(window.location.origin + '/Wallet/getmycashback', function (data) {
    var cashbackData = JSON.parse(data);
    console.log('data cashback', cashbackData)
    orderNotFound(cashbackData);
    $('#btnReset').on('click', function () {
        $('#fPeriod').val('').trigger('change');
        $('#earnCashback-table .my-navigation').show();
        $('#earnCashback-table').simplePagination({
            items_per_page: 10,
            items_per_page_content: {
                '10': 10,
                '50': 50,
                '100': 100,
                '200': 200,
                '500': 500
            },
        });
        orderNotFound(cashbackData)
    });
})

function cashbackMyAssetsData(data) {
    var txt = '';
    $('#CashbackStaking').html('');

    data.forEach(function (c) {
        var end = c.CreatedOn;
        var now = moment.utc(new Date());
        var days = now.diff(end, 'days');
        console.log('days', days)
        txt += '<tr>',
            txt += '<td class="td-sticky-left days"  data-days="' + days + '"><div class="datereward44"><span>' + new moment.utc(c.CreatedOn).local().format('LL') + '</span> <small> ' + new moment.utc(c.CreatedOn).local().format('LT') + '</small></div></td>',
            txt += '<td><span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + c.TradeId + '" data-bs-original-title="" title="">' + c.TradeId.split('-')[0].toUpperCase() + '..' + '</span>  </td>',            
            txt += '<td>' + GetFormatedVal(c.TradeValue, 6) + '</td>',
            txt += '<td>' + GetFormatedVal(c.CashBackTechnoSavvyValue, 6) + '</td>',
            txt += '<td>' + GetFormatedVal(c.CashBackTechnoSavvyTokens, 6) + '</td>';
        if (c.RewardTransactionId == '') {
            txt += '<td>PAID</td>';
        } else {
            txt += '<td>ACCRUED</td>';
        }
        txt += '</tr>'
    });
    $('#CashbackStaking').html(txt);
    $('#MyCashback33').simplePagination({
        items_per_page: 10,
        items_per_page_content: {
            '10': 10,
            '50': 50,
            '100': 100,
            '200': 200,
            '500': 500
        },
    });
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
}


function orderNotFound(data) {
    $('#earnCashback-table table tfoot').hide();
    if (data.length == 0) {
        setInterval(function () {
            $('#CashbackStaking').hide();
            $('#earnCashback-table table tfoot').show();
        }, 3000)
    } else {
        cashbackMyAssetsData(data)
    }
}


function filter_function() {
    $('#earnCashback-table table tbody tr').hide(); //hide all rows

    var fPeriod = 0;
    var fPeriodValue = $('#fPeriod').val();

    $('#earnCashback-table table tr').each(function () {

        if (fPeriodValue == 0) {
            fPeriod = 1;
        }
        else if (fPeriodValue == $(this).find('td.days').data('days')) {
            fPeriod = 1;
        }
        else {
            fPeriod = 0;
        }
        if (fPeriod) {
            console.log('search');
            $(this).show();  //displaying row which satisfies all conditions

        }

    });

}
$('.filter').change(function () {

    $('#earnCashback-table table tbody tr').show();
    $('#earnCashback-table .my-navigation').hide();
    filter_function();


});




