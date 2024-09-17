
var MarketToketData = [];

$.get(MktAPI + 'GetMarketSummary', (data) => { MarketToketDetails(data); });

function MarketToketDetails(data) {
    console.log('data', data);
    MarketToketData = data;
}


function GetTokenDetails(d) {

    var getToken = MarketToketData;
    
    console.log('d', d);
    let fMyToken = getToken.filter((n) => {
        return n.base.toUpperCase() == d.Code.toUpperCase() || n.quote.toUpperCase() == d.Code.toUpperCase()
    });

    console.log('filterMyToken', fMyToken[0]);
    var gTime = $('#barTime').val();
    aboutNavGraph(fMyToken[0], gTime)
    $('#barTime').on('change', function () {
        gTime = $(this).val();

        aboutNavGraph(fMyToken[0], gTime)
        console.log('call 1')
    });
    console.log('token Details', d);
    var txttokPrice = "";
    txttokPrice += '<h4>' + d.Code + ' Price </h4><h5 class="dyCur" data-oval="' + fMyToken[0].value + '" >' + fMyToken[0].value + ' </h5 > '
    $('#tokPrice').html(txttokPrice)
    var wl = d.FavList;
    var WebURL = d.Attr.WebURL;
    var urlText = WebURL.replace(/^(?:https?:\/\/)?(?:www\.)?/i, "").split('/')[0];
    var whiteList = d.Attr.WhitePaperURL;    
    $('#wtList').text(wl);
    $('#WebURL').attr('href', WebURL);
    $('#WebURL').text(urlText);
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

    var mktCapPercent = ((cms * cv) - (pss * (cv /( 1 + pcp)))) / (pss * (cv / (1 + pcp)));

    var mktCapPrice = d.Attr.CurrentCirculatingSupply * fMyToken[0].value;
    txtmktCap += '<span class="dyCurLen" data-oval="' + mktCapPrice + '"> ' + GetFormatedCurrencyMBTQ(mktCapPrice) + ' </span><br>';
    if (0 <= mktCapPercent) {
        txtmktCap += '<span class="css44-up" > ' +  GetPercentFormat(mktCapPercent, 2) + ' <i class="fa fa-long-arrow-up" aria-hidden="true"></i></span>';
    } else {
        txtmktCap += '<span class="css44-down" > ' + GetPercentFormat(mktCapPercent, 2) + '<i class="fa fa-long-arrow-down" aria-hidden="true"></i></span>';
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

    txtCirSupply += '<span  class="me-2" data-oval="' + d.Attr.CurrentCirculatingSupply + '"> ' + GetFormatCountMBTQ(d.Attr.CurrentCirculatingSupply, 2) + '</span>' + d.Code + '</span>',
        txtCirSupply += '<span class="nineright ms-auto">' + GetPercentFormat(FDmktCapPercent, 2) +'</span>',
    txtCirSupply += '<div class="progress about-TechnoSavvy-progress">',
    txtCirSupply += '<div class="progress-bar" style="width:' + FDmktCapPercent +'%" role="progressbar" aria-valuenow="90" aria-valuemin="0" aria-valuemax="100"></div></div>',
                    
    $('#CirSupply').html(txtCirSupply);

    var txtTcPara = d.Details;
    $('#tcPara').html(txtTcPara);
}


    
