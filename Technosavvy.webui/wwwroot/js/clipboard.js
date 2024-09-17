var copyTextareaBtn = document.querySelector('.js-textareacopybtn');
$('.copytext34').hide();

copyTextareaBtn.addEventListener('click', function (event) {
    var copyTextarea = document.querySelector('.js-copytextarea');
    copyTextarea.focus();
    copyTextarea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
        $('.copytext34').show();
        $('.ercClipboard span').addClass('active');

        setTimeout(function () {
            $('.copytext34').hide();
            $('.ercClipboard span').removeClass('active');
        }, 3000)
    } catch (err) {
        console.log('Oops, unable to copy');
    }
});

//second

var copyTextareaBtn = document.querySelector('.js-textareacopybtn2');
copyTextareaBtn.addEventListener('click', function (event) {
    var copyTextarea = document.querySelector('.js-copytextarea2');
    copyTextarea.focus();
    copyTextarea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
    } catch (err) {
        console.log('Oops, unable to copy');
    }
});

//third

var copyTextareaBtn = document.querySelector('.js-textareacopybtn3');
copyTextareaBtn.addEventListener('click', function (event) {
    var copyTextarea = document.querySelector('.js-copytextarea3');
    copyTextarea.focus();
    copyTextarea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
    } catch (err) {
        console.log('Oops, unable to copy');
    }
});

//fourth

var copyTextareaBtn = document.querySelector('.js-textareacopybtn4');
copyTextareaBtn.addEventListener('click', function (event) {
    var copyTextarea = document.querySelector('.js-copytextarea4');
    copyTextarea.focus();
    copyTextarea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
    } catch (err) {
        console.log('Oops, unable to copy');
    }
});

//fifith

var copyTextareaBtn = document.querySelector('.js-textareacopybtn5');
copyTextareaBtn.addEventListener('click', function (event) {
    var copyTextarea = document.querySelector('.js-copytextarea5');
    copyTextarea.focus();
    copyTextarea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
    } catch (err) {
        console.log('Oops, unable to copy');
    }
});
