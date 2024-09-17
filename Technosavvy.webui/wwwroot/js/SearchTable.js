
// pagging size 
var pagesize = 10;

// datatable configration for searching and pagging
$(document).ready(function () {

    oTable = $('#Commontb').DataTable({
        "pageLength": pagesize, "lengthChange": false, sDom: 'lrtip', "ordering": false
    });

    $('#commonInputSearch').keyup(function () {
        oTable.search($(this).val()).draw();
        var tdata = oTable.rows({ filter: 'applied' }).data();
        //updateUserGridData(tdata);

    });


    $("#pagginginfo").append($("#Commontb_info"));
    $("#datapaging").append($("#Commontb_paginate"));

})