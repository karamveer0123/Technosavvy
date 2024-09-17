 
function CryptoValMatrxChange() {
    var va = $(this).find(':selected').data('bval');
    var name = $(this).val();
    var all = $('.dyCryo');
    all.each(function () {
        var val = $(this).attr('data-oval');
        var dval = $(this).attr('data-dval');
        var txt = GetFormatedVal(val / bval, dval) + ' ' + name;
        $(this).html(txt);
    });
}
 