var MyRewardsData = [
    { id: '1', date: '2023-12-01', bonus: 2, rewardType: 'Referral', RewardName: 'SignUp', status: 'Pending' },
    { id: '2', date: '2023-12-02', bonus: 3, rewardType: 'Airdrop', RewardName: 'Discord', status: 'Approved' },
    { id: '2', date: '2023-12-03', bonus: 5, rewardType: 'Referral', RewardName: 'SignUp', status: 'Pending' },
    { id: '3', date: '2023-12-04', bonus: 2, rewardType: 'Giveaway', RewardName: 'SignUp', status: 'Approved' },
    { id: '5', date: '2023-12-05', bonus: 4, rewardType: 'Referral', RewardName: 'Registration', status: 'Pending' },
    { id: '6', date: '2023-12-06', bonus: 2, rewardType: 'Referral', RewardName: 'SignUp', status: 'Approved' },
    { id: '7', date: '2023-12-07', bonus: 3, rewardType: 'Referral', RewardName: 'SignUp', status: 'Pending' }

];
var MyReferralsData = [
    { date: '2023-12-01', status: 'Pending', reward: 'Active Trader', Count: 'Count 1B' },
    { date: '2023-12-02', status: 'Approved', reward: 'Registered', Count: 'Count 2B' },
    { date: '2023-12-03', status: 'Pending', reward: 'Registered', Count: 'Count 3B' },
    { date: '2023-12-04', status: 'Approved', reward: 'Pending', Count: 'Count 4B' },
    { date: '2023-12-05', status: 'Pending', reward: 'Pending', Count: 'Count 5B' },
    { date: '2023-12-06', status: 'Approved', reward: 'Community Member ', Count: 'Count 6B' },
    { date: '2023-12-07', status: 'Pending', reward: 'Community Member ', Count: 'Count 7B' }

];





function ApplySearch() {
    $('.market-search').each(function () {
        var tn = $(this).attr('data-tab');
        $(this).on("keyup", function () {
            var value = $(this).val().toLowerCase();
            console.log(value);
            var f = '#' + tn + ' tr';
            $(f).filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            });
            $('.market-search').on('search', function () {
                $('.market-search').val('');
                $('.market-search').trigger('keyup');
            });
        });
    });
}
ApplySearch()


function MyRewardsdataTableList(data) {
    function dataTableList() {
        var txt = '';
        $('#myTableMyRewardsTable tbody').html('');
        data.forEach(function (item) {
            txt += '<tr>',
                txt += '<td>' + item.id + '</td>',
                txt += '<td>' + item.date + '</td>',
                txt += '<td>' + item.bonus + '</td>',
                txt += '<td>' + item.rewardType + '</td>',
                txt += '<td>' + item.RewardName + '</td>',
                txt += '<td>' + item.status + '</td>',
                txt += '</tr>'

        });
        $('#myTableMyRewardsTable tbody').html(txt);
        ApplySearch()
    }
    dataTableList();


    function filterTableData(startDate, endDate) {
        var txt = '';
        var filteredData = data.filter(function (item) {
            return item.date >= startDate && item.date <= endDate;
        });
        console.log('filteredData', filteredData);
        $('#myTableMyRewardsTable tbody').html('');

        filteredData.forEach(function (item) {       
            txt += '<tr>',
            txt += '<td>' + item.id + '</td>',
            txt += '<td>' + item.date + '</td>',
            txt += '<td>' + item.bonus + '</td>',
            txt += '<td>' + item.rewardType + '</td>',
            txt += '<td>' + item.RewardName + '</td>',
            txt += '<td>' + item.status + '</td>',
            txt += '</tr>'        
        });

        $('#myTableMyRewardsTable tbody').html(txt);
        ApplySearch()
    }

    $(document).ready(function () {
        $('#filterBtn').on('click', function () {
            var startDate = $('#startDate').val();
            var endDate = $('#endDate').val();
       
            if (startDate && endDate) {
                filterTableData(startDate, endDate);
            } else {
            
            }
        });
    });


    $(document).ready(function () {
        $('#resetBtn').on('click', function () {
            $('#startDate').val('');
            $('#endDate').val('');
            $('#userStatus').val('');
            dataTableList();
        })
    });
}

//MyRewardsdataTableList(MyRewardsData);

// MyReferrals
function BuildMyReferrals(data) {

    function MyReferralsTableList() {
        var txt = '';
        $('#MyReferralsTable tbody').html('');      
        data.forEach(function (item) {
            txt += '<tr>',
                txt += '<td>' + item.date + '</td>',
                txt += '<td>' + item.status + '</td>',
                txt += '<td>' + item.reward + '</td>',
                txt += '<td>' + item.Count + '</td>',
                txt += '</tr>'

        });
        $('#MyReferralsTable tbody').html(txt);
        ApplySearch()
    }
    MyReferralsTableList();


    function MyReferralsfilterData(startDate, endDate, statusValue) {
        var txt = '';
        var filteredData = data.filter(function (item) {
            return item.date >= startDate && item.date <= endDate;
        });
        console.log('filteredData', filteredData);
        $('#MyReferralsTable tbody').html('');

        var value = statusValue;
        console.log('value', value);
        filteredData.forEach(function (item) {
            if (value == item.reward) {
                txt += '<tr>',
                    txt += '<td>' + item.date + '</td>',
                    txt += '<td>' + item.status + '</td>',
                    txt += '<td>' + item.reward + '</td>',
                    txt += '<td>' + item.Count + '</td>',
                    txt += '</tr>'
            }
        });

        $('#MyReferralsTable tbody').html(txt);
        ApplySearch()
    }

    $(document).ready(function () {
        $('#filterBtn2').on('click', function () {
            var startDate = $('#startDate2').val();
            var endDate = $('#endDate2').val();
            var statusValue = $('#userStatus').val();
            if (startDate && endDate && statusValue) {
                MyReferralsfilterData(startDate, endDate, statusValue);
            } else {
            
            }
        });
    });
    $(document).ready(function () {
        $('#resetBtn2').on('click', function () {
            $('#startDate').val('');
            $('#endDate').val('');
            $('#userStatus').val('');
            MyReferralsTableList();
        })
    });

}

//BuildMyReferrals(MyReferralsData)