var dom = document.getElementById('volumemarket-graph');
var myChart = echarts.init(dom, null, {
  renderer: 'canvas',
  useDirtyRect: false
});
var app = {};

var option;

option = {
xAxis: {
type: 'category',
data: ['BTC', 'USDT', 'TechnoSavvy', 'USDC', 'MATIC', 'ETH', 'CHZ']
},
yAxis: {
type: 'value'
},
grid: {
  bottom:30,
  top:30,
  left:40,
  right:30
  },
series: [
{
  data: [
    120,
    {
      value: -200,
      itemStyle: {
        color: '#a90000'
      }
    },
    150,
    {
      value: -70,
      itemStyle: {
        color: '#a90000'
      }
    },
    70,
    110,
    400,
    130
  ],
  type: 'bar'
},
 {
  data: [
    120,
    -200,
    150,   
    -70,
    70,
    110,
    400,
    130
  ],
  type: 'line'
}

]
};

if (option && typeof option === 'object') {
  myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);