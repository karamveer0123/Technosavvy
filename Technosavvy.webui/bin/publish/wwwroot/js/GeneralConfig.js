var allCur = '';
var curValue = '';
var curSyb = '';
var curCountry = '';

var consoleDevMode = true;
console.log = consoleDevMode ? console.log : () => { };

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
    console.log('theme', res, data);
    if (res[0] == "True") {
        if (res[1] == DarkTheme) {
            $("#themeChange").attr("href", "/css/dark-theme-root.css");
            $("#mlogo").attr("src", "/images/b-logo.png");
            $("#chMoon").addClass("d-none");
            $("#chSun").removeClass("d-none");
            $("#chMoon2").addClass("d-none");
            $("#chSun2").removeClass("d-none");
        } else if (res[1] == LightTheme) {
            $("#themeChange").attr("href", "/css/light-theme-root.css");
            $("#mlogo").attr("src", "/images/w-logo.png");
            $("#chMoon").removeClass("d-none");
            $("#chSun").addClass("d-none");
            $("#chMoon2").removeClass("d-none");
            $("#chSun2").addClass("d-none");
        }
    } else if (res[0] == "False") {
        CreateToasterMessage("Theme", "Theme Change failed.", "error");
        console.log("error in Themechange");
    }
    else {
        CreateToasterMessage("Theme", "Theme Change failed for unknow reasons.", "error");
        console.log("error in Themechange");
    }

}


function UpdateCurrency(cur) {
    event.stopPropagation();
    $.ajax({
        url: '/home/UpdateCurrency?str=' + cur, success: function (obj) {
            console.log('obj', obj);
            $('#ddmbCurr').text(obj);
            $('#CurrMobileRes').text(obj);
            for (var i = 0; i < allCur.length; i++) {
                selCur = cur;
                if (allCur[i].abbr == cur) {
                    curSyb = allCur[i].symbol;
                    curValue = allCur[i].value;
                    console.log(allCur[i].symbol);
                    console.log(allCur[i].value);
                }
            }
            var all = $('.dyCur');
            all.each(function () {
                var val = $(this).attr('data-oval');
                $(this).html(GetFormatedCurrency(val));
            });

            var allCurLen = $('.dyCurLen');
            allCurLen.each(function () {
                var val = $(this).attr('data-oval');
                $(this).html(GetFormatedCurrencyMBTQ(val));
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
    var allCurLen = $('.dyCurLen');
    allCurLen.each(function () {
        var val = $(this).attr('data-oval');
        $(this).html(GetFormatedCurrencyMBTQ(val));
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
function GetFormatedValWithSepTrad(value, decimal) {
    if (value == undefined || decimal == undefined) return 'NaN';
    return parseFloat(parseFloat(value).toFixed(decimal)).toLocaleString(undefined, { minimumFractionDigits: decimal, maximumFractionDigits: decimal });
}



function GetFormatedCurrency(value) {
    if (value == undefined) return 'NaN';
    //console.log('curValue', curValue)
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
    //var valueTxt = value.toString().replace(/-|%|−/g, '');
    //console.log('valueTxt', valueTxt);
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
        var v = val / bval;
        if (v <= 0 || val <= 0 || bval <= 0)
            v = 0;
        var txt = GetFormatedVal(v, dval) + ' ' + name;
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
LoadCurrIfNot();//Trigger on Load
function LoadCurrIfNot() {
    if (allCur.length <= 0)
        $.get(MktAPI + 'GetCurrencies', (data) => CurrenciesLoad(data));//GetTokenSummary
}
function CurrenciesLoad(data) {
    if (data == '')
        console.log('error:No Currency Loaded..');
    else
        console.log(data);
    $('#curOp').html(GetCurrTable(data));
    $('#currMob').html(GetCurrTableMobile(data))
    ApplyCurrSearch();
}
function GetCountryTable(array) {
    allCur = array;
    var selected = '';
    selected += '<ul class="dropdown-menu megamenu drop55" aria-labelledby="navbarDropdown">';
    selected += '<div id="sub-menu23">';
    selected += '<div class="acadesearch mb-2 "><form><input type="text" class="market-search" placeholder="Search"  id="divCurSearch"><i class="fa fa-search" aria-hidden="true"></i><button class="hide-search-bar" id="close-divCurSearch">x</button></form></div>';
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

    selected += '<div class="acadesearch mb-2 "><input type="search" autocomplete="off" class="market-search w-100" placeholder="Search"  id="divCurSearch"><i class="fa fa-search" aria-hidden="true"></i></div>';
    selected += '<ul id="dropusadiv35" class="dropusadiv35">';
    for (var i = 0; i < array.length; i++) {

        selected += "<li onclick=\"UpdateCurrency('" + array[i].abbr + "');\"><div class=\"dropdown-item\">";
        if (selCur == array[i].abbr)
            selected += '<div class="css222-usd  active">';
        else
            selected += '<div class="css222-usd">';

        selected += '<div class="form-check form-switch">';
        selected += '<div class="css222-usd-img"><img src="../images/coun-flags/' + array[i].country.toLowerCase() + '.png" title="' + array[i].name + '"></div>';
        selected += '<input type="checkbox" hidden id="flexSwitchCheckusd-' + array[i].abbr + '"/>';
        selected += '<label class="form-check-label" for="flexSwitchCheckusd-' + array[i].abbr + '">' + array[i].abbr + ' - ' + array[i].symbol + ' </label>';
        selected += '</div></div></div></li>';
    }
    selected += '</ul ></div ></ul > </div</span>';
    return selected;
}
function GetCurrTableMobile(array) {
    allCur = array;
    var selected = '';
    selected += '<div id="sub-menu23-mob">',
        selected += '<div class="acadesearch mb-2">',
        selected += '<form><input type="search" autocomplete="off" class="market-search w-100" placeholder="Search" id="divCurSearch2"><i class="fa fa-search" aria-hidden="true"></i></form></div>',
        selected += '<ul id="dropusadiv42" class="dropusadiv40">';
    for (var i = 0; i < array.length; i++) {
        selected += "<li onclick=\"UpdateCurrency('" + array[i].abbr + "');\"><a class=\"dropdown-item\" href=\"\">";
        if (selCur == array[i].abbr)
            selected += '<div class="css222-usd  active">';
        else
            selected += '<div class="css222-usd">';
        selected += '<div class="form-check form-switch">',
            selected += '<div class="css222-usd-img">',
            selected += '<img src="../images/coun-flags/' + array[i].country.toLowerCase() + '.png" class="proper" tabindex="0"',
            selected += 'data-bs-placement="top" data-bs-toggle="popover" data-bs-trigger="hover focus"',
            selected += 'data-bs-content="' + array[i].abbr + '" data-bs-original-title="" title="">',
            selected += '</div>',
            selected += '<input type="checkbox" hidden id="flexSwitchCheckusdMob-' + array[i].abbr + '"/>';
        selected += '<label class="form-check-label" for="flexSwitchCheckusdMob-' + array[i].abbr + '">' + array[i].abbr + ' - ' + array[i].symbol + '</label>',
            selected += '</div>',
            selected += '</div>',
            selected += '</a></li>';
    }
    selected += '</ul></div>'
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
//bindaccordionSearch(txtSearchFaq,apFAQs)

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
//$(window).load(function () {
//    console.log('Entered in function window load');
//    $.ajax({
//        url: '/home/LoadEvent', data: { "ev": window.location.pathname }, success: function (obj) {
//            console.log(obj);
//            // $.post("/home/LoadEvent", { "ev": window.location.pathname });
//        }, error: function (e) { console.log(e); }
//    });
//    });
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
        var value = $(this).val().replace(/\s/g, '').toLowerCase();
        //var value = $(this).val().toLowerCase();
        var f = '#dropusadiv35 li';
        $(f).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $('#divCurSearch2').on('keyup', function () {
        var value = $(this).val().replace(/\s/g, '').toLowerCase();
        //var value = $(this).val().toLowerCase();
        var f = '#dropusadiv42 li';
        $(f).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $('#close-divCurSearch').on('click', function () {
        $('#divCurSearch').val('');
        $('#divCurSearch').trigger('keyup');
    });
    $('#close-divCurSearch').on('click', function () {
        $('#divCurSearch2').val('');
        $('#divCurSearch2').trigger('keyup');
    });
}

// Country Dropdown

// Update Country url
function UpdateCountry(countCode) {
    /*$('#countryTxt').text(countCode);
    var srcUrl = '../images/coun-flags/' + countCode.toLowerCase() + '.png';
    $('#drpCountry img').attr('src', srcUrl);*/
    var loc = window.location.origin + '/home/SetCountry?Abbr=' + countCode;

    $.ajax({
        url: loc,
        success: function (obj) {
            console.log('Object', obj);
            if (obj) {
                $("#backdrop").removeClass("d-none");
                setInterval(function () {
                    $('#countryTxt').text(obj);
                    $('#countryTxt2').text(obj);
                    var srcUrl = '../images/coun-flags/' + obj.toLowerCase() + '.png';
                    $('#drpCountry img').attr('src', srcUrl);
                    $('#CountryMobileRes img').attr('src', srcUrl);
                    window.location.reload();
                }, 1000);

            }
            //
        }, async: true
    });


}



// Country Display on menu
function GetCountry(data) {
    allCountry = data;
    //console.log(allCountry)
    var selected = '';
    selected += '<ul class="dropdown-menu megamenu drop55">';
    selected += '<div id="sub-menu23 ">';
    selected += '<div class="acadesearch mb-2  ">';
    selected += '<form>';
    selected += '<input type="search" class="market-search w-100" placeholder="Search" id="divCuntrySearch" name="search" autocomplete="off"/><i class="fa fa-search" aria-hidden="true"></i>';
    selected += '</form>';
    selected += '</div>';
    selected += '<ul id="dropusadiv38" class="dropusadiv36">';
    for (var i = 0; i < allCountry.length; i++) {

        selected += "<li onclick=\"UpdateCountry('" + allCountry[i].abbri + "');\" ><div class=\"dropdown-item\" >";
        if (selCur == allCountry[i].abbri)
            selected += '<div class="css222-usd active" >';
        else
            selected += '<div class="css222-usd">';
        selected += '<div class="form-check form-switch">';
        selected += '<div class="css222-usd-img proper" tabindex="0" data-bs-toggle="popover" data-bs-placement="top" data-bs-trigger="hover focus" data-bs-content="' + allCountry[i].countryName + '"  title="">';
        selected += '<img src="../images/coun-flags/' + allCountry[i].abbri.toLowerCase() + '.png" />';
        selected += '</div>';
        selected += '<input type="checkbox" hidden id="flexSwitchCheckind-' + allCountry[i].abbri + '"/>',
            selected += '<label class="form-check-label" for="flexSwitchCheckind-' + allCountry[i].abbri + '">' + allCountry[i].abbri + ' - ' + allCountry[i].countryName + '</label>';
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

function GetCountryMobile(data) {
    allCountry = data;
    var selected = '';
    selected += '<div id="sub-menu23-mob2">',
        selected += '<div class="acadesearch mb-2">',
        selected += '<form><input type="search" autocomplete="off" class="market-search w-100" placeholder="Search" id="divCuntrySearch2"></form></div>',
        selected += '<ul id="dropusadiv40" class="dropusadiv40">';
    for (var i = 0; i < allCountry.length; i++) {
        selected += "<li onclick=\"UpdateCountry('" + allCountry[i].abbri + "');\"><a class=\"dropdown-item\" href=\"\">";
        if (selCur == allCountry[i].abbri)
            selected += '<div class="css222-usd  active">';
        else
            selected += '<div class="css222-usd">';
        selected += '<div class="form-check form-switch">',
            selected += '<div class="css222-usd-img">',
            selected += '<img src="../images/coun-flags/' + allCountry[i].abbri.toLowerCase() + '.png" class="proper" tabindex="0"',
            selected += 'data-bs-placement="top" data-bs-toggle="popover" data-bs-trigger="hover focus"',
            selected += 'data-bs-content="' + allCountry[i].abbri + '" data-bs-original-title="" title="">',
            selected += '</div>',
            selected += '<input type="checkbox" hidden id="flexSwitchCheckusdMob-' + allCountry[i].abbri + '"/>';
        selected += '<label class="form-check-label" for="flexSwitchCheckusdMob-' + allCountry[i].abbri + '">' + allCountry[i].abbri + ' - ' + allCountry[i].countryName + '</label>',
            selected += '</div>',
            selected += '</div>',
            selected += '</a></li>';
    }
    selected += '</ul></div>'
    return selected;



}

function ApplyCountrySearch() {
    $('#divCuntrySearch').on('keyup', function () {
        var value = $(this).val().replace(/\s/g, '').toLowerCase();
        //var value = $(this).val().toLowerCase();
        var f = '#dropusadiv38 li';
        $(f).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $('#divCuntrySearch').on('search', function () {
        $('#divCuntrySearch').val('');
        $('#divCuntrySearch').trigger('keyup');
    });

    $('#divCuntrySearch2').on('keyup', function () {
        var value = $(this).val().replace(/\s/g, '').toLowerCase();
        //var value = $(this).val().toLowerCase();
        var f = '#dropusadiv40 li';
        $(f).filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $('#divCuntrySearch2').on('search', function () {
        $('#divCuntrySearch2').val('');
        $('#divCuntrySearch2').trigger('keyup');
    });

}

function CountryLoad() {
    var locationOrigin = window.location.origin;
    $.get(locationOrigin + '/home/GetCountries', (data) => {
        console.log(data)
        $('#curCuntry').html(GetCountry(data));
        $('#countryMob').html(GetCountryMobile(data));
        ApplyCountrySearch();
    });

}
CountryLoad();

function loadFormatAll() {
    let no = 1;
    setInterval(function () {
        if (no < 11) {
            formateAll();
            no++;
        } else {
            clearInterval(this);
        }
    }, 2000);
}
loadFormatAll();



function Switch(data) {
    event.stopPropagation();
    $.ajax({
        url: '/trade/switchView?name=' + data, success: function (obj) {
            console.log('Theam Obj', obj);
            if (obj) {
                $("#backdrop").removeClass("d-none");
                window.location.reload();
            }
        }
    });
}


function GetFormatedCurrencyMBTQ(value) {
    if (value == undefined) return 'NaN';

    let abbrev = ['', 'K', 'M', 'B', 'T'];
    let rangeOrder = Math.floor(Math.log10(Math.abs(value)) / 3)
    let order = Math.max(0, Math.min(rangeOrder, abbrev.length - 1))
    let suffix = abbrev[order];
    let x = (value / Math.pow(10, order * 3)).toFixed(2);

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
    return parseFloat(x * curValue).toLocaleString(loc, op) + ' ' + suffix;
}



function GetFormatCountMBTQ(value, decimals) {
    if (value == undefined) return 'NaN';

    let abbrev = ['', 'K', 'M', 'B', 'T'];
    let rangeOrder = Math.floor(Math.log10(Math.abs(value)) / 3)
    let order = Math.max(0, Math.min(rangeOrder, abbrev.length - 1))
    let suffix = abbrev[order];
    let x = (value / Math.pow(10, order * 3)).toFixed(decimals);
    return parseFloat(x) + ' ' + suffix;
}




$(".themebox").on("click", function () {
    var switchColorList = [
        {
            "bsred": $(this).data("color"),
            "lightred": $(this).data("color"),
            "secondColor": $(this).data("second"),
            "themeActive": $(this).data("active")

        }];
    let string = JSON.stringify(switchColorList)
    localStorage.setItem('switchColor', string);
    themeboxColor();
    window.location.reload();
});

function themeboxColor() {
    var getList = localStorage.getItem('switchColor')
    if (getList) {
        let retArray = JSON.parse(getList)
        //console.log('getList', retArray)
        var selColTheme = retArray[0].themeActive;

        $('.themebox-purple,.themebox-red, .themebox-blue').removeClass("active");
        var themePurple = $('.themebox-purple').data("active");
        var themeRed = $('.themebox-red').data("active");
        var themeBlue = $('.themebox-blue').data("active");
        $(":root").css({
            //"--bs-red": retArray[0].bsred,
            "--lightred": retArray[0].lightred,
            "--bs-green": retArray[0].secondColor,
        });
        console.log('selColTheme', selColTheme, 'themebox', themePurple, themeRed, themeBlue);
        if (selColTheme == themePurple) {
            $('.themebox-purple').addClass("active");
        } else if (selColTheme == themeRed) {
            $('.themebox-red').addClass("active");
        } else if (selColTheme == themeBlue) {
            $('.themebox-blue').addClass("active");
        }
    }
}
themeboxColor();

$('.nav-icon').click(function () {
    $('.nav-icon').toggleClass('open');
    $('body').toggleClass('menu-responsive');
    $('html').toggleClass('menu-responsive');
});

$('.TechnoSavvy-mobile-blkdrop').click(function () {
    $('.nav-icon').removeClass('open');
    $('body').removeClass('menu-responsive');
    $('html').removeClass('menu-responsive');
});



$("#menu-mobile a").click(function (event) {
    // event.preventDefault();
    if ($(this).next('ul').length) {
        $(this).next().toggle('fast');
        $(this).children('i:last-child').toggleClass('rotate');
    }
});

$('.css44-cross').on('click', function () {
    $('.top-section').hide();
})

$('.dropListMob').on('click', function () {
    var $dropList = $(this).data('country');

    if ($dropList == 'Country') {
        $('#currMob').hide();
        $('#countryMob').show();
        $('#NotificationMobileList').hide();
        $('#TradeTheme').hide();
    }
    if ($dropList == 'Currency') {
        $('#currMob').show();
        $('#countryMob').hide();
        $('#NotificationMobileList').hide();
        $('#TradeTheme').hide();
    }
    if ($dropList == 'NotificationMobile') {
        $('#currMob').hide();
        $('#countryMob').hide();
        $('#TradeTheme').hide();
        $('#NotificationMobileList').show();
    }
    if ($dropList == 'TradeTheme') {
        $('#currMob').hide();
        $('#countryMob').hide();
        $('#NotificationMobileList').hide();
        $('#TradeTheme').show();
    }

});


$('.css44-cross').on('click', function () {
    $('.top-section').hide();
});


$(function () {
    $('#wallet-left33 ul').find('.nav-link.active').removeClass('active');
    $('.dropdown-menu').find('.dropdown-item.active').removeClass('active');
    $('#navbarSupportedContent ul').find('.nav-link.active').removeClass('active');
    var path = window.location.href;
    var pathn = window.location.pathname;
    $('#wallet-left33 ul .nav-link, #navbarSupportedContent ul .nav-link').each(function () {
        if (this.href === path) {
            $(this).addClass('active');
        }
        if (pathn === '/trade') {
            $('.tradeMenu').addClass('active');
        }
    });
    $('.dropdown-menu .dropdown-item').each(function () {
        if (this.href === path) {
            $(this).addClass('active');
        }
    });

    // Wallet sidebar menu
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
});

function bankDetailsList() {


    var txt = '';
    txt += '<div class="bankbox543">',
        txt += '<div class=" banktitle433 align-items-center">',
        txt += '<div class="w-50"><span class="placeholder placeholder-sm w-50"></span> </div>',
        txt += '<div class="w-50 bankedbt654 text-end"><span class="placeholder placeholder-sm w-50 ms-auto" style="height:10px;"></span></div></div>',
        txt += '<div class="bandivde233">',
        txt += '<div class="css77-withdrawlS">',
        txt += '<div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt += '<div class="css77-withdrawlS">',
        txt += '<div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt += '<div class="css77-withdrawlS">',
        txt += '<div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt += '<div class="css77-withdrawlS">',
        txt += '<div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt += '<div class="css77-withdrawlS">',
        txt += '<div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt += '<div class="css77-withdrawlS">',
        txt += '<div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt += '<div class="addupi4354 text-right   text">',
        txt += '<span class="placeholder placeholder-sm w-100"></span></div></div></div>';

    var txt2 = '';
    txt2 += '<div class="noFound noheight54 w-100 comunity-css566-box"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div>'

    var bankDL = $('#bankDetailsList').children().length;
    if (bankDL == 0) {
        $('#bankDetailsList').html(txt);
        setInterval(function () {
            $('#bankDetailsList').html(txt2);
        }, 3000)
    }

}
bankDetailsList()

function upiDetailsList() {


    var txt3 = '';
    txt3 += '<div class="bankbox543">',
        txt3 += '<div class="banktitle433"><span class="placeholder placeholder-sm w-50"> </span><span class="placeholder placeholder-sm w-50"></span></div>',
        txt3 += '<div class="bandivde233">',
        txt3 += '<div class="css77-withdrawlS"><div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt3 += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt3 += '<div class="css77-withdrawlS">',
        txt3 += '<div class="csssum-title55"><span class="placeholder placeholder-sm w-50"></span></div>',
        txt3 += '<div class="csssum-detail55"><span class="placeholder placeholder-sm w-50"></span> </div></div>',
        txt3 += '<div class="addupi4354 text-right text"><span class="placeholder placeholder-sm w-50"></span></div></div></div>';

    var txt4 = '';
    txt4 += '<div class="noFound noheight54 w-100 comunity-css566-box"><div class="norecord-div44"><span><img src="../images/no-data.png"></span><span> No records found</span></div></div>'

    var upiDL = $('#upiDetailsList').children().length;
    if (upiDL == 0) {
        $('#upiDetailsList').html(txt3);
        setInterval(function () {
            $('#upiDetailsList').html(txt4);
        }, 3000)
    }
}
upiDetailsList()



function NotificationAlert() {
    var data = [
        { 'title': 'Treasury Manager requested', 'body': '5,000 TechnoSavvy tokens to trade in this specifc network.', 'date': new Date() },
        { 'title': 'Treasury Manager requested2', 'body': '10,000 TechnoSavvy tokens to trade in this specifc network.', 'date': new Date() },
        { 'title': 'Treasury Manager requested3', 'body': '50,000 TechnoSavvy tokens to trade in this specifc network.', 'date': new Date() },
    ];

    $('#notificationMsgBody-Main').html('');
    var msgBody = '';    
    var countMobile = 1;
    var maxCountMobile = 3;
    msgBody += '<div class="not-title"><span>Notification</span> <span> <a href="/notification"> View All</a></span></div>',
    msgBody += '<ul id="notificationMsgBody">';
    data.forEach(function (b) {
        if (countMobile <= maxCountMobile) {
            msgBody += '<li class="notify-item">',
                msgBody += '<div class="notidiv33">',
                msgBody += '<div class="noitileft44">',
                msgBody += '<a href="dashboard-notification-detail.html">',
                msgBody += '<span class="not-head3">' + b.title + ' </span>',
                msgBody += '</a>',
                msgBody += '<span class="note-text2">' + b.body + '</span>',
                msgBody += '<div class="timenote44">',
                msgBody += '<span> <i class="fa fa-calendar" aria-hidden="true"></i>' + new moment.utc(b.date).local().format('LL') + ' </span>',
                msgBody += '<span><i class="fa fa-clock-o" aria-hidden="true"></i> ' + new moment.utc(b.data).local().format('LT') + ' </span>',
                msgBody += '</div></div >',
                msgBody += '<div class="noitiright44 "> <span class="dis-btn4 btnDismiss"> Dismiss</span> </div>',
                msgBody += '</div>',
                msgBody += '</li>'
            countMobile++
        }
    });
    msgBody += '</ul><div class="mesdiv43">  <span> <strong class="pen-notify">' + data.length + '</strong> pending notification</span> <a href="/notification" class="btn-view23"> View All</a> </div>';
    $('#notificationMsgBody-Main').html(msgBody);
  
    if (data.length != 0) {
        $('#NotifyCount').removeClass('d-none');
        $('#NotifyCount').html(data.length);
        $('#NotifyCount').attr("data-count", data.length);
    }
    var container = $("#notificationMsgBody");
    $('.btnDismiss').on("click", function (event) {
        var item = $(event.currentTarget).parents(".notify-item");  
        var noRecord = '<li class="notify-item norecord-div44"><span>No Record!</span></li>';
        item.remove(); 
        /* update notifications counter */
        var countElement = $("#NotifyCount");
        var prevCount = +countElement.attr("data-count");
        var newCount = prevCount - 1;
        countElement.attr("data-count", newCount).html(newCount);
        $('.pen-notify').html(newCount);
        if (newCount === 0) {
            countElement.remove();            
            container.html(noRecord)
        }        
    });    
}
NotificationAlert()    
