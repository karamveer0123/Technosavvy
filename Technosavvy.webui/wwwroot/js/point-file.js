function getPoint(id) {
    var inputFile = $("input[type='file']");
    var carParent = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').children('.carPt');
    var pointTotal = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').children('.ptTotal');
    var point = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').children().children('.kyc-upload-addmore').text();
    var gtplh = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').children('.kyc-doc-name:nth-child(2)').find("input");
    var validValue = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('.kyc-doc-name:nth-child(2)').find('input').val();
    var inputLast = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('.col-lg-3:last-child').find('input').val();
    var validLth = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('input.valid').length;
    var cat = $('#' + id).data('cat');
    var plh = $('#' + id).data('plh');
    var pt = $('#' + id).data('pt');
    var catPoint = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').data('cat');
    var catPs = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').data('ps');
    console.log(cat, plh, pt, point.replace(' Pts', ''), carParent);
    //console.log(catPs, gtplh, validLth);    
    var total = Number(pointTotal.text());
    var x = Number(carParent.text());
    var y = Number(point.replace(' Pts', ''));
    console.log('length Value', validValue, inputLast);
    //var diN = validLth.length[2].value;
    if (cat == catPoint) {
        if (catPs == x) {
            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').addClass('basegreen')
        } else {
            if (pt <= catPs) {
                if (inputLast) {
                    if (validValue) {
                        sum = x + y;
                        if (sum <= total) {
                            carParent.text(sum);
                        }
                        console.log('sum', sum)
                        if (sum == total) {
                            carParent.text(sum);
                            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').addClass('basegreen');
                            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('baseorange')
                        }
                        if (sum > total) {
                            carParent.text(y);
                            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').addClass('basegreen');
                            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('baseorange')
                        }
                        if (sum == catPs || sum > catPs) {
                            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').addClass('basegreen');
                        } else {
                            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').addClass('baseorange');
                        }
                    }
                }
            }
            else if (catPs == pt) {
                if (plh) {
                    if (inputLast) {
                        if (validValue) {
                            var y = Number(point.replace(' Pts', ''));
                            carParent.text(y);
                            $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').addClass('basegreen')
                        }
                    }
                }

            } else { }
        }
    }



}


function RemovePoint(id) {

    var inputFile = $("input[type='file']");
    var carParent = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').children('.carPt');
    var pointTotal = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').children('.ptTotal');
    var point = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').children().children('.kyc-upload-addmore').text();
    var gtplh = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find("input");
    var validValue = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('.kyc-doc-name:nth-child(2)').find('input').val();
    var inputSecPT = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('.col-lg-3:nth-child(3)').find('input').val();
    var inputLastPT = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('.col-lg-3:last-child').find('input').val();
    var inputLast = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('.col-lg-3:last-child').find('input').val();
    var btnremove = $('#' + id).closest('.TechnoSavvy-kyc-div-table-body').find('.col-lg-3:last-child').find(".upload__img-close")
    var cat = $('#' + id).data('cat');
    var plh = $('#' + id).data('plh');
    var pt = $('#' + id).data('pt');
    var catPoint = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').data('cat');
    var catPs = $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').data('ps');

   // console.log(catPs, btnremove);

    
    var total = Number(pointTotal.text());
    var x = Number(carParent.text());
    var y = Number(point.replace(' Pts', ''));

    console.log('Point', catPs, x, y)
    if (cat == catPoint) {
        if (inputSecPT != '' || inputSecPT != '') {

        } else {
            if (pt <= catPs) {                         
                if (x != 0) {

                    mins = x - y;
                    console.log('Mins', mins);
                    if (mins < 0) {
                        carParent.text('0');
                        $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('basegreen');
                    } else {
                        carParent.text(mins);
                    }               
                    
                    if (mins == 0) {
                        $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('baseorange');
                        $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('basegreen');
                    } else {
                        $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('basegreen');
                        $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').addClass('baseorange');
                    }
                }
        
            } else if (catPs == pt) {
                if (plh) {
                    var y = Number(point.replace(' Pts', ''));
                    carParent.text('');
                    $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('basegreen');
                    $('#' + id).closest('.TechnoSavvy-kyc-doc').find('.cat').removeClass('baseorange');
                }
            
            } else {
                
            }
        }
    }
}