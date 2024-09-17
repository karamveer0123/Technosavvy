
//ToDo:Naveen Fix this Clock to reflect actual Time left in Cashback event
setInterval(function time() {
    var d = new Date();
    var hours = 150 - d.getHours();
    var min = 60 - d.getMinutes();
    if ((min + '').length == 1) {
        min = '0' + min;
    }
    var sec = 60 - d.getSeconds();
    if ((sec + '').length == 1) {
        sec = '0' + sec;
    }
    jQuery('#the-final-countdown p').html(hours + ':' + min + ':' + sec)
}, 1000);

var days, hours, minutes, seconds; // variables for time units
var countdown = document.getElementById("tiles"); // get tag element

function getCountdown(data) {
    var target_date = moment.utc(data.cBeventEnd).valueOf();
    // find the amount of "seconds" between now and target
    var current_date = moment.utc(new Date()).valueOf();
    var seconds_left = (target_date - current_date) / 1000;    

    if (seconds_left >= 0) {
        days = pad(parseInt(seconds_left / 86400));
        seconds_left = seconds_left % 86400;

        hours = pad(parseInt(seconds_left / 3600));
        seconds_left = seconds_left % 3600;

        minutes = pad(parseInt(seconds_left / 60));
        seconds = pad(parseInt(seconds_left % 60));

        // format countdown string + set tag value
        countdown.innerHTML = "<span>" + hours + ":</span><span>" + minutes + "</span>";

        // console.log('seconds', seconds)
      
    } else {
           
    }
}

function pad(n) {
    return (n < 10 ? '0' : '') + n;
}