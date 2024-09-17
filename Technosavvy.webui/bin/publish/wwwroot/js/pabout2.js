

var MarketToketData = [];

$.get(MktAPI + 'GetMarketSummary', (data) => { MarketToketDetails(data); });
var abToken = [];
$(function () {
    $.get(window.location.origin + '/token/detailsOf?', { tName: 'TechnoSavvy' }, function (d) {
        //console.log('abToken', abToken);
        abToken = JSON.parse(d);
        GetAboutToken(abToken)
    });
});
function MarketToketDetails(data) {
    console.log('data', data);
    MarketToketData = data;
}


function GetAboutToken(d) {
    console.log('token Details', d);

    var getToken = MarketToketData;
    let fMyToken = getToken.filter((n) => {
        return n.base.toUpperCase() == d.Code.toUpperCase()
    });
    console.log('filterMyToken', fMyToken[0]);

    var gTime = $('#barTime').val();
    aboutNavGraph(gTime)
    $('#barTime').on('change', function () {
        gTime = $(this).val();

        aboutNavGraph(gTime)
        console.log('call 1', gTime)
    });
        
    var wl = d.FavList;
    var WebURL = d.Attr.WebURL;
    var whiteList = d.Attr.WhitePaperURL;
    $('#wtList').text(wl);
    $('#WebURL').attr('href', WebURL);
    $('#whitePaper').attr('href', whiteList);
    var myTokPercent = (fMyToken[0].lowest24H / fMyToken[0].highest24H) * 100;
    console.log('myTokPercent', myTokPercent);

    var lowHigh = '';
    lowHigh += '<div class="css566-marpro justify-content-end col5-sec2 mt-1"><span class="css566-marchfl">Low: ' + fMyToken[0].lowest24H + ' </span><span class="css-mar7yxy"> <div class="css-mar7rit" style="width:' + myTokPercent + '%;"></div> </span><span class="css566-marhi">High: ' + fMyToken[0].highest24H + ' </span></div>'
    $('#lowHigh').html(lowHigh);


    /* Market Cap */
    var txtmktCap = '';
    var cms = d.Attr.CurrentCirculatingSupply;
    var pss = d.Attr.PreviousCirculatingSupply;
    var cmc = d.Attr.CurrentMarketCap;
    var pmc = d.Attr.PreviousMarketCap;
    var cv = fMyToken[0].value;
    var pcp = fMyToken[0].change;

    var mktCapPercent = ((cms * cv) - (pss * (cv / (1 + pcp)))) / (pss * (cv / (1 + pcp)));
    var mktCapPrice = d.Attr.CurrentCirculatingSupply * fMyToken[0].value;

    txtmktCap += '<span class="dyCurLen" data-oval="' + mktCapPrice + '"> ' + GetFormatedCurrencyMBTQ(mktCapPrice) + ' </span><br>';
    if (0 <= mktCapPercent) {
        txtmktCap += '<span class="css44-up" > ' + GetPercentFormat(mktCapPercent, 2) + ' <i class="fa fa-long-arrow-up" aria-hidden="true"></i></span>';
    } else {
        txtmktCap += '<span class="css44-down" > ' + GetPercentFormat(mktCapPercent, 2) + ' <i class="fa fa-long-arrow-down" aria-hidden="true"></i></span>';
    }
    $('#mktCap').html(txtmktCap);

    /* Fully Diluted Market Cap */
    var txtfullyMktCap = '';
    var fdmcPrice = d.Attr.CurrentMarketCap * fMyToken[0].value;
    var fdmcPercent = ((cmc * cv) - (pmc * (cv / (1 + pcp)))) / (pmc * (cv / (1 + pcp)));

    txtfullyMktCap += '<span class="dyCurLen" data-oval="' + fdmcPrice + '"> ' + GetFormatedCurrencyMBTQ(fdmcPrice) + ' </span><br>';
    if (0 <= mktCapPercent) {
        txtfullyMktCap += '<span class="css44-up" > ' + GetPercentFormat(fdmcPercent, 2) + ' <i class="fa fa-long-arrow-up" aria-hidden="true"></i></span>';
    } else {
        txtfullyMktCap += '<span class="css44-down" > ' + GetPercentFormat(fdmcPercent, 2) + ' <i class="fa fa-long-arrow-down" aria-hidden="true"></i></span>';
    }
    $('#fullyMktCap').html(txtfullyMktCap);

    /* Volume 24h */
    var txtValume24 = '';

    txtValume24 += '<span  class="dyCurLen" data-oval="' + fMyToken[0].volume * fMyToken[0].value + '"> ' + GetFormatedCurrencyMBTQ(fMyToken[0].volume * fMyToken[0].value) + ' </span><br>';
   /* if (0 <= mktCapPercent) {
        txtValume24 += '<span class="css44-up" > ' + GetPercentFormat(mktCapPercent, 2) + ' <i class="fa fa-long-arrow-up" aria-hidden="true"></i></span>';
    } else {
        txtValume24 += '<span class="css44-down" > ' + GetPercentFormat(mktCapPercent, 2) + ' <i class="fa fa-long-arrow-down" aria-hidden="true"></i></span>';
    }*/
    $('#valume24').html(txtValume24);
    /* Circulating Supply */

    var FDmktCapPercent = (d.Attr.CurrentCirculatingSupply / d.Attr.CurrentMarketCap) * 100;
    console.log('FDmktCapPercent', FDmktCapPercent)

    var txtCirSupply = '';
    txtCirSupply += '<span  class=" me-2" data-oval="' + d.Attr.CurrentCirculatingSupply + '"> ' + GetFormatCountMBTQ(d.Attr.CurrentCirculatingSupply, 2) + '</span>' + d.Code + '</span>',
        txtCirSupply += '<span class="nineright ms-auto">' + GetPercentFormat(FDmktCapPercent, 2) + '</span>',
        txtCirSupply += '<div class="progress about-TechnoSavvy-progress">',
        txtCirSupply += '<div class="progress-bar" style="width:' + FDmktCapPercent + '%" role="progressbar" aria-valuenow="90" aria-valuemin="0" aria-valuemax="100"></div></div>',

        $('#CirSupply').html(txtCirSupply);

    /* 24h Volume / Market Cap */
    var txtvolMCap = '';    
    var volMCapPercenta = fMyToken[0].volume / d.Attr.CurrentMarketCap;
    txtvolMCap += '<span class="css566-marlah dex-text">' + volMCapPercenta.toFixed(4) + '</span>';
    $('#volMCap').html(txtvolMCap);

    /* total Supply */
    var txttotalSupply = '';
    txttotalSupply += '<span class="dyCurLen me-2" data-oval="112500000000">' + GetFormatedCurrencyMBTQ(112500000000) +'</span>';
    $('#totalSupply').html(txttotalSupply);
}



function loadAboutTechnoSavvy() {
    
    $.get(MktAPI + 'GetCurrentCashBackCycle', (data) => aboutTechnoSavvyPrice(data));
}

function aboutTechnoSavvyPrice(data) {
    var priceTxt = '';
    priceTxt += '<div class="an-d-class">';
    priceTxt += '<h4> <img src="../images/nav-icon.png" width="10" /> TechnoSavvy Price </h4>';
    if (data.TechnoSavvyPrice > data.TechnoSavvyFloorPrice) {
        priceTxt += '<h5 class="dyCur"  data-oval="' + data.TechnoSavvyPrice + '">' + GetFormatedCurrency(data.TechnoSavvyPrice, __uf) + ' </h5>';
    } else {
        priceTxt += '<h5 class="dyCur"  data-oval="' + data.TechnoSavvyFloorPrice + '">' + GetFormatedCurrency(data.TechnoSavvyFloorPrice, __uf) + ' </h5>';
    }
    priceTxt += '</div>';
    priceTxt += '<p> Floor Price <spna class="dyCur"  data-oval="' + data.TechnoSavvyFloorPrice + '">' + GetFormatedCurrency(data.TechnoSavvyFloorPrice, __uf) + '</span> </p>';
    $('#aboutTechnoSavvyPrice').html(priceTxt);
}
loadAboutTechnoSavvy();
setInterval(() => {
    loadAboutTechnoSavvy()
}, 60000);


