function LoadOpenOrders() {
    if (myOpenOrders == undefined || myOpenOrders == null) {
        $('#tbopenorder').html(txt);
        $('#walletOpenOrderTable').paging({ limit: 10 });
        $('.paging-nav a:nth-child(2)').addClass('selected-page');
        return;
    }
    //var data = myOpenOrders;
    var data = myOpenOrders.sort(function (a, b) {
        let x = new Date(b.PlacedOn) - new Date(a.PlacedOn);
        return x;
    });
    var txt = '';
    var le = data.length;
    for (var i = 0; i < le; i++) {
        var d = data[i];
       
        txt += '<tr>';
        //txt += '<td><div class="form-check">';
        //txt += '       <input class="form-check-input" type="checkbox" value="" id=" "></div>';
        //txt += ' </td>';
        txt += '    <td class="pair td-sticky-left" data-pair="' + d.MarketName.toLowerCase() + '">' + d.MarketName + '</td>';

        txt += '   <td ><div class="datettrade44">';
        txt += '        <span> ' + new moment.utc(d.PlacedOn).local().format('LL') + ' </span> <span> <small> ' + new moment.utc(d.PlacedOn).local().format('LT') + '</small></span></div> </td>';
        /*  txt += '  <td class="text-left">' + d.marketCode + '</td>';*/
        txt += '   <td> <span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + d.InternalOrderID + '" data-bs-original-title="" title="">' + d.InternalOrderID.split('-')[0].toUpperCase() + '..' + '</span>  </td>';
        txt += '    <td class="orderType" data-type="' + d.OrderTypeT + '">' + d.OrderTypeT + '</td>';

        if (d.OrderSide == 'Buy')
            txt += '   <td class="orderSide" data-side="' + d.OrderSideT + '"><span class="basegreen">' + d.OrderSideT + '</span> </td>';
        else
            txt += '   <td class="orderSide" data-side="' + d.OrderSideT + '"><span class="basered">' + d.OrderSideT + '</span> </td>';
        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.Price, _fsl) + '</span> <small> ' + d.QuoteTokenCodeName + ' </small></div></td>';
        /* txt += '   <td>' + GetFormatedVal(d.Price,_fsl) + ' <small> ' + d.QuoteTokenCodeName + ' </small></td>';*/
        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.OriginalVolume, _fsl) + '</span> <small> ' + d.BaseTokenCodeName + ' </small></div></td>';
        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.CurrentVolume, _fsl) + '</span> <small> ' + d.BaseTokenCodeName + ' </small></div></td>';
        txt += '<td><div class="datettrade44"><span> ' + GetFormatedVal(d.CurrentVolume * d.Price, _fsl) + '</span> <small> ' + d.QuoteTokenCodeName + ' </small></div></td>';

        txt += '    <td>' + GetFormatedVal(d._OrderAssetAmount, 6) +' </td>';

        //txt += '    <td nowrap="nowrap">';
        //txt += '        <div class="css5asset44">';
        //txt += '           <span> 0.9998 BTC </span> <span> <small> 19996 USDT</small></span>';
        //txt += '        </div>';
        //txt += '   </td>';

        txt += '<td><div class="datettrade44"><span> ' + d._OrderSwapTradeValue + '</span> <small> ' + d.QuoteTokenCodeName + ' </small></div></td>';
        if (d._OrderTrigger > 0) {
            txt += '    <td>' + d._OrderTrigger + ' </td>';
        } else {
            txt += '    <td>N/A </td>';
        }

        txt += '   <td class="csscancel44"><a class="btnFetch" href="javascript:cancelOrder(\'' + d.MarketCode+'\',\'' + d.InternalOrderID + '\')"> Cancel </a></td>    </tr>';
    }
    var obj = $('#tbopenorder');
    obj.html(txt);
    cancelingBtn();
   $('#openOrderTableMain').simplePagination({
        items_per_page: 10
    });
    
    ApplySearch();
    
}
function cancelOrder(m,id) {
    var data = { mcode: m, id: id };
    $.ajax({
        type: "POST",
        url: '/Trade/CancelMyOrder',
        data: data,
        success: function (d) {
            //alert("delete request sent.." + d.toString());
            if (d) {
                $('.btnFetch').prop("disabled", false);
                $('.btnFetch').html('Cancel');
                window.location.reload();
            }
        },
        error: function (error) {
            console.log(error.statusCode + ':' + error.statusText + ':' + error.responseText);
            alert("delete request failed..");
        }
    });
}

function cancelingBtn() {

    $(".btnFetch").on('click', function () {
           
        //disable button
        $(this).prop("disabled", true);
        //ad spinner to button
       /* let toast = new bootstrap.Toast(toastLive);
        toast.show();*/
        $(this).html('<i class="fa fa-circle-o-notch fa-spin"></i> Canceling...');
      
    })
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
                $('.my-navigation').hide();
            } else {
                $(fntb).nextAll('tfoot').show();
                $('.my-navigation').hide();
                $(fntb).parent().parent().children('.trasaction-table').hide();
            }
            $(fn).hide();
            matchingRows.show();
            if (value == '') {                
                $('#openOrderTableMain').simplePagination({
                    items_per_page: 10
                });
                $('.my-navigation').show();
            }
        });
        $('.market-search').on('search', function () {
            $('.market-search').val('');
            $('.market-search').trigger('keyup');
            $('.my-navigation').show();
            $('#openOrderTableMain').simplePagination({
                items_per_page: 10
            });
            $('.my-navigation').show();
        });
    });
}

function orderNotFound() {
    $('#walletOpenOrderTable tfoot').hide();
    if (myOpenOrders.length == 0) {
        setInterval(function () {
            $('#tbopenorder').hide();
            $('#walletOpenOrderTable tfoot').show();
        }, 3000)
    } else {
        LoadOpenOrders();
    }
}
orderNotFound();


var Pair = [];
var Side = [];
var OrderType = [];
function filterOrders() {
    var data = myOpenOrders;
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
    
    LoadOpenOrders();
    sortTableOrder();
}

let toastTrigger = $('#liveToastBtn');
let toastLive = $('#liveToast');
toastTrigger.on('click', function () {
    var canText = $(this).text();
    console.log('canText', canText);
    if (canText == ' Cancel All Orders ') {
        $(this).prop("disabled", true);
        let toast = new bootstrap.Toast(liveToast);
        toast.show();        
        setTimeout(() => {
            $(this).prop("disabled", false);
            $(this).addClass('d-green');
            $(this).html('Confirm');
        }, 4000);
    }

    if (canText == 'Confirm') {       
        $(this).prop("disabled", true);
        $(this).html('<i class="fa fa-circle-o-notch fa-spin"></i> Canceling...');
        setTimeout(() => {
            $(this).prop("disabled", false);
            location.reload();
            //$(this).html('Cancel All Orders');
        }, 3000);        
    }
}); 


$('.filter').change(function () {
    $('table tbody tr').show();
    $('.my-navigation').hide();
    filter_function();

});

function filter_function() {
    $('table tbody tr').hide();

    var fPair = 0;
    var fPairValue = $('#fPair').val();
    var fSide = 0;
    var fSideValue = $('#fSide').val();
    var fOrderType = 0;
    var fOrderTypeValue = $('#fOrderType').val();

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



        if (fPair && fSide && fOrderType) {
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
    $('#openOrderTableMain').simplePagination({
        items_per_page: 10
    });
});