
var dom = document.getElementById('lineGraph');
var myChart = echarts.init(dom, null, {
  renderer: 'canvas',
  useDirtyRect: false
});
var app = {};

var option;

function randomData() {
now = new Date(+now + oneDay);
value = value + Math.random() * 21 - 10;
return {
name: now.toString(),
value: [
  [now.getFullYear(), now.getMonth() + 1, now.getDate()].join('/'),
  Math.round(value)
]
};
}
let data = [];
let now = new Date(1997, 9, 3);
let oneDay = 24 * 3600 * 1000;
let value = Math.random() * 1000;
for (var i = 0; i < 1000; i++) {
data.push(randomData());
}
option = {
title: {
text: 'Dynamic Data & Time Axis'
},

tooltip: {
trigger: 'axis',
formatter: function (params) {
  params = params[0];
  var date = new Date(params.name);
  return (
    date.getDate() +
    '/' +
    (date.getMonth() + 1) +
    '/' +
    date.getFullYear() +
    ' : ' +
    params.value[1]
  );
},
axisPointer: {
  animation: false
}
},
xAxis: {
type: 'time',
splitLine: {
  show: false
}
},
yAxis: {
type: 'value',
boundaryGap: [0, '100%'],
splitLine: {
  show: false
}
},

series: [
{
  name: 'Fake Data',
  type: 'line',
  symbol: 'none',
  sampling: 'lttb',
  itemStyle: {
    color: 'rgb(204 154 6);'
  },
  areaStyle: {
    color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
      {
        offset: 0,
        color: 'rgb(200 242 5)'
      },
      {
        offset: 1,
        color: 'rgb(8 0 3 / 36%)'
      }
    ])
  },
  data: data
}
]
};
setInterval(function () {
for (var i = 0; i < 5; i++) {
data.shift();
data.push(randomData());
}
myChart.setOption({
series: [
  {
    data: data
  }
]
});
}, 1000);

if (option && typeof option === 'object') {
  myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);
