//window.addEventListener('resize', myChart.resize);

var dom = document.getElementById('volume-piechart');
var myChart = echarts.init(dom, null, {
    renderer: 'canvas',
    useDirtyRect: false
});
var app = {};

var option;

option = {
    title: {
        text: ' Volume Data',
        subtext: '',
        left: 'center'
    },
    tooltip: {
        trigger: 'item',
        formatter: '{a} <br/>{b} : {c} ({d}%)'
    },
    legend: {
        left: 'center',
        top: 'bottom',
        data: [
            'BTC',
            'USDT',
            'TechnoSavvy',
            'USDC',
            'ETH',
            'BUSD',
            'MATIC',
            'LTC'
        ]
    },
    toolbox: {
        show: true,
        feature: {
            mark: { show: true },
            dataView: { show: true, readOnly: false },
            restore: { show: true },
            saveAsImage: { show: true }
        }
    },
    series: [
        {
            name: 'Area Mode',
            type: 'pie',
            radius: [20, 140],
            center: ['50%', '50%'],
            roseType: 'area',
            itemStyle: {
                borderRadius: 5
            },
            data: [
                { value: 30, name: 'BTC' },
                { value: 28, name: 'USDT' },
                { value: 26, name: 'TechnoSavvy' },
                { value: 24, name: 'USDC' },
                { value: 22, name: 'ETH' },
                { value: 20, name: 'BUSD' },
                { value: 18, name: 'MATIC' },
                { value: 16, name: 'LTC' }
            ]
        }
    ]
};

if (option && typeof option === 'object') {
    myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);