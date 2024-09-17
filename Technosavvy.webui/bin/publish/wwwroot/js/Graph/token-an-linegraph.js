var dom = document.getElementById('lineGraph');
var myChart = echarts.init(dom, null, {
	renderer: 'svg',
	useDirtyRect: false
});
var app = {};

var option;

option = {
	title: {
		text: ''
	},
	tooltip: {
		trigger: 'axis',
		axisPointer: {
			type: 'cross',
			label: {
				backgroundColor: '#ff9416'
			}
		}
	},

	grid: {
		left: '3%',
		right: '4%',
		bottom: '3%',
		containLabel: true
	},
	xAxis: [{
		type: 'category',
		boundaryGap: false,
		data: [
			
		]
	}],
	yAxis: [{
		type: 'value'
	}],
	series: [{
		name: '',
		type: 'line',
		stack: 'Total',
		areaStyle: {
			color: '#ff9416',
			opacity: 0.15
		},
		lineStyle: {
			normal: {
				color: '#ff9416',
				width: 2,

			}
		},
		emphasis: {
			focus: 'series'
		},
		data: [
			
		]
	},


	],

};

if (option && typeof option === 'object') {
	myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);

function aboutNavGraph(d, n) {

	console.log('graph d', d);
	var gTime = n;
	var LineGraph = [];
	var LineGraphTime = [];
	
		$.ajax({
			url: MktAPI + 'GetBars?mCode='+d.mCode.toLowerCase()+'&pulse='+gTime+'&count=10',
			method: "GET",
			headers: {
				"Accept": "application/json; odata=nometadata"
			},
			success: function (data) {
				console.log('success', data);
				if (data == undefined) {
					$('#lineGraph').hide();
				} else {
					let gfilter = data.filter((n) => {
						return n.type.toUpperCase() == gTime.toUpperCase()
					});
					var lineGraphData = gfilter;
					if (lineGraphData.length <= 0) return;
					lineGraphData.forEach((x) => {
						var value = x.tradesValue;
						var time = moment(x.time).format('DD-MMM');
						LineGraph.push(value);
						LineGraphTime.push(time);
					});

					function updatePieChart(LineGraph, LineGraphTime) {
						var updatedOption = {
							xAxis: [{
								type: 'category',
								boundaryGap: false,
								data: LineGraphTime
							}],
							series: [{
								name: '',
								type: 'line',
								stack: 'Total',
								areaStyle: {},
								emphasis: {
									focus: 'series'
								},
								data: LineGraph
							}

							]
						};


						myChart.setOption(updatedOption);
					}
					updatePieChart(LineGraph, LineGraphTime);
				}
			
			},
			error: function (data) {
				console.log('data error', data);
			}
		});
	}
