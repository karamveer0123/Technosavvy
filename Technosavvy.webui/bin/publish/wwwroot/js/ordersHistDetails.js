function LoadOrdersHist() {
    //var data = myOrders; 

    var data = myOrders.sort(function (a, b) {        
        let x = new Date(b.PlacedOn) - new Date(a.PlacedOn);
        return x;
    });
    
    var txt = '';
    var le = data.length;
    for (var i = 0; i < le; i++) {
        var d = data[i];
        var end = d.PlacedOn;
        var now = moment.utc(new Date());
        var days = now.diff(end, 'days');
        console.log('days', days)
        txt += '<tr>';
        txt += '    <td class="pair  td-sticky-left" data-pair="' + d.MarketName.toLowerCase() + '">' + d.MarketName + '</td>';

        txt += '   <td class="text-left days"  data-days="' + days +'"><div class="datettrade44">';
        txt += '        <span> ' + new moment.utc(d.PlacedOn).local().format('LL') + ' </span> <span> <small class="me-0"> ' + new moment.utc(d.PlacedOn).local().format('LT')  + '</small></span></div> </td>';
        /*txt += '  <td class="text-left">' + d.marketCode + '</td>';*/
        txt += '   <td> <span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + d.InternalOrderID + '" data-bs-original-title="" title="">' + d.InternalOrderID.split('-')[0].toUpperCase() + '..' + '</span>  </td>';
        txt += '    <td class="orderType" data-type="' + d.OrderTypeT + '" >' + d.OrderTypeT + '</td>';
        if (d.OrderSide == 'Buy')
            txt += '   <td class="orderSide" data-side="' + d.OrderSideT + '"><span class="basegreen">' + d.OrderSideT + '</span> </td>';
        else
            txt += '   <td class="orderSide" data-side="' + d.OrderSideT + '"><span class="basered">' + d.OrderSideT + '</span> </td>';

        txt += '   <td> <div class="datettrade44">' + GetFormatedVal(d.Price, _fsl) + ' <small> ' + d.QuoteTokenCodeName + ' </small></div></td>';
        //Amount
        txt += '    <td> <div class="datettrade44">' + GetFormatedVal(d.OriginalVolume, _fsl) + ' <small>' + d.BaseTokenCodeName + '</small></div></td>';
        //Filled
        txt += '   <td nowrap="nowrap"> <div class="datettrade44">' + GetFormatedVal(d.ProcessedVolume, _fsl) + '<small> ' + d.BaseTokenCodeName + '</small></div></td>';

        //Total
        txt += '    <td> <div class="datettrade44">' + GetFormatedVal(d.OriginalVolume * d.Price, _fsl) + ' <small> ' + d.QuoteTokenCodeName + '</small></div></td>';

        //Asset Amount
        if (d._OrderAssetAmount > 0)
            txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.OriginalVolume, _fsl) + '</span>  <small>  ' + d.BaseTokenCodeName + ' </small></div></td>';
        else
            txt += '<td><div class="datettrade44"><span class="me-0"> -- </span>  <small>  ' + d.BaseTokenCodeName + ' </small></div></td>';
        //SWAP trade Value
        txt += '    <td nowrap="nowrap"> <div class="datettrade44">' + d._OrderSwapTradeValue + ' <small> USDT</small></div></td>';
        if (d.Trigger > 0) {
            txt += '    <td>' + d.Trigger + ' </td>';
        } else {
            txt += '    <td>N/A </td>';
        }


        txt += '<td nowrap="nowrap"> ' + d.StatusT + '</td></tr>';
    }
    var obj = $('#tbOrderHist');
    obj.html(txt);

    /*$('#wallerOrderHistoryTable').paging({ limit: 10 });
    $('.paging-nav a:nth-child(2)').addClass('selected-page');*/
    $('#orderHistoryTable').simplePagination({
        items_per_page: 10
    });
    ApplySearch();
    
}



function ApplySearch() {
    $('#orderHistoryTable table tfoot' ).hide();
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
                $('.my-navigation').hide();
            } else {
                $(fntb).nextAll('tfoot').show();
                $('.my-navigation').hide();
                $(fntb).parent().parent().children('.trasaction-table').hide();
            }
            $(fn).hide();
            matchingRows.show();
            if (value == '') {
                $('#orderHistoryTable').simplePagination({
                    items_per_page: 10
                });
                $('.my-navigation').show();
            }
        });
        $('.market-search').on('search', function () {
            $('.market-search').val('');
            $('.market-search').trigger('keyup');
            $('#orderHistoryTable').simplePagination({
                items_per_page: 10
            });
            $('.my-navigation').show();
        });
    });
}

function orderNotFound() {
    $('#orderHistoryTable  tfoot').hide();
    if (myOrders.length == 0) {
        setInterval(function () {
            $('#tbOrderHist').hide();
            $('#orderHistoryTable tfoot').show();
        }, 000)
    } else {
        LoadOrdersHist()
    }
}
orderNotFound();




var Pair = [];
var Side = [];
var OrderType = [];
function filterOrders() {    
    var data = myOrders;
    if (data.length == 0) {
        $('.my-select2').chosen({ width: "100%" });
    }
    if (data.length <= 0) return;
    data.forEach(function (x) {
        if (jQuery.inArray(x.MarketName, Pair) <= -1)
            Pair.push(x.MarketName);

        if (jQuery.inArray(x.OrderSide, Side) <= -1)
            Side.push(x.OrderSide);

        if (jQuery.inArray(x.OrderType, OrderType) <= -1)
            OrderType.push(x.OrderType);
    });
    let fPair = Pair.filter((value, index, self) => {
        return self.indexOf(value) === index;
    }); console.log('uniqueArray', fPair);

    let fSide = Side.filter((value, index, self) => {
        return self.indexOf(value) === index;
    }); console.log('uniqueArray', fSide);

    let fOrderType = OrderType.filter((value, index, self) => {
        return self.indexOf(value) === index;
    }); console.log('uniqueArray', fOrderType);


    var pairLe = Pair.length;
    var txtPair = '';
    for (var i = 0; i < pairLe; i++) {
        var p = Pair[i];
        if (i == 0) {
            txtPair += '<option value="" selected>All</option>';
        }
        txtPair += '<option value="' + p.toLowerCase() + '">' + p + '</option>';
    }
    var obj = $('#fPair');
    obj.html(txtPair);
    $('#fPair').chosen({ width: "100%" });

    var sideLe = Side.length;
    var txtSide = '';
    for (var i = 0; i < sideLe; i++) {
        var s = Side[i];
        if (i == 0) {
            txtSide += '<option value="" selected>All</option>';
        }
        txtSide += '<option value="' + s + '">' + s + '</option>';
    }
    var obj2 = $('#fSide');
    obj2.html(txtSide);

    var otLe = OrderType.length;
    var txtOrderType = '';
    for (var i = 0; i < otLe; i++) {
        var o = OrderType[i];
        if (i == 0) {
            txtOrderType += '<option value="" selected>All</option>';
        }
        txtOrderType += '<option value="' + o + '">' + o + '</option>';
    }
    var obj3 = $('#fOrderType')
    obj3.html(txtOrderType);
    $('#fOrderType').chosen({ width: "100%" });
   // filterData();
   sortTableOrder();
}

/*
function filterData() {
    $('#fPair').on('change', function (e) {
        var location = e.target.value.toLowerCase();
        console.log('location', location);
        var table = $('#orderHistoryTable table tbody');
        if (location.length) {
            table.find('tr[data-pair!="' + location + '"]').hide();
            table.find('tr[data-pair="' + location + '"]').show();
            $('.my-navigation').hide();
        } else {
            table.find('tr').show();
            $('#orderHistoryTable').simplePagination({
                items_per_page: 10
            });
            if (location.length == 0) {
                $('.my-navigation').show();
            } else {
                $('.my-navigation').show();
            }
        }

    });
    $('#fSide').on('change', function (e) {
        var location = e.target.value;
        var table = $('#orderHistoryTable table tbody');
        if (location.length) {
            table.find('tr[data-side!="' + location + '"]').hide();
            table.find('tr[data-side="' + location + '"]').show();
            $('.my-navigation').hide();
        } else {
            table.find('tr').show();
            $('#orderHistoryTable').simplePagination({
                items_per_page: 10
            });
            if (location.length == 0) {
                $('.my-navigation').show();
            } else {
                $('.my-navigation').show();
            }
        }

    });
    $('#fOrderType').on('change', function (e) {
        var location = e.target.value;
        var table = $('#orderHistoryTable table tbody');
        if (location.length) {
            table.find('tr[data-type!="' + location + '"]').hide();
            table.find('tr[data-type="' + location + '"]').show();
            $('.my-navigation').hide();
        } else {
            table.find('tr').show();
            $('#orderHistoryTable').simplePagination({
                items_per_page: 10
            });
            if (location.length == 0) {
                $('.my-navigation').show();
            } else {
                $('.my-navigation').show();
            }

        }
    });
    $('#fPeriod').on('change', function (e) {
        var location = e.target.value;
        var table = $('#orderHistoryTable table tbody');
        if (location.length) {
            table.find('tr[data-days!="' + location + '"]').hide();
            table.find('tr[data-days="' + location + '"]').show();
            $('.my-navigation').hide();
        } else {
            table.find('tr').show();
            $('#orderHistoryTable').simplePagination({
                items_per_page: 10
            });
            if (location.length == 0) {
                $('.my-navigation').show();
            } else {
                $('.my-navigation').show();
            }
        }
    });
}

*/


$('.filter').change(function () {

    $('table tbody tr').show();
    $('.my-navigation').hide();
    filter_function();

    //calling filter function each select box value change

});

//intially all rows will be shown

function filter_function() {
    $('table tbody tr').hide(); //hide all rows

    var fPair = 0;
    var fPairValue = $('#fPair').val();
    var fSide = 0;
    var fSideValue = $('#fSide').val();
    var fOrderType = 0;
    var fOrderTypeValue = $('#fOrderType').val();
    var fPeriod = 0;
    var fPeriodValue = $('#fPeriod').val();

   
    //setting intial values and flags needed

    //traversing each row one by one
    $('table tr').each(function () {

        if (fPairValue == 0) {   //if no value then display row
            fPair = 1;
        }

        else if (fPairValue == $(this).find('td.pair').data('pair')) {
            fPair = 1;       //if value is same display row
        }
        else {
            fPair = 0;
        }


        if (fSideValue == 0) {
            fSide = 1;
        }
        else if (fSideValue == $(this).find('td.orderSide').data('side')) {
            fSide = 1;
        }
        else {
            fSide = 0;
        }

        if (fOrderTypeValue == 0) {
            fOrderType = 1;
        }
        else if (fOrderTypeValue == $(this).find('td.orderType').data('type')) {
            fOrderType = 1;
        }
        else {
            fOrderType = 0;
        }


        if (fPeriodValue == 0) {
            fPeriod = 1;
            
        }
        else if (fPeriodValue == $(this).find('td.days').data('days')) {
            fPeriod = 1;
        }
        else {
            fPeriod = 0;
        }


        if (fPair && fSide && fOrderType && fPeriod) {
            $(this).show();  //displaying row which satisfies all conditions
            
        } 

    });

  
}

$('#btnReset').on('click', function () {    
    $('#fPair').val('').trigger('chosen:updated');
    $('#fSide').val('').trigger('change');  
    $('#fOrderType').val('').trigger('chosen:updated');  
    $('#fPeriod').val('').trigger('change');  
    $('.my-navigation').show();
    $('#orderHistoryTable').simplePagination({
        items_per_page: 10
    });
});