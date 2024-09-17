var dom = document.getElementById('smallvolume-piechart');
var pieChart = echarts.init(dom, 'null', {
	renderer: 'svg',
	useDirtyRect: false,

});

var colorPalette = ['#00b04f', '#ffbf00', '#ff0000'];

var option;

option = {
		title: {
			text: '  ',
			subtext: '',
			left: 'center'
		},
	tooltip: {
		trigger: 'item',
		formatter: '{b} : {c}%'
	},
	
	legend: {
		orient: 'vertical',
		left: 'left'
	},
		toolbox: {
			show: true,
			feature: {
				mark: {
					show: false
				},
				dataView: {
					show: false,
					readOnly: false
				},
				restore: {
					show: false
				},
				saveAsImage: {
					show: false
				},
			}
		},

		series: [
			{			
				type: 'pie',
				radius: [10, 90],
				center: ['50%', '50%'],				
				itemStyle: {					
					textStyle: {
						color: '#777',
					}
				},				
				data: [
				],
				emphasis: {
					itemStyle: {
						shadowBlur: 10,
						shadowOffsetX: 0,
						shadowColor: 'rgba(0, 0, 0, 0.5)'
					}
				}
			}
	],
	
	};



if (option && typeof option === 'object') {
	pieChart.setOption(option);
}

window.addEventListener('resize', pieChart.resize);
// ===============================

var PieChartMix = [];



$.get(MktAPI + 'GetMarketSummary', (data) => { graphPieShow(data); });

function graphPieShow(data) {	
	var piewallerTime = setInterval(function () {
		pieWallet = walletDataArr;
		graphPieChart(data, pieWallet);
		if (pieWallet.length != 0) {
			clearInterval(piewallerTime);
        }
	}, 2000);
}

function graphPieChart(data, pieWallet) {
	var getToken = data;	
	var fiatFlag = allCur;
	var walletTotal;
	var WalletAmountPie = [];
	
	
	
	pieWallet.forEach(function (x) {
		let filterMyToken = getToken.filter((n) => {
			return n.base == x.Code
		});



		var BalAmt;
		if (x.IsFiat == true) {
			BalAmt = x.Amt;
			console.log('IsFiat', BalAmt);
		} else {
			if (filterMyToken.length > 0) {

				if (x.BaseVal == 'NaN' || x.BaseVal == 0) {
					BalAmt = filterMyToken[0].value * x.Amt;
					console.log('Nan or 0', BalAmt, filterMyToken[0].name);
				} else {
					BalAmt = x.BalAmt;
					console.log('allBal', BalAmt);
				}

			} else {
				if (x.Code == 'USDT') {
					BalAmt = 1;
				}
			}
		}
		WalletAmountPie.push({ GAmount: BalAmt });
		console.log('Pie', filterMyToken, WalletAmountPie);

	});


	const total = WalletAmountPie.reduce((accumulator, currentObject) => {
		if (currentObject.GAmount) {
			return accumulator + currentObject.GAmount;
		} else { return accumulator; }
	}, 0);
	console.log('total pie', total);
	var valDecmal;

	pieWallet.forEach(function (x) { 


		let filterMyToken = getToken.filter((n) => {
			return n.base == x.Code
		});



		if (fiatFlag.length != 0) {
			let filterFiat = fiatFlag.filter((f) => {
				return f.abbr.toUpperCase() == x.Code.toUpperCase();
			});
			if (filterFiat.length) {
				var cunValue = filterFiat[0].value;
			} else {
				var cunValue = 0;
			}
		} else {
			var cunValue = 0;
		}




		if (filterMyToken.length > 0) {
			var tokName = filterMyToken[0].base;
		} 
		if (x.Code == 'USDT') {
			var tokName = 'USDT';
		} else {
			var tokName = x.Code;
		}

		
		if (filterMyToken.length > 0) {
			if (x.BaseVal == 'NaN' || x.BaseVal == 0) {
				valDecmal = ((x.Amt * filterMyToken[0].value) / total) * 100;
			} else {
				valDecmal = (x.BaseVal / total) * 100;
			}
		} else {
			valDecmal = (x.BaseVal / total) * 100;
		}


		console.log('Pie valDecmal', valDecmal);
		PieChartMix.push({
			value: valDecmal.toFixed(6),
			name: tokName
		});
	});

	console.log('PieChartMix', PieChartMix)
	function updatePieChart(PieChartMix) {
		var updatedOption = {
			series: [
				{

					type: 'pie',
					radius: [10, 90],
					center: ['60%', '50%'],
					itemStyle: {

						textStyle: {
							color: '#777',
						},
						emphasis: {
							itemStyle: {
								shadowBlur: 10,
								shadowOffsetX: 0,
								shadowColor: 'rgba(0, 0, 0, 0.5)'
							}
						}
					},
					data: PieChartMix,

				}
			]
		};
		pieChart.setOption(updatedOption);
	}
	updatePieChart(PieChartMix);
}
