function BarLoad(data) {
    $('#Three').css("color", "lime");

    console.log(data[0]);
    $('#Three').html(GetBarTable(data));
    console.log(data);
}

function MarketLoad(data) {
    console.log(data);
    
}

function GetBarTable(array) {
    var arrlen = array.length;
    var selected = '';
    var row = '';
    if (arrlen > 0) {
        selected = '<table class="table table-responsive">';
        selected = selected + '<thead class="trasaction-table-title">';
        selected = selected + '<tr><th class="css566-table-text"><div class="name-css46"><span>Code</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="price-css46"><span>Type</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="price-css46"><span>Time</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="price-css46"><span>Stamp</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="change24-css46"><span>Open</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>High</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Low</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Close</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Volume</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Value</span></div></th>';
        selected = selected + '<th class="css566-table-text"><div class="volume-css46"><span>Cashback</span></div></th>';
        selected = selected + '</tr></thead>';

        for (let i = 0; i < arrlen; i++) {
            var newD = new Date(0);
            newD.setUTCSeconds(array[i].time);
            var reported = newD.toUTCString();
            row = row + '<tbody class="trasaction-table">';
            row = row + '<tr><td><div class="coin-name-css46">' + array[i].code + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].type + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].time + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + newD + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].open + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].high + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].low + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].close + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].volume + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].tradesValue + '</div></td>';
            row = row + '<td><div class="coin-24change-css46">' + array[i].cashBack + '</div></td>';
            row = row + '</tr></tbody>';

        }
        selected = selected + row + '</div>';
    }
    return selected;
}

