
loadFormatAll();


var MarketArr = [];
var CashbackMarketArr = [];
var CashbackArr = [];
function loadCashBack() {
    $.get(MktAPI + 'GetCurrentCashBackCycle', (data) => cashbackShowData(data));
}

$.get(MktAPI + 'GetMarketSummary', (data1) => { buildMarkets(data1); });
$.get(MktAPI + 'CashbackOfMarkets', (data) => { cashbackMarket(data); });



function cashbackShowData(data) {
    var cash = '';
    cash += '<div class="col-detnavtop">';
    cash += '<div class="col-detnav">';
    cash += '<span><img src="images/nav-icon.png"> </span>';
    cash += '<div class="cssnavpr44"><span class="pricecss33-text">';
    if (data.TechnoSavvyPrice > data.TechnoSavvyFloorPrice) {
        cash += '<span class="dyCur" data-oval="' + data.TechnoSavvyPrice + '">' + GetFormatedValAdjWithSep(data.TechnoSavvyPrice, __nf) + '</span>';
    } else {
        cash += '<span class="dyCur" data-oval="' + data.TechnoSavvyFloorPrice + '">' + GetFormatedValAdjWithSep(data.TechnoSavvyFloorPrice, __nf) + '</span>';
    }

    cash += '</span><div class="colfloor77">Floor Price: <span class="dyCur" data-oval="' + data.TechnoSavvyFloorPrice + '">' + GetFormatedValAdjWithSep(data.TechnoSavvyFloorPrice, __nf) + '</span> </div>';
    cash += '</div>';
    cash += '</div>';
    cash += '</div>';
    cash += '<div class="col-cashnav">';
    cash += '<span class="colcash77"> Cashback</span>';
    cash += '<span class="colamoun77">24h: <span class="dyCur" data-oval="' + data.cashBack24H + '">' + GetFormatedValAdjWithSep(data.cashBack24H, __nf) + '</span></span>';
    cash += '<span class="colamoun77">YTD: <span class="dyCur" data-oval="' + data.cashBackYTD + '">' + GetFormatedValAdjWithSep(data.cashBackYTD, __nf) + '</span></span>';
    cash += '</div>';
    $("#cashbackNavTop").html(cash);
}
loadCashBack();
setInterval(() => {
    loadCashBack()
}, 60000);




function buildMarkets(data1) {
    console.log('market data', data1);
    if (data1.length <= 0) return;
    data1.forEach(function (x) {
        MarketArr.push({
            name: x.name, base: x.base, quote: x.quote
        });
    });
}

function cashbackMarket(data) {
    // console.log('cashback market data', data, MarketArr[0].name);
    var Arr = MarketArr;
    if (data.length <= 0) return;
    data.forEach(function (x, i) {
        CashbackMarketArr.push({
            market: x.market, current: x.current, ytd: x.ytd, name: Arr[i].name, base: Arr[i].base, quote: Arr[i].quote
        });
    });

    cashBackTableData(CashbackMarketArr)

}


function cashBackTableData(data) {
    $('#cashBackTable tbody tr').remove();
    console.log('data', data);
    var txt = '';
    data.forEach(function (cashBackVal) {
        txt += '<tr><td><a href="/trade?cat=trade&t=' + cashBackVal.base.toLowerCase() + '&q=usdt" target="_blank"><div class="coin-name-css46 ">',
            txt += '<span><img src="../images/coin/coin/' + cashBackVal.base.toUpperCase() + '.png" width="18" class="z-top"></span>',
            txt += '<span class="paircoin-img"><img width="16" src="../images/coin/coin/' + cashBackVal.quote.toUpperCase() + '.png"></span>',
            txt += '<span>' + cashBackVal.name + '</span></div></a></td>',
            txt += '<td><span class="dyCurLen" data-oval="' + cashBackVal.current + '">' + GetFormatedCurrencyMBTQ(cashBackVal.current) + '</span></td>',
            txt += '<td><span class="dyCurLen" data-oval="' + cashBackVal.ytd + '">' + GetFormatedCurrencyMBTQ(cashBackVal.ytd) + '</span></td>',
            txt += '</tr>';
    });
    $('#cashBackTable tbody').html(txt);
}


var dom = document.getElementById('cashback');
var myChart = echarts.init(dom, null, {
    renderer: 'svg',
    useDirtyRect: false
});
var app = {};

var option;

option = {
    title: {
        left: 'center'
    },
    tooltip: {
        trigger: 'item',
        formatter: '{a} <br/>{b} : {c}'
    },
    xAxis: {
        type: 'category',
        nameLocation: 'center',
        splitLine: { show: false },
        data: [


        ]
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '6%',
        containLabel: true
    },
    yAxis: {
        type: 'log',
        minorSplitLine: {
            show: false
        }
    },
    series: [
        {
            name: 'Cashback',
            type: 'line',
            data: [


            ]
        },

    ]
};

if (option && typeof option === 'object') {
    myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);


function cahsbackChart() {

    var LineChart = [];
    var LineChartTime = [];
    $.ajax({
        url: MktAPI + 'Cashback',
        method: "GET",
        headers: { "Accept": "application/json; odata=nometadata" },
        success: function (data) {

            var lineChartData = data.slice(0, 50);
            if (lineChartData.length <= 0) return;
            lineChartData.forEach((x) => {
                var value = x.cashBackYTD
                var time = moment(x.cBeventEnd).format('DD-MMM')
                LineChart.push(value);
                LineChartTime.push(time);
            });
            console.log('LineChart', LineChart);
            console.log('LineChartTime', LineChartTime);
            function updatePieChart(LineChart, LineChartTime) {
                var updatedOption = {
                    xAxis: {
                        type: 'category',
                        nameLocation: 'center',
                        splitLine: { show: false },
                        data: LineChartTime
                    },
                    series: [
                        {
                            name: 'Cashback',
                            type: 'line',
                            data: LineChart
                        }

                    ]
                };
                myChart.setOption(updatedOption);
            }
            updatePieChart(LineChart, LineChartTime);
        },
        error: function (data) {
            console.log('data error', data);
        }
    });
}
cahsbackChart();

