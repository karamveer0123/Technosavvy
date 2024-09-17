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
ApplySearch();





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




$('#mobileMyAssets').simplePagination({
    pagination_container: '#myMktTaball-m-566',
    items_per_page: 10
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

$('.iconFilter1').on('click', function () {
    $('#crytoDepositSearch').toggleClass('active');
});
$('.iconFilter2').on('click', function () {
    $('#fiatDepositSearch').toggleClass('active');
});

$('.btnTrnsFilter').on('click', function () {
    $('#crytoDepositSearch').removeClass('active');
    $('#fiatDepositSearch').removeClass('active');
})
$('#myTab a[data-bs-toggle="tab"], #interExterTabs a[data-bs-toggle="tab"]').on('show.bs.tab', function (e) {
    let target = $(e.target).data('target');
    $(target)
        .addClass('active show')
        .siblings('.tab-pane.active')
        .removeClass('active show');
    $('#crytoDepositSearch').removeClass('active');
    $('#fiatDepositSearch').removeClass('active');
});



var TransactionData = [];
LoadTra(JSON.parse(dCheck));
console.log('TransactionData', TransactionData);
function LoadTra(d) {
    TransactionData = d;
    InternalTransaction();
    ExternalTransaction();
    FiatTransaction();
    TradeRefundTransaction();
}


function InternalTransaction() {
    var txt = '';
    var now = moment.utc(new Date());
    var icount = 0;
    TransactionData.forEach(function (t) {
        var id = t.TransactionId;
        var result = $.grep(TransactionData, function (e) { return e.TransactionId == id; });
        icount = icount + 1;
        console.log(icount,t.TransactionId + ' has ' + result.length);
        console.log('result', result[0].isFrom)
        if (!t.IsFiatRepresentative) {
            if (t.IsWithInMyWallet) {
                if (t.TransactionId != '00000000-0000-0000-0000-000000000000') {
                    var type = (t.Amount <= 0) ? 'Deposit' : 'Withdraw';
                    var status = (t.TransactionId == '') ? 'Cancelled' : 'Successful';
                    var end = t.Date;
                    var days = now.diff(end, 'days');
                    if (t.isFrom) {
                        txt += '<tr>',
                            txt += '<td class="coin td-sticky-left" data-coin="' + t.TokenName + '"><a href="/token/?tname=' + t.TokenName + '"><div class="coin-name-css46">',
                            txt += '<span><img src="/images/coin/coin/' + t.TokenName + '.png"></span>',
                            txt += '<span>' + t.TokenName + '</span> <small>' + t.TokenName + '</small></div></a></td>',
                            txt += '<td class="days" data-days="' + days + '"><div class="css5datet44"><span> ' + new moment.utc(t.Date).local().format('LL') + ' </span> <span> <small> ' + new moment.utc(t.Date).local().format('LTS') + '</small></span></div></td>',
                            result.forEach(function (r) {
                                var rw = r.wName.replace('Wallet', '');
                                if (r.isFrom) {
                                    txt += '<td class="walName" data-wName="' + t.wName + '">' + rw + '</td>'
                                }
                            });
                        result.forEach(function (r) {
                            if (!r.isFrom) {
                                txt += '<td class="type" data-type="' + type + '" >' + Math.abs(GetFormatedValAdj(r.Amount, _fsl)) + '</td>'
                            }
                        });


                        result.forEach(function (r) {
                            var rw2 = r.wName.replace('Wallet', '');
                            if (!r.isFrom) {
                                txt += '<td >' + rw2 + '</td>';
                            }
                        });

                        txt += '<td data-trans="transHash"><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + t.TransactionId + '" data-bs-original-title="" title="" aria-describedby="popover531066"><a  href="javascript:void(0)">' + t.TransactionId + '</a></span></td>';
                        if (t.TransactionId == '') {
                            txt += '<td class="basered status"  data-status="' + status + '">Cancelled </td>';
                        } else {
                            txt += '<td  class="basegreen status"  data-status="' + status + '">Successful </td>';
                        }
                        txt += '</tr>'
                    }
                }
            } else {

            }
        } else {
            InternalTransactionNotFound();
        }
    });

    $('#InternalTransaction').html(txt);
    ApplySearchWithNoRecord();

    $('#transactionDataTable2').simplePagination({
        items_per_page: 10,
        items_per_page_content: {
            '10': 10,
            '50': 50,
            '100': 100,
            '200': 200,
            '500': 500
        },
    });

    InterlFilter();
}

function InterlFilter() {

    $('.Interfilter').change(function () {
        $('table#topscroll2 tbody tr').show();
        $('.my-navigation').hide();
        filter_function();

    });

    function filter_function() {
        $('table#topscroll2 tbody tr').hide();

        var interType = 0;
        var interTypeValue = $('#interType').val();
        var interPeriod = 0;
        var interPeriodValue = $('#interPeriod').val();
        var interAsset = 0;
        var interAssetValue = $('#interAsset').val();
        var interWallet = 0;
        var interWalletValue = $('#interWallet').val();
        var interStatus = 0;
        var interStatusValue = $('#interStatus').val();



        //traversing each row one by one
        $('table#topscroll2 tr').each(function () {
            // Inter Type
            if (interTypeValue == 0) {   //if no value then display row
                interType = 1;
            }

            else if (interTypeValue == $(this).find('td.type').data('type')) {
                interType = 1;       //if value is same display row
            }
            else {
                interType = 0;
            }

            // Inter Period
            if (interPeriodValue == 0) {
                interPeriod = 1;
            }
            else if (interPeriodValue == $(this).find('td.days').data('days')) {
                interPeriod = 1;
            }
            else {
                interPeriod = 0;
            }

            // Inter Asset
            if (interAssetValue == 0) {
                interAsset = 1;
            }
            else if (interAssetValue == $(this).find('td.coin').data('coin')) {
                interAsset = 1;
            }
            else {
                interAsset = 0;
            }

            // Inter Wallet
            if (interWalletValue == 0) {
                interWallet = 1;
            }
            else if (interWalletValue == $(this).find('td.walName').data('wname')) {
                interWallet = 1;
            }
            else {
                interWallet = 0;
            }

            // Inter Wallet
            if (interStatusValue == 0) {
                interStatus = 1;
            }
            else if (interStatusValue == $(this).find('td.status').data('status')) {
                interStatus = 1;
            }
            else {
                interStatus = 0;
            }


            if (interType && interPeriod && interAsset && interStatus && interWallet) {
                $(this).show();  //displaying row which satisfies all conditions
            }

        });
    }

    $('#InterBtnReset').on('click', function () {
        $('#interType').val('').trigger('change');
        $("#interAsset").val('').trigger('chosen:updated');
        $('#interWallet').val('').trigger('change');
        $('#interStatus').val('').trigger('change');
        $('#interPeriod').val('').trigger('change');
        $('.my-navigation').show();
        $('#transactionDataTable2').simplePagination({
            items_per_page: 10,
            items_per_page_content: {
                '10': 10,
                '50': 50,
                '100': 100,
                '200': 200,
                '500': 500
            },
        });
    });
}



function InternalTransactionNotFound() {
    $('#transactionDataTable2 table tfoot').hide();
    if (TransactionData.length == 0) {
        setInterval(function () {
            $('#InternalTransaction').hide();
            $('#transactionDataTable2 table tfoot').show();
        }, 5000)
    }
    //else {
    //    InternalTransaction()
    //}
    var ittr = $('#InternalTransaction tr').length;
    console.log('ittr', ittr);
    if (ittr <= 0) {
        $('#transactionDataTable2 .my-navigation').hide();
        $('#transactionDataTable2 table tfoot').show();
    }
}
InternalTransactionNotFound();



function ExternalTransaction() {
    var txt = '';

    TransactionData.forEach(function (t) {
        if (!t.IsFiatRepresentative) {
            if (!t.IsWithInMyWallet) {
                console.log('t.IsWithInMyWallet', t.IsWithInMyWallet)
                var type = (t.Amount <= 0) ? 'Deposit' : 'Withdraw';
                var status = (t.TransactionId == '') ? 'Cancelled' : 'Successful';
                var end = t.Date;
                var now = moment.utc(new Date());
                var days = now.diff(end, 'days');
                console.log('days', days)
                if (t.TransactionId != '00000000-0000-0000-0000-000000000000') {
                    if (t.wName != 'SpotWallet')
                    {
                        txt += '<tr>',
                            txt += '<td class="coin" data-coin="' + t.TokenName + '"><a href="/token/?tname=' + t.TokenName + '""><div class="coin-name-css46"><span><img src="/images/coin/coin/' + t.TokenName + '.png"></span><span>' + t.TokenName + '</span> <small>' + t.TokenName + '</small></div></a></td>',
                            txt += '<td class="days" data-days="' + days + '"><div class="css5datet44"><span> ' + new moment.utc(t.Date).local().format('LL') + '  </span> <span> <small> ' + new moment.utc(t.Date).local().format('LTS') + '</small></span></div></td>';
                        if (t.isFrom) {
                            txt += '<td class="type" data-type="' + type + '" >Withdraw</td>';
                        } else {
                            txt += '<td class="type" data-type="' + type + '" >Deposit</td>';
                        }
                        txt += '<td>' + Math.abs(GetFormatedValAdj(t.Amount, _fsl)) + ' </td>',
                            txt += '<td class="wName walName" data-wName="' + t.wName + '">' + t.wName + '</td>',
                            txt += '<td><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + t.TransactionId + '" data-bs-original-title="" title="" aria-describedby="popover531066"><a  target="_new" href="' + ethScanPre + t.TransactionId + '">' + t.TransactionId + '</a></span></td>';
                        if (t.TransactionId == '') {
                            txt += '<td class="basered status" data-status="' + status + '" >Cancelled </td>';
                        } else {
                            txt += '<td class="basegreen status" data-status="' + status + '" >Successful </td>';
                        }
                        txt += '</tr>'
                    }
                }
                //else {

                //}
            } else { }
        } else {
            ExternalTransactionNotFound()
        }
    });
    $('#ExternalTransaction').html(txt);
    ApplySearchWithNoRecord();
    $('#transactionDataTable3').simplePagination({
        items_per_page: 10,
        items_per_page_content: {
            '10': 10,
            '50': 50,
            '100': 100,
            '200': 200,
            '500': 500
        },
    });
    //ExternalFulter();
    $('.wName').each(function () {
        var wn = $(this).text();
        var nwn = wn.replace("Wallet", "");
        $(this).text(nwn);
    });

    ExternalFilter();

}

function ExternalFilter() {


    $('.Exterfilter').change(function () {
        $('table#topscroll3 tbody tr').show();
        $('.my-navigation').hide();
        filter_function();

    });

    function filter_function() {
        $('table#topscroll3 tbody tr').hide();

        var enterType = 0;
        var enterTypeValue = $('#enterType').val();
        var enterPeriod = 0;
        var enterPeriodValue = $('#enterPeriod').val();
        var enterAsset = 0;
        var enterAssetValue = $('#enterAsset').val();
        var enterWallet = 0;
        var enterWalletValue = $('#enterWallet').val();
        var enterStatus = 0;
        var enterStatusValue = $('#enterStatus').val();



        //traversing each row one by one
        $('table#topscroll3 tr').each(function () {
            // Inter Type
            if (enterTypeValue == 0) {   //if no value then display row
                enterType = 1;
            }

            else if (enterTypeValue == $(this).find('td.type').data('type')) {
                enterType = 1;       //if value is same display row
            }
            else {
                enterType = 0;
            }

            // Inter Period
            if (enterPeriodValue == 0) {
                enterPeriod = 1;
            }
            else if (enterPeriodValue == $(this).find('td.days').data('days')) {
                enterPeriod = 1;
            }
            else {
                enterPeriod = 0;
            }

            // Inter Asset
            if (enterAssetValue == 0) {
                enterAsset = 1;
            }
            else if (enterAssetValue == $(this).find('td.coin').data('coin')) {
                enterAsset = 1;
            }
            else {
                enterAsset = 0;
            }

            // Inter Wallet
            if (enterWalletValue == 0) {
                enterWallet = 1;
            }
            else if (enterWalletValue == $(this).find('td.walName').data('wname')) {
                enterWallet = 1;
            }
            else {
                enterWallet = 0;
            }

            // Inter Wallet
            if (enterStatusValue == 0) {
                enterStatus = 1;
            }
            else if (enterStatusValue == $(this).find('td.status').data('status')) {
                enterStatus = 1;
            }
            else {
                enterStatus = 0;
            }


            if (enterType && enterPeriod && enterAsset && enterStatus && enterWallet) {
                $(this).show();  //displaying row which satisfies all conditions
            }

        });
    }

    $('#btnExtReset').on('click', function () {
        $('#enterType').val('').trigger('change');
        $("#enterAsset").val('').trigger('chosen:updated');
        $('#enterWallet').val('').trigger('change');
        $('#enterStatus').val('').trigger('change');
        $('#enterPeriod').val('').trigger('change');
        $('#transactionDataTable3 .my-navigation').show();
        $('#transactionDataTable3').simplePagination({
            items_per_page: 10,
            items_per_page_content: {
                '10': 10,
                '50': 50,
                '100': 100,
                '200': 200,
                '500': 500
            },
        });
    });
}

function ExternalTransactionNotFound() {
    $('#transactionDataTable3 table tfoot').hide();
    if (TransactionData.length == 0) {
        setInterval(function () {
            $('#ExternalTransaction').hide();
            $('#transactionDataTable3 table tfoot').show();
        }, 5000)
    }
    //else {
    //    ExternalTransaction()
    //}
    var ittr = $('#ExternalTransaction tr').length;
    console.log('ittr', ittr);
    if (ittr <= 0) {
        $('#transactionDataTable3 .my-navigation').hide();
        $('#transactionDataTable3 table tfoot').show();
    }
}
ExternalTransactionNotFound()




function FiatTransaction() {
    var txt = '';

    TransactionData.forEach(function (t) {
        if (t.IsFiatRepresentative) {

            if (t.wName != 'SpotWallet') {
                var type = (t.Amount <= 0) ? 'Deposit' : 'Withdraw';
                var status = (t.TransactionId == '') ? 'Cancelled' : 'Successful';
                var end = t.Date;
                var now = moment.utc(new Date());
                var days = now.diff(end, 'days');
                console.log('days', days)

                txt += '<tr   >',
                    txt += '<td class="coin" data-coin="' + t.TokenName + '"><a href="/token/?tname=' + t.TokenName + '""><div class="coin-name-css46"><span><img src="/images/coin/coin/' + t.TokenName + '.png"></span><span>' + t.TokenName + '</span> <small>' + t.TokenName + '</small></div></a></td>',
                    txt += '<td class="days" data-days="' + days + '"><div class="css5datet44"><span> ' + new moment.utc(t.Date).local().format('LL') + '  </span> <span> <small> ' + new moment.utc(t.Date).local().format('LTS') + '</small></span></div></td>';
                if (t.Amount <= 0) {
                    txt += '<td class="type" data-type="' + type + '">Withdraw</td>';
                } else {
                    txt += '<td class="type" data-type="' + type + '">Deposit</td>';
                }
                txt += '<td>' + Math.abs(GetFormatedVal(t.Amount, _fsl)) + ' </td>',
                    txt += '<td class="wName">' + t.wName + '</td>',
                    txt += '<td><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + t.TransactionId + '" data-bs-original-title="" title="" aria-describedby="popover531066"><a  href="javascript:void(0)">' + t.TransactionId + '</a></span></td>';
                if (t.TransactionId == '') {
                    txt += '<td class="status" data-status="' + status + '">Cancelled </td>';
                } else {
                    txt += '<td class="basegreen status" data-status="' + status + '">Successful </td>';
                }
                txt += '</tr>';
            }
        } else {
           
        }
    });
    $('#fiatTransactionData').html(txt);
    $('.wName').each(function () {
        var wn = $(this).text();
        var nwn = wn.replace("Wallet", "");
        $(this).text(nwn);
    });

    $('#transactionDataTable4').simplePagination({
        items_per_page: 10,
        items_per_page_content: {
            '10': 10,
            '50': 50,
            '100': 100,
            '200': 200,
            '500': 500
        },
    });
    
    ApplySearchWithNoRecord();
    FiatFilter()
}


function FiatFilter() {


    $('.fiatfilter').change(function () {
        $('table#topscroll4 tbody tr').show();
        $('.my-navigation').hide();
        filter_function();

    });

    function filter_function() {
        $('table#topscroll4 tbody tr').hide();

        var fiatType = 0;
        var fiatTypeValue = $('#fiatType').val();
        var fiatPeriod = 0;
        var fiatPeriodValue = $('#fiatPeriod').val();
        var fiatAsset = 0;
        var fiatAssetValue = $('#fiatAsset').val();
        var fiatStatus = 0;
        var fiatStatusValue = $('#fiatStatus').val();



        //traversing each row one by one
        $('table#topscroll4 tr').each(function () {
            // Inter Type
            if (fiatTypeValue == 0) {   //if no value then display row
                fiatType = 1;
            }

            else if (fiatTypeValue == $(this).find('td.type').data('type')) {
                fiatType = 1;       //if value is same display row
            }
            else {
                fiatType = 0;
            }

            // Inter Period
            if (fiatPeriodValue == 0) {
                fiatPeriod = 1;
            }
            else if (fiatPeriodValue == $(this).find('td.days').data('days')) {
                fiatPeriod = 1;
            }
            else {
                fiatPeriod = 0;
            }

            // Inter Asset
            if (fiatAssetValue == 0) {
                fiatAsset = 1;
            }
            else if (fiatAssetValue == $(this).find('td.coin').data('coin')) {
                fiatAsset = 1;
            }
            else {
                fiatAsset = 0;
            }

           

            // Inter Wallet
            if (fiatStatusValue == 0) {
                fiatStatus = 1;
            }
            else if (fiatStatusValue == $(this).find('td.status').data('status')) {
                fiatStatus = 1;
            }
            else {
                fiatStatus = 0;
            }


            if (fiatType && fiatPeriod && fiatAsset && fiatStatus) {
                $(this).show();  //displaying row which satisfies all conditions
            }

        });
    }
    
    $('#btnFiatReset').on('click', function () {

        
        $('#fiatType').val('').trigger('change');
        $("#fiatAsset").val('').trigger('chosen:updated');
        $('#fiatStatus').val('').trigger('change');
        $('#fiatPeriod').val('').trigger('change');        

        $('#transactionDataTable4 .my-navigation').show();
        $('#transactionDataTable4').simplePagination({
            items_per_page: 10,
            items_per_page_content: {
                '10': 10,
                '50': 50,
                '100': 100,
                '200': 200,
                '500': 500
            },
        });
    });
}

function FiatTransactionNotFound() {
    $('#transactionDataTable4 table tfoot').hide();
    if (TransactionData.length == 0) {
        setInterval(function () {
            $('#fiatTransactionData').hide();
            $('#transactionDataTable4 table tfoot').show();
        }, 5000)
    } else {
       /* if ($('#fiatTransactionData').hide()) {
            $('#fiatTransactionData').hide();
            $('#transactionDataTable4 table tfoot').show();
        }*/
        //else {
        //    FiatTransaction();
        //}
    }
    var ittr = $('#fiatTransactionData tr').length;
    console.log('ittr', ittr);
    if (ittr <= 0) {
        $('#transactionDataTable4 .my-navigation').hide();
        $('#transactionDataTable4 table tfoot').show();
    }
}
FiatTransactionNotFound()
function stausKycBubbol() {
    var status = $('#stausKyc .kyc22detext').text();
    if (status == ' Not Started ') {
        $('.statusBubbol').show();
        $('.wallDeposit').addClass("indicate");
    } else {
        $('.statusBubbol').hide();
    }
}
stausKycBubbol();






function TradeRefundTransaction() {
    var txt = '';

    TransactionData.forEach(function (t) {
        if (!t.IsFiatRepresentative) {
            if (!t.IsWithInMyWallet) {
                console.log('t.IsWithInMyWallet', t.IsWithInMyWallet)
                var type = (t.Amount <= 0) ? 'Deposit' : 'Withdraw';
                var status = (t.TransactionId == '') ? 'Cancelled' : 'Successful';
                var end = t.Date;
                var now = moment.utc(new Date());
                var days = now.diff(end, 'days');
                console.log('days', days)
                if (t.TransactionId != '00000000-0000-0000-0000-000000000000') {
                    if (t.wName == 'SpotWallet') {
                        txt += '<tr>',
                            txt += '<td class="coin" data-coin="' + t.TokenName + '"><a href="/token/?tname=' + t.TokenName + '""><div class="coin-name-css46"><span><img src="/images/coin/coin/' + t.TokenName + '.png"></span><span>' + t.TokenName + '</span> <small>' + t.TokenName + '</small></div></a></td>',
                            txt += '<td class="days" data-days="' + days + '"><div class="css5datet44"><span> ' + new moment.utc(t.Date).local().format('LL') + '  </span> <span> <small> ' + new moment.utc(t.Date).local().format('LTS') + '</small></span></div></td>';
                        if (t.isFrom) {
                            txt += '<td class="type" data-type="' + type + '" >Withdraw</td>';
                        } else {
                            txt += '<td class="type" data-type="' + type + '" >Deposit</td>';
                        }
                        txt += '<td>' + Math.abs(GetFormatedValAdj(t.Amount, _fsl)) + ' </td>',
                            txt += '<td class="wName walName" data-wName="' + t.wName + '">' + t.wName + '</td>',
                            txt += '<td><span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + t.TransactionId + '" data-bs-original-title="" title="" aria-describedby="popover531066"><a  target="_new" href="' + ethScanPre + t.TransactionId + '">' + t.TransactionId + '</a></span></td>';
                        if (t.TransactionId == '') {
                            txt += '<td class="basered status" data-status="' + status + '" >Cancelled </td>';
                        } else {
                            txt += '<td class="basegreen status" data-status="' + status + '" >Successful </td>';
                        }
                        txt += '</tr>'
                    }
                }
                //else {

                //}
            } else { }
        } else {
            TradeRefundTransactionNotFound()
        }
    });
    $('#TradesRefunds').html(txt);
    ApplySearchWithNoRecord();

    $('#transactionTradesRefunds').simplePagination({
        items_per_page: 10,
        items_per_page_content: {
            '10': 10,
            '50': 50,
            '100': 100,
            '200': 200,
            '500': 500
        },
    });
    
    $('.wName').each(function () {
        var wn = $(this).text();
        var nwn = wn.replace("Wallet", "");
        $(this).text(nwn);
    });
    RefundFilter()
   

}

function TradeRefundTransactionNotFound() {
    $('#transactionTradesRefunds table tfoot').hide();
    if (TransactionData.length == 0) {
        setInterval(function () {
            $('#TradesRefunds').hide();
            $('#transactionTradesRefunds table tfoot').show();
        }, 5000)
    }
    //else {
    //    ExternalTransaction()
    //}
    var ittr = $('#TradesRefunds tr').length;
    console.log('ittr', ittr);
    if (ittr <= 0) {
        $('#ttransactionTradesRefunds .my-navigation').hide();
        $('#transactionTradesRefunds table tfoot').show();
    }
}
TradeRefundTransactionNotFound()


function RefundFilter() {


    $('.refundfilter').change(function () {
        $('#TradesRefundsTable tbody tr').show();
        $('.my-navigation').hide();
        filter_function();

    });

    function filter_function() {
        $('#TradesRefundsTable tbody tr').hide();

        var refundType = 0;
        var refundTypeValue = $('#refundType').val();
        var refundPeriod = 0;
        var refundPeriodValue = $('#refundPeriod').val();
        var refundAsset = 0;
        var refundAssetValue = $('#refundAsset').val();
        var refundWallet = 0;
        var refundWalletValue = $('#refundWallet').val();
        var refundStatus = 0;
        var refundStatusValue = $('#refundStatus').val();



        //traversing each row one by one
        $('#TradesRefundsTable tbody tr').each(function () {
            // Inter Type
            if (refundTypeValue == 0) {   //if no value then display row
                refundType = 1;
            }

            else if (refundTypeValue == $(this).find('td.type').data('type')) {
                refundType = 1;       //if value is same display row
            }
            else {
                refundType = 0;
            }

            // Inter Period
            if (refundPeriodValue == 0) {
                refundPeriod = 1;
            }
            else if (refundPeriodValue == $(this).find('td.days').data('days')) {
                refundPeriod = 1;
            }
            else {
                refundPeriod = 0;
            }

            // Inter Asset
            if (refundAssetValue == 0) {
                refundAsset = 1;
            }
            else if (refundAssetValue == $(this).find('td.coin').data('coin')) {
                refundAsset = 1;
            }
            else {
                refundAsset = 0;
            }

            // Inter Wallet
            if (refundWalletValue == 0) {
                refundWallet = 1;
            }
            else if (refundWalletValue == $(this).find('td.walName').data('wname')) {
                refundWallet = 1;
            }
            else {
                refundWallet = 0;
            }

            // Inter Wallet
            if (refundStatusValue == 0) {
                refundStatus = 1;
            }
            else if (refundStatusValue == $(this).find('td.status').data('status')) {
                refundStatus = 1;
            }
            else {
                refundStatus = 0;
            }


            if (refundType && refundPeriod && refundAsset && refundStatus && refundWallet) {
                $(this).show();  //displaying row which satisfies all conditions
            }

        });
    }
    
    $('#btnRefundReset').on('click', function () {
        $('#refundType').val('').trigger('change');
        $("#refundAsset").val('').trigger('chosen:updated');
        $('#refundWallet').val('').trigger('change');
        $('#refundStatus').val('').trigger('change');
        $('#refundPeriod').val('').trigger('change');
        $('#transactionTradesRefunds .my-navigation').show();
        $('#transactionTradesRefunds').simplePagination({
            items_per_page: 10,
            items_per_page_content: {
                '10': 10,
                '50': 50,
                '100': 100,
                '200': 200,
                '500': 500
            },
        });
    });
}





