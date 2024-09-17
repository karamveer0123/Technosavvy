
// Wallet sidebar menu
function sidebarMenu() {
    $(".menu-icon").on('click', function () {
        var checkBox = $('#menu-btn').is(':checked');
        console.log(checkBox);
        if (!checkBox) {
            // alert('check')
            $('body').addClass('walletMenuBody');
            $('.menu-icon').html('<i class="fa fa-times" aria-hidden="true"></i>');
        } else {
            // alert('nocheck')
            $('body').removeClass('walletMenuBody')
            $('.menu-icon').html('<i class="fa fa-th" aria-hidden="true"></i>');
        }
    });
}
sidebarMenu()
$('.nav-item').on('click', function () {
    $('body').removeClass('walletMenuBody')
    $('.menu-icon').html('<i class="fa fa-th" aria-hidden="true"></i>');
});

