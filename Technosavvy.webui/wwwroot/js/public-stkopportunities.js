


$(function () {
    $.get(window.location.origin + '/staking/opportunities', (data) => {
        var stakPublic = JSON.parse(data);
        console.log('stakPublic', stakPublic);
        orderNotFound(stakPublic)

    });
});




function stakingPublic(data) {
    var txt = '';
    $('#StakingOpportunities').html('');
    if (data.length <= 0) return;
    data.forEach(function (f) {
        txt += '<tr>',
            txt += '<td class="td-sticky-left">',
            txt += '<div class="coin-name-css46"><span><img src="../images/coin/coin/' + $.trim(f.Name.toUpperCase()) + '.png"></span><span>' + f.Name + '</span> <small> ' + f.Name + '</small></div>',
            txt += '</td>',
            txt += '<td>' + f.MinAmount + ' </td>',
            txt += '<td>' + f.MaxAmount + ' </td>',
            txt += '<td>' + f.Duration + ' days </td>',
            txt += '<td>' + f.AYPOffered + '%</td>',
            txt += '<td class="staktab1"><a href="/Stake/Commit?sid=' + f.StakingOpportunityId + '">Stake</a> </td>',
            txt += '</tr >'
    });
    $('#StakingOpportunities').html(txt);
    $('#StakingOpportunities-main').simplePagination({
        items_per_page: 10,
    });
    ApplySearchWithNoRecord();
}

function orderNotFound(data) {
    $('#StakingOpportunities-main  tfoot').hide();
    if (data.length == 0) {
        setInterval(function () {
            $('#StakingOpportunities').hide();
            $('#StakingOpportunities-main tfoot').show();
        }, 3000)
    } else {
        stakingPublic(data)
    }
}


function ApplySearchWithNoRecord() {
    $('table tfoot').hide();
    $('.market-search').each(function () {
        var tn = $(this).attr('data-tab');
        $(this).on("keyup", function () {
            var value = $(this).val().replace(/\s/g, '').toLowerCase();
            //var value = $(this).val().toLowerCase();
            var fn = '#' + tn + ' tr';
            var fntb = $('#' + tn);
            var matchingRows = $(fn).filter(function () {
                return $(this).text().toLowerCase().indexOf(value) > -1;
            });
            if (matchingRows.length > 0) {
                $(fntb).nextAll('tfoot').hide();
                $('.my-navigation').show();
            } else {
                $(fntb).nextAll('tfoot').show();
                $('.my-navigation').hide();
            }
            $(fn).hide();
            matchingRows.show();
        });
        $('.market-search').on('search', function () {
            $('.market-search').val('');
            $('.market-search').trigger('keyup');
            $('.my-navigation').show();
        });
    });
}
ApplySearchWithNoRecord();
