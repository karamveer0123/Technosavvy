

/*var timeleft = 30;
var downloadTimer = setInterval(function(){
timeleft--;
    
    $('#countdowntimer').text(timeleft);
    console.log(timeleft);
    if (timeleft == 0) {
        $('.optCountSecond').hide();
        $('#countdowntimer').text('');
        clearInterval(downloadTimer);
    }
}, 1000);*/

function fiveSecondTimer(sec) {
    const timeCount = sec
    let timing = timeCount

    let interval = null

    countdowntimer.innerText = sec;
    hideResenOpt(); 


    interval = setInterval(() => {
        timing--
        countdowntimer.innerText = timing;
        

        if (timing < 1) {
            clearInterval(interval);
            hideTimer()
        }
    }, 1000)
}
function hideTimer() {
    $('.optCountSecond').hide();
    $('#btnResend').show();
}
function hideResenOpt() {
    $('#btnResend').hide();    
}
fiveSecondTimer(120);

$('#btnResend').on('click', function () {
    $('.optCountSecond').show();
    fiveSecondTimer(120);
});