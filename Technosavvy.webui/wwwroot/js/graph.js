var dom = document.getElementById('candlestickgrphindex1');


var myChart = echarts.init(dom, null, {
    renderer: 'canvas',
    useDirtyRect: false
});
var app = {};
var ROOT_PATH = '/js/Graph/src/candel.json';

var option;
var getList2 = localStorage.getItem('switchColor')
let retArray2 = JSON.parse(getList2)
if (retArray2) {
    var upColor = retArray2[0].bsred;
    var downColor = retArray2[0].secondColor;
} else {
    var upColor = '#076641';
    var downColor = '#f6465d';
}


function displayUpdate() {
    var op = process();
    MAprocess(op);
    op.dataZoom = myChart.getOption().dataZoom;
    myChart.setOption(op, true);
}

function MAprocess(op) {
    MA.forEach(function (v) {
        op.series.push(getMA(v.days));
    });
}
function createOrupdateBAR(t, tr) {
    var current = new moment().unix();
    if (tr.tradePrice == null || tr.tradePrice == undefined)
        tr = tr.trade;
    if (liveBARS.length > 0) {
        var b = liveBARS[liveBARS.length - 1]
        if (b.time >= t) {
            b.x.close = tr.tradePrice;
            if (b.x.low > tr.tradePrice) b.x.low = tr.tradePrice;
            if (b.x.high < tr.tradePrice) b.x.high = tr.tradePrice;
            if (b.z.volumn != null | b.z.volumn != undefined)
                b.z.volumn += tr.tradeVolumn;
            else
                b.z.volumn = tr.tradeVolumn;

            liveBARS[liveBARS - 1] = b;
            return b;
        }
    }
    var i = (dataBARS.length + liveBARS.length - 1)
    var b = buildData(i, null);
    var t1 = $('#barTime option:selected').attr('data-du');//60;//60sec default
    var ti = 0;
    if (dataBARS.length > 1) {
        t1 = dataBARS[1].time - dataBARS[0].time;
        if (liveBARS.length > 0) {
            var t0 = liveBARS[liveBARS.length - 1].time;
            var t2 = t0 + t1;
            while (t2 < current) {
                ti++;
                t2 = t0 + (t1 * ti);
            }
            b.time = t2;
        }
        else {
            var t0 = dataBARS[dataBARS.length - 1].time;
            var t2 = t0 + t1;
            while (t2 < current) {
                ti++;
                t2 = t0 + (t1 * ti);
            }
            b.time = t2;
        }
        var ud = new Date(0);
        ud.setUTCSeconds(b.time);

        b.date = moment.utc(ud).local();
        b.x.open = tr.tradePrice;
        b.x.close = tr.tradePrice;
        b.x.high = tr.tradePrice;
        b.x.low = tr.tradePrice;
        b.z.volumn = tr.tradeVolumn;
    }
    return b;
}
var dataBARS = [];
var liveBARS = [];
//from BAR Array
function buildBars(rawData) {
    dataBARS.splice(0, dataBARS.length);
    for (let i = 0; i < rawData.length; i++) {
        dataBARS.push(buildData(i, rawData[i]));
    }
}
//from single BAR
function buildData(i, d) {
    if (d != null) {
        var ud = new Date(0);
        ud.setUTCSeconds(d.time);
        var m = moment.utc(ud).local();
        return {
            i: i, time: d.time, date: m, x: { open: d.open, close: d.close, low: d.low, high: d.high }, y: m, z: { i: i, volumn: d.volumn, status: d.status }
        };
    }
    else {
        return {
            i: i, time: 0, date: new moment(), x: { open: 0, close: 0, low: 0, high: 0 }, y: Date(0), z: { i: i, volumn: 0, status: 0 }
        };
    }
}
function get_yAxis() {
    var ret = [];
    var last = -1;
    for (let i = 0; i < dataBARS.length; i++) {
        var dd = dataBARS[i];
        var d = dd.date.format('hh:mm');
        if ((d == '12:00' || d == '00:00') && (i - last) > 10) {
            last = i; ret.push(dd.date.format('DD-MMM hh:mm'));
        }
        else ret.push(dd.date.format('hh:mm'));
    }
    last = -1;

    for (let i = 0; i < liveBARS.length; i++) {
        var dd = liveBARS[i];
        var d = dd.date.format('hh:mm');
        if ((d == '12:00' || d == '00:00') && (i - last) > 10) {
            last = i; ret.push(dd.date.format('DD-MMM hh:mm'));
        }
        else ret.push(dd.date.format('hh:mm'));
    }
    return ret;
    //
}
function get_xAxis() {
    var ret = [];
    for (let i = 0; i < dataBARS.length; i++) {
        var dd = dataBARS[i];
        var x = dd.x;
        var val = [];
        val.push(x.open); val.push(x.close); val.push(x.low); val.push(x.high);
        ret.push(val);
    }
    for (let i = 0; i < liveBARS.length; i++) {
        var dd = liveBARS[i];
        var x = dd.x;
        var val = [];
        val.push(x.open); val.push(x.close); val.push(x.low); val.push(x.high);
        ret.push(val);
    }
    return ret;
    //
}
function get_zAxis() {
    var ret = [];
    var ii = 0;
    for (let i = 0; i < dataBARS.length; i++) {
        var dd = dataBARS[i];
        var z = dd.z;
        ret.push([z.i, z.volumn, dd.x.open > dd.x.close ? 0 : 1]);
    }
    ii = ret.length;
    for (let i = 0; i < liveBARS.length; i++) {
        var dd = liveBARS[i];
        var z = dd.z;
        ret.push([z.i, z.volumn, dd.x.open > dd.x.close ? 0 : 1]);
    }
    return ret;
}
function doLoadMoreLeftBars() {
    var link = 'getbarsbefore?mcode=' + __mCode + '&pulse=' + $('#barTime').val() + '&before=' + dataBARS[0].time;
    $.get(MktAPI + link, function (rawData) {
        var nb = []
        for (let i = 0; i < rawData.length; i++) {
            nb.push(buildData(i, rawData[i]));
        }
        if (nb.length > 0) {
            var bars = $.merge(nb, dataBARS);
            buildBars(bars);
            displayUpdate();
            console.log('-->Load more on Left');
        }
        else 
            console.log('-->NO more Data to Load on Left');
    });
}

function getTime(td) {
    if (td == null || td == undefined)
        return new moment().unix();
    else {
        if (td.timeStamp == null || td.timeStamp == undefined)
            return td.trade.timeStamp;
            else
            return td.timeStamp;
    }
}
//Last calculated Data for Graph
//let categoryData = [];
//let values = [];
//let volumes = [];
let plastStamp = 0;
let lastStamp = 0;
let NextStamp = 0;
//function splitData2(rawData) {

//    for (let i = 0; i < rawData.length; i++) {
//        var data = rawData[i];
//        var ud = new Date(0);
//        ud.setUTCSeconds(data.time);
//        var m = moment(ud);
//        var d = m.format('hh:mm');
//        if (d == '12:00' || d == '00:00')
//            categoryData.push(m.format('DD-MMM hh:mm'));
//        else categoryData.push(m.format('hh:mm'));
//        var val = []; val.push(data.open); val.push(data.close); val.push(data.low); val.push(data.high); val.push(data.volumn);
//        if (lastStamp > 0) plastStamp = lastStamp;
//        lastStamp = data.time;
//        console.log(i.toString() + '|' + data.time.toString() + ' - ' + m.toString('dd-MMM hh:mm:ss') + '|O:' + data.open.toString() + '|C:' + data.close.toString() + '|L:' + data.low.toString() + '|H:' + data.high.toString());
//        values.push(val);
//        volumes.push([i, data.volumn, data.open > data.close ? 0 : 1]);
//    }
//    return {
//        categoryData: categoryData,
//        values: values,
//        volumes: volumes
//    };
//}
//function updateBAR(p, v) {
//    option.series.forEach(function (v, i) {
//        if (v == __mCode) {
//            var s = option.series[i];
//            //s.data;
//        }

//    }
//}
function offMAData(name) {
    MA.forEach(function (v, i) {
        if (v.name == name) {
            MA.splice(i, 1);
        }
    });
    option.series.forEach(function (v, i) {
        if (v.name == name) {
            option.series.splice(i, 1);
        }
    });
}
function onMAData(name, days) {
    var ret = false;
    option.series.forEach(function (v) {
        if (v == name) ret = true;
    });
    if (!ret) {
        option.series.push(getMA(days));
    }
}

function calculateMA(dayCount) {
    var result = [];
    for (var i = 0, len = dataBARS.length; i < len; i++) {
        if (i < dayCount) {
            result.push('-');
            continue;
        }
        var sum = 0;
        for (var j = 0; j < dayCount; j++) {
            sum += dataBARS[i - j].x.close;
        }
        result.push(+(sum / dayCount).toFixed(3));
    }
    return result;
}
function getMA(days) {
    var n = 'MA' + days.toString();

    return {
        name: n,
        type: 'line',
        data: calculateMA(days),
        smooth: true,
        showSymbol: true,
        lineStyle: {
            opacity: 2.5
        }
    };
}
var liveOn = true;
function mainData(link) {
    link = 'getbars?mcode=' + __mCode + '&pulse=' + link;
    $.get(MktAPI + link, function (d) {
        if (d == null || d == undefined) { console.log('Initial Bar Data Load Returns NULL'); return; };
        liveOn = false;
        buildBars(d);
        liveBARS = [];
        option = process();
        myChart.setOption(option);
        liveOn = true;
    });
}

function updateInfo(d) {
    if (dataBARS.length >= d) {
        $('#ud').html(dataBARS[d].date.format('DD-MMM hh:mm'));
        if (d > 0) {
            var o = dataBARS[d - 1].x.close;
            var n = dataBARS[d].x.close;
            var r = ((n - o) / n) * 100;
            $('#uch').html(GetPercentFormat(r, 2));
            if (r >= 0) {
                $('#uch').addClass('green');
                $('#uch').removeClass('red');
            } else {
                $('#uch').addClass('red');
                $('#uch').removeClass('green');
            }
        }
        $('#uo').html(GetFormatedValAdjWithSep(dataBARS[d].x.open, __qf));
        $('#uc').html(GetFormatedValAdjWithSep(dataBARS[d].x.close, __qf));
        $('#uh').html(GetFormatedValAdjWithSep(dataBARS[d].x.high, __qf));
        $('#ul').html(GetFormatedValAdjWithSep(dataBARS[d].x.low, __qf));
        var txt = '<div  class="chartdigt665"><span class="default-label-box">Volume:</span><span class="default-label-box purple">' + GetFormatedValAdjWithSep(dataBARS[d].z.volumn, __bf) + '</span>'

        myChart.getOption().series.forEach(function (x) {
            if (x.name.match(/^ma/i) != null) {
                txt += '<div class="chartdigt665"><span class="default-label-box">' + x.name + ':</span><span class="default-label-box">' + x.data[d] + '</span></div>';

            }
            txt += '</div>';
        });
        $('#ul2').html(txt);

    }
    /*<div class="css33-graphtrightpan">
                    <div class="sell-btn-22">
                        <div class="chartdigt665"><span class="default-label-box">2023/02/08</span>
                          <span class="default-label-box  white">13:00</span></div>
                        <div class="chartdigt665"><span class="default-label-box">Open:</span>
                        <span class="default-label-box  white">23233.23</span></div>
                        <div class="chartdigt665"><span class="default-label-box">High:</span>
                        <span class="default-label-box  white">23239.44</span></div>
                        <div class="chartdigt665"><span class="default-label-box">Low:</span>
                        <span class="default-label-box  white">23163.00</span></div>
                        <div class="chartdigt665"><span class="default-label-box">Close:</span>
                        <span class="default-label-box  white">23198.56</span></div>
                        <div class="chartdigt665"><span class="default-label-box">Change:</span>
                      </div>
                    </div>
                    <div class="sell-btn-22">
                      <div class="chart-title-indicator-container">     
                        <div class="chartdigt665">  <span class="default-label-box">Cashback</span>
                          <span class="default-label-box  purple">23239.44</span></div>       
                        <div class="chartdigt665">  <span class="default-label-box">Vol</span>
                          <span class="default-label-box  purple">23239.44</span></div>   
                       <div class="chartdigt665">  <span class="default-label-box">MA:</span>
                        <span class="default-label-box  purple">23239.44</span></div>
                        <div class="chartdigt665"> <span class="default-label-box">MA5:</span>
                        <span class="default-label-box  blue">23163.00</span></div>
                        <div class="chartdigt665"> <span class="default-label-box">MA10:</span>
                        <span class="default-label-box orange">23198.56</span></div>
                        <div class="chartdigt665"> <span class="default-label-box">MA20:</span>
                        <span class="default-label-box teal">032.0%</span></div>
                        <div class="chartdigt665"><span class="default-label-box">MA30:</span>
                        <span class="default-label-box purple">4.52%</span></div>
                      </div>
                    </div>
                  </div>
     */
    //console.log(d);
}
//function updateOp() {
//    var x = get_xAxis();
//    var y = get_yAxis();
//    var z = get_zAxis();
//    var op = {
//        dataZoom: [{}],
//        series: [
//            {
//                name: __mCode,
//                type: 'candlestick',
//                data: x,
//            }],
//        xAxis: [
//            {
//                type: 'category',
//                data: y
//            },
//            {
//                type: 'category',
//                gridIndex: 1,
//                data: y
//            }, {
//                name: 'Volume',
//                type: 'bar',
//                xAxisIndex: 1,
//                yAxisIndex: 1,
//                data: z
//                //data: data.volumes
//            }]
//    }
//    // op.series[0].data = myChart.getOption().series[0].data;
//    //op.series[0].data[op.series[0].data.length - 1][1] = p;
//    op.dataZoom = myChart.getOption().dataZoom;
//    myChart.setOption(op);
//}
function __CheckAndDisplay() {
    //var l = liveBARS[liveBARS.length - 1];
    return;
    var d = myChart.getOption().series[0].data;
    var n = d[d.length - 1];
    console.log(d[0].toString() + '|' + d[1].toString() + '<-->' + n[0].toString() + '|' + n[1].toString());
}
function process() {
    var x = get_xAxis();
    var y = get_yAxis();
    var z = get_zAxis();
    return opt = {
        animation: false,
        legend: {
            show: false,
        },
        tooltip: {
            show: true,
            trigger: 'axis',
            axisPointer: {
                type: 'cross'
            },
            borderWidth: 1,
            borderColor: '#ccc',
            padding: 10,
            textStyle: {
                color: '#777',
                fontSize: '12'
            },
            position: function (pos, params, el, elRect, size) {
                const obj = {
                    top: -1000
                };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 30;
                try {
                    updateInfo(params[0].data[0]);

                } catch (e) {
                }
                return obj;
            }
        },
        axisPointer: {
            show: true,
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
                    show: false,
                    yAxisIndex: false
                },
                brush: {
                    show: false,
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
                left: '5%',
                right: '8%',
                top: '3%',
                height: '76%'
            },
            {
                left: '5%',
                right: '8%',
                top: '86%',
                height: '10%'
            }
        ],
        xAxis: [
            {
                type: 'category',
                data: y,
                boundaryGap: false,
                axisLine: { onZero: false },
                splitLine: {
                    show: true,
                    lineStyle: { color: '#0c03094a' },
                },
                min: 'dataMin',
                max: 'dataMax',
                axisPointer: {
                    z: 100
                }
            },
            {
                type: 'category',
                gridIndex: 1,
                data: y,
                boundaryGap: false,
                axisLine: { onZero: false },
                axisTick: { show: false },
                splitLine: {
                    show: true,
                    lineStyle: { color: '#0c03094a' },
                },
                axisLabel: { show: true },
                min: 'dataMin',
                max: 'dataMax'
            }
        ],
        yAxis: [
            {
                scale: true,
                position: 'right',
                splitArea: {
                    show: false,
                },
                splitLine: {
                    show: true,
                    lineStyle: { color: '#0c03094a' },
                }
            },
            {
                scale: true,
                gridIndex: 1,
                scale: true,
                position: 'right',
                splitNumber: 2,
                axisLabel: { show: true },
                axisLine: { show: true },
                axisTick: { show: true },
                splitLine: {
                    show: true,
                    lineStyle: { color: '#0c03094a' },
                },
            }
        ],
        dataZoom: [
            {
                type: 'inside',
                xAxisIndex: [0, 1],
                start: 75,
                end: 100
            },

        ],
        series: [
            {
                name: __mCode,
                type: 'candlestick',
                data: x,
                //data: data.values,
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
                    className: 'd-none'
                }
            },
            {
                name: 'Volume',
                type: 'bar',
                xAxisIndex: 1,
                yAxisIndex: 1,
                data: z
            }
        ]
    };
}
var MA = [];
var shouldLoad = false;
var pLeft = 0;
function subscribe() {
    $('input:checkbox').change(function () {
        var v = $(this).attr('data-ma');
        if (v != undefined) {
            if ($(this).is(':checked')) {
                var days = $(this).attr('data-days');
                onMAData(v, days);
                MA.push({ name: v, days: days });
                myChart.setOption(option);
            }
            else {
                offMAData(v);
                myChart.setOption(option, { replaceMerge: ['xAxis', 'yAxis', 'series'] });
            }
        }
    });
    var dd = $('#barTime');
    dd.children().remove();
    dd.append('<option>', BarOps());
    $('#barTime option')[1].selected = true;
    mainData($('#barTime').val());
    dd.on('change', function (d) {
        SwitchBARS($(this).val(), $('#barTime option:selected').attr('data-du'));
    });

    myChart.on('mousemove', function (lo) {
        var data = lo.data[0];
        updateInfo(data);
        // console.log('-->mouse move' + data.toString());
        //console.log(data);//
    });
    myChart.on('dataZoom', function (data) {
        data = data.batch[0];
        if (data.start < pLeft)
            shouldLoad = true;
        else if (data.start > pLeft)
            shouldLoad = false;
        if (shouldLoad && data.start == 0)
            doLoadMoreLeftBars();

        pLeft = data.start;
    });
}
var BarOp = [];
function BarOps() {
    BarOp.push({ name: '1min', duration: 60 });
    BarOp.push({ name: '2min', duration: 120 });
    BarOp.push({ name: '15min', duration: 900 });
    BarOp.push({ name: '30min', duration: 1800 });
    BarOp.push({ name: '1hrs', duration: 3600 });
    BarOp.push({ name: '4hrs', duration: 1440 });
    BarOp.push({ name: '6hrs', duration: 21600 });
    BarOp.push({ name: '8hrs', duration: 28800 });
    BarOp.push({ name: '1day', duration: 86400 });
    BarOp.push({ name: '3day', duration: 259200 });
    BarOp.push({ name: '7day', duration: 604800 });
    var opt = '';
    var isSel = false;
    BarOp.forEach(x => {
        if (isSel == false) {
            opt += '<option selected="true" data-du="' + x.duration + '" value=' + x.name + '>' + x.name + '</option>'
            isSel = true;
        } else {
            opt += '<option data-du="' + x.duration + '" value=' + x.name + '>' + x.name + '</option>'
        }
    });
    return opt;
}

function SwitchBARS(name, du) {
    //ToDo: Naveen Change here for Loading other Time Bars
    // var r = await connection.invoke("ChangeTo", name);
    mainData(name);
    Refresh()
    console.log('BAR Changed to ' + name + ' ' + du.toString());
}


if (option && typeof option === 'object') {
    myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);
//mainData($('#barTime').val());
