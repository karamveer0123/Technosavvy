var dom = document.getElementById('candlestickgrphindex1');
 

 var myChart = echarts.init(dom, null, {
    renderer: 'canvas',
    useDirtyRect: false
});
var app = {};
var ROOT_PATH = 'http://TechnoAppwebui.tst.TechnoApp.int.lab';

var option;
const upColor = '#076641';
const downColor = '#f6465d';

function splitData(rawData) {
    let categoryData = [];
    let values = [];
    let volumes = [];
    for (let i = 0; i < rawData.length; i++) {
        var a = rawData[i].splice(0, 1)[0];
        categoryData.push(a);
        console.log(a);
        values.push(rawData[i]);
        console.log(rawData[i]);
        volumes.push([i, rawData[i][4], rawData[i][0] > rawData[i][1] ? 1 : -1]);
    }
    return {
        categoryData: categoryData,
        values: values,
        volumes: volumes
    };
}
var spikeMin=0;
var spikeMax=0;
function updateSpike(data) {
    if (data.length > 0) {
        var d = Math.max.apply(null, data);
        if (d > spikeMax)
            spikeMax = d;
        if (d < spikeMin)
            spikeMax = d;
    }
}
function splitData2(rawData) {
    let categoryData = [];
    let values = [];
    let volumes = [];
    for (let i = 0; i < rawData.length; i++) {
        var data = rawData[i];
        var ud = new Date(0);
        ud.setUTCSeconds(data.time);
        var m = moment(ud);
        categoryData.push(m.format('hh:mm'));
        var val = []; val.push(data.open); val.push(data.close); val.push(data.low); val.push(data.high); val.push(data.volumn);
        updateSpike(val);
        values.push(val);
        volumes.push([i, data.volumn, data.open > data.close ? 0 : 1]);
    }
    return {
        categoryData: categoryData,
        values: values,
        volumes: volumes
    };
}
function calculateMA(dayCount, data) {
    var result = [];
    for (var i = 0, len = data.values.length; i < len; i++) {
        if (i < dayCount) {
            result.push('-');
            continue;
        }
        var sum = 0;
        for (var j = 0; j < dayCount; j++) {
            sum += data.values[i - j][1];
        }
        result.push(+(sum / dayCount).toFixed(3));
    }
    return result;
}
mainData();
function sampleData() {
    $.get('../js/Graph/src/candel.json', function (d) {
        var data = splitData(d);
        process(data);
    });
}
function mainData() {
    $.get(MktAPI + 'getbars?mcode=wbtcusdt&pulse=1min&count=100', function (d) {
        var data = splitData2(d);
        process(data);
    });
}
  
function process(data) {
    myChart.setOption(
        (option = {
            animation: false,
            legend: {
                show:false,
                top: 10,
                left: 'center',
                data: ['ABC', 'MA5', 'MA10', 'MA20', 'MA30']
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                  type: 'cross'
                },
                borderWidth: 1,
                borderColor: '#ccc',
                padding: 10,
                textStyle: {
                  color: '#000'
                },
                position: function (pos, params, el, elRect, size) {
                  const obj = {
                    top: -100
                  };
                  obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 30;
                  return obj;
                }
                // extraCssText: 'width: 170px'
              },
            axisPointer: {
                show:true,
                link: [
                    {
                        xAxisIndex: 'all'
                    }
                ],
                label: {
                    backgroundColor: '#777'
                }
            },
            toolbox: {
                feature: {
                  dataZoom: {
                    show:false,
                    yAxisIndex: false
                  },
                  brush: {
                     show:false,
                    type: ['lineX', 'clear']
                  }
                }
              },
            brush: {
                xAxisIndex: 'all',
                brushLink: 'all',
                outOfBrush: {
                    colorAlpha: 0.1
                }
            },
            visualMap: {
                show: false,
                seriesIndex: 5,
                dimension: 2,
                pieces: [
                    {
                        value: 0,
                        color: downColor
                    },
                    {
                        value: 1,
                        color: upColor
                    }
                ]
            },
            grid: [
                {
                    left: '10%',
                    right: '8%',
                    top: '2%',
                    height: '85%'
                },
                {
                    left: '10%',
                    right: '8%',
                    top: '77%',
                    height: '10%'
                }
            ],
            xAxis: [
                {
                type: 'category',
                  data: data.categoryData,
                  boundaryGap:false,
                   axisTick: { show: false },
                  axisLine: { onZero: false },
                  splitLine: { show: false },
                 axisLabel: { show: true,
                    padding:'30, 20 , 0 , 0'},
                  min: 'dataMin',
                  max: 'dataMax',
                  axisPointer: {
                    z: 100
                  
                  }
                },
                {
                  type: 'category',
                  gridIndex: 1,
                  data: data.categoryData,
                  boundaryGap: false,
                  axisLine: { onZero: false },
                  axisTick: { show: false },
                     splitLine: { show: true,
                    lineStyle: {color: '#0c03094a'},
                  },
                  axisLabel: { show: false },
                  min: 'dataMin',
                  max: 'dataMax'
                }
              ],
            yAxis: [
                {
                  scale:true,
                  position: 'right',
                  splitArea: {
                    show: false,
                  },
                     splitLine: { show: true,
                    lineStyle: {color: '#0c03094a'}, 
                  }
                },
                {
                  scale: true,
                  position: 'right',
                  gridIndex: 1,
                  splitNumber: 2,
                  axisLabel: { show: true },
                  axisLine: { show: false },
                  axisTick: { show: false },
                 splitLine: { show: true,
                    lineStyle: {color: '#0c03094a'},
                  },
                }
              ],
            dataZoom: [
                {
                    type: 'inside',
                    xAxisIndex: [0, 1],
                    start: 75,//ToDo: Naveen this is to set the Focused Part of the Chart 1-100% of BARS
                    end: 100
                },

            ],
            series: [
                {
                    name: 'ABC',
                    type: 'candlestick',
                    data: data.values,
                    itemStyle: {
                        color: upColor,
                        color0: downColor,
                        borderColor: undefined,
                        borderColor0: undefined
                    },
                    tooltip: {
                        formatter: function (param) {
                            param = param[0];
                            return [
                                'Date: ' + param.name + '<hr size=1 style="margin: 3px 0">',
                                'Open: ' + param.data[0] + '<br/>',
                                'Close: ' + param.data[1] + '<br/>',
                                'Lowest: ' + param.data[2] + '<br/>',
                                'Highest: ' + param.data[3] + '<br/>'
                            ].join('');
                        },
                        className:'d-none'
                    }
                },
                {
                    name: 'MA5',
                    type: 'line',
                    data: calculateMA(5, data),
                    smooth: true,
                    showSymbol: false,
                    lineStyle: {
                        opacity: 0.5
                    }
                },
                {
                    name: 'MA10',
                    type: 'line',
                    data: calculateMA(10, data),
                    smooth: true,
                    showSymbol: false,
                    lineStyle: {
                        opacity: 0.5
                    }
                },
                {
                    name: 'MA20',
                    type: 'line',
                    data: calculateMA(20, data),
                    smooth: true,
                    showSymbol: false,
                    lineStyle: {
                        opacity: 0.5
                    }
                },
                {
                    name: 'MA30',
                    type: 'line',
                    data: calculateMA(30, data),
                    smooth: true,
                    showSymbol: false, //ToDo: Naveen hiode circles on the line
                    lineStyle: {
                        opacity: 0.5
                    }
                },
                {
                    name: 'Volume',
                    type: 'bar',
                    xAxisIndex: 1,
                    yAxisIndex: 1,
                    data: data.volumes
                }
            ]
        }),
        true
    );
    myChart.dispatchAction({
        type: 'line',
        areas: [
            {
                brushType: 'lineX',
               // coordRange: ['2015/1/5', '2016-06-20'],
                coordRange: [0, spikeMax],
                xAxisIndex: 0
            }
        ]
    });
}

if (option && typeof option === 'object') {
    myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);

 