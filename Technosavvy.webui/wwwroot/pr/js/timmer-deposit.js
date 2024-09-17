
function processTwoMin(){
let progressElm = document.getElementsByClassName('progress-time')[0];
let circumference = 2 * Math.PI * progressElm.getAttribute('r');

progressElm.style.strokeDasharray = circumference;
progressElm.style.strokeDashoffset = circumference * 0;

let max = parseInt(document.getElementsByClassName('seconds1')[0].textContent);
let seconds1 = max;

let secondsElm = document.getElementsByClassName('seconds1')[0];

let timerId = setInterval(() => {
    seconds1--;
    if(seconds1 <= 0)
        clearInterval(timerId);

    percentage = seconds1/max * 100;
    progressElm.style.strokeDashoffset = circumference - (percentage/100) * circumference;

    secondsElm.textContent = seconds1.toString().padStart(2, '0');
    console.log('second1', seconds1);
    if (seconds1 == 0) {
        $('#transactionHash').addClass('d-none');
        $('#messageBox').removeClass('d-none');
        $('#messageBox').addClass('d-block');
        $('#trnsConfirmed').addClass('d-none');
      
    }
}, 1000);

    
}