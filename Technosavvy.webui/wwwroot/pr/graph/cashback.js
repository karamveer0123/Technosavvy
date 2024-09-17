var dom = document.getElementById('cashback');
var myChart = echarts.init(dom, null, {
  renderer: 'canvas',
  useDirtyRect: false
});
var app = {};

var option;

option = {
title: {
// text: 'Log Axis',
left: 'center'
},
tooltip: {
trigger: 'item',
formatter: '{a} <br/>{b} : {c}'
},
// legend: {
//   left: 'left'
// },
xAxis: {
type: 'category',
name: 'x',
splitLine: { show: false },
data: ['13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00']
},
grid: {
left: '3%',
right: '4%',
bottom: '3%',
containLabel: true
},
yAxis: {
type: 'log',
name: 'y',
minorSplitLine: {
  show: false
}
},
series: [
{
  name: 'Log2',
  type: 'line',
  data: [1, 3, 9, 27, 81, 247, 741, 2223, 6669]
},
{
  name: 'Log3',
  type: 'line',
  data: [1, 2, 4, 8, 16, 32, 64, 128, 256]
},
// {
//   name: 'Log1/2',
//   type: 'line',
//   data: [
//     1 / 2,
//     1 / 4,
//     1 / 8,
//     1 / 16,
//     1 / 32,
//     1 / 64,
//     1 / 128,
//     1 / 256,
//     1 / 512
//   ]
// }
]
};

if (option && typeof option === 'object') {
  myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);