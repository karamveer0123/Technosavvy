if (myBuy == '') {
    $('#PurchaseData').addClass('d-none');
    $('#noPurchase').removeClass('d-none');
    $('#noPurchase').removeClass('d-block');
   
} else {

}
function PurchaseLoad(d) {
    
   
    // Purchase page
    purchaseDataList(d);


}

PurchaseLoad(myBuy)


function purchaseDataList(p) {
    console.log('purchase Data List', p);

    $('#myPurchaseDataTable tbody tr').remove();
    var linkHash = ethScanPre;
    console.log(linkHash);
    var txt = '';
    p.forEach(function (x) {
        var date = new Date(x.DateOf);      
        txt += '<tr>';
        txt += '<td class="td-sticky-left">';
        txt += '<div class="css5leftdf">';
        txt += '<span>' + new moment.utc(date).local().format('LL') + ' </span> <span> <small>' + new moment.utc(date).local().format('LTS') + '</small></span>';
        txt += '</div>';
        txt += '</td>';
        txt += '<td><a href="' + linkHash + '' + x.TxnID + '" target="_blank"> <span class="d-inline-block opencss11" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + x.TxnID + '" data-bs-original-title="' + x.TxnID + '" title="' + x.TxnID + '"> ' + x.TxnID +'</span></a> </td>';
        txt += '<td><div class="dflex22"><span> ' + x.BuyWith.toFixed(4) + '</span> <small> ' + x.BuyWithName + ' </small><img src="../images/coin/coin/' + x.BuyWithName + '.png" width="14" class="ms-1"/></div></td>';
        txt += '<td><div class="dflex22"><span> ' + x.Amount.toFixed(4) + '</span> <small> TechnoSavvy </small><img src="../images/coin/coin/TechnoSavvy.png" width="14" class="ms-1"/></div> </td>';
        txt += '</tr>';
    });

    $('#myPurchaseDataTable tbody').html(txt);
}