
/*function ApplySearch() {
    $('.market-search').each(function () {
        var tn = $(this).attr('data-tab');
        $(this).on("keyup", function () {
            var value = $(this).val().replace(/\s/g, '').toLowerCase();
            //var value = $(this).val().toLowerCase();
            var f = '#' + tn + ' tr';
            $(f).filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1
            });
            $('.market-search').on('search', function () {
                $('.market-search').val('');
                $('.market-search').trigger('keyup');
            });
        });
    });
}*/
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
ApplySearch()
$('#fundingWalletTable-main').simplePagination({
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




var sportData = [];
var TotalPortfolio = [];
try {
    $.get(MktAPI + 'GetMarketSummary', (data) => {
        replaceTotal(data);
        depositHand()
    });
} catch (e) {
    console.log('Catch Error', e)
}

$('#myMktTaball-566 tr').each(function (e) {
    var x = $(this).find('td:nth-child(2) .dyVal').data('oval');
    var y = $(this).find('td:nth-child(1) .coin-name-css46 span:nth-child(2)').text();
    // console.log('value', x, y)
    sportData.push({ value: x, token: $.trim(y) });
});
function depositHand() {
    var Alloval = $('#id_funding .dyCur').attr('data-oval');
    if (Alloval == 0) {
        $('.wallDeposit').addClass("indicate");
    } else {
        $('.wallDeposit').removeClass("indicate");
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

    console.log('Sport Total', AllTotal);

    var Alloval = $('#id_funding .dyCur').text();
    if (Alloval == '$NaN') {
        $('#id_funding small').replaceWith('<small class="dyCur" data-dval="6" data-oval="' + AllTotal + '">' + AllTotal + '</small>');
    }
}


$(function () {
    try {
        $.get(window.location.origin + '/stake/GetOpportunities', (data) => {
            var stakData = JSON.parse(data);
            console.log('SwapData', stakData);

            fundingStake(stakData);

        });
    } catch (e) {
        console.log('Catch Error', e);
    }
    
    function fundingStake(data) {
        
        sportData.forEach((x, i) => {
            var xtok = $('.stakeitem-' + i).closest('tr').find('td:nth-child(1) .coin-name-css46 span:nth-child(2)').text();   
            let filterSport = data.filter((s) => {
                return s.Name == x.token;
            });

            if (filterSport.length > 0) {
                var tokName = filterSport[0].Name;
            }
           
                              
            console.log('Funding Stake', tokName, xtok, i);
           
            if ($.trim(xtok) === $.trim(tokName)) {           
                $('.stakeitem-' + i).addClass('active');                
            } else {
                $('.stakeitem-' + i).addClass('unactive');
                $('.stakeitem-' + i).children('a').attr('href', 'javascipt:void(0)');
                $('.stakeitem2-' + i).attr('href', 'javascipt:void(0)');
            }
        });
    }
});
