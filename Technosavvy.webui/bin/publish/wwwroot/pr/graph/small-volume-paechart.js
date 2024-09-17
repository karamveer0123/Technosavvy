var dom = document.getElementById('smallvolume-piechart');
var myChart = echarts.init(dom, null,{
  renderer: 'canvas',
  useDirtyRect: false
});

var colorPalette = ['#00b04f', '#ffbf00', '#ff0000'];

var app = {};

var option;

option = {
title: {
text: '  ',
subtext: '',
left: 'center'
},
tooltip: {
trigger: 'item',

},
legend: {
orient: 'vertical',
left: 'left',
top: '5',
textStyle:{
  color: '#777',
},
data: [
  'BTC',
  'USDT',
  'TechnoSavvy',
  'USDC',
  'ETH',
  'BUSD',
  'MATIC',
  'LTC'
],
itemStyle: {textStyle:{
  color: '#777',
},}
},
toolbox: {
show: true,
feature: {
  mark: { show: false },
  dataView: { show: false, readOnly: false },
  restore: { show: false },
  saveAsImage: { show: false },
}
},

series: [
  
{
  name: 'Area Mode',
  type: 'pie',
  radius: [10, 90],
  center: ['60%', '50%'],

  // roseType: 'area',
  // color: colorPalette,
  itemStyle: {
    borderRadius: 5,
    textStyle: {
      color:'#777',        
    }
  },
  data: [
    { value: 30, name: 'BTC' },
    { value: 28, name: 'USDT' },
    { value: 26, name: 'TechnoSavvy' },
    { value: 24, name: 'USDC' },
    { value: 22, name: 'ETH' },
    { value: 20, name: 'BUSD' },
    { value: 18, name: 'MATIC' },
    { value: 16, name: 'LTC' },
  ],
    itemStyle: {
      textStyle: {
        color:'#777',        
      }
    }
  }]
};

if (option && typeof option === 'object') {
  myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);


// ===============================








