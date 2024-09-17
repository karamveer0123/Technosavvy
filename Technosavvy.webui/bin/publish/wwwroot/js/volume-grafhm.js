var BarchartID = document.getElementById('volumemarket-graph');
var barChart = echarts.init(BarchartID, null, {
    renderer: 'svg',
    useDirtyRect: false
});
var app = {};

var option;

option = {
    tooltip: {
        trigger: 'item',
        formatter: '{b} : {c}%'
    },
    xAxis: {
        type: 'category',
        data: []
    },
    yAxis: {
        type: 'value'
    },
    grid: {
        bottom: 40,
        top: 30,
        left: 70,
        right: 30
    },
    series: [
        {
            data: [
               
            ],
            type: 'bar'
        },
        {
            data: [
               
            ],
            type: 'line'
        }

    ]
};


barChart.setOption(option);


window.addEventListener('resize', barChart.resize);


walletxAxis = [];
walletSeries = [];
walletYAxis = [];


$.get(MktAPI + 'GetTokenSummary', (data) => { showWalletGraph(data); });

function showWalletGraph(data) {
   
    var piewallerTime = setInterval(function () {
        pieWallet = walletDataArr;
        valumen(data, pieWallet);
        if (pieWallet.length != 0) {
            clearInterval(piewallerTime);
        }
    }, 500);

}

function valumen(data, Wallet) {
    console.log('data', data);
    var getToken = data;
    var fiatFlag = allCur;
    var walletTotal;
    var WalletAmount = [];
    Wallet.forEach(function (d) {        
        let filterMyToken = getToken.filter((n) => {
            return n.name.toLowerCase() == d.Code.toLowerCase()
        });

        console.log('d.Amt', d.Code, d.Amt, filterMyToken,);
        var BalAmt;
        if (d.IsFiat == true) {
            BalAmt = d.Amt;
            console.log('IsFiat', BalAmt);
        } else {
            if (filterMyToken.length > 0) {

                if (d.BaseVal == 'NaN' || d.BaseVal == 0) {
                    BalAmt = filterMyToken[0].value * d.Amt;
                    console.log('Nan or 0', BalAmt, filterMyToken[0].name);
                } else {
                    BalAmt = d.BalAmt;
                    console.log('allBal', BalAmt);
                }

            } else {
                if (d.Code == 'USDT') {
                    BalAmt = 1;
                }
            }
        }
        WalletAmount.push({ GAmount: BalAmt });        
      
    });

    walletTotal = WalletAmount.reduce((accumulator, currentObject) => {        
        if (currentObject.GAmount) {
            return accumulator + currentObject.GAmount;
        } else { return accumulator; }        
    }, 0);
    console.log('pra walletTotal', walletTotal);   
    var total = walletTotal;    
    
    Wallet.forEach((x) => {
        console.log('Wallet', x);
        let filterMyToken = getToken.filter((n) => {
            return n.name == x.Code
        });

        var flag;
        if (fiatFlag.length != 0) {
            let filterFiat = fiatFlag.filter((f) => {
                return f.abbr.toUpperCase() == x.Code.toUpperCase();
            });
            if (filterFiat.length) {                
                var cunValue = filterFiat[0].value;
                var cunChange = filterFiat[0].change;

            } else {
                var cunValue = 0;
                var cunChange = 0;
            }

        } else {
            var cunValue = 0;
            var cunChange = 0;
        }

        if (x.IsFiat == true) {
            var tokValue = cunValue;
            var change = cunChange;
        } else {
            if (filterMyToken.length > 0) {
                var tokValue = filterMyToken[0].value;
                var change = filterMyToken[0].change;
            } else {
                var tokValue = 1;
                var change = 1;
            }
            if (x.Code == 'USDT') {
                var tokValue = 1;
                var change = 1;
            }
        }
        console.log('x.Code', x.Code, 'value', tokValue, 'change', change, 'Amt', x.Amt);
        var name = x.Code;
        var diferValue = (x.Amt * tokValue) * change;
        var diferPercent = (diferValue / total) * 100;
        console.log('difer', total, diferValue, name, diferPercent);
        walletxAxis.push(name);
        walletYAxis.push(diferPercent.toFixed(4));
        
    });

    for (let i = 0; i < walletYAxis.length; i++) {
        //console.log('walletYAxis[i]', walletYAxis[i])
        if (walletYAxis[i] >= 0) {
            walletSeries.push(walletYAxis[i]);
        }
        if (0 > walletYAxis[i]) {
            walletSeries.push({
                value: walletYAxis[i],
                itemStyle: {
                    color: '#a90000'
                }               
            });
        }
    }
    console.log('walletxAxis', walletxAxis);
    console.log('walletYAxis', walletYAxis);
    console.log('walletSeries', walletSeries);

    function Barchart(walletxAxis, walletYAxis, walletSeries) {
        var updateOptions = {
            xAxis: {
                type: 'category',
                data: walletxAxis
            },
            series: [
                {
                    data: walletSeries,
                    type: 'bar'
                },
                {
                    data: walletYAxis,
                    type: 'line'
                }

            ]
        }
        barChart.setOption(updateOptions);
    }
    Barchart(walletxAxis, walletYAxis, walletSeries)
}

/*valumen(wdata.Assets)*/
