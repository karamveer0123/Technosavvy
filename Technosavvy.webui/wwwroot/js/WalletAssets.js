var walletDataArr = [];
var Markets = [];
var GetStake = [];
var TotalPortfolio = [];
var TotalCryptoHolding = [];
$.get(window.location.origin + '/Wallet/GetWalletAssets', (data) => {
    var wallet = JSON.parse(data);
    walletDataArray(wallet);
    wdataAssetsNotFound();
    // searchMarketCoin(wallet);
    // searchMarketCoinMobile(wallet);
    combindTokenMyAssets();
    combindTokenMyAssetsMobile(wallet);
    // LoadTotalPortfolioValue();

});

$.get(MktAPI + 'GetMarketSummary', (data) => {
    buildMarkets(data);
    CashbackAssets(data);
});


$(function () {
    function getUniqueListBy(arr, key) {
        return [...new Map(arr.map(item => [item[key], item])).values()]
    }

    $.get(window.location.origin + '/stake/GetOpportunities', (data) => {
        var stakData = JSON.parse(data);
        const stakFilter = getUniqueListBy(stakData, 'GroupName')
        console.log('stakFilter', stakFilter, stakData);
        stakFilter.forEach(function (s) {
            GetStake.push({ name: $.trim(s.Name) });
        })
    });


});



function buildMarkets(data) {
    console.log('buildMarkets', data)
    if (data.length <= 0) return;

    data.forEach(function (x) {
        Markets.push({
            name: x.name, code: x.mCode, base: x.base, quote: x.quote, high: x.highest24H, low: x.lowest24H, cat: x.category, price: x.value, volume: x.volume, change: x.change, cashback: x.cashBackYTD, cashback24h: x.cashBack24h, poly: x.poly24Hr, marketcap: x.marketCap, highlight: x.highlightTill, newList: x.newListingTill
        });
    })

    console.log('Markets', Markets);

}

function walletDataArray(data) {
    console.log('buildMarketsFee', data)
    if (data.length <= 0) return;
    data.forEach(function (x) {
        walletDataArr.push({
            Code: x.Code,
            ShortName: x.ShortName,
            cType: x.cType,
            Amt: x.Amt,
            BaseVal: x.BaseVal,
            IsFiat: x.IsFiat,
            tokenPrecision: x.tokenPrecision,
        });
    })
    combindTokenMyAssets();
    loadTotalPortfoilo();
    depositHand();
}


function wdataAssetsNotFound() {
    var myAssets = walletDataArr;
    $('#myassetdata1 table tfoot').hide();
    console.log('myAssets No record', myAssets.length)
    if (myAssets.length == 0) {
        setInterval(function () {
            $('#myAssetTabal-01').hide();
            $('#myassetdata1 table tfoot').show();
            $('#pageParent .my-navigation').hide();
        }, 5000)
    } else {
        combindTokenMyAssets();
    }
}

function combindTokenMyAssets() {
    var txt = '';
    var getToken = Markets;
    var fiatFlag = allCur;
    var statData = GetStake;
    var myAssets = walletDataArr;
    $('#myassetdata1 table tbody').html('');
    myAssets.forEach(function (d) {
        console.log('myAssets', d, fiatFlag)
        let filterMyToken = getToken.filter((n) => {
            return n.base.toUpperCase() == d.Code.toUpperCase()
        });

        var stake;
        if (statData.length != 0) {
            let filterStake = statData.filter((s) => {
                return s.name.toUpperCase() == d.Code.toUpperCase()
            });
            if (filterStake.length) {
                stake = filterStake[0].name;
            } else {
                stake = 'Empty';
            }
        }

        var flag;
        if (fiatFlag.length != 0) {
            let filterFiat = fiatFlag.filter((f) => {
                return f.abbr.toUpperCase() == d.Code.toUpperCase();
            });
            if (filterFiat.length) {
                flag = filterFiat[0].country;
                var cunValue = filterFiat[0].value;
            } else {
                flag = 'Empty';
                var cunValue = 0;
            }

        } else {
            flag = "Empty";
            var cunValue = 0;
        }
        console.log('filterMyToken', filterMyToken);
        if (filterMyToken.length > 0) {
            if (filterMyToken[0].price) {
                var BalAmt = filterMyToken[0].price * d.Amt;
            } else {
                var BalAmt = 0 * d.Amt;
            }
        } else {
            var BalAmt = d.Amt;
        }
        console.log('Desk BalAmt', BalAmt, d.Code);
        if (d.Code == 'USDT') {
            var BalAmt = d.Amt * 1;
        }
        var fiatVal = d.Amt / cunValue;
        txt += '<tr>',
            txt += '<td><div class="coinchangecss46"><a href="/token/?tname=' + d.Code.toLowerCase() + '"><div class="coin-name-css46 "><span>';
        if (!d.IsFiat) {
            txt += '<img src="../images/coin/coin/' + d.Code.toUpperCase() + '.png">';
        } else {
            txt += '<img src="../images/coun-flags/' + flag.toLowerCase() + '.png" width="25" height="25" class="rounded-circle">';
        }

        txt += '</span ><span>' + d.Code + ' </span><small> ' + d.ShortName + '</small></div ></a ></div ></td > ',
            txt += '<td><div class="coin-24change-css46  "><div class="css5waltab44"><span> ' + GetFormatedValWithSep(d.Amt, _fsl) + ' </span> <span> ';
        if (!d.IsFiat) {
            txt += '<small class="dyCur" data-oval="' + BalAmt + '"> ' + GetFormatedCurrency(BalAmt) + '</small>';
        } else {
            txt += '<small class="dyCur isfiat" data-oval="' + fiatVal + '"> ' + GetFormatedCurrency(fiatVal) + '</small>';
        }
        txt += '</span ></div ></div ></td > ',
            txt += '<td><span class="graph-css444 coin-24change-css46">',
            txt += '<svg viewBox="0 0 500 100" class="chart">',
            txt += '<polyline fill="none" stroke="#d5960f" stroke-width="10" points="';
        if (filterMyToken.length > 0) {
            for (let i = 0; i < filterMyToken[0].poly.length; i++) {
                txt += i * 20 + ',' + filterMyToken[0].poly[i] + ' ';
            }
        } else {
            txt += '00, 10  20, 10  40, 10  60, 10  80, 10  100, 10  120, 10   140, 10  160, 10  180, 10  200, 10    220, 10   240, 10            260, 10     280, 10    300, 10    320, 10    340, 10    360, 10     380, 10     400, 10   420, 10    440, 10';
        }
        txt += '"></polyline>',
            txt += '</svg>',
            txt += '</span>',
            txt += '</td>',
            txt += '<td class="linkoverflow">',
            txt += '<div class="tablediv432"><div class="spot-trade-css44">';
        if (!d.IsFiat) {
            txt += '<span> <a href="/buy?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Buy cryptocurrency effortlessly and conveniently with Fiat." data-bs-original-title="" title="" aria-describedby="popover531066">Buy</span></a></span>',
                txt += '<span> <a href="/trade?cat=trade&t=' + d.Code.toLowerCase() + '&q=usdt" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Buy cryptocurrency effortlessly and conveniently with Fiat." data-bs-original-title="" title="" aria-describedby="popover531066">Trade</span></a></span>';
            if (stake == d.Code) {
                txt += '<span> <a href="/stake?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Buy cryptocurrency effortlessly and conveniently with Fiat." data-bs-original-title="" title="" aria-describedby="popover531066">Stake</span></a></span>';
            } else {
                txt += '<span> <a href="javascript:void(0)" class=""><span class="d-inline-block disable2">Stake</span></a></span>';
            }
            txt += '<span> <a href="/Deposit?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Conveniently deposit crypto from another account to your TechnoApp account." data-bs-original-title="" title="" aria-describedby="popover531066">Deposit</span></a></span>',
                txt += '<span> <a href="/withdraw?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Withdraw your crypto from your TechnoApp account to another account." data-bs-original-title="" title="" aria-describedby="popover531066">Withdraw</span></a></span>',
                txt += '<span> <a href="/wallet/inter-wallet-transfer?code=' + d.Code.toLowerCase() + '" asp-action="inter-wallet-transfer"><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Transfers crypto Internal wallet. Internal transfers are free on TechnoApp." data-bs-original-title="" title="" aria-describedby="popover531066">Transfer</span></a></span>',
                txt += '<span> <a href="/convert?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Convert your crypto into your desired asset, from as low as 10 USD to multi-million USD." data-bs-original-title="" title="" aria-describedby="popover531066">Convert</span></a></span>',
                txt += '<span> <a href="/convert/convertwithdraw?code=' + d.Code.toLowerCase() + '" asp-action="convertwithdraw"><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Convert & withdraw your crypto into your desired asset, from as low as 10 USD to multi-million USD." data-bs-original-title="" title="" aria-describedby="popover531066">Convert & Widthdraw</span></a></span>';
        } else {
            txt += '<span> <a href="/buy?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Buy cryptocurrency effortlessly and conveniently with Fiat." data-bs-original-title="" title="" aria-describedby="popover531066">Buy</span></a></span>',
                txt += '<span> <a href="/Deposit?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Conveniently deposit crypto from another account to your TechnoApp account." data-bs-original-title="" title="" aria-describedby="popover531066">Deposit</span></a></span>',
                txt += '<span> <a href="/withdraw?code=' + d.Code.toLowerCase() + '" ><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Withdraw your crypto from your TechnoApp account to another account." data-bs-original-title="" title="" aria-describedby="popover531066">Withdraw</span></a></span>';
        }
        txt += '</div>',
            txt += '<span class="smalldevice332">',
            txt += '<div id="market-more" class="market-more-css45">',
            txt += '<div class="dropdowndiv togglenone">',
            txt += '<a type="button" class="btn btn-more-css44 dropdown-toggle" data-bs-toggle="dropdown"> <i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>',
            txt += '<ul class="dropdown-menu   dropdown-content">';
        if (!d.IsFiat) {
            txt += '<li><a class="dropdown-item" href="/buy?code=' + d.Code + '" >Buy  </a></li>',
                txt += '<li><a class="dropdown-item" href="/trade?cat=trade&t=' + d.Code + '&q=usdt" >Trade</a></li>';
            if (stake == d.Code) {
                txt += '<li><a class="dropdown-item" href="/stake?code=' + d.Code + '" > Stake</a></li>';
            } else {
                txt += '<li><a class="dropdown-item" href="javascript:void(0)" > Stake</a></li>';
            }
            txt += '<li><a class="dropdown-item" href="/Deposit?code=' + d.Code + '" > Deposit</a></li>',
                txt += '<li><a class="dropdown-item" href="/withdraw?code=' + d.Code + '" >Withdraw  </a></li>',
                txt += '<li><a class="dropdown-item" href="/wallet/inter-wallet-transfer?code=' + d.Code + '" >Transfer</a></li>',
                txt += '<li><a class="dropdown-item" href="/convert?code=' + d.Code + '" > Convert</a></li>',
                txt += '<li><a class="dropdown-item" href="/convert/convertwithdraw?code=' + d.Code + '" > Convert &amp; Withdraw</a></li>';
        } else {
            txt += '<li><a class="dropdown-item" href="/buy?code=' + d.Code + '" >Buy  </a></li>',
                txt += '<li><a class="dropdown-item" href="/Deposit?code=' + d.Code + '" > Deposit</a></li>',
                txt += '<li><a class="dropdown-item" href="/withdraw?code=' + d.Code + '" >Withdraw  </a></li>';
        }
        txt += '</ul>',
            txt += '</div>',
            txt += '</div>',
            txt += '</span>',
            txt += '</div>',
            txt += '</td>',
            txt += '</tr>'
    });

    $('#myassetdata1 table tbody').append(txt);

    $('#pageParent').simplePagination({
        items_per_page: 10
    });

    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
    ApplySearch()
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
                $('.my-navigation').show();
            } else {
                $(fntb).nextAll('tfoot').show();
                $('.my-navigation').hide();
                $(fntb).parent().parent().children('.trasaction-table').hide();
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



function loadTotalPortfoilo() {
    var myAssets = walletDataArr;
    var txt = '';
    var getToken = Markets;
    var fiatFlag = allCur;
    myAssets.forEach(function (x) {
        console.log('TotalPortfolio', x)
        let filterMyToken = getToken.filter((n) => {
            return n.name.toUpperCase() == x.Code.toUpperCase()
        });
        if (filterMyToken.length > 0) {
            var bVal = filterMyToken[0].value;
        } else {
            var bVal = 1;
        }

        var flag;
        if (fiatFlag.length != 0) {
            let filterFiat = fiatFlag.filter((f) => {
                return f.abbr.toUpperCase() == x.Code.toUpperCase()
            });
            if (filterFiat.length > 0) {
                flag = filterFiat[0].abbr;
            } else {
                flag = 'Empty'
            }
        }
        if (flag == x.Code) {
            console.log('true flag', flag);
        } else {
            console.log('false flag', flag)
            TotalCryptoHolding.push({ ammount: x.Amt, tokVal: bVal });
        }


        TotalPortfolio.push({ ammount: x.Amt, tokVal: bVal })


    });

    const AllTotal = TotalPortfolio.reduce((accumulator, currentObject) => {
        return accumulator + (currentObject.ammount * currentObject.tokVal);
    }, 0);

    const AllCriptoTotal = TotalCryptoHolding.reduce((accumulator, currentObject) => {
        return accumulator + (currentObject.ammount * currentObject.tokVal);
    }, 0);

    var Alloval = $('#id_password2 .dyCur').attr('data-oval');
    if (Alloval == 'NaN') {
        $('#id_password2 small').replaceWith('<small class="dyCur" data-dval="6" data-oval="' + AllTotal + '">' + AllTotal + '</small>');
        
    }
    
    var CryOval = $('#CryHolding').find('.dyCur').attr('data-oval');
    if (CryOval == 'NaN') {
        $('#CryHolding').find('.dyCur').replaceWith('<small class="dyCur" data-dval="6" data-oval="' + AllCriptoTotal + '">' + AllCriptoTotal + '</small>')
    }

    //console.log('Totla my Assets', myAssets, TotalPortfolio, TotalCryptoHolding, AllTotal, AllCriptoTotal, Alloval, CryOval);
}



/*
function searchMarketCoin(data) {
    return;
    var myAssets = walletDataArr;
    $('#searchbox435').html('');
    var txt = ''; 
   
    txt += '<select id="searchMarketByCoin" class="my-select" aria-label="Default select example" style="display: none;">';    
    myAssets.forEach(function (d) {
        
        if (!d.IsFiat) {
            txt += '<option data-dval="' + d.tokenPrecision + '" data-oval="' + d.Amt + '" selected data-img-src="../images/coin/coin/' + d.Code.toUpperCase() + '.png">' + d.Code + '</option>';
        }
       // if (d.Code == 'ETH') {
       //   txt += '<option data-dval="' + d.tokenPrecision + '" data-oval="' + d.Amt + '" selected data-img-src="../images/coin/coin/' + d.Code.toUpperCase() + '.png">' + d.Code + '</option>';
       // } else {
       //  txt += '<option data-dval="' + d.tokenPrecision + '" data-oval="' + d.Amt + '" selected data-img-src="../images/coin/coin/' + d.Code.toUpperCase() + '.png">' + d.Code + '</option>';
       // }
        
    });
    txt += '</select>';   
    $('#searchbox435').html(txt);
    
    selectDropdown();
    SearchByAssets();
}

function selectDropdown() {
    $("#searchMarketByCoin").chosen({ width: "100%" });
    
}
*/

/*
function SearchByAssets() {
    var searchValue = $('#searchMarketByCoin');
    searchValue.on('change', function () {
        console.log($(this).val());
        var assetSelect = $(this).val();
        var getToken = Markets;
        var txt = '';
        var txt2 = '';
        $('#id_password2').html('');
        $('#CryHolding').html('');
        const total = walletDataArr.reduce((accumulator, currentObject) => {
            if (!isNaN(currentObject.BaseVal)) {
                return accumulator + currentObject.BaseVal;
            } else { return accumulator }
        }, 0);
        console.log('fToken total', total);

       // const totalCripto = walletDataArr.reduce((accumulator, cb) => {
       //     if (!cb.IsFiat) {
       //         console.log('cb.BaseVal', cb.BaseVal);
       //       if (!isNaN(cb.BaseVal)) {
       //         return accumulator + cb.BaseVal;
       //   } else { return accumulator }
       // }
       // }, 0);
       //console.log('fToken total', totalAll, 'totalCripto', totalCripto);


        
        walletDataArr.forEach(function (d) {
            let filterMyToken = getToken.filter((n) => {
                return n.name.toUpperCase() == d.Code.toUpperCase()
            });
            
            console.log('filterMyToken', filterMyToken);
            if (d.IsFiat) {              
                var bVal = d.Amt;                
            } else {
                if (filterMyToken.length > 0) {
                    var bVal = filterMyToken[0].value;
                    var Change24 = filterMyToken[0].change;
                } else {
                    var bVal = 1;
                    var Change24 = 0;
                }
            }
            
            
            var avgPrice = total / bVal;            

            console.log('avgPrice', d.BaseVal, bVal, avgPrice)
            if (d.Code == assetSelect) {
                txt += '<span class="dyCur2" data-dval="' + d.tokenPrecision + '" data-oval="' + avgPrice + '">' + avgPrice.toFixed(6) + ' </span><span>' + d.Code + '</span>  <small data-dval="' + d.tokenPrecision + '" data-oval="' + total + '">' + GetFormatedCurrency(total) + ' </small>';

                
                txt2 += '<span class="css77ethno"><span class="dyCur2" data-dval="' + d.tokenPrecision + '" data-oval="' + avgPrice + '">' + avgPrice.toFixed(6) + ' ' + d.Code + '</span> <small class="dyCur2" data-dval="' + d.tokenPrecision + '" data-oval="' + total + '"> ' + GetFormatedCurrency(total) + ' </small> </span>',
                txt2 += '<span class="css77-value24per">';
                if (0 <= Change24) {
                    txt2 += '<span class="green wallet-i-2"><i class="fa fa-long-arrow-up" aria-hidden="true"></i> ' + GetPercentFormat(Change24, 2) + '  </span >';
                } else {
                    txt2 += '<span class="red wallet-i-2"><i class="fa fa-long-arrow-down" aria-hidden="true"></i> ' + GetPercentFormat(Change24, 2) + '  </span >';
                }
                
                txt2 += '<span></span><small>(24h)</small></span > '


            }
        });
        $('#id_password2').html(txt);
        $('#CryHolding').html(txt2);
        formateAll();
    });
  
}

*/

/*
function searchMarketCoinMobile(data) {
    var myAssets = walletDataArr;
    $('#searchbox4356').html('');
    var txt2 = ""
    txt2 += '<lable class="css77-abal">Select Coin</lable><select id="searchMarketByCoin2" class="my-select form-control" >';
    myAssets.forEach(function (d) {
        if (!d.IsFiat) {
            txt2 += '<option data-dval="' + d.tokenPrecision + '" data-oval="' + d.Amt + '" selected data-img-src="../images/coin/coin/' + d.Code.toUpperCase() + '.png">' + d.Code + '</option>';
        }
       
    });
    txt2 += '</select>';

    $('#searchbox4356').html(txt2);
    selectDropdown2();
    SearchByAssetsMobile()
}

function selectDropdown2() {
    $("#searchMarketByCoin2").chosen({ width: "100%" });
}

function SearchByAssetsMobile() {
    var searchValue = $('#searchMarketByCoin2');
    searchValue.on('change', function () {
        console.log($(this).val());
        var assetSelect = $(this).val();
        var getToken = Markets;
        var txt = '';
        var txt2 = '';
        $('#id_password3').html('');
        $('#CryHolding2').html('');
        const total = walletDataArr.reduce((accumulator, currentObject) => {
            if (!isNaN(currentObject.BaseVal)) {
                return accumulator + currentObject.BaseVal;
            } else { return accumulator; }
        }, 0);
        console.log('total', total);

        var cruptoTotal = 0;
        $.each(myAssets, function (index, data) {
            if (!data.IsFiat) {
                if (!isNaN(data.BaseVal)) {
                    cruptoTotal += data.BaseVal
                }
            }
            return cruptoTotal;
        })
        console.log('total2', cruptoTotal);

        walletDataArr.forEach(function (d) {
            let filterMyToken = getToken.filter((n) => {
                return n.name.toUpperCase() == d.Code.toUpperCase()
            });

            if (d.IsFiat) {
                var bVal = d.Amt;
            } else {
                if (filterMyToken.length > 0) {
                    var bVal = filterMyToken[0].value;
                    var Change24 = filterMyToken[0].change;
                } else {
                    var bVal = 1;
                    var Change24 = 0;
                }
            }

            var avgPrice = total / bVal;
            var avgPrice2 = cruptoTotal / bVal;
            console.log('avgPrice', d.BaseVal, bVal, avgPrice)
            if (d.Code == assetSelect) {
                txt += '<span class="dyCur2" data-dval="' + d.tokenPrecision + '" data-oval="' + avgPrice + '">' + avgPrice.toFixed(6) + ' </span><span>' + d.Code + '</span>  <small data-dval="' + d.tokenPrecision + '" data-oval="' + total + '">' + GetFormatedCurrency(total) + ' </small>';
                txt2 += '<span class="css77ethno"><span class="dyCur2" data-dval="' + d.tokenPrecision + '" data-oval="' + avgPrice2 + '">' + avgPrice2.toFixed(6) + ' ' + d.Code + '</span> <small class="dyCur2" data-dval="' + d.tokenPrecision + '" data-oval="' + cruptoTotal + '"> ' + GetFormatedCurrency(cruptoTotal) + ' </small> </span>',
                txt2 += '<span class="css77-value24per">';
                if (0 <= Change24) {
                    txt2 += '<span class="green wallet-i-2"><i class="fa fa-long-arrow-up" aria-hidden="true"></i> ' + GetPercentFormat(Change24, 2) + '  </span >';
                } else {
                    txt2 += '<span class="red wallet-i-2"><i class="fa fa-long-arrow-down" aria-hidden="true"></i> ' + GetPercentFormat(Change24, 2) + '  </span >';
                }
                txt2 += '<span></span><small>(24h)</small></span > '
            }
        });
        $('#id_password3').html(txt);
        $('#CryHolding2').html(txt2);
        formateAll();
    });

}

function LoadTotalPortfolioValue() {
    
    var myAssets = walletDataArr
    var txt = '';
    var txt2 = '';
    var getToken = Markets;
    $('#id_password2, #id_password3').html('');
    $('#CryHolding, #CryHolding2').html('');
    var selectVal = $('#searchMarketByCoin').val();
    var selectVal2 = $('#searchMarketByCoin2').val();
    const total = myAssets.reduce((accumulator, currentObject) => {
        if (!isNaN(currentObject.BaseVal)) {
            return accumulator + currentObject.BaseVal;
        } else { return accumulator; }
    }, 0);
       

    var cruptoTotal = 0;    
    $.each(myAssets, function (index, data) {
        if (!data.IsFiat) {
            if (!isNaN(data.BaseVal)) {
                cruptoTotal += data.BaseVal
            }
        }
        return cruptoTotal;
    })
    console.log('total2', cruptoTotal);
    myAssets.forEach(function (x) {

        let filterMyToken = getToken.filter((n) => {
            return n.name.toUpperCase() == x.Code.toUpperCase()
        });
        if (filterMyToken.length > 0) {
            var bVal = filterMyToken[0].value;
            var Change24 = filterMyToken[0].change;
        } else {
            var bVal = 1;
            var Change24 = 0;
        }

        var avgPrice = total / bVal;
        var avgPrice2 = cruptoTotal / bVal;
        
        console.log('avgPrice', x.BaseVal, bVal, avgPrice)
        if (x.Code == selectVal || x.Code == selectVal2) {
            txt += '<span class="dyCur2" data-dval="' + x.tokenPrecision + '" data-oval="' + avgPrice + '">' + avgPrice.toFixed(6) + ' </span><span> ' + x.Code + '</span>  <small  class="dyCur" data-dval="' + x.tokenPrecision + '" data-oval="' + total + '">' + GetFormatedValAdjWithSep(total) + ' </small>';
            txt2 += '<span class="css77ethno"><span class="d-flex gap-1 dyCur2" data-dval="' + x.tokenPrecision + '" data-oval="' + avgPrice2 + '">' + avgPrice2.toFixed(6) + ' <span>' + x.Code + '</span></span> <small class="dyCur"   data-oval="' + cruptoTotal + '"> ' + GetFormatedCurrency(cruptoTotal, __nf) + ' </small> </span>',
                txt2 += '<span class="css77-value24per">';
            if (0 <= Change24) {
                txt2 += '<span class="green wallet-i-2"><i class="fa fa-long-arrow-up" aria-hidden="true"></i> ' + GetPercentFormat(Change24, 2) + '  </span >';
            } else {
                txt2 += '<span class="red wallet-i-2"><i class="fa fa-long-arrow-down" aria-hidden="true"></i> ' + GetPercentFormat(Change24, 2) + '  </span >';
            }

            txt2 += '<span></span><small>(24h)</small></span > '
        }
    });
    $('#id_password2,  #id_password3').html(txt);
    $('#CryHolding, #CryHolding2').html(txt2);
    formateAll();
}
*/

/*
function LoadTotalPortfolioValue() {

    var myAssets = walletDataArr;
    var txt = '';  
    var getToken = Markets;
  
    

    myAssets.forEach(function (x) {
        console.log('TotalPortfolio', x)
        let filterMyToken = getToken.filter((n) => {
            return n.name.toUpperCase() == x.Code.toUpperCase()
        });
        if (filterMyToken.length > 0) {
            var bVal = filterMyToken[0].value;
        } else {
            var bVal = 1;
        }

        TotalPortfolio.push({ ammount: x.Amt, tokVal: bVal })

       
    });
   const total = myAssets.reduce((accumulator, currentObject) => {
        if (!isNaN(currentObject.BaseVal)) {
            return accumulator + currentObject.BaseVal;
        } else { return accumulator; }
    }, 0);
    console.log('totalPortfolio', TotalPortfolio);
}
LoadTotalPortfolioValue();*/
function uniqDataList(data, key) {
    return [
        ...new Map(
            data.map(x => [key(x), x])
        ).values()
    ]
}

$.get(window.location.origin + '/Wallet/getmycashback', function (data) {
    var cashbackData = JSON.parse(data);
    console.log('data cashback', cashbackData)
    orderNotFound(cashbackData);
    CashbackAssets(cashbackData)
})


function CashbackAssets(data) {
    var cbData = uniqDataList(Markets, it => it.name);
    var txt = "";
    $('#cashbackAssets, #cashbackAssets2').html('');
    const totalCashback = data.reduce((accumulator, currentObject) => {
        return accumulator + currentObject.CashBackTechnoSavvyValue;
    }, 0);
    console.log('cashback', Markets, cbData, totalCashback);
    console.log('cbData', cbData);
    cbData.forEach(function (x) {
        if (x.base == 'TechnoSavvy') {
            var npst = (x.price - (x.price / (1 + x.change))) * 1;
            var cbTechnoSavvy = totalCashback / x.price;
            txt += '<div class="nav22cash"><div class="textcasb">Total Cashback Earned</div>',
                txt += '<div class="css33-token csswallet77"><span><img src="images/nav-icon.png"> </span>',
                txt += '<span class="css33-icon-name"> ' + GetFormatedVal(cbTechnoSavvy, 6) + ' ' + x.base + ' <br> <small  class="dyCur"  data-oval="' + totalCashback + '"> ' + totalCashback + ' </small></span>',
                txt += '</div>',
                txt += '</div>',
                txt += '<div class="nav22cash">',
                txt += '<div class="css77-valuecheck css77-casgh mt-0">',
                txt += '<span class="css77ethno"><span>24h Change</span> ';
            if (0 <= x.change) {
                txt += '<div class="d-flex"><small class="me-2 green dyCur" data-oval="' + npst + '">' + GetFormatedCurrency(npst) + ' </small>';
                txt += '<span class="css44-up">(<i class="fa fa-long-arrow-up" aria-hidden="true"></i><small class="pl-1">' + GetPercentFormat(x.change, 2) + '</small>)</span>';
            } else {
                txt += '<div class="d-flex"><small class="me-2 red dyCur" data-oval="' + npst + '">' + GetFormatedCurrency(npst) + ' </small>';
                txt += '<span class="css44-down">(<i class="fa fa-long-arrow-down" aria-hidden="true"></i><small class="pl-1">' + GetPercentFormat(x.change, 2) + '</small>)</span>';
            }
            txt += '</div></span>',
                txt += '</div>',
                txt += '</div>'
        }
    });
    $('#cashbackAssets, #cashbackAssets2').html(txt);
}

function cashbackMyAssetsData(data) {
    var txt = '';
    $('#myCashback-566').html('');
    $('#myCashback2-566').html('');
    var count = 1;
    var maxCount = 50;
    $.each(data, function (key, c) {
        if (count <= maxCount) {
            txt += '<tr>',
                txt += '<td class="td-sticky-left"><div class="datereward44"><span>' + new moment.utc(c.CreatedOn).local().format('LL') + '</span> <small> ' + new moment.utc(c.CreatedOn).local().format('LT') + '</small></div></td>',

                txt += '<td><span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + c.TradeId + '" data-bs-original-title="" title="">' + c.TradeId.split('-')[0].toUpperCase() + '..' + '</span>  </td>',
                txt += '<td>' + GetFormatedVal(c.TradeValue, 6) + '</td>',
                txt += '<td>' + GetFormatedVal(c.CashBackTechnoSavvyValue, 6) + '</td>',
                txt += '<td>' + GetFormatedVal(c.CashBackTechnoSavvyTokens, 6) + '</td>';
            if (c.RewardTransactionId == '') {
                txt += '<td>PAID</td>';
            } else {
                txt += '<td>ACCRUED</td>';
            }
            txt += '</tr>';

            count++
        }
    });
    $('#myCashback-566').html(txt);
    $('#myCashback2-566').html(txt);
    $('#cashbackNorecords').hide();
    $('#cashbackNorecords2').hide();
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
}


function orderNotFound(data) {
    $('#cashbackDesktop table tfoot').hide();
    if (data.length == 0) {
        setInterval(function () {
            $('#cashbackNorecords').hide();
            $('#cashbackDesktop table tfoot').show();
        }, 3000)
    } else {
        cashbackMyAssetsData(data)
    }
}




var MyRewards = [];
function WallerRewards(data) {
    var txt = '';
    $('#walletReward table tbody, #walletReward2 table tbody').html('');
    $('#myrefrelNorecodes, #myrefrelNorecodes2').hide();
    $('.refLink').hide();
    data.forEach(function (r) {
        txt += '<tr>',
            txt += '<td nowrap class="text-left td-sticky-left"> 12/01/2023</td>',
            txt += '<td> 12/02/2022  </td>',
            txt += '<td>Signup </td>',
            txt += '<td nowrap>',
            txt += '<div class="datettrade44">',
            txt += '<div class="dflex22"><span>-</span> <small> TechnoSavvy</small> </div>',
            txt += '<div class="dflex22"><span> 1200</span> <small>USDT </small></div>',
            txt += '</div>',
            txt += '</td>',
            txt += '<td> Pending  </td>',
            txt += '</tr>'
    });
    if (data == '') {
        $('#myrefrelNorecodes, #myrefrelNorecodes2').show();
        $('.refLink').hide();
    } else {
        $('#walletReward table tbody, #walletReward2 table tbody').html(txt);
        $('#myrefrelNorecodes #myrefrelNorecodes2').hide();
        $('.refLink').show();
    }
}
WallerRewards(MyRewards)


/* Mobile View my Assets */

function combindTokenMyAssetsMobile() {
    var txt = '';
    var myAssets = walletDataArr;
    var getToken = Markets;
    var fiatFlag = allCur;
    var statData = GetStake;
    console.log('getToken', getToken);
    $('#myMktTaball-m-566').html('');
    myAssets.forEach(function (d) {
        let filterMyToken = getToken.filter((n) => {
            return n.base.toUpperCase() == d.Code.toUpperCase()
        });
        var stake;
        if (statData.length != 0) {
            let filterStake = statData.filter((s) => {
                return s.name.toUpperCase() == d.Code.toUpperCase()
            });
            if (filterStake.length) {
                stake = filterStake[0].name;
            } else {
                stake = 'Empty';
            }
        }
        console.log('stake', stake);
        var flag;
        if (fiatFlag.length != 0) {
            let filterFiat = fiatFlag.filter((f) => {
                return f.abbr.toUpperCase() == d.Code.toUpperCase()
            });
            if (filterFiat.length > 0) {
                flag = filterFiat[0].country;
                var cunValue = filterFiat[0].value;
            } else {
                flag = 'Empty';
                var cunValue = 0;
            }
        } else {
            flag = 'Empty';
            var cunValue = 0;
        }

        console.log('filterMyToken', filterMyToken);
        if (filterMyToken.length > 0) {
            if (filterMyToken[0].price) {
                var BalAmt = filterMyToken[0].price * d.Amt;
            } else {
                var BalAmt = 0 * d.Amt;
            }
        } else {
            var BalAmt = d.Amt;
        }
        console.log('Desk BalAmt', BalAmt, d.Code);
        if (d.Code == 'USDT') {
            var BalAmt = d.Amt * 1;
        }
        var fiatVal = d.Amt / cunValue;


        txt += '<div class="wallet__assets--items" data-filter-item data-filter-name="' + d.Code.toLowerCase() + '">',
            txt += '<div class="wallet__assets--item-top">',
            txt += '<div class="wallet__assets--item-top-name">';
        if (!d.IsFiat) {
            txt += '<span class="image"><img src="../images/coin/coin/' + d.Code.toUpperCase() + '.png" width="20" height="20"></span>';
        } else {
            txt += '<span class="image"><img src="../images/coun-flags/' + flag.toLowerCase() + '.png" width="20" height="20" class="rounded-circle"></span>';
        }
        txt += '<span class="assets__name">' + d.Code + ' <small>' + d.Code + ' </small></span></div>',
            txt += '<div class="wallet__assets--item-top-amount"><span class="sotAmt">' + GetFormatedValWithSep(d.Amt, _fsl) + '</span> <span class="amountToggle" ><i class="fa fa-angle-down" aria-hidden="true"></i></span></div>',
            txt += '<div class="wallet__assets--item-top-action">',
            txt += '<div class="dropdown walletDropdown dropstart">',
            txt += '<a class="btn btn-sm btn-secondary dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-bs-toggle="dropdown" aria-expanded="false">',
            txt += '<i class="fa fa-ellipsis-v" aria-hidden="true"></i>',
            txt += '</a>',
            txt += '<ul class="dropdown-menu" aria-labelledby="dropdownMenuLink">';
        if (!d.IsFiat) {
            txt += '<li><a class="dropdown-item" href="/buy?code=' + d.Code + '" >Buy  </a></li>',
                txt += '<li><a class="dropdown-item" href="/trade?cat=trade&t=' + d.Code + '&q=usdt" >Trade</a></li>';
            if (stake == d.Code.toLowerCase) {
                txt += '<li><a class="dropdown-item" href="/stake?code=' + d.Code + '" > Stake</a></li>'
            } else {
                txt += '<li><a class="dropdown-item" href="javascript:void(0)" > Stake</a></li>'
            }
            txt += '<li><a class="dropdown-item" href="/Deposit?code=' + d.Code + '"> Deposit</a></li>',
                txt += '<li><a class="dropdown-item" href="/withdraw?code=' + d.Code + '">Withdraw  </a></li>',
                txt += '<li><a class="dropdown-item" href="/wallet/inter-wallet-transfer?code=' + d.Code + '">Transfer</a></li>',
                txt += '<li><a class="dropdown-item" href="/convert?code=' + d.Code + '" > Convert</a></li>',
                txt += '<li><a class="dropdown-item" href="convert/convertwithdraw?code=' + d.Code + '"> Convert &amp; Withdraw</a></li>';
        } else {
            txt += '<li><a class="dropdown-item" href="/buy?code=' + d.Code + '" >Buy  </a></li>',
                txt += '<li><a class="dropdown-item" href="/Deposit?code=' + d.Code + '" > Deposit</a></li>',
                txt += '<li><a class="dropdown-item" href="/withdraw?code=' + d.Code + '" >Withdraw  </a></li>';
        }
        txt += '</ul>',
            txt += '</div>',
            txt += '</div>',
            txt += '</div>',
            txt += '<div class="wallet__assets--item-bottom">',
            txt += '<div class="wallet__assets--item--bottom-item">',
            txt += '<span class="wallet__assets--item--bottom-text">Total Balance</span>',
            txt += '<span class="wallet__assets--item--bottom-data">' + GetFormatedValWithSep(d.Amt, _fsl) + '</span>',
            txt += '</div>',
            txt += '<div class="wallet__assets--item--bottom-item">',
            txt += '<span class="wallet__assets--item--bottom-text">Available Balance</span>';
        if (!d.IsFiat) {
            txt += '<span class="wallet__assets--item--bottom-data dyCur" data-oval="' + BalAmt + '">' + GetFormatedCurrency(BalAmt) + '</span>';
        } else {
            txt += '<span class="wallet__assets--item--bottom-data dyCur" data-oval="' + fiatVal + '">' + GetFormatedCurrency(fiatVal) + '</span>';
        }
        txt += '</div>',
            txt += '</div>',
            txt += '</div>'
    });

    $('#myMktTaball-m-566').append(txt);
    $('#mobileMyAssets').simplePagination({
        pagination_container: '#myMktTaball-m-566',
        items_per_page: 10
    });

    $('.amountToggle').on('click', function () {
        $(this).closest('.wallet__assets--items').children('.wallet__assets--item-bottom').slideToggle();
    });

    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });

    var dropdownElementList = [].slice.call(document.querySelectorAll('.dropdown-toggle'))
    var dropdownList = dropdownElementList.map(function (dropdownToggleEl) {
        return new bootstrap.Dropdown(dropdownToggleEl)
    })
    divSearch()
}

function divSearch() {
    $('#myAssetNoRecodes4').hide();

    $('[data-search]').on('keyup', function () {

        var searchVal = $(this).val().replace(/\s/g, '').toLowerCase();
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



function depositHand() {
    var Alloval = $('#id_password2 .dyCur').attr('data-oval');
    if (Alloval == 0) {
        $('.wallDeposit').addClass("indicate");
    }
}
