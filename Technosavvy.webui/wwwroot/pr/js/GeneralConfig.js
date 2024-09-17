var allCur = '';
var curValue = '';
var curSyb = '';
var curCountry = '';

function CreateToasterMessage(title, msgbody, msgtype) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "5000",
        "hideDuration": "3000",
        "timeOut": "5000",
        "extendedTimeOut": "3000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
        //"tapToDismiss": false
    };

    if (msgtype === 'success')
        toastr.success(msgbody, title);
    else if (msgtype === 'error')
        toastr.error(msgbody, title);
    else if (msgtype === 'info')
        toastr.info(msgbody, title);
    else if (msgtype === 'warning')
        toastr.warning(msgbody, title);
}

function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}

function ProcessStart() {

    $("#backdrop").removeClass("d-none");
    return;
}
function ProcessStop() {

    $("#backdrop").addClass("d-none");
    return;

}
const DarkTheme = "Dark";
const LightTheme = "Light";
function Themechange(data) {
    var res = data.split("|");
    if (res[0] == "True") {
        if (res[1] == DarkTheme) {
            $("#themeChange").attr("href", "/css/dark-theme-root.css");
            $("#mlogo").attr("src", "/images/b-logo.png");
            $("#chMoon").addClass("d-none");
            $("#chSun").removeClass("d-none");
        } else if (res[1] == LightTheme) {
            $("#themeChange").attr("href", "/css/light-theme-root.css");
            $("#mlogo").attr("src", "/images/w-logo.png");
            $("#chMoon").removeClass("d-none");
            $("#chSun").addClass("d-none");
        }
    } else if (res[0] == "False") {
        CreateToasterMessage("Theme", "Theme Change failed.", "error");
    }
    else {
        CreateToasterMessage("Theme", "Theme Change failed for unknow reasons.", "error");
    }

}
function UpdateCurrency(cur) {
    event.stopPropagation();
    $.ajax({
        url: '/home/UpdateCurrency?str=' + cur, success: function (obj) {
            $('#ddmbCurr').text(obj);
            for (var i = 0; i < allCur.length; i++) {
                selCur = cur;
                if (allCur[i].abbr == cur) {
                    curSyb = allCur[i].symbol;
                    curValue = allCur[i].value;
                }
            }
            var all = $('.dyCur');
            all.each(function () {
                var val = $(this).attr('data-oval');
                $(this).html(GetFormatedCurrency(val));
            });
        }, async: true
    });
}


var _isInit = false;
function _InitialformatAll() {
    if (_isInit == 'false' || _isInit == false) {
        formateAll();
        _isInit = true;
    }
}
function formateDate() {
    var all = $('.dydate');
    all.each(function () {
        var val = $(this).attr('data-oval');
        var txt = '<div><span> ' + new moment.utc(val).local().format('LL') + ' </span> <div> <small> ' + new moment.utc(val).local().format('LTS') + '</small></div></div>'
        $(this).html(txt);
    });
}
function formateJustDate() {
    var all = $('.dydate');
    all.each(function () {
        var val = $(this).attr('data-oval');
        var txt = '<div><span> ' + new moment.utc(val).local().format('LL') + ' </span> </div>'; 
        $(this).html(txt);
    });
}
    function formateAll() {
    var all = $('.dyCur');
    all.each(function () {
        var val = $(this).attr('data-oval');
        $(this).html(GetFormatedCurrency(val));
    });
    var all = $('.dyVal');
    all.each(function () {
        var val = $(this).attr('data-oval');
        var dval = $(this).attr('data-dval');
        $(this).html(GetFormatedVal(val, dval));
    });
    CryptoValMatrxChange();// in some pages only
}
//function GetFormatedValWithSep(value, decimal) {
//    if (value == undefined || decimal == undefined) return 'NaN';
//    var op = { locales: "bn-IN", style: "currency", currency: "USD", useGrouping: "true" }
//    return (parseFloat(value).toFixed(decimal)).toLocaleString("bn-IN", op);
//}
function GetFormatedVal(value, decimal) {
    if (value == undefined || decimal == undefined) return 'NaN';
    return parseFloat(value).toFixed(decimal);
}
function GetFormatedValWithSep(value, decimal) {
    if (value == undefined || decimal == undefined) return 'NaN';
    var loc = _selLang + '-' + _selCountry;
    return parseFloat(parseFloat(value).toFixed(decimal)).toLocaleString(loc);
}
function GetFormatedCurrency(value) {
    if (value == undefined) return 'NaN';
    if (curValue.length <= 0) {
        for (var i = 0; i < allCur.length; i++) {
            if (allCur[i].abbr == selCur) {
                curSyb = allCur[i].symbol;
                curValue = allCur[i].value;
                curCountry = allCur[i].Country;
            }
        }
    }
    var loc = _selLang + '-' + _selCountry;
    var op = { style: "currency", currency: selCur, useGrouping: "true" }
    return parseFloat(value * curValue).toLocaleString(loc, op);
}
function GetFormatedValAdj(value, decimal) {
    if (value == undefined || decimal == undefined) return 'NaN';
    var r = (parseFloat(value).toFixed(decimal)).toString();
    var pre, po, le;
    if (r.match('[0-9]*.') != null) pre = r.match(/[0-9]*[.]/g).toString().length - 1;
    if (r.match('.[0-9]*') != null) po = r.match(/[.][0-9]*/g).toString().length - 1;
    if (r.match('[0-9]*') != null) le = r.match(/[0-9]/g).length;
    if (pre >= decimal)
        return (parseFloat(value).toFixed(0)).toString();
    else if (le >= decimal && pre < decimal) {
        var l = decimal - pre;
        return (parseFloat(value).toFixed(l)).toString();
    }
    else {
        var r = decimal - po;
        return ((parseFloat(value).toFixed(decimal)).toString() + (0).repeat(r));
    }

}
function GetFormatedValAdjWithSep(value, decimal) {
    if (value == undefined || decimal == undefined) return 'NaN';
    var loc = _selLang + '-' + _selCountry;

    var r = (parseFloat(value).toFixed(decimal)).toString();
    var pre, po, le;
    if (r.match('[0-9]*.') != null) pre = r.match(/[0-9]*[.]/g).toString().length - 1;
    if (r.match('.[0-9]*') != null) po = r.match(/[.][0-9]*/g).toString().length - 1;
    if (r.match('[0-9]*') != null) le = r.match(/[0-9]/g).length;
    if (pre >= decimal)
        return parseFloat(parseFloat(value).toFixed(0)).toLocaleString(loc);
    else if (le >= decimal && pre < decimal) {
        var l = decimal - pre;
        return parseFloat(parseFloat(value).toFixed(l)).toLocaleString(loc);
    }
    else {
        var r = decimal - po;
        return parseFloat(parseFloat(value).toFixed(decimal)).toLocaleString(loc) + (0).repeat(r);
    }

}
function ___qf() {
    if (__qf == null || __qf == undefined || __qf >= 1) return __qf;
    var i = 0;
    while (__qf < 1) {
        __qf *= 10;
        i++;
    }
    __qf = i;
    return i;
}
function ___bf() {
    if (__bf == null || __bf == undefined || __bf >= 1) return __bf;
    var i = 0;
    while (__bf < 1) {
        __bf *= 10;
        i++;
    }
    __bf = i;
    return i;
}
function GetPercentFormat(value, decimal) {
    return parseFloat(value).toFixed(decimal).toString() + '%';
}

function CryptoValMatrxChange(obj) {
    var bval = $(obj).find(':selected').data('bval');
    var name = $(obj).val();
    var all = $('.dyCryo');
    all.each(function () {
        var val = $(this).attr('data-oval');
        var dval = $(this).attr('data-dval');
        var txt = GetFormatedVal(val / bval, dval) + ' ' + name;
        $(this).html(txt);
    });
} function CryptoValMatrxChange() {
    var bval = $('#selectedCryptoCoin').find(':selected').data('bval');
    var name = $('#selectedCryptoCoin').val();
    var all = $('.dyCryo');
    all.each(function () {
        var val = $(this).attr('data-oval');
        var dval = $(this).attr('data-dval');
        var txt = GetFormatedVal(val / bval, dval) + ' ' + name;
        $(this).html(txt);
    });
}

function CheckFormValid(fromId) {
    var IsValid = $("#" + fromId).valid();
    if (IsValid) {
        ProcessStart();
    }
    return;
}
//LoadCurrIfNot();//Trigger on Load
//function LoadCurrIfNot() {
//    if (allCur.length <= 0)
//        $.get(MktAPI + 'GetCurrencies', (data) => CurrenciesLoad(data));//GetTokenSummary
//}
//function CurrenciesLoad(data) {
//    $('#curOp').html(GetCurrTable(data));
//    ApplyCurrSearch();
//}
function GetCountryTable(array) {
    allCur = array;
    var selected = '';
    selected += '<ul class="dropdown-menu megamenu drop55" aria-labelledby="navbarDropdown">';
    selected += '<div id="sub-menu23">';
    selected += '<div class="acadesearch mb-2 "><form><input type="text" class="market-search" placeholder="Search"  id="divCurSearch"><button class="hide-search-bar" id="close-divCurSearch">x</button></form></div>';
    selected += '<ul id="dropusadiv36" class="dropusadiv36">';
    for (var i = 0; i < array.length; i++) {

        selected += "<li onclick=\"UpdateCurrency('" + array[i].abbr + "');\"><div class=\"dropdown-item\" >";
        if (selCur == array[i].abbr)
            selected += '<div class="css222-usd  active">';
        else
            selected += '<div class="css222-usd">';

        selected += '<div class="form-check form-switch">';
        selected += '<div class="css222-usd-img"><img src="../images/coun-flags/' + array[i].country.toLowerCase() + '.png" title="' + array[i].name + '"></div>';
        selected += '<label class="form-check-label" for="flexSwitchCheckusd">' + array[i].abbr + ' - ' + array[i].symbol + ' </label>';
        selected += '</div></div></div></li>';
    }
    selected += '</ul ></div ></ul > </div</span>';
    return selected;
}
function GetCurrTable(array) {
    allCur = array;
    var selected = '';
    selected += '<ul class="dropdown-menu megamenu drop55" aria-labelledby="navbarDropdown">';
    selected += '<div id="sub-menu23">';
    selected += '<div class="acadesearch mb-2 "><form><input type="text" class="market-search" placeholder="Search"  id="divCurSearch"><button class="hide-search-bar" id="close-divCurSearch">x</button></form></div>';
    selected += '<ul id="dropusadiv35" class="dropusadiv35">';
    for (var i = 0; i < array.length; i++) {

        selected += "<li onclick=\"UpdateCurrency('" + array[i].abbr + "');\"><div class=\"dropdown-item\">";
        if (selCur == array[i].abbr)
            selected += '<div class="css222-usd  active">';
        else
            selected += '<div class="css222-usd">';

        selected += '<div class="form-check form-switch">';
        selected += '<div class="css222-usd-img"><img src="../images/coun-flags/' + array[i].country.toLowerCase() + '.png" title="' + array[i].name + '"></div>';
        selected += '<label class="form-check-label" for="flexSwitchCheckusd">' + array[i].abbr + ' - ' + array[i].symbol + ' </label>';
        selected += '</div></div></div></li>';
    }
    selected += '</ul ></div ></ul > </div</span>';
    return selected;
}
function removeAll(o, p) {
    $(o).removeClass('d-none');
    if ($(o).parent().length > 0 && $(o).parent != p)
        removeAll($(o).parent, p);
}
function recoverAll(bdy) {
    $('#' + bdy + ' .accordion-button').each(function () {
        $(this).removeClass('d-none');
    });
    $('#' + bdy + ' .accordion-body p').each(function () {
        $(this).removeClass('d-none');
    });
}
function bindaccordionSearch(txt, bdy) {
    $('#' + txt).on('keyup', function (e) {
        var str = $(this).val();
        if (str.length < 3) {
            recoverAll(bdy);
            return;
        }
        recoverAll(bdy);
        var pat = new RegExp(str, "i");
        $('#' + bdy + ' .accordion-button').each(function () {

            if ($(this).text().match(pat)) {
                removeAll($(this), bdy);
            }
            else {
                if ($(this).hasClass('d-none') == false) {
                    $(this).addClass('d-none');
                }
            }
        });
        $('#' + bdy + ' .accordion-body p').each(function () {

            if ($(this).text().match(pat)) {
                removeAll($(this), bdy);
            }
            else {
                if ($(this).hasClass('d-none') == false) {
                    $(this).addClass('d-none');
                }
            }
        });
    });

    $('#' + txt).on('click', function () {
        $('#' + txt).val('');
        $('#' + txt).trigger('keyup');
    });
}
function accordionBody() {
    var str = $('#txtSearchFaq').val();
    var pat = new RegExp(str, "i");
    $('.accordion-button').each(function () {
        if ($(this).text().match(pat)) {
            if ($(this).hasClass('d-none'))
                $(this).removeClass('d-none');
        }
        else {
            if ($(this).hasClass('d-none') == false)
                $(this).addClass('d-none');
        }

    });
}

var pid;


$(document).ready(function () {
    $.ajax({
        url: '/home/LoadEvent', data: { "ev": window.location.pathname }, success: function (data) { pid = data; }
    }, { passive: true });
});
$(document).scroll(function () {
    $.ajax({
        url: '/home/ScrollEvent', data: { "ev": window.location.pathname, "pid": pid, "sc": window.scrollY, "scH": screen.availHeight }
    }, { passive: true });
});
$(window).on("beforeunload", function () {
    $.ajax({
        url: '/home/NavigationEvent', data: { "ev": window.location.pathname, "pid": pid, "sc": window.scrollY, "scH": screen.availHeight }
    }, { passive: true });
});
function ApplyCurrSearch() {
    $('#divCurSearch').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        var f = '#dropusadiv35 li';
        $(f).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $('#close-divCurSearch').on('click', function () {
        $('#divCurSearch').val('');
        $('#divCurSearch').trigger('keyup');
    });
}

// Country Dropdown

// Update Country url
function UpdateCountry(countCode) {
    var loc = window.location.origin + '/home/SetCountry?Abbr=' + countCode;
    $.ajax({
        url: loc,
        success: function (obj) {
            if (obj) {
                $("#backdrop").removeClass("d-none");
                setInterval(function () {
                    $('#countryTxt').text(obj);
                    var srcUrl = '../images/coun-flags/' + obj.toLowerCase() + '.png';
                    $('#drpCountry img').attr('src', srcUrl);
                    window.location.reload();
                }, 1000);
            }
        }
    });
}

function GetCountry(data) {
    allCountry = data;
    var selected = '';
    selected += '<ul class="dropdown-menu megamenu drop55" aria-labelledby="navbarDropdown">';
    selected += '<div id="sub-menu23 ">';
    selected += '<div class="acadesearch mb-2  ">';
    selected += '<form>';
    selected += '<input type="search" class="market-search w-100" placeholder="Search" id="divCuntrySearch" name="search"/>';
    selected += '</form>';
    selected += '</div>';
    selected += '<ul id="dropusadiv38" class="dropusadiv36">';
    for (var i = 0; i < allCountry.length; i++) {

        selected += "<li onclick=\"UpdateCountry('" + allCountry[i].abbri + "');\" ><div class=\"dropdown-item\" >";
        if (selCur == allCountry[i].abbri)
            selected += '<div class="css222-usd active" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + allCountry[i].countryName + '" data-bs-original-title="' + allCountry[i].countryName + '" title="' + allCountry[i].countryName +'">';
        else
            selected += '<div class="css222-usd" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="' + allCountry[i].countryName + '" data-bs-original-title="' + allCountry[i].countryName + '" title="' + allCountry[i].countryName +'">';
        selected += '<div class="form-check form-switch">';
        selected += '<div class="css222-usd-img">';
        selected += '<img src="../images/coun-flags/' + allCountry[i].abbri.toLowerCase() + '.png" class="proper"   />';
        selected += '</div>';
        selected += '<label class="form-check-label" for="flexSwitchCheckind">' + allCountry[i].abbri + ' - ' + allCountry[i].countryName + '</label>';
        selected += '</div>';
        selected += '</div>';
        selected += '</div>';
        selected += '</a></li>';
    }
    selected += '</ul>';
    selected += '</div>';
    selected += '</ul>';
    return selected;

  
}

function ApplyCountrySearch() {
    $('#divCuntrySearch').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        var f = '#dropusadiv38 li';
        $(f).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $('#divCuntrySearch').on('search', function () {
        $('#divCuntrySearch').val('');
        $('#divCuntrySearch').trigger('keyup');
    });

}

function CountryLoad() {
    var locationOrigin = window.location.origin;
    $.get(locationOrigin + '/home/GetCountries', (data) => {
        $('#curCuntry').html(GetCountry(data));
        ApplyCountrySearch();
    });  
    
}
CountryLoad();
